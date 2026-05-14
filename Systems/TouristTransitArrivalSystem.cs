using Colossal.Logging;
using System.Collections.Generic;
using Game;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Events;
using Game.Net;
using Game.Pathfind;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;

namespace CustomizeIt.Systems
{
    // Runs before vanilla TouristFindTargetSystem. Vanilla's origin pathfind uses
    // Pedestrian only, which fails on Train/Ship/Air OCs (no PedestrianLane sub-lane)
    // and kills the household with TouristNoTarget. We submit our own pathfind with
    // the right per-mode flags and add PathInformation ourselves; vanilla then sees
    // it already exists and skips the submit branch, consuming our result normally.
    // For Air, when the path still returns null we fall back to manually reserving
    // a hotel or assigning an attraction so the tourist isn't wasted.
    [UpdateBefore(typeof(TouristFindTargetSystem))]
    public partial class TouristTransitArrivalSystem : GameSystemBase
    {
        private static readonly ILog log = Mod.log;

        private EntityQuery m_SeekerQuery;
        private EntityQuery m_ResultQuery;
        private EntityQuery m_HotelQuery;
        private EntityQuery m_AttractionBuildingQuery;
        private PathfindSetupSystem m_PathfindSetupSystem;
        private AddMeetingSystem m_AddMeetingSystem;
        private readonly Dictionary<Entity, PendingTransitPath> m_PendingPaths = new Dictionary<Entity, PendingTransitPath>();
        private readonly List<Entity> m_PendingCleanup = new List<Entity>();
        private uint m_FallbackRngState = 0x9E3779B9u;

        private enum TransitOriginMode
        {
            Unknown,
            Train,
            Ship,
            AirDirectOutsideConnection,
            AirRouteWaypoint,
            AirTransportStopActive,
            AirTransportStopIdle,
            AirTakeoffLocation
        }

        private struct PendingTransitPath
        {
            public OutsideConnectionTransferType Type;
            public TransitOriginMode OriginMode;
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            m_PathfindSetupSystem = World.GetOrCreateSystemManaged<PathfindSetupSystem>();

            m_SeekerQuery = GetEntityQuery(
                ComponentType.ReadOnly<TouristHousehold>(),
                ComponentType.ReadOnly<LodgingSeeker>(),
                ComponentType.Exclude<MovingAway>(),
                ComponentType.Exclude<Target>(),
                ComponentType.Exclude<PathInformation>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>());

            m_ResultQuery = GetEntityQuery(
                ComponentType.ReadOnly<TouristHousehold>(),
                ComponentType.ReadOnly<LodgingSeeker>(),
                ComponentType.ReadOnly<PathInformation>(),
                ComponentType.Exclude<MovingAway>(),
                ComponentType.Exclude<Target>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>());

            m_HotelQuery = GetEntityQuery(
                ComponentType.ReadWrite<LodgingProvider>(),
                ComponentType.ReadWrite<Renter>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>());

            // AttractionData lives on the prefab, not the runtime building, so we
            // query all buildings and filter at pick time.
            m_AttractionBuildingQuery = GetEntityQuery(
                ComponentType.ReadOnly<Game.Buildings.Building>(),
                ComponentType.ReadOnly<PrefabRef>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>());

            m_AddMeetingSystem = World.GetOrCreateSystemManaged<AddMeetingSystem>();

            log.Info("TouristTransitArrivalSystem created.");
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

        protected override void OnUpdate()
        {
            using var households = m_SeekerQuery.ToEntityArray(Allocator.Temp);
            var citizenBufLookup = GetBufferLookup<HouseholdCitizen>(true);
            var currentBuildingLookup = GetComponentLookup<CurrentBuilding>(true);
            var prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            var ocDataLookup = GetComponentLookup<OutsideConnectionData>(true);
            var ownedVehicleLookup = GetBufferLookup<OwnedVehicle>(true);
            var connectedRouteLookup = GetBufferLookup<Game.Routes.ConnectedRoute>(true);
            var routeLaneLookup = GetComponentLookup<Game.Routes.RouteLane>(true);
            var transportStopLookup = GetComponentLookup<Game.Routes.TransportStop>(true);
            var takeoffLocationLookup = GetComponentLookup<Game.Routes.TakeoffLocation>(true);
            var connectedLookup = GetComponentLookup<Game.Routes.Connected>(true);
            var boardingVehicleLookup = GetComponentLookup<Game.Routes.BoardingVehicle>(true);
            var publicTransportLookup = GetComponentLookup<Game.Vehicles.PublicTransport>(true);
            var pathInfoLookup = GetComponentLookup<PathInformation>(true);
            var touristLookup = GetComponentLookup<TouristHousehold>(false);
            var lodgingProviderLookup = GetComponentLookup<LodgingProvider>(false);
            var renterLookup = GetBufferLookup<Renter>(false);

            CleanupPendingPathTags(ref pathInfoLookup, ref touristLookup);
            ProcessCompletedPathResults(
                ref citizenBufLookup,
                ref currentBuildingLookup,
                ref prefabRefLookup,
                ref ocDataLookup,
                ref pathInfoLookup,
                ref touristLookup,
                ref lodgingProviderLookup,
                ref renterLookup);

            if (households.Length == 0)
                return;

            var queue = m_PathfindSetupSystem.GetQueue(this, 64);
            using var ecb = new EntityCommandBuffer(Allocator.Temp);

            int submittedTrain = 0, submittedAir = 0, submittedShip = 0;
            int airRouteOrigin = 0, airStopActiveOrigin = 0, airStopIdleOrigin = 0, airTakeoffOrigin = 0, airDirectOrigin = 0;
            int skippedNotTrain = 0;
            int skippedNoOrigin = 0;

            for (int i = 0; i < households.Length; i++)
            {
                Entity household = households[i];

                if (!citizenBufLookup.HasBuffer(household)) { skippedNoOrigin++; continue; }
                var buf = citizenBufLookup[household];
                Entity originEntity = Entity.Null;
                for (int j = 0; j < buf.Length; j++)
                {
                    Entity citizen = buf[j].m_Citizen;
                    if (currentBuildingLookup.HasComponent(citizen))
                        originEntity = currentBuildingLookup[citizen].m_CurrentBuilding;
                }
                if (originEntity == Entity.Null) { skippedNoOrigin++; continue; }

                if (!prefabRefLookup.HasComponent(originEntity)) { skippedNotTrain++; continue; }
                Entity prefab = prefabRefLookup[originEntity].m_Prefab;
                if (!ocDataLookup.HasComponent(prefab)) { skippedNotTrain++; continue; }

                var ocType = ocDataLookup[prefab].m_Type;
                if ((ocType & OutsideConnectionTransferType.Road) != 0) { skippedNotTrain++; continue; }

                bool isTrain = (ocType & OutsideConnectionTransferType.Train) != 0;
                bool isAir = (ocType & OutsideConnectionTransferType.Air) != 0;
                bool isShip = (ocType & OutsideConnectionTransferType.Ship) != 0;
                if (!isTrain && !isAir && !isShip) { skippedNotTrain++; continue; }

                var parameters = new PathfindParameters
                {
                    m_MaxSpeed = 277.77777f,
                    m_WalkSpeed = 1.6666667f,
                    m_Weights = new PathfindWeights(0.1f, 0.1f, 0.1f, 0.2f),
                    m_Methods = PathMethod.Pedestrian | PathMethod.PublicTransportDay
                              | PathMethod.PublicTransportNight | PathMethod.Taxi
                              | PathMethod.Track | PathMethod.Road | PathMethod.MediumRoad
                              | PathMethod.Offroad | PathMethod.Flying,
                    m_TaxiIgnoredRules = VehicleUtils.GetIgnoredPathfindRulesTaxiDefaults(),
                    m_PathfindFlags = PathfindFlags.IgnoreFlow | PathfindFlags.Simplified | PathfindFlags.IgnorePath
                };

                var origin = new SetupQueueTarget
                {
                    m_Type = SetupTargetType.CurrentLocation,
                    m_Entity = originEntity
                };
                TransitOriginMode originMode;
                if (isTrain)
                {
                    origin.m_Methods = PathMethod.Pedestrian | PathMethod.Track;
                    origin.m_TrackTypes = TrackTypes.Train;
                    originMode = TransitOriginMode.Train;
                }
                else if (isShip)
                {
                    // Ship SpawnLocations use Road/Cargo/Offroad-flavored ConnectionType
                    // with RoadTypes.Watercraft. Cover all three so the seeker accepts.
                    origin.m_Methods = PathMethod.Pedestrian | PathMethod.Road
                                     | PathMethod.MediumRoad | PathMethod.Offroad;
                    origin.m_RoadTypes = RoadTypes.Watercraft;
                    originMode = TransitOriginMode.Ship;
                }
                else // isAir
                {
                    // Air OCs are aircraft route endpoints; the raw OC airway anchor
                    // can't bridge into the passenger route graph, so we prefer the
                    // connected route waypoint when an airplane line is wired up.
                    parameters.m_PathfindFlags &= ~PathfindFlags.Simplified;
                    origin.m_Methods = PathMethod.Pedestrian | PathMethod.Road;
                    origin.m_RoadTypes = RoadTypes.Airplane;

                    if (TryGetAirRouteWaypoint(
                            originEntity,
                            ref connectedRouteLookup,
                            ref routeLaneLookup,
                            ref transportStopLookup,
                            ref takeoffLocationLookup,
                            ref connectedLookup,
                            ref boardingVehicleLookup,
                            ref publicTransportLookup,
                            out Entity routeWaypoint,
                            out originMode))
                    {
                        origin.m_Entity = routeWaypoint;
                        origin.m_Methods |= PathMethod.Flying;
                        origin.m_FlyingTypes = RoadTypes.Airplane;
                        if (originMode == TransitOriginMode.AirTransportStopActive)
                            airStopActiveOrigin++;
                        else if (originMode == TransitOriginMode.AirTransportStopIdle)
                            airStopIdleOrigin++;
                        else if (originMode == TransitOriginMode.AirTakeoffLocation)
                            airTakeoffOrigin++;
                        else
                            airRouteOrigin++;
                    }
                    else
                    {
                        originMode = TransitOriginMode.AirDirectOutsideConnection;
                        airDirectOrigin++;
                    }
                }
                var destination = new SetupQueueTarget
                {
                    m_Type = SetupTargetType.TouristFindTarget,
                    m_Methods = PathMethod.Pedestrian,
                    m_Entity = household
                };
                PathUtils.UpdateOwnedVehicleMethods(household, ref ownedVehicleLookup, ref parameters, ref origin, ref destination);

                m_PendingPaths[household] = new PendingTransitPath
                {
                    Type = ocType,
                    OriginMode = originMode
                };
                queue.Enqueue(new SetupQueueItem(household, parameters, origin, destination));
                ecb.AddComponent(household, new PathInformation { m_State = PathFlags.Pending });
                if (isTrain) submittedTrain++;
                else if (isAir) submittedAir++;
                else submittedShip++;
            }

            ecb.Playback(EntityManager);
            m_PathfindSetupSystem.AddQueueWriter(Dependency);

            int totalSubmitted = submittedTrain + submittedAir + submittedShip;
            if (totalSubmitted > 0 || skippedNotTrain > 0)
            {
                log.Info($"[CT-TTA] eligible={households.Length} submitted[Train={submittedTrain} Air={submittedAir} Ship={submittedShip}] skippedRoad={skippedNotTrain} skippedNoOrigin={skippedNoOrigin}");
                if (submittedAir > 0)
                    log.Info($"[CT-TTA] airOrigins stopActive={airStopActiveOrigin} stopIdle={airStopIdleOrigin} takeoffLocation={airTakeoffOrigin} routeWaypoint={airRouteOrigin} directOC={airDirectOrigin}");
            }
        }

        private static bool TryGetAirRouteWaypoint(
            Entity outsideConnection,
            ref BufferLookup<Game.Routes.ConnectedRoute> connectedRouteLookup,
            ref ComponentLookup<Game.Routes.RouteLane> routeLaneLookup,
            ref ComponentLookup<Game.Routes.TransportStop> transportStopLookup,
            ref ComponentLookup<Game.Routes.TakeoffLocation> takeoffLocationLookup,
            ref ComponentLookup<Game.Routes.Connected> connectedLookup,
            ref ComponentLookup<Game.Routes.BoardingVehicle> boardingVehicleLookup,
            ref ComponentLookup<Game.Vehicles.PublicTransport> publicTransportLookup,
            out Entity waypoint,
            out TransitOriginMode originMode)
        {
            // For Air OCs the actual passenger-boarding entity lives at
            // Connected[waypoint].m_Connected, not at the waypoint itself — the route
            // waypoint is just a node on the route graph; the entity carrying
            // BoardingVehicle (and typically TransportStop) is the one m_Connected
            // points at. Same shape as how TransportAircraftAISystem.StartBoarding
            // dereferences `Connected.m_Connected` before calling BeginBoarding.
            waypoint = Entity.Null;
            originMode = TransitOriginMode.Unknown;
            if (!connectedRouteLookup.HasBuffer(outsideConnection))
                return false;

            Entity takeoffEntity = Entity.Null;
            Entity boardingEntity = Entity.Null;
            Entity rawRouteWaypoint = Entity.Null;
            var routes = connectedRouteLookup[outsideConnection];
            for (int i = 0; i < routes.Length; i++)
            {
                Entity candidate = routes[i].m_Waypoint;
                if (!routeLaneLookup.HasComponent(candidate))
                    continue;

                var routeLane = routeLaneLookup[candidate];
                if (routeLane.m_StartLane == Entity.Null && routeLane.m_EndLane == Entity.Null)
                    continue;

                if (rawRouteWaypoint == Entity.Null)
                    rawRouteWaypoint = candidate;

                Entity connected = Entity.Null;
                if (connectedLookup.HasComponent(candidate))
                    connected = connectedLookup[candidate].m_Connected;

                if (connected != Entity.Null && boardingVehicleLookup.HasComponent(connected))
                {
                    TransitOriginMode stopMode = IsBoardingVehicleActive(connected, ref boardingVehicleLookup, ref publicTransportLookup)
                        ? TransitOriginMode.AirTransportStopActive
                        : TransitOriginMode.AirTransportStopIdle;

                    if (transportStopLookup.HasComponent(connected))
                    {
                        waypoint = connected;
                        originMode = stopMode;
                        return true;
                    }
                    if (boardingEntity == Entity.Null)
                    {
                        boardingEntity = connected;
                        originMode = stopMode;
                    }
                }

                if (takeoffLocationLookup.HasComponent(candidate) && takeoffEntity == Entity.Null)
                    takeoffEntity = candidate;
            }

            if (boardingEntity != Entity.Null)
            {
                waypoint = boardingEntity;
                return true;
            }
            if (takeoffEntity != Entity.Null)
            {
                waypoint = takeoffEntity;
                originMode = TransitOriginMode.AirTakeoffLocation;
                return true;
            }
            if (rawRouteWaypoint != Entity.Null)
            {
                waypoint = rawRouteWaypoint;
                originMode = TransitOriginMode.AirRouteWaypoint;
                return true;
            }
            return false;
        }

        private static bool IsBoardingVehicleActive(
            Entity boardingEntity,
            ref ComponentLookup<Game.Routes.BoardingVehicle> boardingVehicleLookup,
            ref ComponentLookup<Game.Vehicles.PublicTransport> publicTransportLookup)
        {
            var boardingVehicle = boardingVehicleLookup[boardingEntity];
            Entity vehicle = boardingVehicle.m_Vehicle;
            if (vehicle == Entity.Null || !publicTransportLookup.HasComponent(vehicle))
                return false;

            var publicTransport = publicTransportLookup[vehicle];
            return (publicTransport.m_State & PublicTransportFlags.Boarding) != 0;
        }

        private void ProcessCompletedPathResults(
            ref BufferLookup<HouseholdCitizen> citizenBufLookup,
            ref ComponentLookup<CurrentBuilding> currentBuildingLookup,
            ref ComponentLookup<PrefabRef> prefabRefLookup,
            ref ComponentLookup<OutsideConnectionData> ocDataLookup,
            ref ComponentLookup<PathInformation> pathInfoLookup,
            ref ComponentLookup<TouristHousehold> touristLookup,
            ref ComponentLookup<LodgingProvider> lodgingProviderLookup,
            ref BufferLookup<Renter> renterLookup)
        {
            using var results = m_ResultQuery.ToEntityArray(Allocator.Temp);
            if (results.Length == 0)
                return;

            int trainOk = 0, trainFail = 0;
            int airOk = 0, airFail = 0;
            int shipOk = 0, shipFail = 0;
            int otherOk = 0, otherFail = 0;
            int airStopActiveOk = 0, airStopActiveFail = 0;
            int airStopIdleOk = 0, airStopIdleFail = 0;
            int airTakeoffOk = 0, airTakeoffFail = 0;
            int airRouteOk = 0, airRouteFail = 0;
            int airDirectOk = 0, airDirectFail = 0;
            int airFailedReservedHotel = 0, airFailedAssignedAttraction = 0, airFailedNoTarget = 0;
            using var rescueEcb = new EntityCommandBuffer(Allocator.Temp);

            var attractionDataLookup = GetComponentLookup<AttractionData>(true);
            using var allBuildings = m_AttractionBuildingQuery.ToEntityArray(Allocator.Temp);
            var attractions = new NativeList<Entity>(64, Allocator.Temp);
            for (int b = 0; b < allBuildings.Length; b++)
            {
                Entity building = allBuildings[b];
                if (!prefabRefLookup.HasComponent(building))
                    continue;
                Entity prefab = prefabRefLookup[building].m_Prefab;
                if (attractionDataLookup.HasComponent(prefab))
                    attractions.Add(building);
            }

            NativeQueue<AddMeetingSystem.AddMeeting> meetingQueue = m_AddMeetingSystem.GetMeetingQueue(out var meetingDeps);
            meetingDeps.Complete();

            for (int i = 0; i < results.Length; i++)
            {
                Entity household = results[i];
                PathInformation path = pathInfoLookup[household];
                if ((path.m_State & PathFlags.Pending) != 0)
                    continue;

                PendingTransitPath pending;
                bool hasPendingOrigin = m_PendingPaths.TryGetValue(household, out pending);
                if (!hasPendingOrigin)
                    continue;

                OutsideConnectionTransferType type = pending.Type;
                bool ok = path.m_Destination != Entity.Null;

                if ((type & OutsideConnectionTransferType.Train) != 0)
                {
                    if (ok) trainOk++; else trainFail++;
                }
                else if ((type & OutsideConnectionTransferType.Air) != 0)
                {
                    if (ok) airOk++; else airFail++;

                    if (hasPendingOrigin)
                    {
                        if (pending.OriginMode == TransitOriginMode.AirTransportStopActive)
                        {
                            if (ok) airStopActiveOk++; else airStopActiveFail++;
                        }
                        else if (pending.OriginMode == TransitOriginMode.AirTransportStopIdle)
                        {
                            if (ok) airStopIdleOk++; else airStopIdleFail++;
                        }
                        else if (pending.OriginMode == TransitOriginMode.AirTakeoffLocation)
                        {
                            if (ok) airTakeoffOk++; else airTakeoffFail++;
                        }
                        else if (pending.OriginMode == TransitOriginMode.AirRouteWaypoint)
                        {
                            if (ok) airRouteOk++; else airRouteFail++;
                        }
                        else if (pending.OriginMode == TransitOriginMode.AirDirectOutsideConnection)
                        {
                            if (ok) airDirectOk++; else airDirectFail++;
                        }
                    }

                    if (!ok && hasPendingOrigin)
                    {
                        // 50/50 between hotel and attraction. Vanilla's split is
                        // pathfind-score driven, not a fixed ratio — this is a
                        // workable approximation. Each path tries its alternate if
                        // the first is unavailable (no rooms / no attractions).
                        bool preferHotel = (NextFallbackRoll() & 1u) == 0u;
                        bool assigned = false;
                        if (preferHotel)
                        {
                            if (TryReserveHotelForFailedAirTourist(household, ref touristLookup, ref lodgingProviderLookup, ref renterLookup, rescueEcb))
                            {
                                airFailedReservedHotel++;
                                assigned = true;
                            }
                            else if (TryAssignAttractionForFailedAirTourist(household, attractions, meetingQueue, rescueEcb))
                            {
                                airFailedAssignedAttraction++;
                                assigned = true;
                            }
                        }
                        else
                        {
                            if (TryAssignAttractionForFailedAirTourist(household, attractions, meetingQueue, rescueEcb))
                            {
                                airFailedAssignedAttraction++;
                                assigned = true;
                            }
                            else if (TryReserveHotelForFailedAirTourist(household, ref touristLookup, ref lodgingProviderLookup, ref renterLookup, rescueEcb))
                            {
                                airFailedReservedHotel++;
                                assigned = true;
                            }
                        }
                        if (!assigned)
                            airFailedNoTarget++;
                    }
                }
                else if ((type & OutsideConnectionTransferType.Ship) != 0)
                {
                    if (ok) shipOk++; else shipFail++;
                }
                else
                {
                    if (ok) otherOk++; else otherFail++;
                }

                m_PendingPaths.Remove(household);
            }

            rescueEcb.Playback(EntityManager);
            attractions.Dispose();

            int total = trainOk + trainFail + airOk + airFail + shipOk + shipFail + otherOk + otherFail;
            if (total == 0)
                return;

            log.Info(
                $"[CT-TTA] results total={total} " +
                $"ok[Train={trainOk} Air={airOk} Ship={shipOk} Other={otherOk}] " +
                $"fail[Train={trainFail} Air={airFail} Ship={shipFail} Other={otherFail}] " +
                $"airModes ok[stopActive={airStopActiveOk} stopIdle={airStopIdleOk} takeoff={airTakeoffOk} route={airRouteOk} direct={airDirectOk}] " +
                $"fail[stopActive={airStopActiveFail} stopIdle={airStopIdleFail} takeoff={airTakeoffFail} route={airRouteFail} direct={airDirectFail}] " +
                $"airFallback hotel={airFailedReservedHotel} attraction={airFailedAssignedAttraction} noTarget={airFailedNoTarget} " +
                $"pendingTracked={m_PendingPaths.Count}");
        }

        private uint NextFallbackRoll()
        {
            uint x = m_FallbackRngState;
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 5;
            m_FallbackRngState = x;
            return x;
        }

        private bool TryAssignAttractionForFailedAirTourist(
            Entity household,
            NativeList<Entity> attractions,
            NativeQueue<AddMeetingSystem.AddMeeting> meetingQueue,
            EntityCommandBuffer ecb)
        {
            if (attractions.Length == 0)
                return false;

            Entity attraction = attractions[(int)(NextFallbackRoll() % (uint)attractions.Length)];

            meetingQueue.Enqueue(new AddMeetingSystem.AddMeeting
            {
                m_Household = household,
                m_Type = LeisureType.Attractions
            });

            if (EntityManager.HasComponent<PathInformation>(household))
                ecb.RemoveComponent<PathInformation>(household);
            if (EntityManager.HasComponent<LodgingSeeker>(household))
                ecb.RemoveComponent<LodgingSeeker>(household);
            if (!EntityManager.HasComponent<Target>(household))
                ecb.AddComponent(household, new Target(attraction));

            return true;
        }

        private bool TryReserveHotelForFailedAirTourist(
            Entity household,
            ref ComponentLookup<TouristHousehold> touristLookup,
            ref ComponentLookup<LodgingProvider> lodgingProviderLookup,
            ref BufferLookup<Renter> renterLookup,
            EntityCommandBuffer ecb)
        {
            if (!touristLookup.HasComponent(household))
                return false;

            using var hotels = m_HotelQuery.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < hotels.Length; i++)
            {
                Entity hotel = hotels[i];
                if (!lodgingProviderLookup.HasComponent(hotel) || !renterLookup.HasBuffer(hotel))
                    continue;

                LodgingProvider provider = lodgingProviderLookup[hotel];
                if (provider.m_FreeRooms <= 0)
                    continue;

                provider.m_FreeRooms--;
                lodgingProviderLookup[hotel] = provider;
                renterLookup[hotel].Add(new Renter { m_Renter = household });

                TouristHousehold tourist = touristLookup[household];
                tourist.m_Hotel = hotel;
                touristLookup[household] = tourist;

                if (EntityManager.HasComponent<PathInformation>(household))
                    ecb.RemoveComponent<PathInformation>(household);
                if (EntityManager.HasComponent<LodgingSeeker>(household))
                    ecb.RemoveComponent<LodgingSeeker>(household);
                if (!EntityManager.HasComponent<Target>(household))
                    ecb.AddComponent(household, new Target(hotel));

                return true;
            }

            return false;
        }

        private void CleanupPendingPathTags(
            ref ComponentLookup<PathInformation> pathInfoLookup,
            ref ComponentLookup<TouristHousehold> touristLookup)
        {
            if (m_PendingPaths.Count == 0)
                return;

            m_PendingCleanup.Clear();
            foreach (var pair in m_PendingPaths)
            {
                Entity household = pair.Key;
                if (!touristLookup.HasComponent(household) || !pathInfoLookup.HasComponent(household))
                    m_PendingCleanup.Add(household);
            }

            if (m_PendingCleanup.Count == 0)
                return;

            for (int i = 0; i < m_PendingCleanup.Count; i++)
                m_PendingPaths.Remove(m_PendingCleanup[i]);

            log.Info($"[CT-TTA] pending cleanup removed={m_PendingCleanup.Count} remaining={m_PendingPaths.Count}");
        }

        private static OutsideConnectionTransferType ResolveOriginType(
            Entity household,
            ref BufferLookup<HouseholdCitizen> citizenBufLookup,
            ref ComponentLookup<CurrentBuilding> currentBuildingLookup,
            ref ComponentLookup<PrefabRef> prefabRefLookup,
            ref ComponentLookup<OutsideConnectionData> ocDataLookup)
        {
            if (!citizenBufLookup.HasBuffer(household))
                return OutsideConnectionTransferType.None;

            var citizens = citizenBufLookup[household];
            for (int i = 0; i < citizens.Length; i++)
            {
                Entity citizen = citizens[i].m_Citizen;
                if (!currentBuildingLookup.HasComponent(citizen))
                    continue;

                Entity building = currentBuildingLookup[citizen].m_CurrentBuilding;
                if (!prefabRefLookup.HasComponent(building))
                    continue;

                Entity prefab = prefabRefLookup[building].m_Prefab;
                if (ocDataLookup.HasComponent(prefab))
                    return ocDataLookup[prefab].m_Type;
            }

            return OutsideConnectionTransferType.None;
        }
    }
}

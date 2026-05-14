using Colossal.Logging;
using Game;
using Game.Agents;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CustomizeIt.Systems
{
    public partial class TouristBoostSystem : GameSystemBase
    {
        private static readonly ILog log = Mod.log;

        private CitySystem m_CitySystem;
        private CityStatisticsSystem m_CityStatisticsSystem;
        private SimulationSystem m_SimulationSystem;
        private EndFrameBarrier m_EndFrameBarrier;
        private TouristSpawnSystem m_VanillaSpawnSystem;
        private PrefabSystem m_PrefabSystem;
        private int m_LastDelta;

        private EntityQuery m_HouseholdPrefabQuery;
        private EntityQuery m_OutsideConnectionQuery;
        private EntityQuery m_TouristHouseholdQuery;
        private EntityQuery m_AllTouristQuery;
        private EntityQuery m_DemandParameterQuery;

        private bool m_OCsLogged;
        private int m_CensusTickCounter;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_CitySystem = World.GetOrCreateSystemManaged<CitySystem>();
            m_CityStatisticsSystem = World.GetOrCreateSystemManaged<CityStatisticsSystem>();
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
            m_EndFrameBarrier = World.GetOrCreateSystemManaged<EndFrameBarrier>();
            m_VanillaSpawnSystem = World.GetExistingSystemManaged<TouristSpawnSystem>();
            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            // Same queries the game's TouristSpawnSystem uses
            m_HouseholdPrefabQuery = GetEntityQuery(
                ComponentType.ReadOnly<ArchetypeData>(),
                ComponentType.ReadOnly<HouseholdData>());

            m_OutsideConnectionQuery = GetEntityQuery(
                ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
                ComponentType.Exclude<Game.Objects.ElectricityOutsideConnection>(),
                ComponentType.Exclude<Game.Objects.WaterPipeOutsideConnection>(),
                ComponentType.Exclude<Temp>(),
                ComponentType.Exclude<Deleted>());

            m_TouristHouseholdQuery = GetEntityQuery(
                ComponentType.ReadOnly<TouristHousehold>(),
                ComponentType.ReadOnly<Household>(),
                ComponentType.Exclude<MovingAway>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>());

            m_AllTouristQuery = GetEntityQuery(
                ComponentType.ReadOnly<TouristHousehold>(),
                ComponentType.ReadOnly<Household>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>());

            m_DemandParameterQuery = GetEntityQuery(ComponentType.ReadOnly<DemandParameterData>());

            RequireForUpdate(m_HouseholdPrefabQuery);
            RequireForUpdate(m_OutsideConnectionQuery);

            log.Info("TouristBoostSystem created.");
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 64;
        }

        protected override void OnUpdate()
        {
            Setting setting = Mod.Setting;
            if (setting == null)
                return;

            int target = setting.TargetTouristCount;

            if (m_VanillaSpawnSystem != null)
                m_VanillaSpawnSystem.Enabled = target <= 0;

            if (target <= 0)
            {
                m_LastDelta = 0;
                return;
            }

            int agg = math.clamp(setting.Aggressiveness, 1, 10);

            int statCurrent = m_CityStatisticsSystem.GetStatisticValue(
                Game.City.StatisticType.TouristCount);

            int predictedCurrent = statCurrent + m_LastDelta;
            int diff = target - predictedCurrent;
            int absDiff = math.abs(diff);

            int deadband = math.max(50, target / 50);
            int thisDelta = 0;

            log.Info($"[CT-DBG] tick frame={m_SimulationSystem.frameIndex} target={target} stat={statCurrent} predicted={predictedCurrent} diff={diff} agg={agg} deadband={deadband}");

            if (diff > deadband)
            {
                int batch;
                if      (absDiff > 2000) batch = math.max(1, math.min(90, agg * agg));
                else if (absDiff > 1000) batch = math.max(1, agg * agg * 2 / 5);
                else if (absDiff > 200)  batch = math.max(1, agg * agg / 10);
                else                     batch = math.max(1, agg / 3);

                int count = math.min(diff, batch);
                SpawnTourists(count);
                thisDelta = count;
            }
            else if (diff < -deadband)
            {
                int batch;
                if      (absDiff > 1000) batch = math.max(1, agg * agg / 2);
                else if (absDiff > 200)  batch = math.max(1, agg * agg / 5);
                else                     batch = math.max(1, agg / 2);

                int count = math.min(absDiff, batch);
                DespawnTourists(count);
                thisDelta = -count;
            }

            m_LastDelta = thisDelta;

            if (++m_CensusTickCounter >= 2)
            {
                m_CensusTickCounter = 0;
                LogTouristCensus();
            }
        }

        private void SpawnTourists(int count)
        {
            using var prefabEntities = m_HouseholdPrefabQuery.ToEntityArray(Allocator.Temp);
            using var archetypes = m_HouseholdPrefabQuery.ToComponentDataArray<ArchetypeData>(Allocator.Temp);
            using var outsideConnectionsArr = m_OutsideConnectionQuery.ToEntityArray(Allocator.Temp);

            if (prefabEntities.Length == 0 || outsideConnectionsArr.Length == 0)
                return;

            // OC pick is filtered by OutsideConnectionData.m_Type via BuildingUtils, same
            // as vanilla TouristSpawnSystem — without this we'd land on cargo-only or
            // None-type connections and the resulting pathfind dies as TouristNoTarget.
            var prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            var ocDataLookup = GetComponentLookup<OutsideConnectionData>(true);

            Setting setting = Mod.Setting;
            float wR = math.max(0f, setting.RoadWeight);
            float wT = math.max(0f, setting.TrainWeight);
            float wA = math.max(0f, setting.AirWeight);
            float wS = math.max(0f, setting.ShipWeight);
            float wTotal = wR + wT + wA + wS;
            float4 ocSpawnParams = wTotal > 0f
                ? new float4(wR, wT, wA, wS) / wTotal
                : new float4(0.25f);

            // BuildingUtils.GetRandomOutsideConnectionByParameters expects a NativeList.
            var outsideConnections = new NativeList<Entity>(outsideConnectionsArr.Length, Allocator.Temp);
            for (int i = 0; i < outsideConnectionsArr.Length; i++)
                outsideConnections.Add(outsideConnectionsArr[i]);

            if (!m_OCsLogged)
            {
                LogOCInventory(outsideConnections, prefabRefLookup, ocDataLookup);
                LogHouseholdArchetypes(prefabEntities, archetypes);
                m_OCsLogged = true;
            }

            var commandBuffer = m_EndFrameBarrier.CreateCommandBuffer();
            var random = new Random((uint)(m_SimulationSystem.frameIndex + 1));

            int created = 0;
            int attempts = 0;
            int maxAttempts = count * 4;
            int pickRoad = 0, pickTrain = 0, pickAir = 0, pickShip = 0, pickNone = 0, pickOther = 0;

            while (created < count && attempts < maxAttempts)
            {
                attempts++;
                if (!BuildingUtils.GetRandomOutsideConnectionByParameters(
                        ref outsideConnections,
                        ref ocDataLookup,
                        ref prefabRefLookup,
                        random,
                        ocSpawnParams,
                        out var oc))
                {
                    continue;
                }

                OutsideConnectionTransferType pickedType = OutsideConnectionTransferType.None;
                if (prefabRefLookup.HasComponent(oc))
                {
                    Entity ocPrefab = prefabRefLookup[oc].m_Prefab;
                    if (ocDataLookup.HasComponent(ocPrefab))
                        pickedType = ocDataLookup[ocPrefab].m_Type;
                }
                if (pickedType == OutsideConnectionTransferType.None) pickNone++;
                else if ((pickedType & OutsideConnectionTransferType.Road) != 0) pickRoad++;
                else if ((pickedType & OutsideConnectionTransferType.Train) != 0) pickTrain++;
                else if ((pickedType & OutsideConnectionTransferType.Air) != 0) pickAir++;
                else if ((pickedType & OutsideConnectionTransferType.Ship) != 0) pickShip++;
                else pickOther++;

                int prefabIndex = random.NextInt(prefabEntities.Length);
                Entity prefab = prefabEntities[prefabIndex];
                EntityArchetype archetype = archetypes[prefabIndex].m_Archetype;

                Entity household = commandBuffer.CreateEntity(archetype);
                commandBuffer.SetComponent(household, new PrefabRef { m_Prefab = prefab });
                commandBuffer.SetComponent(household, new Household { m_Flags = HouseholdFlags.Tourist });
                commandBuffer.AddComponent(household, new TouristHousehold
                {
                    m_Hotel = Entity.Null,
                    m_LeavingTime = 0u
                });
                commandBuffer.AddComponent(household, new CurrentBuilding
                {
                    m_CurrentBuilding = oc
                });
                created++;
            }

            log.Info(
                $"[CT-DBG] spawn req={count} created={created} attempts={attempts} " +
                $"params=R{ocSpawnParams.x:F2}/T{ocSpawnParams.y:F2}/A{ocSpawnParams.z:F2}/S{ocSpawnParams.w:F2} " +
                $"picks[Road={pickRoad} Train={pickTrain} Air={pickAir} Ship={pickShip} None={pickNone} Other={pickOther}]");

            outsideConnections.Dispose();
            m_EndFrameBarrier.AddJobHandleForProducer(Dependency);
        }

        private void DespawnTourists(int count)
        {
            using var tourists = m_TouristHouseholdQuery.ToEntityArray(Allocator.Temp);

            if (tourists.Length == 0)
            {
                log.Info($"[CT-DBG] despawn req={count} created=0 (no eligible tourists)");
                return;
            }

            var commandBuffer = m_EndFrameBarrier.CreateCommandBuffer();

            int toRemove = math.min(count, tourists.Length);
            for (int i = 0; i < toRemove; i++)
            {
                commandBuffer.AddComponent(tourists[tourists.Length - 1 - i], new MovingAway
                {
                    m_Reason = MoveAwayReason.TouristNoTarget
                });
            }

            log.Info($"[CT-DBG] despawn req={count} marked={toRemove} pool={tourists.Length}");

            m_EndFrameBarrier.AddJobHandleForProducer(Dependency);
        }

        private void LogOCInventory(NativeList<Entity> ocs, ComponentLookup<PrefabRef> refs, ComponentLookup<OutsideConnectionData> ocData)
        {
            // Boarding-chain lookups. For an inbound passenger to board at an OC waypoint
            // the route AI (Train/Watercraft/Aircraft StartBoarding gate) requires the
            // waypoint to have Connected.m_Connected pointing to a BoardingVehicle entity.
            // Train/Ship OCs are observed to work; Air OCs do not. Logging the chain for
            // every OC pins the structural difference.
            var connectedRouteLookup = GetBufferLookup<Game.Routes.ConnectedRoute>(true);
            var connectedLookup = GetComponentLookup<Game.Routes.Connected>(true);
            var boardingVehicleLookup = GetComponentLookup<Game.Routes.BoardingVehicle>(true);

            int road = 0, train = 0, air = 0, ship = 0, none = 0;
            log.Info($"[CT-DBG] OC inventory ({ocs.Length} entities) ===");
            for (int i = 0; i < ocs.Length; i++)
            {
                Entity e = ocs[i];
                Entity prefab = refs.HasComponent(e) ? refs[e].m_Prefab : Entity.Null;
                OutsideConnectionTransferType t = ocData.HasComponent(prefab)
                    ? ocData[prefab].m_Type
                    : OutsideConnectionTransferType.None;

                string name = "<no prefab>";
                if (prefab != Entity.Null && m_PrefabSystem.TryGetPrefab(prefab, out PrefabBase pb) && pb != null)
                    name = pb.name;

                if (t == OutsideConnectionTransferType.None) none++;
                else
                {
                    if ((t & OutsideConnectionTransferType.Road) != 0) road++;
                    if ((t & OutsideConnectionTransferType.Train) != 0) train++;
                    if ((t & OutsideConnectionTransferType.Air) != 0) air++;
                    if ((t & OutsideConnectionTransferType.Ship) != 0) ship++;
                }

                int routeCount = 0, hasConnected = 0, hasBoardingVehicle = 0;
                if (connectedRouteLookup.HasBuffer(e))
                {
                    var routes = connectedRouteLookup[e];
                    routeCount = routes.Length;
                    for (int j = 0; j < routes.Length; j++)
                    {
                        Entity wp = routes[j].m_Waypoint;
                        if (connectedLookup.HasComponent(wp))
                        {
                            hasConnected++;
                            Entity connected = connectedLookup[wp].m_Connected;
                            if (boardingVehicleLookup.HasComponent(connected))
                                hasBoardingVehicle++;
                        }
                    }
                }

                log.Info($"[CT-DBG]   OC[{i}] entity={e.Index}:{e.Version} prefab='{name}' type={OCTypeString(t)} hasOcData={ocData.HasComponent(prefab)} routes={routeCount} wpConnected={hasConnected}/{routeCount} wpBoarding={hasBoardingVehicle}/{routeCount}");
            }
            log.Info($"[CT-DBG] OC totals Road={road} Train={train} Air={air} Ship={ship} None/Cargo-only={none}");
        }

        private void LogTouristCensus()
        {
            using var tourists = m_AllTouristQuery.ToEntityArray(Allocator.Temp);
            if (tourists.Length == 0)
            {
                log.Info("[CT-DBG] census total=0");
                return;
            }

            var thLookup = GetComponentLookup<TouristHousehold>(true);
            var movingAwayLookup = GetComponentLookup<MovingAway>(true);
            var lodgingLookup = GetComponentLookup<LodgingSeeker>(true);
            var householdCitizenLookup = GetBufferLookup<HouseholdCitizen>(true);
            var currentBuildingLookup = GetComponentLookup<CurrentBuilding>(true);
            var householdCurrentBuildingLookup = GetComponentLookup<CurrentBuilding>(true);
            var prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            var ocDataLookup = GetComponentLookup<OutsideConnectionData>(true);
            var ocLookup = GetComponentLookup<Game.Objects.OutsideConnection>(true);

            int withHotel = 0, lodging = 0, movingAway = 0;
            int atRoad = 0, atTrain = 0, atAir = 0, atShip = 0, atOcNone = 0;
            int inCity = 0, noCitizen = 0, noBuilding = 0, householdHasOC = 0;

            int dyingNoTarget = 0, dyingOther = 0;
            int dyingAtRoad = 0, dyingAtTrain = 0, dyingAtAir = 0, dyingAtShip = 0, dyingAtOther = 0;

            for (int i = 0; i < tourists.Length; i++)
            {
                Entity h = tourists[i];

                bool isDying = movingAwayLookup.HasComponent(h);
                if (isDying) movingAway++;
                if (lodgingLookup.HasComponent(h)) lodging++;
                if (thLookup.HasComponent(h) && thLookup[h].m_Hotel != Entity.Null) withHotel++;
                if (householdCurrentBuildingLookup.HasComponent(h)) householdHasOC++;

                if (isDying)
                {
                    var reason = movingAwayLookup[h].m_Reason;
                    if (reason == MoveAwayReason.TouristNoTarget) dyingNoTarget++;
                    else dyingOther++;
                }

                if (!householdCitizenLookup.HasBuffer(h)) { noCitizen++; continue; }
                var buf = householdCitizenLookup[h];
                if (buf.Length == 0) { noCitizen++; continue; }

                Entity citizen = buf[0].m_Citizen;

                if (!currentBuildingLookup.HasComponent(citizen)) { noBuilding++; continue; }
                Entity building = currentBuildingLookup[citizen].m_CurrentBuilding;
                if (building == Entity.Null) { noBuilding++; continue; }

                OutsideConnectionTransferType t = OutsideConnectionTransferType.None;
                bool isOC = ocLookup.HasComponent(building);

                if (isOC && prefabRefLookup.HasComponent(building))
                {
                    Entity prefab = prefabRefLookup[building].m_Prefab;
                    if (ocDataLookup.HasComponent(prefab))
                        t = ocDataLookup[prefab].m_Type;
                }

                if (!isOC)
                {
                    inCity++;
                }
                else if (t == OutsideConnectionTransferType.None) atOcNone++;
                else if ((t & OutsideConnectionTransferType.Road) != 0) atRoad++;
                else if ((t & OutsideConnectionTransferType.Train) != 0) atTrain++;
                else if ((t & OutsideConnectionTransferType.Air) != 0) atAir++;
                else if ((t & OutsideConnectionTransferType.Ship) != 0) atShip++;

                if (isDying && isOC)
                {
                    if ((t & OutsideConnectionTransferType.Road) != 0) dyingAtRoad++;
                    else if ((t & OutsideConnectionTransferType.Train) != 0) dyingAtTrain++;
                    else if ((t & OutsideConnectionTransferType.Air) != 0) dyingAtAir++;
                    else if ((t & OutsideConnectionTransferType.Ship) != 0) dyingAtShip++;
                    else dyingAtOther++;
                }
            }

            log.Info(
                $"[CT-DBG] census total={tourists.Length} withHotel={withHotel} lodgingSeeker={lodging} householdStillHasCB={householdHasOC} " +
                $"movingAway={movingAway} (NoTarget={dyingNoTarget} Other={dyingOther}) " +
                $"atOC[Road={atRoad} Train={atTrain} Air={atAir} Ship={atShip} None={atOcNone}] " +
                $"dyingAtOC[Road={dyingAtRoad} Train={dyingAtTrain} Air={dyingAtAir} Ship={dyingAtShip} Other={dyingAtOther}] " +
                $"inCity={inCity} noCitizen={noCitizen} noBuilding={noBuilding}");
        }

        private void LogHouseholdArchetypes(NativeArray<Entity> prefabEntities, NativeArray<ArchetypeData> archetypes)
        {
            log.Info($"[CT-DBG] household prefab archetypes ({prefabEntities.Length} entries) ===");
            int dumped = 0;
            for (int i = 0; i < prefabEntities.Length && dumped < 3; i++)
            {
                EntityArchetype arch = archetypes[i].m_Archetype;
                string prefabName = "<no name>";
                if (m_PrefabSystem.TryGetPrefab(prefabEntities[i], out PrefabBase pb) && pb != null)
                    prefabName = pb.name;

                using var types = arch.GetComponentTypes(Allocator.Temp);
                var sb = new System.Text.StringBuilder();
                for (int j = 0; j < types.Length; j++)
                {
                    if (j > 0) sb.Append(", ");
                    sb.Append(types[j].GetManagedType().Name);
                }
                log.Info($"[CT-DBG]   archetype[{i}] prefab='{prefabName}' types=[{sb}]");
                dumped++;
            }
            if (prefabEntities.Length > 3)
                log.Info($"[CT-DBG]   ... {prefabEntities.Length - 3} more not dumped");
        }

        private static string OCTypeString(OutsideConnectionTransferType t)
        {
            if (t == OutsideConnectionTransferType.None) return "None";
            var sb = new System.Text.StringBuilder();
            if ((t & OutsideConnectionTransferType.Road) != 0) sb.Append("Road");
            if ((t & OutsideConnectionTransferType.Train) != 0) { if (sb.Length > 0) sb.Append('|'); sb.Append("Train"); }
            if ((t & OutsideConnectionTransferType.Air) != 0) { if (sb.Length > 0) sb.Append('|'); sb.Append("Air"); }
            if ((t & OutsideConnectionTransferType.Ship) != 0) { if (sb.Length > 0) sb.Append('|'); sb.Append("Ship"); }
            if (sb.Length == 0) sb.Append($"raw=0x{(int)t:X}");
            return sb.ToString();
        }

        protected override void OnDestroy()
        {
            if (m_VanillaSpawnSystem != null)
                m_VanillaSpawnSystem.Enabled = true;
            base.OnDestroy();
        }
    }
}

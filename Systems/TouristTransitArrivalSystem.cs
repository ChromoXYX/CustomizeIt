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
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CustomizeIt.Systems
{
    // Vanilla starts every tourist with a Pedestrian-only pathfind from their OC.
    // Road OCs sit on a road and walk works; Train/Air/Ship OCs have no pedestrian
    // start lane, the pathfind returns nothing and the tourist gets TouristNoTarget.
    // We pre-assign those a hotel or attraction with vanilla's scoring formula
    // (SetupTouristTargetJob): hotel = -10*FreeRooms + min(Price, 500) - avail*0.01,
    // attraction weighted by road-edge attractiveness availability.
    [UpdateBefore(typeof(TouristFindTargetSystem))]
    public partial class TouristTransitArrivalSystem : GameSystemBase
    {
        private EntityQuery m_SeekerQuery;
        private EntityQuery m_HotelQuery;
        private EntityQuery m_AttractionQuery;
        private AddMeetingSystem m_AddMeetingSystem;
        private uint m_RngState = 0x9E3779B9u;

        private struct HotelCandidate
        {
            public Entity Company;
            public Entity Building;
            public int FreeRooms;
            public int Price;
            public float Availability;
        }

        private struct AttractionCandidate
        {
            public Entity Entity;
            public float Availability;
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            m_AddMeetingSystem = World.GetOrCreateSystemManaged<AddMeetingSystem>();

            m_SeekerQuery = GetEntityQuery(
                ComponentType.ReadOnly<TouristHousehold>(),
                ComponentType.ReadOnly<LodgingSeeker>(),
                ComponentType.Exclude<MovingAway>(),
                ComponentType.Exclude<Target>(),
                ComponentType.Exclude<PathInformation>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>());

            m_HotelQuery = GetEntityQuery(
                ComponentType.ReadWrite<LodgingProvider>(),
                ComponentType.ReadOnly<PropertyRenter>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>());

            m_AttractionQuery = GetEntityQuery(
                ComponentType.ReadOnly<AttractivenessProvider>(),
                ComponentType.Exclude<Deleted>(),
                ComponentType.Exclude<Temp>());
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

        protected override void OnUpdate()
        {
            using var households = m_SeekerQuery.ToEntityArray(Allocator.Temp);
            if (households.Length == 0)
                return;

            var citizenBufLookup = GetBufferLookup<HouseholdCitizen>(true);
            var currentBuildingLookup = GetComponentLookup<CurrentBuilding>(true);
            var prefabRefLookup = GetComponentLookup<PrefabRef>(true);
            var ocDataLookup = GetComponentLookup<OutsideConnectionData>(true);
            var touristLookup = GetComponentLookup<TouristHousehold>(false);
            var lodgingProviderLookup = GetComponentLookup<LodgingProvider>(false);
            var renterLookup = GetBufferLookup<Renter>(false);
            var attractivenessLookup = GetComponentLookup<AttractivenessProvider>(true);
            var propertyRenterLookup = GetComponentLookup<PropertyRenter>(true);
            var buildingLookup = GetComponentLookup<Game.Buildings.Building>(true);
            var resourceAvailLookup = GetBufferLookup<ResourceAvailability>(true);

            using var ecb = new EntityCommandBuffer(Allocator.Temp);

            var hotels = CollectHotels(ref lodgingProviderLookup, ref propertyRenterLookup,
                ref buildingLookup, ref resourceAvailLookup);
            var attractions = CollectAttractions(ref attractivenessLookup,
                ref buildingLookup, ref resourceAvailLookup);

            int attractionWeightTotal = 0;
            for (int i = 0; i < attractions.Length; i++)
                attractionWeightTotal += AttractionWeight(attractions[i]);

            NativeQueue<AddMeetingSystem.AddMeeting> meetingQueue = default;
            bool meetingQueueReady = false;

            for (int i = 0; i < households.Length; i++)
            {
                Entity household = households[i];

                if (!citizenBufLookup.HasBuffer(household)) continue;
                var citizens = citizenBufLookup[household];
                Entity origin = Entity.Null;
                for (int j = 0; j < citizens.Length; j++)
                {
                    Entity citizen = citizens[j].m_Citizen;
                    if (currentBuildingLookup.HasComponent(citizen))
                        origin = currentBuildingLookup[citizen].m_CurrentBuilding;
                }
                if (origin == Entity.Null) continue;

                if (!prefabRefLookup.HasComponent(origin)) continue;
                Entity prefab = prefabRefLookup[origin].m_Prefab;
                if (!ocDataLookup.HasComponent(prefab)) continue;

                var ocType = ocDataLookup[prefab].m_Type;
                if ((ocType & OutsideConnectionTransferType.Road) != 0) continue;
                if ((ocType & (OutsideConnectionTransferType.Train
                             | OutsideConnectionTransferType.Air
                             | OutsideConnectionTransferType.Ship)) == 0) continue;

                int hotelIdx = PickBestHotel(hotels);
                if (hotelIdx >= 0)
                {
                    var picked = hotels[hotelIdx];
                    ReserveHotel(household, picked, ref touristLookup, ref lodgingProviderLookup, ref renterLookup, ecb);
                    picked.FreeRooms--;
                    if (picked.FreeRooms <= 0)
                        hotels.RemoveAtSwapBack(hotelIdx);
                    else
                        hotels[hotelIdx] = picked;
                }
                else if (attractions.Length > 0)
                {
                    if (!meetingQueueReady)
                    {
                        meetingQueue = m_AddMeetingSystem.GetMeetingQueue(out var deps);
                        deps.Complete();
                        meetingQueueReady = true;
                    }
                    Entity attraction = PickWeightedAttraction(attractions, attractionWeightTotal);
                    AssignAttraction(household, attraction, meetingQueue, ecb);
                }
                else
                {
                    ecb.AddComponent(household, new MovingAway { m_Reason = MoveAwayReason.TouristNoTarget });
                }
            }

            ecb.Playback(EntityManager);
            hotels.Dispose();
            attractions.Dispose();
        }

        private NativeList<HotelCandidate> CollectHotels(
            ref ComponentLookup<LodgingProvider> lodgingProviderLookup,
            ref ComponentLookup<PropertyRenter> propertyRenterLookup,
            ref ComponentLookup<Game.Buildings.Building> buildingLookup,
            ref BufferLookup<ResourceAvailability> resourceAvailLookup)
        {
            using var entities = m_HotelQuery.ToEntityArray(Allocator.Temp);
            var list = new NativeList<HotelCandidate>(entities.Length, Allocator.Temp);
            for (int i = 0; i < entities.Length; i++)
            {
                Entity company = entities[i];
                if (!lodgingProviderLookup.HasComponent(company)) continue;
                var provider = lodgingProviderLookup[company];
                if (provider.m_FreeRooms <= 0) continue;

                if (!propertyRenterLookup.HasComponent(company)) continue;
                Entity building = propertyRenterLookup[company].m_Property;
                if (building == Entity.Null) continue;
                if (!buildingLookup.HasComponent(building)) continue;

                var b = buildingLookup[building];
                if (BuildingUtils.CheckOption(b, BuildingOption.Inactive)) continue;

                list.Add(new HotelCandidate
                {
                    Company = company,
                    Building = building,
                    FreeRooms = provider.m_FreeRooms,
                    Price = provider.m_Price,
                    Availability = GetAttractivenessAvailability(b, ref resourceAvailLookup)
                });
            }
            return list;
        }

        private NativeList<AttractionCandidate> CollectAttractions(
            ref ComponentLookup<AttractivenessProvider> attractivenessLookup,
            ref ComponentLookup<Game.Buildings.Building> buildingLookup,
            ref BufferLookup<ResourceAvailability> resourceAvailLookup)
        {
            using var entities = m_AttractionQuery.ToEntityArray(Allocator.Temp);
            var list = new NativeList<AttractionCandidate>(entities.Length, Allocator.Temp);
            for (int i = 0; i < entities.Length; i++)
            {
                Entity attraction = entities[i];
                if (!attractivenessLookup.HasComponent(attraction)) continue;
                if (!buildingLookup.HasComponent(attraction)) continue;

                var b = buildingLookup[attraction];
                if (BuildingUtils.CheckOption(b, BuildingOption.Inactive)) continue;

                list.Add(new AttractionCandidate
                {
                    Entity = attraction,
                    Availability = GetAttractivenessAvailability(b, ref resourceAvailLookup)
                });
            }
            return list;
        }

        private static float GetAttractivenessAvailability(
            Game.Buildings.Building building,
            ref BufferLookup<ResourceAvailability> resourceAvailLookup)
        {
            if (building.m_RoadEdge == Entity.Null) return 0f;
            if (!resourceAvailLookup.HasBuffer(building.m_RoadEdge)) return 0f;
            return NetUtils.GetAvailability(
                resourceAvailLookup[building.m_RoadEdge],
                AvailableResource.Attractiveness,
                building.m_CurvePosition);
        }

        private static int PickBestHotel(NativeList<HotelCandidate> hotels)
        {
            if (hotels.Length == 0) return -1;
            int bestIdx = 0;
            float bestScore = HotelScore(hotels[0]);
            for (int i = 1; i < hotels.Length; i++)
            {
                float score = HotelScore(hotels[i]);
                if (score < bestScore)
                {
                    bestScore = score;
                    bestIdx = i;
                }
            }
            return bestIdx;
        }

        private static float HotelScore(HotelCandidate h)
        {
            return -10f * h.FreeRooms + math.min(h.Price, 500f) - h.Availability * 0.01f;
        }

        private static int AttractionWeight(AttractionCandidate a)
        {
            return math.max(1, (int)a.Availability);
        }

        private Entity PickWeightedAttraction(NativeList<AttractionCandidate> attractions, int totalWeight)
        {
            if (totalWeight <= 0)
                return attractions[(int)(NextRoll() % (uint)attractions.Length)].Entity;

            int pick = (int)(NextRoll() % (uint)totalWeight);
            int cumulative = 0;
            for (int i = 0; i < attractions.Length; i++)
            {
                cumulative += AttractionWeight(attractions[i]);
                if (pick < cumulative)
                    return attractions[i].Entity;
            }
            return attractions[attractions.Length - 1].Entity;
        }

        private void ReserveHotel(
            Entity household, HotelCandidate picked,
            ref ComponentLookup<TouristHousehold> touristLookup,
            ref ComponentLookup<LodgingProvider> lodgingProviderLookup,
            ref BufferLookup<Renter> renterLookup,
            EntityCommandBuffer ecb)
        {
            var provider = lodgingProviderLookup[picked.Company];
            provider.m_FreeRooms--;
            lodgingProviderLookup[picked.Company] = provider;

            if (renterLookup.HasBuffer(picked.Company))
                renterLookup[picked.Company].Add(new Renter { m_Renter = household });

            if (touristLookup.HasComponent(household))
            {
                var tourist = touristLookup[household];
                tourist.m_Hotel = picked.Company;
                touristLookup[household] = tourist;
            }

            ecb.RemoveComponent<LodgingSeeker>(household);
            ecb.AddComponent(household, new Target(picked.Building));
        }

        private void AssignAttraction(
            Entity household, Entity attraction,
            NativeQueue<AddMeetingSystem.AddMeeting> meetingQueue,
            EntityCommandBuffer ecb)
        {
            meetingQueue.Enqueue(new AddMeetingSystem.AddMeeting
            {
                m_Household = household,
                m_Type = LeisureType.Attractions
            });
            ecb.RemoveComponent<LodgingSeeker>(household);
            ecb.AddComponent(household, new Target(attraction));
        }

        private uint NextRoll()
        {
            uint x = m_RngState;
            x ^= x << 13;
            x ^= x >> 17;
            x ^= x << 5;
            m_RngState = x;
            return x;
        }
    }
}

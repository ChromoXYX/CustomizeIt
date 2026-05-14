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

        private CityStatisticsSystem m_CityStatisticsSystem;
        private SimulationSystem m_SimulationSystem;
        private EndFrameBarrier m_EndFrameBarrier;
        private TouristSpawnSystem m_VanillaSpawnSystem;
        private int m_LastDelta;

        private EntityQuery m_HouseholdPrefabQuery;
        private EntityQuery m_OutsideConnectionQuery;
        private EntityQuery m_TouristHouseholdQuery;
        private EntityQuery m_DemandParameterQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_CityStatisticsSystem = World.GetOrCreateSystemManaged<CityStatisticsSystem>();
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
            m_EndFrameBarrier = World.GetOrCreateSystemManaged<EndFrameBarrier>();
            m_VanillaSpawnSystem = World.GetExistingSystemManaged<TouristSpawnSystem>();

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

            m_DemandParameterQuery = GetEntityQuery(ComponentType.ReadOnly<DemandParameterData>());

            RequireForUpdate(m_HouseholdPrefabQuery);
            RequireForUpdate(m_OutsideConnectionQuery);
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

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
            int statCurrent = m_CityStatisticsSystem.GetStatisticValue(Game.City.StatisticType.TouristCount);
            int predictedCurrent = statCurrent + m_LastDelta;
            int diff = target - predictedCurrent;
            int absDiff = math.abs(diff);

            int deadband = math.max(50, target / 50);
            int thisDelta = 0;

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
        }

        private void SpawnTourists(int count)
        {
            using var prefabEntities = m_HouseholdPrefabQuery.ToEntityArray(Allocator.Temp);
            using var archetypes = m_HouseholdPrefabQuery.ToComponentDataArray<ArchetypeData>(Allocator.Temp);
            using var outsideConnectionsArr = m_OutsideConnectionQuery.ToEntityArray(Allocator.Temp);

            if (prefabEntities.Length == 0 || outsideConnectionsArr.Length == 0)
                return;

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
                : GetVanillaOCSpawnParameters();

            var outsideConnections = new NativeList<Entity>(outsideConnectionsArr.Length, Allocator.Temp);
            for (int i = 0; i < outsideConnectionsArr.Length; i++)
                outsideConnections.Add(outsideConnectionsArr[i]);

            var commandBuffer = m_EndFrameBarrier.CreateCommandBuffer();
            var random = new Random((uint)(m_SimulationSystem.frameIndex + 1));

            int created = 0;
            int attempts = 0;
            int maxAttempts = count * 4;

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

            outsideConnections.Dispose();
            m_EndFrameBarrier.AddJobHandleForProducer(Dependency);
        }

        private void DespawnTourists(int count)
        {
            using var tourists = m_TouristHouseholdQuery.ToEntityArray(Allocator.Temp);
            if (tourists.Length == 0)
                return;

            var commandBuffer = m_EndFrameBarrier.CreateCommandBuffer();
            int toRemove = math.min(count, tourists.Length);
            for (int i = 0; i < toRemove; i++)
            {
                commandBuffer.AddComponent(tourists[tourists.Length - 1 - i], new MovingAway
                {
                    m_Reason = MoveAwayReason.TouristNoTarget
                });
            }

            m_EndFrameBarrier.AddJobHandleForProducer(Dependency);
        }

        private float4 GetVanillaOCSpawnParameters()
        {
            if (!m_DemandParameterQuery.IsEmptyIgnoreFilter)
                return m_DemandParameterQuery.GetSingleton<DemandParameterData>().m_TouristOCSpawnParameters;
            return new float4(0.1f, 0.1f, 0.5f, 0.3f);
        }

        protected override void OnDestroy()
        {
            if (m_VanillaSpawnSystem != null)
                m_VanillaSpawnSystem.Enabled = true;
            base.OnDestroy();
        }
    }
}

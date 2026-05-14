using Colossal.Serialization.Entities;
using Game;
using Game.Common;
using Game.Prefabs;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace CustomizeIt.Systems
{
    public partial class AttractivenessOverrideSystem : GameSystemBase
    {
        private PrefabSystem m_PrefabSystem;

        private readonly Dictionary<Entity, int> m_Overrides = new Dictionary<Entity, int>();

        protected override void OnCreate()
        {
            base.OnCreate();
            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            Enabled = false;
        }

        protected override void OnUpdate() { }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            if (mode != GameMode.Game)
                return;
            LoadAndApplyOverrides();
        }

        public void SetOverride(Entity prefabEntity, int attractiveness)
        {
            if (!EntityManager.HasComponent<AttractionData>(prefabEntity))
                return;

            EntityManager.SetComponentData(prefabEntity, new AttractionData
            {
                m_Attractiveness = attractiveness
            });

            if (!EntityManager.HasComponent<Updated>(prefabEntity))
                EntityManager.AddComponent<Updated>(prefabEntity);

            m_Overrides[prefabEntity] = attractiveness;
            SaveOverrides();
        }

        public bool RemoveOverride(Entity prefabEntity)
        {
            if (!m_Overrides.ContainsKey(prefabEntity))
                return false;

            if (m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase)
                && prefabBase.TryGet<Attraction>(out var attraction))
            {
                EntityManager.SetComponentData(prefabEntity, new AttractionData
                {
                    m_Attractiveness = attraction.m_Attractiveness
                });

                if (!EntityManager.HasComponent<Updated>(prefabEntity))
                    EntityManager.AddComponent<Updated>(prefabEntity);
            }

            m_Overrides.Remove(prefabEntity);
            SaveOverrides();
            return true;
        }

        public bool TryGetOverride(Entity prefabEntity, out int attractiveness)
        {
            return m_Overrides.TryGetValue(prefabEntity, out attractiveness);
        }

        public int GetBaseAttractiveness(Entity prefabEntity)
        {
            if (m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase)
                && prefabBase.TryGet<Attraction>(out var attraction))
            {
                return attraction.m_Attractiveness;
            }

            if (EntityManager.HasComponent<AttractionData>(prefabEntity))
            {
                return EntityManager.GetComponentData<AttractionData>(prefabEntity).m_Attractiveness;
            }

            return 0;
        }

        private void SaveOverrides()
        {
            Setting setting = Mod.Setting;
            if (setting == null)
                return;

            var names = new List<string>();
            var values = new List<int>();

            foreach (var kvp in m_Overrides)
            {
                if (m_PrefabSystem.TryGetPrefab(kvp.Key, out PrefabBase prefab))
                {
                    names.Add(prefab.name);
                    values.Add(kvp.Value);
                }
            }

            setting.OverridePrefabNames = names.ToArray();
            setting.OverrideValues = values.ToArray();
            setting.ApplyAndSave();
        }

        private void LoadAndApplyOverrides()
        {
            Setting setting = Mod.Setting;
            if (setting == null)
                return;

            string[] names = setting.OverridePrefabNames;
            int[] values = setting.OverrideValues;

            if (names == null || values == null || names.Length == 0 || names.Length != values.Length)
                return;

            var savedOverrides = new Dictionary<string, int>();
            for (int i = 0; i < names.Length; i++)
                savedOverrides[names[i]] = values[i];

            EntityQuery prefabQuery = GetEntityQuery(ComponentType.ReadOnly<AttractionData>());
            using NativeArray<Entity> entities = prefabQuery.ToEntityArray(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                if (m_PrefabSystem.TryGetPrefab(entities[i], out PrefabBase prefab)
                    && savedOverrides.TryGetValue(prefab.name, out int attractiveness))
                {
                    EntityManager.SetComponentData(entities[i], new AttractionData
                    {
                        m_Attractiveness = attractiveness
                    });
                    m_Overrides[entities[i]] = attractiveness;
                }
            }
        }
    }
}

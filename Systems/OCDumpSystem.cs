using Colossal.Logging;
using Game;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CustomizeIt.Systems
{
    public partial class OCDumpSystem : GameSystemBase
    {
        private static readonly ILog log = Mod.log;

        private EntityQuery m_OCQuery;
        private PrefabSystem m_PrefabSystem;
        private bool m_Done;
        private int m_WaitFrames;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();

            m_OCQuery = GetEntityQuery(
                ComponentType.ReadOnly<Game.Objects.OutsideConnection>(),
                ComponentType.Exclude<Game.Objects.ElectricityOutsideConnection>(),
                ComponentType.Exclude<Game.Objects.WaterPipeOutsideConnection>(),
                ComponentType.Exclude<Temp>(),
                ComponentType.Exclude<Deleted>());

            RequireForUpdate(m_OCQuery);
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 16;
        }

        protected override void OnUpdate()
        {
            if (m_Done) return;
            if (m_WaitFrames++ < 4) return;

            var ocs = m_OCQuery.ToEntityArray(Allocator.Temp);
            var prefabRef = GetComponentLookup<PrefabRef>(true);
            var ocData = GetComponentLookup<OutsideConnectionData>(true);
            var routeConnData = GetComponentLookup<RouteConnectionData>(true);
            var owner = GetComponentLookup<Owner>(true);
            var transform = GetComponentLookup<Game.Objects.Transform>(true);
            var connectedEdge = GetBufferLookup<ConnectedEdge>(true);
            var subLane = GetBufferLookup<Game.Net.SubLane>(true);
            var pedestrianLane = GetComponentLookup<Game.Net.PedestrianLane>(true);
            var carLane = GetComponentLookup<Game.Net.CarLane>(true);
            var trackLane = GetComponentLookup<Game.Net.TrackLane>(true);
            var connLane = GetComponentLookup<Game.Net.ConnectionLane>(true);
            var parkingLane = GetComponentLookup<Game.Net.ParkingLane>(true);

            int totalRoad = 0, totalTrain = 0, totalAir = 0, totalShip = 0, totalNone = 0;
            int totalEdgeBased = 0, totalRouteBased = 0, totalNeither = 0;
            int pedAnchorRoad = 0, pedAnchorTrain = 0, pedAnchorAir = 0, pedAnchorShip = 0;

            log.Info($"[CT-OCDUMP] === begin dump, ocs.Length={ocs.Length} ===");

            for (int i = 0; i < ocs.Length; i++)
            {
                Entity oc = ocs[i];

                string prefabName = "<unknown>";
                OutsideConnectionTransferType ocType = OutsideConnectionTransferType.None;
                float remoteness = 0f;
                if (prefabRef.HasComponent(oc))
                {
                    Entity p = prefabRef[oc].m_Prefab;
                    if (m_PrefabSystem.TryGetPrefab<PrefabBase>(p, out var pb) && pb != null)
                        prefabName = pb.name;
                    if (ocData.HasComponent(p))
                    {
                        ocType = ocData[p].m_Type;
                        remoteness = ocData[p].m_Remoteness;
                    }
                }

                if ((ocType & OutsideConnectionTransferType.Road) != 0) totalRoad++;
                else if ((ocType & OutsideConnectionTransferType.Train) != 0) totalTrain++;
                else if ((ocType & OutsideConnectionTransferType.Air) != 0) totalAir++;
                else if ((ocType & OutsideConnectionTransferType.Ship) != 0) totalShip++;
                else totalNone++;

                float3 pos = default;
                if (transform.HasComponent(oc)) pos = transform[oc].m_Position;

                Entity edgeOwner = Entity.Null;
                bool hasEdges = false;
                int edgeCount = 0;
                if (owner.HasComponent(oc))
                {
                    edgeOwner = owner[oc].m_Owner;
                    if (connectedEdge.HasBuffer(edgeOwner))
                    {
                        hasEdges = true;
                        edgeCount = connectedEdge[edgeOwner].Length;
                    }
                }

                bool hasRouteConn = false;
                RouteConnectionType accessType = default;
                RoadTypes accessRoad = default;
                TrackTypes accessTrack = default;
                if (prefabRef.HasComponent(oc))
                {
                    Entity p = prefabRef[oc].m_Prefab;
                    if (routeConnData.HasComponent(p))
                    {
                        hasRouteConn = true;
                        accessType = routeConnData[p].m_AccessConnectionType;
                        accessRoad = routeConnData[p].m_AccessRoadType;
                        accessTrack = routeConnData[p].m_AccessTrackType;
                    }
                }

                if (hasEdges) totalEdgeBased++;
                else if (hasRouteConn) totalRouteBased++;
                else totalNeither++;

                int subCount = subLane.HasBuffer(oc) ? subLane[oc].Length : 0;

                log.Info(
                    $"[CT-OCDUMP] oc={oc.Index}:{oc.Version} prefab=\"{prefabName}\" type={ocType} remote={remoteness:F2} pos=({pos.x:F0},{pos.y:F0},{pos.z:F0}) " +
                    $"edges={(hasEdges ? edgeCount.ToString() : "no")} routeConn={(hasRouteConn ? $"yes(access={accessType} road={accessRoad} track={accessTrack})" : "no")} subLanes={subCount}");

                bool foundPedAnchor = false;

                if (subLane.HasBuffer(oc))
                {
                    var buf = subLane[oc];
                    for (int j = 0; j < buf.Length; j++)
                    {
                        Entity sl = buf[j].m_SubLane;
                        string kinds = "";
                        if (pedestrianLane.HasComponent(sl)) { kinds += "Pedestrian "; foundPedAnchor = true; }
                        if (carLane.HasComponent(sl)) kinds += "Car ";
                        if (trackLane.HasComponent(sl)) kinds += "Track ";
                        if (parkingLane.HasComponent(sl)) kinds += "Parking ";

                        string connDetail = "";
                        if (connLane.HasComponent(sl))
                        {
                            var cl = connLane[sl];
                            kinds += "Connection ";
                            connDetail = $" connFlags={cl.m_Flags} connRoad={cl.m_RoadTypes} connTrack={cl.m_TrackTypes}";
                            if ((cl.m_Flags & ConnectionLaneFlags.Pedestrian) != 0) foundPedAnchor = true;
                        }
                        if (kinds.Length == 0) kinds = "<unknown>";

                        log.Info($"[CT-OCDUMP]   [{j}] sub={sl.Index}:{sl.Version} kind={kinds.Trim()}{connDetail}");
                    }
                }

                if (foundPedAnchor)
                {
                    if ((ocType & OutsideConnectionTransferType.Road) != 0) pedAnchorRoad++;
                    else if ((ocType & OutsideConnectionTransferType.Train) != 0) pedAnchorTrain++;
                    else if ((ocType & OutsideConnectionTransferType.Air) != 0) pedAnchorAir++;
                    else if ((ocType & OutsideConnectionTransferType.Ship) != 0) pedAnchorShip++;
                }
            }

            log.Info($"[CT-OCDUMP] totals: Road={totalRoad} Train={totalTrain} Air={totalAir} Ship={totalShip} None={totalNone}");
            log.Info($"[CT-OCDUMP] connection model: edge-based={totalEdgeBased} route-based={totalRouteBased} neither={totalNeither}");
            log.Info($"[CT-OCDUMP] pedestrian anchor present: Road={pedAnchorRoad}/{totalRoad} Train={pedAnchorTrain}/{totalTrain} Air={pedAnchorAir}/{totalAir} Ship={pedAnchorShip}/{totalShip}");
            log.Info($"[CT-OCDUMP] === end dump ===");

            ocs.Dispose();
            m_Done = true;
            Enabled = false;
        }
    }
}

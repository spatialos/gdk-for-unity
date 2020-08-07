using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;
using Unity.Profiling;

namespace Improbable.Gdk.Core.NewWorld
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateBefore(typeof(WorkerSystem))]
    public class NetworkStatisticsSystem : ComponentSystem
    {
        private NetworkStatistics statistics;
        private readonly NetFrameStats lastIncomingData = new NetFrameStats();
        private readonly NetFrameStats lastOutgoingData = new NetFrameStats();
        private ProfilerMarker applyDiffMarker = new ProfilerMarker("NetworkStatisticsSystem.ApplyDiff");

        private float lastFrameTime;

        protected override void OnCreate()
        {
            statistics = World.GetWorker().NetworkStatistics;
#if !UNITY_EDITOR
            Enabled = false;
#endif
        }

        protected override void OnUpdate()
        {
            statistics.FinishFrame(lastFrameTime);
            lastFrameTime = Time.DeltaTime;
        }
    }

    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class WorkerSystem : ComponentSystem
    {
        private SpatialOSWorker worker;
        private EntitySystem entitySystem;
        private ProfilerMarker tickMarker = new ProfilerMarker("WorkerSystem.Tick");

        protected override void OnCreate()
        {
            base.OnCreate();
            worker = World.GetWorker();
            entitySystem = World.GetOrCreateSystem<EntitySystem>();
        }

        protected override void OnUpdate()
        {
            entitySystem.Clear();
            using (tickMarker.Auto())
            {
                worker.Tick();
            }
        }
    }

    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(WorkerSystem))]
    [UpdateBefore(typeof(EntitySystem))]
    public class ECSViewSystem : ComponentSystem
    {
        private readonly List<IEcsViewManager> managers = new List<IEcsViewManager>();

        private readonly Dictionary<uint, IEcsViewManager> componentIdToManager = new Dictionary<uint, IEcsViewManager>();

        private SpatialOSWorker worker;

        private ProfilerMarker applyDiffMarker = new ProfilerMarker("EcsViewSystem.ApplyDiff");
        private ProfilerMarker addEntityMarker = new ProfilerMarker("EcsViewSystem.OnAddEntity");
        private ProfilerMarker removeEntityMarker = new ProfilerMarker("EcsViewSystem.OnRemoveEntity");

        internal ComponentType[] GetInitialComponentsToAdd(uint componentId)
        {
            if (!componentIdToManager.TryGetValue(componentId, out var manager))
            {
                throw new ArgumentException($"Can not get initial component for unknown component ID {componentId}");
            }

            return manager.GetInitialComponents();
        }

        protected override void OnUpdate()
        {
            var diff = worker.ViewDiff;
            using (applyDiffMarker.Auto())
            {
                if (diff.Disconnected)
                {
                    OnDisconnect(diff.DisconnectMessage);
                    return;
                }

                foreach (var entityId in diff.GetEntitiesAdded())
                {
                    AddEntity(entityId);
                }

                foreach (var manager in managers)
                {
                    manager.ApplyDiff(diff);
                }

                foreach (var entityId in diff.GetEntitiesRemoved())
                {
                    RemoveEntity(entityId);
                }
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetWorker();

            foreach (var type in ComponentDatabase.Metaclasses.Select(type => type.Value.EcsViewManager))
            {
                var instance = (IEcsViewManager) Activator.CreateInstance(type);
                instance.Init(World);

                componentIdToManager.Add(instance.GetComponentId(), instance);
                managers.Add(instance);
            }
        }

        protected override void OnDestroy()
        {
            foreach (var manager in managers)
            {
                manager.Clean();
            }

            base.OnDestroy();
        }

        private void AddEntity(EntityId entityId)
        {
            using (addEntityMarker.Auto())
            {
                if (worker.EntityIdToEntity.ContainsKey(entityId))
                {
                    throw new InvalidSpatialEntityStateException(
                        string.Format(Errors.EntityAlreadyExistsError, entityId.Id));
                }

                var entity = EntityManager.CreateEntity();

                EntityManager.AddComponentData(entity, new SpatialEntityId { EntityId = entityId });

                EntityManager.AddComponent(entity, ComponentType.ReadWrite<NewlyAddedSpatialOSEntity>());

                worker.EntityIdToEntity.Add(entityId, entity);
            }
        }

        private void RemoveEntity(EntityId entityId)
        {
            using (removeEntityMarker.Auto())
            {
                if (!worker.TryGetEntity(entityId, out var entity))
                {
                    throw new InvalidSpatialEntityStateException(
                        string.Format(Errors.EntityNotFoundForDeleteError, entityId.Id));
                }

                EntityManager.DestroyEntity(entity);
                worker.EntityIdToEntity.Remove(entityId);
            }
        }

        private void OnDisconnect(string reason)
        {
            EntityManager.AddSharedComponentData(worker.WorkerEntity,
                new OnDisconnected { ReasonForDisconnect = reason });
        }

        private static class Errors
        {
            public const string EntityAlreadyExistsError =
                "Received an AddEntityOp with Spatial entity ID {0}, but an entity with that EntityId already exists.";

            public const string EntityNotFoundForDeleteError =
                "Received a DeleteEntityOp with Spatial entity ID {0}, but an entity with that EntityId could not be found."
                + "This could be caused by deleting SpatialOS entities locally. "
                + "Use a DeleteEntity command to delete entities instead.";
        }
    }

    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(WorkerSystem))]
    public class EntitySystem : ComponentSystem
    {
        public int ViewVersion { get; private set; }

        private SpatialOSWorker worker;

        private readonly List<EntityId> entitiesAdded = new List<EntityId>();
        private readonly List<EntityId> entitiesRemoved = new List<EntityId>();

        private ProfilerMarker applyDiffMarker = new ProfilerMarker("EntitySystem.ApplyDiff");

        public List<EntityId> GetEntitiesAdded()
        {
            return entitiesAdded;
        }

        public List<EntityId> GetEntitiesRemoved()
        {
            return entitiesRemoved;
        }

        protected override void OnUpdate()
        {
            var diff = worker.ViewDiff;
            using (applyDiffMarker.Auto())
            {
                entitiesAdded.Clear();
                entitiesRemoved.Clear();

                // todo decide on a container and remove this
                foreach (var entityId in diff.GetEntitiesAdded())
                {
                    entitiesAdded.Add(entityId);
                }

                foreach (var entityId in diff.GetEntitiesRemoved())
                {
                    entitiesRemoved.Add(entityId);
                }

                if (entitiesAdded.Count != 0 || entitiesRemoved.Count != 0)
                {
                    ViewVersion += 1;
                }
            }

            worker.NetworkStatistics.ApplyIncomingDiff(diff.GetNetStats());
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetWorker();
        }

        internal void Clear()
        {
            entitiesAdded.Clear();
            entitiesRemoved.Clear();
        }
    }

    public class NetworkStatistics
    {
        private const int DefaultBufferSize = 60;

        private readonly NetStats NetStats = new NetStats(DefaultBufferSize);
        private readonly NetFrameStats LastIncomingData = new NetFrameStats();
        private readonly NetFrameStats LastOutgoingData = new NetFrameStats();

        private ProfilerMarker applyIncomingDiffMarker = new ProfilerMarker("NetworkStatistics.ApplyIncomingDataDiff");

        public (DataPoint, float) GetSummary(MessageTypeUnion messageType, int numFrames, Direction direction)
        {
            return NetStats.GetSummary(messageType, numFrames, direction);
        }

        public void FinishFrame(float lastFrameTime)
        {
            NetStats.SetFrameStats(LastIncomingData, Direction.Incoming);
            NetStats.SetFrameStats(LastOutgoingData, Direction.Outgoing);
            NetStats.SetFrameTime(lastFrameTime);
            NetStats.FinishFrame();

            LastIncomingData.Clear();
            LastOutgoingData.Clear();
        }

        [Conditional("UNITY_EDITOR")]
        public void ApplyIncomingDiff(NetFrameStats data)
        {
            using (applyIncomingDiffMarker.Auto())
            {
                LastIncomingData.CopyFrom(data);
            }
        }

        [Conditional("UNITY_EDITOR")]
        public void ApplyOutgoingDiff(NetFrameStats data)
        {
            LastOutgoingData.CopyFrom(data);
        }
    }
}

using System;
using Improbable.Worker.CInterop;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    public interface IEcsViewManager
    {
        void Init(World world);
        void Clean();
        void DisposeForEntity(Entity entity);

        void ApplyDiff(ViewDiff diff);
    }

    public abstract class EcsViewManager<TComponent, TUpdate, TAuthority> : IEcsViewManager
        where TComponent : struct, ISpatialComponentData, IComponentData
        where TUpdate : struct, ISpatialComponentUpdate
        where TAuthority : struct, IComponentData
    {
        protected EntityManager EntityManager;
        protected SpatialOSReplicationGroup ReplicationGroupSystem;
        private WorkerSystem workerSystem;
        private SpatialOSReceiveSystem spatialOSReceiveSystem;
        private bool replicationSystemAdded = false;
        private readonly uint componentId;

        protected EcsViewManager()
        {
            componentId = ComponentDatabase.ComponentType<TComponent>.ComponentId;
        }

        public void Init(World world)
        {
            EntityManager = world.EntityManager;
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            ReplicationGroupSystem = world.GetOrCreateSystem<SpatialOSReplicationGroup>();

            if (workerSystem == null)
            {
                throw new ArgumentException("World instance is not running a valid SpatialOS worker");
            }

            spatialOSReceiveSystem = world.GetExistingSystem<SpatialOSReceiveSystem>();

            if (spatialOSReceiveSystem == null)
            {
                throw new ArgumentException("Could not find SpatialOS Receive System in the current world instance");
            }
        }

        public virtual void Clean()
        {
        }

        public void ApplyDiff(ViewDiff diff)
        {
            if (!diff.IsComponentChanged(componentId))
            {
                return;
            }
            
            var diffStorage = (DiffComponentStorage<TUpdate>) diff.GetComponentDiffStorage(componentId);

            foreach (var entityId in diffStorage.GetComponentsAdded())
            {
                AddComponent(entityId);
            }

            var updates = diffStorage.GetUpdates();
            if (updates.Count > 0)
            {
                var dataFromEntity = spatialOSReceiveSystem.GetComponentDataFromEntity<TComponent>();
                for (var i = 0; i < updates.Count; ++i)
                {
                    ref readonly var update = ref updates[i];

                    var entity = workerSystem.GetEntity(update.EntityId);
                    var data = dataFromEntity[entity];

                    ApplyUpdate(ref data, in update.Update);

                    dataFromEntity[entity] = data;
                }
            }

            var authChanges = diffStorage.GetAuthorityChanges();
            for (var i = 0; i < authChanges.Count; ++i)
            {
                ref readonly var change = ref authChanges[i];
                SetAuthority(change.EntityId, change.Authority);
            }

            foreach (var entityId in diffStorage.GetComponentsRemoved())
            {
                RemoveComponent(entityId);
            }
        }

        private void AddComponent(EntityId entityId)
        {
            if (!workerSystem.TryGetEntity(entityId, out var entity) || EntityManager.HasComponent<TComponent>(entity))
            {
                return;
            }

            EntityManager.AddComponentData(entity, CreateEmptyComponent());
        }

        private void SetAuthority(EntityId entityId, Authority authority)
        {
            if (!workerSystem.TryGetEntity(entityId, out var entity))
            {
                return;
            }

            switch (authority)
            {
                case Authority.Authoritative:
                    EntityManager.AddComponent<TAuthority>(entity);
                    if (!replicationSystemAdded)
                    {
                        AddReplicationSystem();
                    }

                    break;
                case Authority.NotAuthoritative:
                    EntityManager.RemoveComponent<TAuthority>(entity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(authority), authority, null);
            }
        }

        private void RemoveComponent(EntityId entityId)
        {
            if (!workerSystem.TryGetEntity(entityId, out var entity) || !EntityManager.HasComponent<TComponent>(entity))
            {
                return;
            }

            var data = EntityManager.GetComponentData<TComponent>(entity);
            DisposeData(data);
            EntityManager.RemoveComponent<TComponent>(entity);
        }

        public virtual void DisposeForEntity(Entity entity)
        {
        }

        protected virtual void DisposeData(TComponent data)
        {
        }

        protected virtual void AddReplicationSystem()
        {
            replicationSystemAdded = true;
        }

        protected abstract TComponent CreateEmptyComponent();
        protected abstract void ApplyUpdate(ref TComponent data, in TUpdate update);
    }
}

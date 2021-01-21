using System;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface IEcsViewManager
    {
        void Init(World world);
        void Clean();

        void ApplyDiff(ViewDiff diff);
    }

    public abstract class EcsViewManager<TComponent, TUpdate, TAuthority> : IEcsViewManager
        where TComponent : struct, ISpatialComponentData, IComponentData
        where TUpdate : struct, ISpatialComponentUpdate
        where TAuthority : struct, IComponentData
    {
        protected EntityManager EntityManager;
        private WorkerSystem workerSystem;
        private SpatialOSReceiveSystem spatialOSReceiveSystem;
        private readonly uint componentId;

        protected EcsViewManager()
        {
            componentId = ComponentDatabase.GetComponentId<TComponent>();
        }

        public void Init(World world)
        {
            EntityManager = world.EntityManager;
            workerSystem = world.GetExistingSystem<WorkerSystem>();

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
            var diffStorage = (DiffComponentStorage<TUpdate>) diff.GetComponentDiffStorage(componentId);

            foreach (var entityId in diffStorage.GetComponentsAdded())
            {
                AddComponent(entityId);
            }

            var updates = diffStorage.GetUpdates();
            var dataFromEntity = spatialOSReceiveSystem.GetComponentDataFromEntity<TComponent>();
            for (var i = 0; i < updates.Count; ++i)
            {
                ref readonly var update = ref updates[i];

                var entity = workerSystem.GetEntity(update.EntityId);
                var data = dataFromEntity[entity];

                ApplyUpdate(ref data, in update.Update);

                dataFromEntity[entity] = data;
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
            var entity = workerSystem.GetEntity(entityId);

            if (EntityManager.HasComponent<TComponent>(entity))
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

        protected virtual void DisposeData(TComponent data)
        {
        }

        protected abstract TComponent CreateEmptyComponent();
        protected abstract void ApplyUpdate(ref TComponent data, in TUpdate update);
    }
}

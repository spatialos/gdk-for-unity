using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Profiling;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    public class EcsViewSystem : ComponentSystem
    {
        private readonly List<IEcsViewManager> managers = new List<IEcsViewManager>();

        private WorkerSystem worker;

        private ProfilerMarker applyDiffMarker = new ProfilerMarker("EcsViewSystem.ApplyDiff");
        private ProfilerMarker addEntityMarker = new ProfilerMarker("EcsViewSystem.OnAddEntity");
        private ProfilerMarker removeEntityMarker = new ProfilerMarker("EcsViewSystem.OnRemoveEntity");

        internal void ApplyDiff(ViewDiff diff)
        {
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

            worker = World.GetExistingSystem<WorkerSystem>();

            foreach (var type in ComponentDatabase.Metaclasses.Select(type => type.Value.EcsViewManager))
            {
                var instance = (IEcsViewManager) Activator.CreateInstance(type);
                instance.Init(World);
                managers.Add(instance);
            }

            Enabled = false;
        }

        protected override void OnDestroy()
        {
            foreach (var manager in managers)
            {
                manager.Clean();
            }

            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
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

                EntityManager.AddComponentData(entity, new SpatialEntityId
                {
                    EntityId = entityId
                });

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
}

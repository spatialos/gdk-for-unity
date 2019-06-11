using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine.Profiling;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    public class EcsViewSystem : ComponentSystem
    {
        private readonly List<IEcsViewManager> managers = new List<IEcsViewManager>();

        private readonly Dictionary<uint, IEcsViewManager> componentIdToManager =
            new Dictionary<uint, IEcsViewManager>();

        private WorkerSystem worker;

        internal ComponentType[] GetInitialComponentsToAdd(uint componentId)
        {
            if (!componentIdToManager.TryGetValue(componentId, out var manager))
            {
                throw new ArgumentException($"Can not get initial component for unknown component ID {componentId}");
            }

            return manager.GetInitialComponents();
        }

        internal void ApplyDiff(ViewDiff diff)
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

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();

            foreach (var type in ReflectionUtility.GetNonAbstractTypes(typeof(IEcsViewManager)))
            {
                var instance = (IEcsViewManager) Activator.CreateInstance(type);
                instance.Init(World);

                componentIdToManager.Add(instance.GetComponentId(), instance);
                managers.Add(instance);
            }

            Enabled = false;
        }

        protected override void OnDestroy()
        {
            foreach (var manager in managers)
            {
                manager.Clean(World);
            }

            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
        }

        private void AddEntity(EntityId entityId)
        {
            if (worker.EntityIdToEntity.ContainsKey(entityId))
            {
                throw new InvalidSpatialEntityStateException(
                    string.Format(Errors.EntityAlreadyExistsError, entityId.Id));
            }

            Profiler.BeginSample("OnAddEntity");

            var entity = EntityManager.CreateEntity();

            EntityManager.AddComponentData(entity, new SpatialEntityId
            {
                EntityId = entityId
            });

            EntityManager.AddComponent(entity, ComponentType.ReadWrite<NewlyAddedSpatialOSEntity>());

            worker.EntityIdToEntity.Add(entityId, entity);
            Profiler.EndSample();
        }

        private void RemoveEntity(EntityId entityId)
        {
            if (!worker.TryGetEntity(entityId, out var entity))
            {
                throw new InvalidSpatialEntityStateException(
                    string.Format(Errors.EntityNotFoundForDeleteError, entityId.Id));
            }

            Profiler.BeginSample("OnRemoveEntity");
            EntityManager.DestroyEntity(entity);
            worker.EntityIdToEntity.Remove(entityId);
            Profiler.EndSample();
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

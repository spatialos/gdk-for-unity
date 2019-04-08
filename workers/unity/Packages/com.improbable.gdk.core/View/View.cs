using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class View
    {
        private readonly Dictionary<Type, IViewStorage> typeToViewStorage = new Dictionary<Type, IViewStorage>();
        private readonly Dictionary<uint, IViewStorage> componentIdToViewStorage = new Dictionary<uint, IViewStorage>();
        private readonly List<IViewStorage> viewStorages = new List<IViewStorage>();

        private readonly HashSet<EntityId> entities = new HashSet<EntityId>();
        private readonly Dictionary<string, string> workerFlags = new Dictionary<string, string>();

        public View()
        {
            var types = ReflectionUtility.GetNonAbstractTypes(typeof(IViewStorage));
            foreach (var type in types)
            {
                var instance = (IViewStorage) Activator.CreateInstance(type);

                typeToViewStorage.Add(instance.GetSnapshotType(), instance);
                typeToViewStorage.Add(instance.GetUpdateType(), instance);
                componentIdToViewStorage.Add(instance.GetComponentId(), instance);

                viewStorages.Add(instance);
            }
        }

        public void UpdateComponent<T>(EntityId entityId, in T update) where T : struct, ISpatialComponentUpdate
        {
            var storage = (IViewComponentUpdater<T>) typeToViewStorage[typeof(T)];
            storage.ApplyUpdate(entityId.Id, in update);
        }

        public HashSet<EntityId> GetEntityIds()
        {
            return entities;
        }

        public bool HasEntity(EntityId entityId)
        {
            return entities.Contains(entityId);
        }

        // TODO: Make ref readonly when we have a dictionary type with ref indexing semantics.
        public T GetComponent<T>(EntityId entityId) where T : struct, ISpatialComponentSnapshot
        {
            if (!HasComponent<T>(entityId))
            {
                throw new ArgumentException($"The view does not have entity with Entity ID: {entityId.Id} and component with ID: {DynamicSnapshot.GetSnapshotComponentId<T>()}");
            }

            var storage = (IViewComponentStorage<T>) typeToViewStorage[typeof(T)];
            return storage.GetComponent(entityId.Id);
        }

        public bool HasComponent<T>(EntityId entityId) where T : struct, ISpatialComponentSnapshot
        {
            if (!HasEntity(entityId))
            {
                return false;
            }

            var storage = typeToViewStorage[typeof(T)];
            return storage.HasComponent(entityId.Id);
        }

        public bool HasComponent(EntityId entityId, uint componentId)
        {
            if (!HasEntity(entityId))
            {
                return false;
            }

            var storage = componentIdToViewStorage[componentId];
            return storage.HasComponent(entityId.Id);
        }

        public Authority GetAuthority<T>(EntityId entityId) where T : struct, ISpatialComponentSnapshot
        {
            if (!HasEntity(entityId))
            {
                throw new ArgumentException($"The view does not have entity with Entity ID: {entityId.Id}");
            }

            return typeToViewStorage[typeof(T)].GetAuthority(entityId.Id);
        }

        public Authority GetAuthority(EntityId entityId, uint componentId)
        {
            if (!HasEntity(entityId))
            {
                throw new ArgumentException($"The view does not have entity with Entity ID: {entityId.Id}");
            }

            return componentIdToViewStorage[componentId].GetAuthority(entityId.Id);
        }

        public bool IsAuthoritative<T>(EntityId entityId) where T : struct, ISpatialComponentSnapshot
        {
            if (!HasComponent<T>(entityId))
            {
                return false;
            }

            return typeToViewStorage[typeof(T)].GetAuthority(entityId.Id) == Authority.Authoritative;
        }

        public bool IsAuthoritative(EntityId entityId, uint componentId)
        {
            if (!HasComponent(entityId, componentId))
            {
                return false;
            }

            return componentIdToViewStorage[componentId].GetAuthority(entityId.Id) == Authority.Authoritative;
        }

        public string GetWorkerFlag(string name)
        {
            return workerFlags.TryGetValue(name, out var value) ? value : null;
        }

        internal void ApplyDiff(ViewDiff diff)
        {
            var entitiesAdded = diff.GetEntitiesAdded();
            foreach (var entity in entitiesAdded)
            {
                entities.Add(entity);
            }

            var entitiesRemoved = diff.GetEntitiesRemoved();
            foreach (var entity in entitiesRemoved)
            {
                entities.Remove(entity);
            }

            foreach (var storage in viewStorages)
            {
                storage.ApplyDiff(diff);
            }

            foreach (var pair in diff.GetWorkerFlagChanges())
            {
                workerFlags[pair.Item1] = pair.Item2;
            }
        }
    }
}

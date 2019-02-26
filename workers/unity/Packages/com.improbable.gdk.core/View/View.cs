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

        public View()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IViewStorage).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (IViewStorage) Activator.CreateInstance(type);

                        typeToViewStorage.Add(instance.GetSnapshotType(), instance);
                        componentIdToViewStorage.Add(instance.GetComponentId(), instance);

                        viewStorages.Add(instance);
                    }
                }
            }
        }

        public void ApplyDiff(ViewDiff diff)
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
                // Resolve this with an actual diff!
                storage.ApplyDiff(diff);
            }
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
    }
}

using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class View
    {
        private readonly Dictionary<Type, IViewStorage> viewStorages = new Dictionary<Type, IViewStorage>();

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

                        viewStorages[instance.GetUpdateType()] = instance;
                        viewStorages[instance.GetSnapshotType()] = instance;
                    }
                }
            }
        }

        public bool HasEntity(EntityId entityId)
        {
            return entities.Contains(entityId);
        }

        // TODO: Make ref readonly when we have a dictionary type with ref indexing semantics.
        public T GetComponent<T>(EntityId entityId) where T : struct, ISpatialComponentSnapshot
        {
            if (!HasEntity(entityId))
            {
                throw new ArgumentException($"The view does not have entity with Entity ID: {entityId.Id}");
            }

            var storage = (IViewComponentStorage<T>) viewStorages[typeof(T)];
            return storage.GetComponent(entityId.Id);
        }

        public bool HasComponent<T>(EntityId entityId) where T : struct, ISpatialComponentSnapshot
        {
            if (!HasEntity(entityId))
            {
                return false;
            }

            var storage = (IViewComponentStorage<T>) viewStorages[typeof(T)];
            return storage.HasComponent(entityId.Id);
        }

        public Authority GetAuthority<T>(EntityId entityId) where T : struct, ISpatialComponentSnapshot
        {
            if (!HasEntity(entityId))
            {
                throw new ArgumentException($"The view does not have entity with Entity ID: {entityId.Id}");
            }

            return ((IViewComponentStorage<T>) viewStorages[typeof(T)]).GetAuthority(entityId.Id);
        }

        public bool IsAuthoritative<T>(EntityId entityId) where T : struct, ISpatialComponentSnapshot
        {
            if (!HasComponent<T>(entityId))
            {
                return false;
            }

            return ((IViewComponentStorage<T>) viewStorages[typeof(T)]).GetAuthority(entityId.Id) == Authority.Authoritative;
        }

        internal void AddEntity(EntityId entityId)
        {
            entities.Add(entityId);
        }

        internal void RemoveEntity(EntityId entityId)
        {
            entities.Remove(entityId);
        }

        internal void AddComponent<T>(EntityId entityId, T component) where T : struct, ISpatialComponentSnapshot
        {
            var storage = (IViewComponentStorage<T>) viewStorages[typeof(T)];
            storage.AddComponent(entityId.Id, component);
        }

        internal void ApplyUpdate<U>(EntityId entityId, U update) where U : struct, ISpatialComponentUpdate
        {
            var storage = (IViewUpdateHandler<U>) viewStorages[typeof(U)];
            storage.ApplyUpdate(entityId.Id, update);
        }

        internal void RemoveComponent<T>(EntityId entityId) where T : struct, ISpatialComponentSnapshot
        {
            var storage = (IViewComponentStorage<T>) viewStorages[typeof(T)];
            storage.RemoveComponent(entityId.Id);
        }

        internal void SetAuthority<T>(EntityId entityId, Authority authority) where T : struct, ISpatialComponentSnapshot
        {
            ((IViewComponentStorage<T>) viewStorages[typeof(T)]).SetAuthority(entityId.Id, authority);
        }
    }
}

using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    public class ComponentUpdateSystem : ComponentSystem
    {
        private WorkerSystem worker;

        public void SendEvent<T>(T eventToSend, EntityId entityId) where T : IEvent
        {
            worker.MessagesToSend.AddEvent(eventToSend, entityId.Id);
        }

        public MessagesSpan<ComponentEventReceived<T>> GetEventsReceived<T>() where T : IEvent
        {
            var manager = (IDiffEventStorage<T>) worker.Diff.GetComponentDiffStorageForEvent<T>();
            if (!manager.Dirty)
            {
                return MessagesSpan<ComponentEventReceived<T>>.Empty();
            }
            return manager.GetEvents();
        }

        public MessagesSpan<ComponentEventReceived<T>> GetEventsReceived<T>(EntityId entityId) where T : IEvent
        {
            var manager = (IDiffEventStorage<T>) worker.Diff.GetComponentDiffStorageForEvent<T>();
            if (!manager.Dirty)
            {
                return MessagesSpan<ComponentEventReceived<T>>.Empty();
            }
            return manager.GetEvents(entityId);
        }

        public MessagesSpan<ComponentUpdateReceived<T>> GetComponentUpdatesReceived<T>()
            where T : ISpatialComponentUpdate
        {
            var componentId = ComponentDatabase.ComponentUpdateType<T>.ComponentId;
            var manager = (IDiffUpdateStorage<T>) worker.Diff.GetComponentDiffStorage(componentId);
            if (!manager.Dirty)
            {
                return MessagesSpan<ComponentUpdateReceived<T>>.Empty();
            }
            return manager.GetUpdates();
        }

        public MessagesSpan<ComponentUpdateReceived<T>> GetEntityComponentUpdatesReceived<T>(EntityId entityId)
            where T : ISpatialComponentUpdate
        {
            var componentId = ComponentDatabase.ComponentUpdateType<T>.ComponentId;
            var manager = (IDiffUpdateStorage<T>) worker.Diff.GetComponentDiffStorage(componentId);
            if (!manager.Dirty)
            {
                return MessagesSpan<ComponentUpdateReceived<T>>.Empty();
            }
            return manager.GetUpdates(entityId);
        }

        public MessagesSpan<AuthorityChangeReceived> GetAuthorityChangesReceived(uint componentId)
        {
            var manager = (IDiffAuthorityStorage) worker.Diff.GetComponentDiffStorage(componentId);
            if (!manager.Dirty)
            {
                return MessagesSpan<AuthorityChangeReceived>.Empty();
            }
            return manager.GetAuthorityChanges();
        }

        public MessagesSpan<AuthorityChangeReceived> GetAuthorityChangesReceived(EntityId entityId,
            uint componentId)
        {
            var manager = (IDiffAuthorityStorage) worker.Diff.GetComponentDiffStorage(componentId);
            if (!manager.Dirty)
            {
                return MessagesSpan<AuthorityChangeReceived>.Empty();
            }
            return manager.GetAuthorityChanges(entityId);
        }

        public HashSet<EntityId> GetComponentsAdded(uint componentId)
        {
            var manager = worker.Diff.GetComponentDiffStorage(componentId);
            return manager.GetComponentsAdded();
        }

        public HashSet<EntityId> GetComponentsRemoved(uint componentId)
        {
            var manager = worker.Diff.GetComponentDiffStorage(componentId);
            return manager.GetComponentsRemoved();
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();

            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}

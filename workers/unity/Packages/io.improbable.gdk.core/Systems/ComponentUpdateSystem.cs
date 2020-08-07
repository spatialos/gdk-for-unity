using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    public class ComponentUpdateSystem : ComponentSystem
    {
        private WorkerSystem worker;

        public void SendUpdate<T>(in T update, EntityId entityId) where T : struct, ISpatialComponentUpdate
        {
            worker.MessagesToSend.AddComponentUpdate(in update, entityId.Id);
        }

        public void SendEvent<T>(T eventToSend, EntityId entityId) where T : IEvent
        {
            worker.MessagesToSend.AddEvent(eventToSend, entityId.Id);
        }

        public MessagesSpan<ComponentEventReceived<T>> GetEventsReceived<T>() where T : IEvent
        {
            var manager = (IDiffEventStorage<T>) worker.Diff.GetComponentDiffStorage(typeof(T));
            return manager.GetEvents();
        }

        public MessagesSpan<ComponentEventReceived<T>> GetEventsReceived<T>(EntityId entityId) where T : IEvent
        {
            var manager = (IDiffEventStorage<T>) worker.Diff.GetComponentDiffStorage(typeof(T));
            return manager.GetEvents(entityId);
        }

        public MessagesSpan<ComponentUpdateReceived<T>> GetComponentUpdatesReceived<T>()
            where T : ISpatialComponentUpdate
        {
            var manager = (IDiffUpdateStorage<T>) worker.Diff.GetComponentDiffStorage(typeof(T));
            return manager.GetUpdates();
        }

        public MessagesSpan<ComponentUpdateReceived<T>> GetEntityComponentUpdatesReceived<T>(EntityId entityId)
            where T : ISpatialComponentUpdate
        {
            var manager = (IDiffUpdateStorage<T>) worker.Diff.GetComponentDiffStorage(typeof(T));
            return manager.GetUpdates(entityId);
        }

        public MessagesSpan<AuthorityChangeReceived> GetAuthorityChangesReceived(uint componentId)
        {
            var manager = (IDiffAuthorityStorage) worker.Diff.GetComponentDiffStorage(componentId);
            return manager.GetAuthorityChanges();
        }

        public MessagesSpan<AuthorityChangeReceived> GetAuthorityChangesReceived(EntityId entityId,
            uint componentId)
        {
            var manager = (IDiffAuthorityStorage) worker.Diff.GetComponentDiffStorage(componentId);
            return manager.GetAuthorityChanges(entityId);
        }

        public List<EntityId> GetComponentsAdded(uint componentId)
        {
            var manager = worker.Diff.GetComponentDiffStorage(componentId);
            return manager.GetComponentsAdded();
        }

        public List<EntityId> GetComponentsRemoved(uint componentId)
        {
            var manager = worker.Diff.GetComponentDiffStorage(componentId);
            return manager.GetComponentsRemoved();
        }

        public void AcknowledgeAuthorityLoss(EntityId entityId, uint componentId)
        {
            worker.MessagesToSend.AcknowledgeAuthorityLoss(entityId.Id, componentId);
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

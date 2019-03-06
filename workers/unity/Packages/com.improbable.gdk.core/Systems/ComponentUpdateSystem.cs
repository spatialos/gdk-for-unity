using System.Collections.Generic;
using Improbable.Worker.CInterop;
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
            worker.View.UpdateComponent(entityId, in update);
            worker.MessagesToSend.AddComponentUpdate(in update, entityId.Id);
        }

        public void SendEvent<T>(T eventToSend, EntityId entityId) where T : IEvent
        {
            worker.MessagesToSend.AddEvent(eventToSend, entityId.Id);
        }

        public ReceivedMessagesSpan<ComponentEventReceived<T>> GetEventsReceived<T>() where T : IEvent
        {
            var manager = (IDiffEventStorage<T>) worker.Diff.GetComponentDiffStorage(typeof(T));
            return manager.GetEvents();
        }

        public ReceivedMessagesSpan<ComponentEventReceived<T>> GetEventsReceived<T>(EntityId entityId) where T : IEvent
        {
            var manager = (IDiffEventStorage<T>) worker.Diff.GetComponentDiffStorage(typeof(T));
            return manager.GetEvents(entityId);
        }

        public ReceivedMessagesSpan<ComponentUpdateReceived<T>> GetComponentUpdatesReceived<T>()
            where T : ISpatialComponentUpdate
        {
            var manager = (IDiffUpdateStorage<T>) worker.Diff.GetComponentDiffStorage(typeof(T));
            return manager.GetUpdates();
        }

        public ReceivedMessagesSpan<ComponentUpdateReceived<T>> GetEntityComponentUpdatesReceived<T>(EntityId entityId)
            where T : ISpatialComponentUpdate
        {
            var manager = (IDiffUpdateStorage<T>) worker.Diff.GetComponentDiffStorage(typeof(T));
            return manager.GetUpdates(entityId);
        }

        public ReceivedMessagesSpan<AuthorityChangeReceived> GetAuthorityChangesReceived(uint componentId)
        {
            var manager = (IDiffAuthorityStorage) worker.Diff.GetComponentDiffStorage(componentId);
            return manager.GetAuthorityChanges();
        }

        public ReceivedMessagesSpan<AuthorityChangeReceived> GetAuthorityChangesReceived(EntityId entityId,
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

        public Authority GetAuthority(EntityId entityId, uint componentId)
        {
            return worker.View.GetAuthority(entityId, componentId);
        }

        public T GetComponent<T>(EntityId entityId) where T : struct, ISpatialComponentSnapshot
        {
            return worker.View.GetComponent<T>(entityId);
        }

        public void AcknowledgeAuthorityLoss(EntityId entityId, uint componentId)
        {
            worker.MessagesToSend.AcknowledgeAuthorityLoss(entityId.Id, componentId);
        }

        public bool HasComponent(uint componentId, EntityId entityId)
        {
            return worker.View.HasComponent(entityId, componentId);
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();

            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}

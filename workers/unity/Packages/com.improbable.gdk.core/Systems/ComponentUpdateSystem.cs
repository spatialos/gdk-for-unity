using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class ComponentUpdateSystem : ComponentSystem
    {
        private readonly SerializedMessagesToSend serializedMessagesToSend = new SerializedMessagesToSend();
        private WorkerSystem worker;

        private readonly List<IComponentManager> managers = new List<IComponentManager>();

        private readonly Dictionary<uint, IComponentManager> componentIdToManager =
            new Dictionary<uint, IComponentManager>();

        public void SendUpdate<T>(T update, EntityId entityId) where T : ISpatialComponentUpdate
        {
            worker.MessagesToSend.AddComponentUpdate(update, entityId.Id);
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
            if (!componentIdToManager.TryGetValue(componentId, out var manager))
            {
                throw new ArgumentException("Component ID not recognized");
            }

            return ((IAuthorityManager) manager).GetAuthority(entityId);
        }

        public void AcknowledgeAuthorityLoss(EntityId entityId, uint componentId)
        {
            worker.MessagesToSend.AcknowledgeAuthorityLoss(entityId.Id, componentId);
        }

        internal ComponentType[] GetInitialComponentsToAdd(uint componentId)
        {
            if (!componentIdToManager.TryGetValue(componentId, out var manager))
            {
                throw new ArgumentException("Component ID not recognized");
            }

            return manager.GetInitialComponents();
        }

        public bool HasComponent(uint componentId, EntityId entityId)
        {
            if (!componentIdToManager.TryGetValue(componentId, out var manager))
            {
                throw new ArgumentException("Component ID not recognized");
            }

            return manager.HasComponent(entityId);
        }

        internal void ApplyDiff(ViewDiff diff)
        {
            foreach (var manager in managers)
            {
                manager.ApplyDiff(diff);
            }
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(IComponentManager).IsAssignableFrom(type) || type.IsAbstract)
                    {
                        continue;
                    }

                    var instance = (IComponentManager) Activator.CreateInstance(type);
                    instance.Init(World);

                    componentIdToManager.Add(instance.GetComponentId(), instance);
                    managers.Add(instance);
                }
            }
        }

        protected override void OnDestroyManager()
        {
            foreach (var manager in managers)
            {
                manager.Clean(World);
            }

            base.OnDestroyManager();
        }

        protected override void OnUpdate()
        {
            worker.SendMessages();
        }
    }
}

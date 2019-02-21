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
        private ViewDiff viewDiff;

        private readonly List<IComponentManager> managers = new List<IComponentManager>();

        private readonly Dictionary<Type, IComponentManager> eventTypeToManager =
            new Dictionary<Type, IComponentManager>();

        private readonly Dictionary<Type, IComponentManager> componentTypeToManager =
            new Dictionary<Type, IComponentManager>();

        private readonly Dictionary<uint, IComponentManager> componentIdToManager =
            new Dictionary<uint, IComponentManager>();

        public void SendComponentUpdate<T>(T updateToSend, EntityId entityId) where T : ISpatialComponentData
        {
            if (!componentTypeToManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException("Type is not a valid update");
            }

            ((IUpdateSender<T>) manager).SendComponentUpdate(updateToSend, entityId);
        }

        public void SendEvent<T>(T eventToSend, EntityId entityId) where T : IEvent
        {
            if (!eventTypeToManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException("Type is not a valid event");
            }

            ((IEventManager<T>) manager).SendEvent(eventToSend, entityId);
        }

        public ReceivedMessagesSpan<ComponentEventReceived<T>> GetEventsReceived<T>() where T : IEvent
        {
            var manager = (IDiffEventStorage<T>) viewDiff.GetComponentDiffStorage(typeof(T));
            return manager.GetEvents();
        }

        public ReceivedMessagesSpan<ComponentEventReceived<T>> GetEventsReceived<T>(EntityId entityId) where T : IEvent
        {
            var manager = (IDiffEventStorage<T>) viewDiff.GetComponentDiffStorage(typeof(T));
            return manager.GetEvents(entityId);
        }

        public ReceivedMessagesSpan<ComponentUpdateReceived<T>> GetComponentUpdatesReceived<T>()
            where T : ISpatialComponentUpdate
        {
            var manager = (IDiffUpdateStorage<T>) viewDiff.GetComponentDiffStorage(typeof(T));
            return manager.GetUpdates();
        }

        public ReceivedMessagesSpan<ComponentUpdateReceived<T>> GetEntityComponentUpdatesReceived<T>(EntityId entityId)
            where T : ISpatialComponentUpdate
        {
            var manager = (IDiffUpdateStorage<T>) viewDiff.GetComponentDiffStorage(typeof(T));
            return manager.GetUpdates(entityId);
        }

        public ReceivedMessagesSpan<AuthorityChangeReceived> GetAuthorityChangesReceived(uint componentId)
        {
            var manager = (IDiffAuthorityStorage) viewDiff.GetComponentDiffStorage(componentId);
            return manager.GetAuthorityChanges();
        }

        public ReceivedMessagesSpan<AuthorityChangeReceived> GetAuthorityChangesReceived(EntityId entityId,
            uint componentId)
        {
            var manager = (IDiffAuthorityStorage) viewDiff.GetComponentDiffStorage(componentId);
            return manager.GetAuthorityChanges(entityId);
        }

        public List<EntityId> GetComponentsAdded(uint componentId)
        {
            var manager = viewDiff.GetComponentDiffStorage(componentId);
            return manager.GetComponentsAdded();
        }

        public List<EntityId> GetComponentsRemoved(uint componentId)
        {
            var manager = viewDiff.GetComponentDiffStorage(componentId);
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
            if (!componentIdToManager.TryGetValue(componentId, out var manager))
            {
                throw new ArgumentException("Component ID not recognized");
            }

            ((IAuthorityManager) manager).AcknowledgeAuthorityLoss(entityId);
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

            viewDiff = World.GetExistingManager<WorkerSystem>().Diff;

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

                    componentTypeToManager.Add(instance.GetComponentType(), instance);
                    componentIdToManager.Add(instance.GetComponentId(), instance);

                    foreach (var eventType in instance.GetEventTypes())
                    {
                        eventTypeToManager.Add(eventType, instance);
                    }

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
            foreach (var manager in managers)
            {
                // todo there isn't currently a reason to couple the storage with the point at which sending happens
                manager.SendAll();
            }
        }
    }
}

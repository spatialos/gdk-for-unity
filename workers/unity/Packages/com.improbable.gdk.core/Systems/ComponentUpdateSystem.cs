using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    [UpdateAfter(typeof(SpatialOSSendSystem))]
    public class ComponentUpdateSystem : ComponentSystem
    {
        private readonly List<IComponentManager> managers = new List<IComponentManager>();

        private readonly Dictionary<Type, IComponentManager> eventTypeToManager =
            new Dictionary<Type, IComponentManager>();

        private readonly Dictionary<Type, IComponentManager> updateTypeToManager =
            new Dictionary<Type, IComponentManager>();

        private readonly Dictionary<Type, IComponentManager> componentTypeToManager =
            new Dictionary<Type, IComponentManager>();

        private readonly Dictionary<uint, IComponentManager> componentIdToManager =
            new Dictionary<uint, IComponentManager>();

        public void SendEvent<T>(T eventToSend, EntityId entityId) where T : IEvent
        {
            if (!eventTypeToManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException("Type is not a valid event");
            }

            ((IEventManager<T>) manager).SendEvent(eventToSend, entityId);
        }

        public List<ComponentEventReceived<T>> GetEventsReceived<T>() where T : IEvent
        {
            if (!eventTypeToManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException("Type is not a valid event");
            }

            return ((IEventManager<T>) managers).GetEventsReceived();
        }

        public void SendComponentUpdate<T>(T updateToSend, EntityId entityId) where T : ISpatialComponentData
        {
            if (!componentTypeToManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException("Type is not a valid update");
            }

            ((IUpdateSender<T>) manager).SendComponentUpdate(updateToSend, entityId);
        }

        public List<ComponentUpdateReceived<T>> GetComponentUpdatesReceived<T>() where T : ISpatialComponentUpdate
        {
            if (!updateTypeToManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException("Type is not a valid update");
            }

            return ((IUpdateReceiver<T>) manager).GetComponentUpdatesReceived();
        }

        public ComponentUpdateSlice<T> GetEntityComponentUpdatesReceived<T>(EntityId entityId)
            where T : ISpatialComponentUpdate
        {
            if (!updateTypeToManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException("Type is not a valid update");
            }

            return ((IUpdateReceiver<T>) manager).GetComponentUpdatesReceived(entityId);
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

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

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

                    updateTypeToManager.Add(instance.GetUpdateType(), instance);
                    componentTypeToManager.Add(instance.GetComponentType(), instance);

                    foreach (var eventType in instance.GetEventTypes())
                    {
                        eventTypeToManager.Add(eventType, instance);
                    }

                    managers.Add(instance);
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (var manager in managers)
            {
                manager.SendAll();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    [UpdateBefore(typeof(SpatialOSSendSystem))]
    public class ComponentUpdateSystem : ComponentSystem
    {
        private readonly List<IUpdateEventManager> managers = new List<IUpdateEventManager>();

        private readonly Dictionary<Type, IUpdateEventManager> eventTypeToManager =
            new Dictionary<Type, IUpdateEventManager>();

        private readonly Dictionary<Type, IUpdateEventManager> updateTypeToManager =
            new Dictionary<Type, IUpdateEventManager>();

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

        public List<ComponentEventToSend<T>> GetEventsToSend<T>() where T : IEvent
        {
            if (!eventTypeToManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException("Type is not a valid event");
            }

            return ((IEventManager<T>) manager).GetEventsToSend();
        }

        // todo turn this on and have it populated from components
        // public void SendComponentUpdate<T>(T updateToSend, EntityId entityId) where T : ISpatialComponentUpdate
        // {
        //     if (!updateTypeToManager.TryGetValue(typeof(T), out var manager))
        //     {
        //         throw new ArgumentException("Type is not a valid update");
        //     }
        //
        //     ((IUpdateSender<T>) manager).SendComponentUpdate(updateToSend, entityId);
        // }

        public List<ComponentUpdateToSend<T>> GetComponentUpdatesReceived<T>() where T : ISpatialComponentUpdate
        {
            if (!updateTypeToManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException("Type is not a valid update");
            }

            return ((IUpdateSender<T>) manager).GetComponentUpdatesToSend();
        }

        public List<ComponentUpdateReceived<T>> GetComponentUpdatesToSend<T>() where T : ISpatialComponentUpdate
        {
            if (!updateTypeToManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException("Type is not a valid update");
            }

            return ((IUpdateReceiver<T>) manager).GetComponentUpdatesReceived();
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(IUpdateEventManager).IsAssignableFrom(type) || type.IsAbstract)
                    {
                        continue;
                    }

                    var instance = (IUpdateEventManager) Activator.CreateInstance(type);
                    instance.Init(World);

                    updateTypeToManager.Add(instance.GetUpdateType(), instance);

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

using System;
using System.Collections.Generic;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface IUpdateEventManager
    {
        void SendAll();
        void Init(World world);

        Type[] GetEventTypes();
        Type GetUpdateType();
    }

    public interface IEventManager<T> where T : IEvent
    {
        void SendEvent(T eventToSend, EntityId entityId);
        List<ComponentEventReceived<T>> GetEventsReceived();
        List<ComponentEventToSend<T>> GetEventsToSend();
    }

    public interface IUpdateSender<T> where T : ISpatialComponentUpdate
    {
        void SendComponentUpdate(T updateToSend, EntityId entityId);
        List<ComponentUpdateToSend<T>> GetComponentUpdatesToSend();
    }

    public interface IUpdateReceiver<T> where T : ISpatialComponentUpdate
    {
        List<ComponentUpdateReceived<T>> GetComponentUpdatesReceived();
    }
}

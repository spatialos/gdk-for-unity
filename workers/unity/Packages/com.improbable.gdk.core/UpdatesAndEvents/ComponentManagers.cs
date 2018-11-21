using System;
using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    // todo look into validating at initialisation that types implementing these interfaces are valid
    public interface IComponentManager
    {
        void SendAll();
        void Init(World world);

        Type[] GetEventTypes();
        Type GetUpdateType();
        uint GetComponentId();
    }

    public interface IAuthorityManager
    {
        Authority GetAuthority(EntityId entityId);
        void AcknowledgeAuthorityLoss(EntityId entityId);
    }

    public interface IEventManager<T> where T : IEvent
    {
        void SendEvent(T eventToSend, EntityId entityId);
        List<ComponentEventReceived<T>> GetEventsReceived();
        List<ComponentEventToSend<T>> GetEventsToSend();
    }

    public interface IUpdateManager<T> where T : ISpatialComponentUpdate
    {
        void SendComponentUpdate(T updateToSend, EntityId entityId);
        List<ComponentUpdateToSend<T>> GetComponentUpdatesToSend();
        List<ComponentUpdateReceived<T>> GetComponentUpdatesReceived();
    }
}

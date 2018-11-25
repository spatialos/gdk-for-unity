using System;
using System.Collections.Generic;
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
        Type GetComponentType();
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
    }

    public interface IUpdateSender<T> where T : ISpatialComponentData
    {
        void SendComponentUpdate(T updateToSend, EntityId entityId);
    }

    public interface IUpdateReceiver<T> where T : ISpatialComponentUpdate
    {
        ReceivedMessagesSpan<ComponentUpdateReceived<T>> GetComponentUpdatesReceived();
        ReceivedMessagesSpan<ComponentUpdateReceived<T>> GetComponentUpdatesReceived(EntityId entityId);
    }
}

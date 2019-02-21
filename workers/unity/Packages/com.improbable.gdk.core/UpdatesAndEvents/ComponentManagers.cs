using System;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    // todo look into validating at initialisation that types implementing these interfaces are valid
    public interface IComponentManager
    {
        void SendAll();
        void Init(World world);
        void Clean(World world);

        Type[] GetEventTypes();
        Type GetComponentType();
        uint GetComponentId();

        // todo this should really be somewhere else
        ComponentType[] GetInitialComponents();

        bool HasComponent(EntityId entityId);

        void ApplyDiff(ViewDiff diff);
    }

    public interface IAuthorityManager
    {
        Authority GetAuthority(EntityId entityId);
        void AcknowledgeAuthorityLoss(EntityId entityId);
    }

    public interface IEventManager<T> where T : IEvent
    {
        void SendEvent(T eventToSend, EntityId entityId);
    }

    public interface IUpdateSender<T> where T : ISpatialComponentData
    {
        void SendComponentUpdate(T updateToSend, EntityId entityId);
    }
}

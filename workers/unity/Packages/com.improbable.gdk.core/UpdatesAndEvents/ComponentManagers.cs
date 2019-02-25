using System;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface IComponentManager
    {
        void Init(World world);
        void Clean(World world);

        Type[] GetEventTypes();
        Type GetComponentType();
        Type GetUpdateType();
        uint GetComponentId();

        // todo this should really be somewhere else
        ComponentType[] GetInitialComponents();

        bool HasComponent(EntityId entityId);

        void ApplyDiff(ViewDiff diff);
    }

    public interface IAuthorityManager
    {
        Authority GetAuthority(EntityId entityId);
    }
}

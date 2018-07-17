using System;
using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.Components
{
    public struct RequestContext
    {
        public long EntityId;
        public IOutgoingCommandRequest Request;

        public RequestContext(long entityId, IOutgoingCommandRequest request)
        {
            EntityId = entityId;
            Request = request;
        }
    }

    public abstract class ComponentDispatcher
    {
        public abstract uint ComponentId { get; }

        public abstract void OnAddComponent(MutableView view, AddComponentOp op);
        public abstract void OnRemoveComponent(MutableView view, RemoveComponentOp op);

        public abstract void OnComponentUpdate(MutableView view, ComponentUpdateOp op);
        public abstract void OnAuthorityChange(MutableView view, AuthorityChangeOp op);

        public abstract void AddCommandCallbacks(Dictionary<SpecificCommandReference, Action<MutableView, CommandRequestOp>> commandCallbacks,
            Dictionary<SpecificCommandReference, Action<MutableView, CommandResponseOp>> commandResponseCallbacks);
    }

    public abstract class ComponentReplication
    {
        public abstract uint ComponentId { get; }
        public abstract ComponentType[] BasicReplicationComponentTypes { get; }
        public abstract ComponentType[] CommandTypes { get; }
        public abstract void ExecuteReplication(EntityManager manager, ComponentGroup replicationGroup, Connection connection);
        public abstract void SendCommands(List<ComponentGroup> commandComponentGroups, Connection connection);
    }

    public abstract class ComponentCleanup
    {
        public abstract ComponentType[] CleanUpComponentTypes { get; }
        public abstract void CleanupComponents(List<ComponentGroup> componentGroups,
            ref EntityCommandBuffer entityCommandBuffer);

        protected void RemoveComponent<T>(ref EntityCommandBuffer entityCommandBuffer, EntityArray entityArray)
        {
            for (var i = 0; i < entityArray.Length; i++)
            {
                entityCommandBuffer.RemoveComponent<T>(entityArray[i]);
            }
        }
    }

}

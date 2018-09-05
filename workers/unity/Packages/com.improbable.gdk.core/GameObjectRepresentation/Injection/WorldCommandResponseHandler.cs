using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

#region Diagnostic control

// ReSharper disable UnusedMember.Local

#endregion

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public static partial class WorldCommandsRequirables
    {
        [InjectableId(InjectableType.WorldCommandResponseHandler, InjectableId.NullComponentId)]
        [InjectionCondition(InjectionCondition.RequireNothing)]
        public class WorldCommandResponseHandler : IInjectable
        {
            private readonly ILogDispatcher logDispatcher;

            private readonly List<Action<WorldCommands.ReserveEntityIds.ReceivedResponse>> reserveEntityIdsDelegates;
            private readonly List<Action<WorldCommands.CreateEntity.ReceivedResponse>> createEntityDelegates;
            private readonly List<Action<WorldCommands.DeleteEntity.ReceivedResponse>> deleteEntityDelegates;

            private WorldCommandResponseHandler(ILogDispatcher logDispatcher)
            {
                this.logDispatcher = logDispatcher;
                reserveEntityIdsDelegates = new List<Action<WorldCommands.ReserveEntityIds.ReceivedResponse>>();
                createEntityDelegates = new List<Action<WorldCommands.CreateEntity.ReceivedResponse>>();
                deleteEntityDelegates = new List<Action<WorldCommands.DeleteEntity.ReceivedResponse>>();
            }

            public event Action<WorldCommands.ReserveEntityIds.ReceivedResponse> OnReserveEntityIdsResponse
            {
                add => reserveEntityIdsDelegates.Add(value);
                remove => reserveEntityIdsDelegates.Remove(value);
            }

            public event Action<WorldCommands.CreateEntity.ReceivedResponse> OnCreateEntityResponse
            {
                add => createEntityDelegates.Add(value);
                remove => createEntityDelegates.Remove(value);
            }

            public event Action<WorldCommands.DeleteEntity.ReceivedResponse> OnDeleteEntityResponse
            {
                add => deleteEntityDelegates.Add(value);
                remove => deleteEntityDelegates.Remove(value);
            }

            internal void OnReserveEntityIdsResponseInternal(
                WorldCommands.ReserveEntityIds.ReceivedResponse receivedResponse)
            {
                GameObjectDelegates.DispatchWithErrorHandling(receivedResponse, reserveEntityIdsDelegates,
                    logDispatcher);
            }

            internal void OnCreateEntityResponseInternal(WorldCommands.CreateEntity.ReceivedResponse receivedResponse)
            {
                GameObjectDelegates.DispatchWithErrorHandling(receivedResponse, createEntityDelegates, logDispatcher);
            }

            internal void OnDeleteEntityResponseInternal(WorldCommands.DeleteEntity.ReceivedResponse receivedResponse)
            {
                GameObjectDelegates.DispatchWithErrorHandling(receivedResponse, deleteEntityDelegates, logDispatcher);
            }

            [InjectableId(InjectableType.WorldCommandResponseHandler, InjectableId.NullComponentId)]
            private class WorldCommandResponseHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(
                    Entity entity,
                    EntityManager entityManager,
                    ILogDispatcher logDispatcher)
                {
                    return new WorldCommandResponseHandler(logDispatcher);
                }
            }
        }
    }
}

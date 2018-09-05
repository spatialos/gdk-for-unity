using System;
using System.Collections.Generic;
using Improbable.Worker.Core;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

#region Diagnostic control

// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local

#endregion

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public static partial class WorldCommandsRequirables
    {
        [InjectableId(InjectableType.WorldCommandResponseHandler)]
        [InjectionCondition(InjectionCondition.RequireNothing)]
        public class WorldCommandResponseHandler : IInjectable
        {
            private readonly ILogDispatcher logDispatcher;

            private readonly List<Action<Commands.WorldCommands.ReserveEntityIds.ReceivedResponse>>
                reserveEntityIdsDelegates
                    = new List<Action<Commands.WorldCommands.ReserveEntityIds.ReceivedResponse>>();

            private readonly List<Action<Commands.WorldCommands.CreateEntity.ReceivedResponse>> createEntityDelegates
                = new List<Action<Commands.WorldCommands.CreateEntity.ReceivedResponse>>();

            private readonly List<Action<Commands.WorldCommands.DeleteEntity.ReceivedResponse>> deleteEntityDelegates
                = new List<Action<Commands.WorldCommands.DeleteEntity.ReceivedResponse>>();

            public event Action<Commands.WorldCommands.ReserveEntityIds.ReceivedResponse> OnReserveEntityIdsResponse
            {
                add => reserveEntityIdsDelegates.Add(value);
                remove => reserveEntityIdsDelegates.Remove(value);
            }

            public event Action<Commands.WorldCommands.CreateEntity.ReceivedResponse> OnCreateEntityResponse
            {
                add => createEntityDelegates.Add(value);
                remove => createEntityDelegates.Remove(value);
            }

            public event Action<Commands.WorldCommands.DeleteEntity.ReceivedResponse> OnDeleteEntityResponse
            {
                add => deleteEntityDelegates.Add(value);
                remove => deleteEntityDelegates.Remove(value);
            }

            private WorldCommandResponseHandler(ILogDispatcher logDispatcher)
            {
                this.logDispatcher = logDispatcher;
            }

            internal void OnReserveEntityIdsResponseInternal(
                Commands.WorldCommands.ReserveEntityIds.ReceivedResponse receivedResponse)
            {
                GameObjectDelegates.DispatchWithErrorHandling(
                    receivedResponse,
                    reserveEntityIdsDelegates,
                    logDispatcher);
            }

            internal void OnCreateEntityResponseInternal(
                Commands.WorldCommands.CreateEntity.ReceivedResponse receivedResponse)
            {
                GameObjectDelegates.DispatchWithErrorHandling(
                    receivedResponse,
                    createEntityDelegates,
                    logDispatcher);
            }

            internal void OnDeleteEntityResponseInternal(
                Commands.WorldCommands.DeleteEntity.ReceivedResponse receivedResponse)
            {
                GameObjectDelegates.DispatchWithErrorHandling(
                    receivedResponse,
                    deleteEntityDelegates,
                    logDispatcher);
            }

            [InjectableId(InjectableType.WorldCommandResponseHandler)]
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

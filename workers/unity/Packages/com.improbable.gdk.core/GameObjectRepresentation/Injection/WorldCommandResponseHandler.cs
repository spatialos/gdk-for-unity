using System;
using System.Collections.Generic;
using Improbable.Worker.Core;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public static partial class WorldCommands
    {
        public struct CreateEntityResponse
        {
            public CreateEntityResponseOp Op { get; }
            public Commands.WorldCommands.CreateEntity.Request RequestPayload { get; }
            public readonly object Context;

            public CreateEntityResponse(Commands.WorldCommands.CreateEntity.ReceivedResponse response, object context)
            {
                RequestPayload = response.RequestPayload;
                Op = response.Op;
                Context = context;
            }
        }

        public struct ReserveEntityIdsResponse
        {
            public ReserveEntityIdsResponseOp Op { get; }
            public Commands.WorldCommands.ReserveEntityIds.Request RequestPayload { get; }
            public readonly object Context;

            public ReserveEntityIdsResponse(Commands.WorldCommands.ReserveEntityIds.ReceivedResponse response,
                object context)
            {
                RequestPayload = response.RequestPayload;
                Op = response.Op;
                Context = context;
            }
        }

        public struct DeleteEntityResponse
        {
            public DeleteEntityResponseOp Op { get; }
            public Commands.WorldCommands.DeleteEntity.Request RequestPayload { get; }
            public readonly object Context;

            public DeleteEntityResponse(Commands.WorldCommands.DeleteEntity.ReceivedResponse response, object context)
            {
                RequestPayload = response.RequestPayload;
                Op = response.Op;
                Context = context;
            }
        }

        [InjectableId(InjectableType.WorldCommandResponseHandler)]
        [InjectionCondition(InjectionCondition.RequireNothing)]
        public class WorldCommandResponseHandler : IInjectable
        {
            private readonly Entity entity;
            private readonly ILogDispatcher logDispatcher;

            private readonly List<Action<CreateEntityResponse>> createEntityDelegates
                = new List<Action<CreateEntityResponse>>();

            private readonly List<Action<ReserveEntityIdsResponse>> reserveEntityIdsDelegates
                = new List<Action<ReserveEntityIdsResponse>>();

            private readonly List<Action<DeleteEntityResponse>> deleteEntityDelegates
                = new List<Action<DeleteEntityResponse>>();

            public event Action<ReserveEntityIdsResponse> OnReserveEntityIdsResponse
            {
                add => reserveEntityIdsDelegates.Add(value);
                remove => reserveEntityIdsDelegates.Remove(value);
            }

            public event Action<CreateEntityResponse> OnCreateEntityResponse
            {
                add => createEntityDelegates.Add(value);
                remove => createEntityDelegates.Remove(value);
            }

            public event Action<DeleteEntityResponse> OnDeleteEntityResponse
            {
                add => deleteEntityDelegates.Add(value);
                remove => deleteEntityDelegates.Remove(value);
            }

            private WorldCommandResponseHandler(Entity entity,
                EntityManager entityManager,
                World world,
                ILogDispatcher logDispatcher)
            {
                world.GetOrCreateManager<GameObjectWorldCommandSystem>().RegisterResponseHandler(entity, this);

                this.entity = entity;
                this.logDispatcher = logDispatcher;
            }

            internal void OnCreateEntityResponseInternal(
                Commands.WorldCommands.CreateEntity.ReceivedResponse receivedResponse)
            {
                if (receivedResponse.Context is WorldCommandContext worldCommandContext
                    && worldCommandContext.OwnerEntity == entity)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(
                        new CreateEntityResponse(receivedResponse, worldCommandContext.Context),
                        createEntityDelegates,
                        logDispatcher);
                }
            }

            internal void OnReserveEntityIdsResponseInternal(
                Commands.WorldCommands.ReserveEntityIds.ReceivedResponse receivedResponse)
            {
                if (receivedResponse.Context is WorldCommandContext worldCommandContext
                    && worldCommandContext.OwnerEntity == entity)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(
                        new ReserveEntityIdsResponse(receivedResponse, worldCommandContext.Context),
                        reserveEntityIdsDelegates,
                        logDispatcher);
                }
            }

            internal void OnDeleteEntityResponseInternal(
                Commands.WorldCommands.DeleteEntity.ReceivedResponse receivedResponse)
            {
                if (receivedResponse.Context is WorldCommandContext worldCommandContext
                    && worldCommandContext.OwnerEntity == entity)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(
                        new DeleteEntityResponse(receivedResponse, worldCommandContext.Context),
                        deleteEntityDelegates,
                        logDispatcher);
                }
            }

            [InjectableId(InjectableType.WorldCommandResponseHandler)]
            // ReSharper disable once UnusedMember.Local
            private class WorldCommandResponseHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(
                    Entity entity,
                    EntityManager entityManager,
                    World world,
                    ILogDispatcher logDispatcher)
                {
                    return new WorldCommandResponseHandler(entity, entityManager, world, logDispatcher);
                }
            }
        }
    }
}

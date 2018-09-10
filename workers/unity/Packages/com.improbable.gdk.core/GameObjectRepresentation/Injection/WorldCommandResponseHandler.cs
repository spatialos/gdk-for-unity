using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

#region Diagnostic control

// ReSharper disable UnusedMember.Local

#endregion


namespace Improbable.Gdk.Core.Commands
{
    public static partial class WorldCommands
    {
        public static partial class Requirable
        {
            [InjectableId(InjectableType.WorldCommandResponseHandler, InjectableId.NullComponentId)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class WorldCommandResponseHandler : RequirableBase
            {
                private readonly ILogDispatcher logDispatcher;

                private readonly List<Action<ReserveEntityIds.ReceivedResponse>>
                    reserveEntityIdsDelegates;

                private readonly List<Action<CreateEntity.ReceivedResponse>> createEntityDelegates;
                private readonly List<Action<DeleteEntity.ReceivedResponse>> deleteEntityDelegates;
                private readonly List<Action<EntityQuery.ReceivedResponse>> entityQueryDelegates;

                private WorldCommandResponseHandler(ILogDispatcher logDispatcher) : base(logDispatcher)
                {
                    this.logDispatcher = logDispatcher;
                    reserveEntityIdsDelegates = new List<Action<ReserveEntityIds.ReceivedResponse>>();
                    createEntityDelegates = new List<Action<CreateEntity.ReceivedResponse>>();
                    deleteEntityDelegates = new List<Action<DeleteEntity.ReceivedResponse>>();
                    entityQueryDelegates = new List<Action<EntityQuery.ReceivedResponse>>();
                }

                public event Action<ReserveEntityIds.ReceivedResponse> OnReserveEntityIdsResponse
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        reserveEntityIdsDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        reserveEntityIdsDelegates.Remove(value);
                    }
                }

                public event Action<CreateEntity.ReceivedResponse> OnCreateEntityResponse
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        createEntityDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        createEntityDelegates.Remove(value);
                    }
                }

                public event Action<DeleteEntity.ReceivedResponse> OnDeleteEntityResponse
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        deleteEntityDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        deleteEntityDelegates.Remove(value);
                    }
                }

                public event Action<EntityQuery.ReceivedResponse> OnEntityQueryResponse
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        entityQueryDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        entityQueryDelegates.Remove(value);
                    }
                }

                internal void OnReserveEntityIdsResponseInternal(ReserveEntityIds.ReceivedResponse receivedResponse)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(receivedResponse, reserveEntityIdsDelegates,
                        logDispatcher);
                }

                internal void OnCreateEntityResponseInternal(CreateEntity.ReceivedResponse receivedResponse)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(receivedResponse, createEntityDelegates,
                        logDispatcher);
                }

                internal void OnDeleteEntityResponseInternal(DeleteEntity.ReceivedResponse receivedResponse)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(receivedResponse, deleteEntityDelegates,
                        logDispatcher);
                }

                internal void OnEntityQueryResponseInternal(EntityQuery.ReceivedResponse receivedResponse)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(receivedResponse, entityQueryDelegates,
                        logDispatcher);
                }

                [InjectableId(InjectableType.WorldCommandResponseHandler, InjectableId.NullComponentId)]
                private class WorldCommandResponseHandlerCreator : IInjectableCreator
                {
                    public IInjectable CreateInjectable(Entity entity, EntityManager entityManager,
                        ILogDispatcher logDispatcher)
                    {
                        return new WorldCommandResponseHandler(logDispatcher);
                    }
                }
            }
        }
    }
}

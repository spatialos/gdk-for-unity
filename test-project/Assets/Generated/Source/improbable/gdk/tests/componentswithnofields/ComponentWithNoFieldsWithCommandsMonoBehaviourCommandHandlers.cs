
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public partial class Cmd
        {
            public struct RequestResponder
            {
                private readonly EntityManager entityManager;
                private readonly Entity entity;
                public Cmd.ReceivedRequest Request { get; }

                internal RequestResponder(EntityManager entityManager, Entity entity, Cmd.ReceivedRequest request)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    Request = request;
                }

                public void SendResponse(global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload)
                {
                    entityManager.GetComponentData<CommandResponders.Cmd>(entity).ResponsesToSend
                        .Add(Cmd.CreateResponse(Request, payload));
                }

                public void SendResponseFailure(string message)
                {
                    entityManager.GetComponentData<CommandResponders.Cmd>(entity).ResponsesToSend
                        .Add(Cmd.CreateResponseFailure(Request, message));
                }
            }
        }

        public partial class Requirable
        {
            [InjectableId(InjectableType.CommandRequestSender, 1005)]
            internal class CommandRequestSenderCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestSender(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestSender, 1005)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class CommandRequestSender : RequirableBase, ICommandRequestSender
            {
                private Entity entity;
                private readonly EntityManager entityManager;
                private readonly ILogDispatcher logger;

                public CommandRequestSender(Entity entity, EntityManager entityManager, ILogDispatcher logger) : base(logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    this.logger = logger;
                }

                public CommandTypeInformation<Cmd, global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty, Cmd.ReceivedResponse> CmdTypeInformation;

                public long SendCmdRequest(EntityId entityId, global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload,
                    uint? timeoutMillis = null, bool allowShortCircuiting = false, object context = null)
                {
                    if (!IsValid())
                    {
                        return -1;
                    }

                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.Cmd>(entity);
                    var request = Cmd.CreateRequest(entityId, payload, timeoutMillis, allowShortCircuiting, context);
                    ecsCommandRequestSender.RequestsToSend.Add(request);
                    return request.RequestId;
                }

                public long SendCmdRequest(EntityId entityId, global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload,
                    Action<Cmd.ReceivedResponse> callback, uint? timeoutMillis = null, bool allowShortCircuiting = false)
                {
                    if (!IsValid())
                    {
                        return -1;
                    }

                    Action<Cmd.ReceivedResponse> wrappedCallback = response =>
                    {
                        if (this.IsValid() && callback != null)
                        {
                            callback(response);
                        }
                    };

                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.Cmd>(entity);
                    var request = Cmd.CreateRequest(entityId, payload, timeoutMillis, allowShortCircuiting, callback);
                    ecsCommandRequestSender.RequestsToSend.Add(request);
                    return request.RequestId;
                }

                long ICommandRequestSender.SendCommand<TCommand, TRequest, TResponse>(CommandTypeInformation<TCommand, TRequest, TResponse> commandTypeInformation, EntityId entityId, TRequest request,
                    Action<TResponse> callback, uint? timeoutMillis = null, bool allowShortCircuiting = false)
                {
                    if (typeof(TCommand) == typeof(Cmd))
                    {
                        if (callback != null && !(typeof(TResponse) == typeof(Cmd.ReceivedResponse)))
                        {
                            throw new ArgumentException(
                                $"Callback for command {nameof(Cmd)} must be an Action taking type {typeof(Cmd.ReceivedResponse).FullName}");
                        }

                        // Can not directly cast to a struct and can not use unsafe code as the request type can not be constrained using unmanaged
                        switch (request)
                        {
                            case global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty concreteRequest:
                            {
                                var concreteCallback = callback as Action<Cmd.ReceivedResponse>;
                                return SendCmdRequest(entityId, concreteRequest, concreteCallback, timeoutMillis,
                                    allowShortCircuiting);
                            }
                            default:
                                throw new ArgumentException(
                                    $"Request payload for command Cmd, must be of type {nameof(global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty)}");
                        }
                    }


                    throw new ArgumentException($"Can not send unknown command {typeof(TRequest)}");
                }
            }

            [InjectableId(InjectableType.CommandRequestHandler, 1005)]
            internal class CommandRequestHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestHandler, 1005)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public class CommandRequestHandler : RequirableBase
            {
                private Entity entity;
                private readonly EntityManager entityManager;
                private readonly ILogDispatcher logger;

                public CommandRequestHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger) : base(logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    this.logger = logger;
                }
                private readonly List<Action<Cmd.RequestResponder>> cmdDelegates = new List<Action<Cmd.RequestResponder>>();
                public event Action<Cmd.RequestResponder> OnCmdRequest
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        cmdDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        cmdDelegates.Remove(value);
                    }
                }

                internal void OnCmdRequestInternal(Cmd.ReceivedRequest request)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(new Cmd.RequestResponder(entityManager, entity, request), cmdDelegates, logger);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 1005)]
            internal class CommandResponseHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandResponseHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 1005)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class CommandResponseHandler : RequirableBase
            {
                private Entity entity;
                private readonly EntityManager entityManager;
                private readonly ILogDispatcher logger;

                public CommandResponseHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger) : base(logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    this.logger = logger;
                }

                private readonly List<Action<Cmd.ReceivedResponse>> cmdDelegates = new List<Action<Cmd.ReceivedResponse>>();
                public event Action<Cmd.ReceivedResponse> OnCmdResponse
                {
                    add
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        cmdDelegates.Add(value);
                    }
                    remove
                    {
                        if (!IsValid())
                        {
                            return;
                        }

                        cmdDelegates.Remove(value);
                    }
                }

                internal void OnCmdResponseInternal(Cmd.ReceivedResponse response)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(response, cmdDelegates, logger);
                }
            }
        }
    }
}


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
            public struct ResponseSender
            {
                private readonly EntityManager entityManager;
                private readonly Entity entity;
                public Cmd.ReceivedRequest Request { get; }

                internal ResponseSender(EntityManager entityManager, Entity entity, Cmd.ReceivedRequest request)
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
            public class CommandRequestSender : RequirableBase
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

                public long SendCmdRequest(EntityId entityId, global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload,
                    uint? timeoutMillis = null, bool allowShortCircuiting = false, object context = null)
                {
                    if (!VerifyNotDisposed())
                    {
                        return -1;
                    }

                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.Cmd>(entity);
                    var request = Cmd.CreateRequest(entityId, payload, timeoutMillis, allowShortCircuiting, context);
                    ecsCommandRequestSender.RequestsToSend.Add(request);
                    return request.RequestId;
                }

            }

            [InjectableId(InjectableType.CommandRequestReceiver, 1005)]
            internal class CommandRequestReceiverCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestReceiver(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestReceiver, 1005)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public class CommandRequestReceiver : RequirableBase
            {
                private Entity entity;
                private readonly EntityManager entityManager;
                private readonly ILogDispatcher logger;

                public CommandRequestReceiver(Entity entity, EntityManager entityManager, ILogDispatcher logger) : base(logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    this.logger = logger;
                }
                private readonly List<Action<Cmd.ResponseSender>> cmdDelegates = new List<Action<Cmd.ResponseSender>>();
                public event Action<Cmd.ResponseSender> OnCmdRequest
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        cmdDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        cmdDelegates.Remove(value);
                    }
                }

                internal void OnCmdRequestInternal(Cmd.ReceivedRequest request)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(new Cmd.ResponseSender(entityManager, entity, request), cmdDelegates, logger);
                }
            }

            [InjectableId(InjectableType.CommandResponseReceiver, 1005)]
            internal class CommandResponseReceiverCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandResponseReceiver(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandResponseReceiver, 1005)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class CommandResponseReceiver : RequirableBase
            {
                private Entity entity;
                private readonly EntityManager entityManager;
                private readonly ILogDispatcher logger;

                public CommandResponseReceiver(Entity entity, EntityManager entityManager, ILogDispatcher logger) : base(logger)
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
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        cmdDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
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

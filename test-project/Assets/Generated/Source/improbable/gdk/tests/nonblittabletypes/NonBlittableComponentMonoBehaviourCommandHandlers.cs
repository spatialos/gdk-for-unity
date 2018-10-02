
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

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public partial class FirstCommand
        {
            public struct ResponseSender
            {
                private readonly EntityManager entityManager;
                private readonly Entity entity;
                public FirstCommand.ReceivedRequest Request { get; }

                internal ResponseSender(EntityManager entityManager, Entity entity, FirstCommand.ReceivedRequest request)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    Request = request;
                }

                public void SendResponse(global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse payload)
                {
                    entityManager.GetComponentData<CommandResponders.FirstCommand>(entity).ResponsesToSend
                        .Add(FirstCommand.CreateResponse(Request, payload));
                }

                public void SendResponseFailure(string message)
                {
                    entityManager.GetComponentData<CommandResponders.FirstCommand>(entity).ResponsesToSend
                        .Add(FirstCommand.CreateResponseFailure(Request, message));
                }
            }
        }

        public partial class SecondCommand
        {
            public struct ResponseSender
            {
                private readonly EntityManager entityManager;
                private readonly Entity entity;
                public SecondCommand.ReceivedRequest Request { get; }

                internal ResponseSender(EntityManager entityManager, Entity entity, SecondCommand.ReceivedRequest request)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    Request = request;
                }

                public void SendResponse(global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse payload)
                {
                    entityManager.GetComponentData<CommandResponders.SecondCommand>(entity).ResponsesToSend
                        .Add(SecondCommand.CreateResponse(Request, payload));
                }

                public void SendResponseFailure(string message)
                {
                    entityManager.GetComponentData<CommandResponders.SecondCommand>(entity).ResponsesToSend
                        .Add(SecondCommand.CreateResponseFailure(Request, message));
                }
            }
        }

        public partial class Requirable
        {
            [InjectableId(InjectableType.CommandRequestSender, 1002)]
            internal class CommandRequestSenderCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestSender(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestSender, 1002)]
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

                public long SendFirstCommandRequest(EntityId entityId, global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest payload,
                    uint? timeoutMillis = null, bool allowShortCircuiting = false, object context = null)
                {
                    if (!VerifyNotDisposed())
                    {
                        return -1;
                    }

                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.FirstCommand>(entity);
                    var request = FirstCommand.CreateRequest(entityId, payload, timeoutMillis, allowShortCircuiting, context);
                    ecsCommandRequestSender.RequestsToSend.Add(request);
                    return request.RequestId;
                }

                public long SendSecondCommandRequest(EntityId entityId, global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest payload,
                    uint? timeoutMillis = null, bool allowShortCircuiting = false, object context = null)
                {
                    if (!VerifyNotDisposed())
                    {
                        return -1;
                    }

                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.SecondCommand>(entity);
                    var request = SecondCommand.CreateRequest(entityId, payload, timeoutMillis, allowShortCircuiting, context);
                    ecsCommandRequestSender.RequestsToSend.Add(request);
                    return request.RequestId;
                }

            }

            [InjectableId(InjectableType.CommandRequestReceiver, 1002)]
            internal class CommandRequestReceiverCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestReceiver(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestReceiver, 1002)]
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
                private readonly List<Action<FirstCommand.ResponseSender>> firstCommandDelegates = new List<Action<FirstCommand.ResponseSender>>();
                public event Action<FirstCommand.ResponseSender> OnFirstCommandRequest
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        firstCommandDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        firstCommandDelegates.Remove(value);
                    }
                }

                internal void OnFirstCommandRequestInternal(FirstCommand.ReceivedRequest request)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(new FirstCommand.ResponseSender(entityManager, entity, request), firstCommandDelegates, logger);
                }
                private readonly List<Action<SecondCommand.ResponseSender>> secondCommandDelegates = new List<Action<SecondCommand.ResponseSender>>();
                public event Action<SecondCommand.ResponseSender> OnSecondCommandRequest
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        secondCommandDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        secondCommandDelegates.Remove(value);
                    }
                }

                internal void OnSecondCommandRequestInternal(SecondCommand.ReceivedRequest request)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(new SecondCommand.ResponseSender(entityManager, entity, request), secondCommandDelegates, logger);
                }
            }

            [InjectableId(InjectableType.CommandResponseReceiver, 1002)]
            internal class CommandResponseReceiverCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandResponseReceiver(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandResponseReceiver, 1002)]
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

                private readonly List<Action<FirstCommand.ReceivedResponse>> firstCommandDelegates = new List<Action<FirstCommand.ReceivedResponse>>();
                public event Action<FirstCommand.ReceivedResponse> OnFirstCommandResponse
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        firstCommandDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        firstCommandDelegates.Remove(value);
                    }
                }

                internal void OnFirstCommandResponseInternal(FirstCommand.ReceivedResponse response)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(response, firstCommandDelegates, logger);
                }

                private readonly List<Action<SecondCommand.ReceivedResponse>> secondCommandDelegates = new List<Action<SecondCommand.ReceivedResponse>>();
                public event Action<SecondCommand.ReceivedResponse> OnSecondCommandResponse
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        secondCommandDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        secondCommandDelegates.Remove(value);
                    }
                }

                internal void OnSecondCommandResponseInternal(SecondCommand.ReceivedResponse response)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(response, secondCommandDelegates, logger);
                }
            }
        }
    }
}


// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public partial class Requirables
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
            public class CommandRequestSender : IInjectable
            {
                private Entity entity;
                private readonly EntityManager entityManager;
                private readonly ILogDispatcher logger;

                public CommandRequestSender(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    this.logger = logger;
                }

                public void SendFirstCommandRequest(EntityId entityId, global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest request)
                {
                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.FirstCommand>(entity);
                    ecsCommandRequestSender.RequestsToSend.Add(FirstCommand.CreateRequest(entityId, request));
                }

                public void SendSecondCommandRequest(EntityId entityId, global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest request)
                {
                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.SecondCommand>(entity);
                    ecsCommandRequestSender.RequestsToSend.Add(SecondCommand.CreateRequest(entityId, request));
                }

            }

            [InjectableId(InjectableType.CommandRequestHandler, 1002)]
            internal class CommandRequestHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestHandler, 1002)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public class CommandRequestHandler : IInjectable
            {
                private Entity entity;
                private readonly EntityManager entityManager;
                private readonly ILogDispatcher logger;

                public CommandRequestHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    this.logger = logger;
                }

                private readonly List<Action<FirstCommand.ReceivedRequest>> firstCommandDelegates = new List<Action<FirstCommand.ReceivedRequest>>();
                public event Action<FirstCommand.ReceivedRequest> OnFirstCommandRequest
                {
                    add => firstCommandDelegates.Add(value);
                    remove => firstCommandDelegates.Remove(value);
                }

                internal void OnFirstCommandRequestInternal(FirstCommand.ReceivedRequest request)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(request, firstCommandDelegates, logger);
                }

                public void SendFirstCommandResponse(FirstCommand.Response response)
                {
                    entityManager
                        .GetComponentData<CommandResponders.FirstCommand>(entity)
                        .ResponsesToSend
                        .Add(response);
                }

                private readonly List<Action<SecondCommand.ReceivedRequest>> secondCommandDelegates = new List<Action<SecondCommand.ReceivedRequest>>();
                public event Action<SecondCommand.ReceivedRequest> OnSecondCommandRequest
                {
                    add => secondCommandDelegates.Add(value);
                    remove => secondCommandDelegates.Remove(value);
                }

                internal void OnSecondCommandRequestInternal(SecondCommand.ReceivedRequest request)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(request, secondCommandDelegates, logger);
                }

                public void SendSecondCommandResponse(SecondCommand.Response response)
                {
                    entityManager
                        .GetComponentData<CommandResponders.SecondCommand>(entity)
                        .ResponsesToSend
                        .Add(response);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 1002)]
            internal class CommandResponseHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandResponseHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 1002)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class CommandResponseHandler : IInjectable
            {
                private Entity entity;
                private readonly EntityManager entityManager;
                private readonly ILogDispatcher logger;

                public CommandResponseHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    this.logger = logger;
                }

                private readonly List<Action<FirstCommand.ReceivedResponse>> firstCommandDelegates = new List<Action<FirstCommand.ReceivedResponse>>();
                public event Action<FirstCommand.ReceivedResponse> OnFirstCommandResponse
                {
                    add => firstCommandDelegates.Add(value);
                    remove => firstCommandDelegates.Remove(value);
                }

                internal void OnFirstCommandResponseInternal(FirstCommand.ReceivedResponse response)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(response, firstCommandDelegates, logger);
                }

                private readonly List<Action<SecondCommand.ReceivedResponse>> secondCommandDelegates = new List<Action<SecondCommand.ReceivedResponse>>();
                public event Action<SecondCommand.ReceivedResponse> OnSecondCommandResponse
                {
                    add => secondCommandDelegates.Add(value);
                    remove => secondCommandDelegates.Remove(value);
                }

                internal void OnSecondCommandResponseInternal(SecondCommand.ReceivedResponse response)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(response, secondCommandDelegates, logger);
                }
            }
        }
    }
}

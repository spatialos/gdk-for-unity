
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

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public partial class Requirables
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

                public void SendCmdRequest(EntityId entityId, global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty request)
                {
                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.Cmd>(entity);
                    ecsCommandRequestSender.RequestsToSend.Add(Cmd.CreateRequest(entityId, request));
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

                private readonly List<Action<Cmd.ReceivedRequest>> cmdDelegates = new List<Action<Cmd.ReceivedRequest>>();
                public event Action<Cmd.ReceivedRequest> OnCmdRequest
                {
                    add => cmdDelegates.Add(value);
                    remove => cmdDelegates.Remove(value);
                }

                internal void OnCmdRequestInternal(Cmd.ReceivedRequest request)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(request, cmdDelegates, logger);
                }

                public void SendCmdResponse(Cmd.Response response)
                {
                    entityManager
                        .GetComponentData<CommandResponders.Cmd>(entity)
                        .ResponsesToSend
                        .Add(response);
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

                private readonly List<Action<Cmd.ReceivedResponse>> cmdDelegates = new List<Action<Cmd.ReceivedResponse>>();
                public event Action<Cmd.ReceivedResponse> OnCmdResponse
                {
                    add => cmdDelegates.Add(value);
                    remove => cmdDelegates.Remove(value);
                }

                internal void OnCmdResponseInternal(Cmd.ReceivedResponse response)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(response, cmdDelegates, logger);
                }
            }
        }
    }
}

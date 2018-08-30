
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

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public partial class Requirables
        {
            [InjectableId(InjectableType.CommandRequestSender, 1001)]
            internal class CommandRequestSenderCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestSender(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestSender, 1001)]
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

                public void SendFirstCommandRequest(EntityId entityId, global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest request)
                {
                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.FirstCommand>(entity);
                    ecsCommandRequestSender.RequestsToSend.Add(FirstCommand.CreateRequest(entityId, request));
                }

                public void SendSecondCommandRequest(EntityId entityId, global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest request)
                {
                    var ecsCommandRequestSender = entityManager.GetComponentData<CommandSenders.SecondCommand>(entity);
                    ecsCommandRequestSender.RequestsToSend.Add(SecondCommand.CreateRequest(entityId, request));
                }

            }

            [InjectableId(InjectableType.CommandRequestHandler, 1001)]
            internal class CommandRequestHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestHandler, 1001)]
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

                private readonly List<Action<FirstCommand.ReceivedRequest>> FirstCommandDelegates = new List<Action<FirstCommand.ReceivedRequest>>();
                public event Action<FirstCommand.ReceivedRequest> OnFirstCommandRequest
                {
                    add => FirstCommandDelegates.Add(value);
                    remove => FirstCommandDelegates.Remove(value);
                }

                internal void OnFirstCommandRequestInternal(FirstCommand.ReceivedRequest request)
                {
                    foreach (var callback in FirstCommandDelegates)
                    {
                        try
                        {
                            callback(request);
                        }
                        catch (Exception e)
                        {
                            // Log the exception but do not rethrow it, as other delegates should still get called
                            logger.HandleLog(LogType.Exception, new LogEvent().WithException(e));
                        }
                    }
                }

                private readonly List<Action<SecondCommand.ReceivedRequest>> SecondCommandDelegates = new List<Action<SecondCommand.ReceivedRequest>>();
                public event Action<SecondCommand.ReceivedRequest> OnSecondCommandRequest
                {
                    add => SecondCommandDelegates.Add(value);
                    remove => SecondCommandDelegates.Remove(value);
                }

                internal void OnSecondCommandRequestInternal(SecondCommand.ReceivedRequest request)
                {
                    foreach (var callback in SecondCommandDelegates)
                    {
                        try
                        {
                            callback(request);
                        }
                        catch (Exception e)
                        {
                            // Log the exception but do not rethrow it, as other delegates should still get called
                            logger.HandleLog(LogType.Exception, new LogEvent().WithException(e));
                        }
                    }
                }

            }

            [InjectableId(InjectableType.CommandResponseHandler, 1001)]
            internal class CommandResponseHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandResponseHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 1001)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class CommandResponseHandler : IInjectable
            {
                public CommandResponseHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }
        }
    }
}

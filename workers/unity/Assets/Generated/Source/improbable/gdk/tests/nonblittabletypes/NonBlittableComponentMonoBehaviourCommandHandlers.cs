
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Unity.Entities;
using UnityEngine;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public partial class Requirables
        {
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

                public CommandRequestSender(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                }

                public void SendFirstCommandRequest(EntityId entityId, global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest request)
                {
                    try
                    {
                        var ecsCommandRequestSender = entityManager.GetComponentData<CommandRequestSender<SpatialOSNonBlittableComponent>>(entity);
                        ecsCommandRequestSender.SendFirstCommandRequest(entityId.Id, request);
                    }
                    catch (Exception e)
                    {
                        throw new SendCommandRequestFailedException(e, entity.Index);
                    }
                    
                }

                public void SendSecondCommandRequest(EntityId entityId, global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest request)
                {
                    try
                    {
                        var ecsCommandRequestSender = entityManager.GetComponentData<CommandRequestSender<SpatialOSNonBlittableComponent>>(entity);
                        ecsCommandRequestSender.SendSecondCommandRequest(entityId.Id, request);
                    }
                    catch (Exception e)
                    {
                        throw new SendCommandRequestFailedException(e, entity.Index);
                    }
                    
                }

            }

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
                private ILogDispatcher logger;

                public CommandRequestHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {
                    this.entity = entity;
                    this.entityManager = entityManager;
                    this.logger = logger;
                }

                private readonly List<Action<FirstCommand.Request>> spatialOSFirstCommandDelegates = new List<Action<FirstCommand.Request>>();
                public event Action<FirstCommand.Request> OnFirstCommandRequest
                {
                    add => spatialOSFirstCommandDelegates.Add(value);
                    remove => spatialOSFirstCommandDelegates.Remove(value);
                }

                public void OnFirstCommandRequestInternal(FirstCommand.Request request)
                {
                    foreach (var callback in spatialOSFirstCommandDelegates)
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
                private readonly List<Action<SecondCommand.Request>> spatialOSSecondCommandDelegates = new List<Action<SecondCommand.Request>>();
                public event Action<SecondCommand.Request> OnSecondCommandRequest
                {
                    add => spatialOSSecondCommandDelegates.Add(value);
                    remove => spatialOSSecondCommandDelegates.Remove(value);
                }

                public void OnSecondCommandRequestInternal(SecondCommand.Request request)
                {
                    foreach (var callback in spatialOSSecondCommandDelegates)
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
                public CommandResponseHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }
        }
    }
}

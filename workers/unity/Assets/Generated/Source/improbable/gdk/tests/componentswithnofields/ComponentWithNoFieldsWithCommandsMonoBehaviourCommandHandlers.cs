
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

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
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

            [InjectableId(InjectableType.CommandRequestSender, 1005)]
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

                public void SendCmdRequest(EntityId entityId, global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty request)
                {
                    try
                    {
                        var ecsCommandRequestSender = entityManager.GetComponentData<CommandRequestSender<SpatialOSComponentWithNoFieldsWithCommands>>(entity);
                        ecsCommandRequestSender.SendCmdRequest(entityId.Id, request);
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

            [InjectableId(InjectableType.CommandRequestHandler, 1005)]
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

                private readonly List<Action<Cmd.Request>> spatialOSCmdDelegates = new List<Action<Cmd.Request>>();
                public event Action<Cmd.Request> OnCmdRequest
                {
                    add => spatialOSCmdDelegates.Add(value);
                    remove => spatialOSCmdDelegates.Remove(value);
                }

                public void OnCmdRequestInternal(Cmd.Request request)
                {
                    foreach (var callback in spatialOSCmdDelegates)
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

            [InjectableId(InjectableType.CommandResponseHandler, 1005)]
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

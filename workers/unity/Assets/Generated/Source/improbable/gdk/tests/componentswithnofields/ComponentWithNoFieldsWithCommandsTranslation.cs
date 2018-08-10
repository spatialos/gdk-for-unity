// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Improbable.Worker;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Components;
using Improbable.Gdk.Tests.ComponentsWithNoFields;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public class Translation : ComponentTranslation, IDispatcherCallbacks<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands>
        {
            private const string LoggerName = "ComponentWithNoFieldsWithCommands.Translation";
        
            public override ComponentType TargetComponentType => targetComponentType;
            private static readonly ComponentType targetComponentType = typeof(SpatialOSComponentWithNoFieldsWithCommands);

            public override ComponentType[] ReplicationComponentTypes => replicationComponentTypes;
            private static readonly ComponentType[] replicationComponentTypes = { typeof(SpatialOSComponentWithNoFieldsWithCommands), typeof(Authoritative<SpatialOSComponentWithNoFieldsWithCommands>), typeof(SpatialEntityId)};

            public override ComponentType[] CleanUpComponentTypes => cleanUpComponentTypes;
            private static readonly ComponentType[] cleanUpComponentTypes = 
            { 
                typeof(AuthoritiesChanged<SpatialOSComponentWithNoFieldsWithCommands>),
                typeof(ComponentAdded<SpatialOSComponentWithNoFieldsWithCommands>),
                typeof(ComponentRemoved<SpatialOSComponentWithNoFieldsWithCommands>),
                typeof(CommandRequests<Cmd.Request>), typeof(CommandResponses<Cmd.Response>),
            };

            private readonly Dictionary<uint, RequestContext> RequestIdToRequestContext = new Dictionary<uint, RequestContext>();

            internal List<Cmd.OutgoingRequest> CmdRequests = new List<Cmd.OutgoingRequest>();
            internal List<Cmd.OutgoingResponse> CmdResponses = new List<Cmd.OutgoingResponse>();
            internal List<CommandFailure> CmdFailure = new List<CommandFailure>();
            private static readonly ComponentPool<CommandRequests<Cmd.Request>> CmdRequestPool =
                new ComponentPool<CommandRequests<Cmd.Request>>(
                    () => new CommandRequests<Cmd.Request>(),
                    (component) => component.Buffer.Clear());
            private static readonly ComponentPool<CommandResponses<Cmd.Response>> CmdResponsePool =
                new ComponentPool<CommandResponses<Cmd.Response>>(
                    () => new CommandResponses<Cmd.Response>(),
                    (component) => component.Buffer.Clear());


            private static readonly ComponentPool<AuthoritiesChanged<SpatialOSComponentWithNoFieldsWithCommands>> AuthsPool =
                new ComponentPool<AuthoritiesChanged<SpatialOSComponentWithNoFieldsWithCommands>>(
                    () => new AuthoritiesChanged<SpatialOSComponentWithNoFieldsWithCommands>(),
                    (component) => component.Buffer.Clear());


            public Translation(MutableView view) : base(view)
            {
            }

            public override void RegisterWithDispatcher(Dispatcher dispatcher)
            {
                dispatcher.OnAddComponent<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands>(OnAddComponent);
                dispatcher.OnComponentUpdate<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands>(OnComponentUpdate);
                dispatcher.OnRemoveComponent<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands>(OnRemoveComponent);
                dispatcher.OnAuthorityChange<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands>(OnAuthorityChange);

                dispatcher.OnCommandRequest<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Commands.Cmd>(OnCommandRequestCmd);
                dispatcher.OnCommandResponse<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Commands.Cmd>(OnCommandResponseCmd);
            }

            public override void AddCommandRequestSender(Unity.Entities.Entity entity, long entityId)
            {
                view.AddComponent(entity, new CommandRequestSender<SpatialOSComponentWithNoFieldsWithCommands>(entityId, translationHandle));
            }

            public void OnAddComponent(AddComponentOp<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnAddComponent.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithCommands"));
                    return;
                }

                var spatialOSComponentWithNoFieldsWithCommands = new SpatialOSComponentWithNoFieldsWithCommands();
                spatialOSComponentWithNoFieldsWithCommands.DirtyBit = false;

                view.AddComponent(entity, spatialOSComponentWithNoFieldsWithCommands);
                view.AddComponent(entity, new NotAuthoritative<SpatialOSComponentWithNoFieldsWithCommands>());

                if (view.HasComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                {
                    view.RemoveComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithCommands>>(entity);
                }
                else if (!view.HasComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                {
                    view.AddComponent(entity, new ComponentAdded<SpatialOSComponentWithNoFieldsWithCommands>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "Received ComponentAdded but have already received one for this entity.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithCommands"));
                }
            }

            public void OnComponentUpdate(ComponentUpdateOp<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnComponentUpdate.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithCommands"));
                    return;
                }

                var componentData = view.GetComponent<SpatialOSComponentWithNoFieldsWithCommands>(entity);


                componentData.DirtyBit = false;

                view.SetComponentData(entity, componentData);

            }

            public void OnRemoveComponent(RemoveComponentOp op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnRemoveComponent.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithCommands"));
                    return;
                }

                view.RemoveComponent<SpatialOSComponentWithNoFieldsWithCommands>(entity);

                if (view.HasComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                {
                    view.RemoveComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithCommands>>(entity);
                }
                else if (!view.HasComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                {
                    view.AddComponent(entity, new ComponentRemoved<SpatialOSComponentWithNoFieldsWithCommands>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                            "Received ComponentRemoved but have already received one for this entity.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithCommands"));
                }
            }

            public void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = op.EntityId.Id;
                view.HandleAuthorityChange(entityId, op.Authority, AuthsPool);
            }

            public override void ExecuteReplication(Connection connection)
            {
                var componentDataArray = ReplicationComponentGroup.GetComponentDataArray<SpatialOSComponentWithNoFieldsWithCommands>();
                var spatialEntityIdData = ReplicationComponentGroup.GetComponentDataArray<SpatialEntityId>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var componentData = componentDataArray[i];
                    var entityId = spatialEntityIdData[i].EntityId;
                    var hasPendingEvents = false;

                    if (componentData.DirtyBit || hasPendingEvents)
                    {
                        var update = new global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Update();
                        SendComponentUpdate(connection, entityId, update);

                        componentData.DirtyBit = false;
                        componentDataArray[i] = componentData;

                    }
                }
            }

            public static void SendComponentUpdate(Connection connection, long entityId, global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Update update)
            {
                connection.SendComponentUpdate(new global::Improbable.EntityId(entityId), update);
            }

            public override void CleanUpComponents(ref EntityCommandBuffer entityCommandBuffer)
            {
                RemoveComponents(ref entityCommandBuffer, AuthsPool, groupIndex: 0);
                RemoveComponents<ComponentAdded<SpatialOSComponentWithNoFieldsWithCommands>>(ref entityCommandBuffer, groupIndex: 1);
                RemoveComponents<ComponentRemoved<SpatialOSComponentWithNoFieldsWithCommands>>(ref entityCommandBuffer, groupIndex: 2);
                
                RemoveComponents(ref entityCommandBuffer, CmdRequestPool, groupIndex: 3);
                RemoveComponents(ref entityCommandBuffer, CmdResponsePool, groupIndex: 4);
                
            }

            public void OnCommandRequestCmd(CommandRequestOp<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Commands.Cmd> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnCommandRequest.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithCommands"));
                    return;
                }

                var requestPayload = op.Request.Get().Value;
                var unityRequestPayload = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.ToNative(requestPayload);
                var request = new Cmd.Request(op.RequestId.Id, this, op.CallerWorkerId, op.CallerAttributeSet, unityRequestPayload);

                view.AddCommandRequest(entity, request, CmdRequestPool);
            }

            public void OnCommandResponseCmd(CommandResponseOp<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Commands.Cmd> op)
            {
                var requestId = op.RequestId.Id;
                RequestContext requestContext;
                if (!RequestIdToRequestContext.TryGetValue(requestId, out requestContext))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent("Entity not found during OnCommandResponse.")
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField(MutableView.Component, "SpatialOSComponentWithNoFieldsWithCommands"));
                    return;
                }

                RequestIdToRequestContext.Remove(requestId);

                Unity.Entities.Entity entity;
                if (requestContext.EntityId == MutableView.WorkerEntityId)
                {
                    entity = view.WorkerEntity;
                }
                else if (!view.TryGetEntity(requestContext.EntityId, out entity))
                {
                    return;
                }

                var unityResponsePayload = op.Response.HasValue 
                    ? global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.ToNative(op.Response.Value.Get().Value) 
                    : (global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty?) null;
                var outgoingRequest = (Cmd.OutgoingRequest) requestContext.Request;
                var response = new Cmd.Response(op.EntityId.Id, op.Message, (CommandStatusCode) op.StatusCode, unityResponsePayload, outgoingRequest.RawRequest);

                view.AddCommandResponse(entity, response, CmdResponsePool);
            }

            private void SendCmdRequest(Connection connection, Cmd.OutgoingRequest outgoingRequest)
            {
                var requestPayload = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.ToSpatial(outgoingRequest.RawRequest);
                var request = new global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Commands.Cmd.Request(requestPayload);

                var requestId = connection.SendCommandRequest(new global::Improbable.EntityId(outgoingRequest.TargetEntityId), request, null);

                RequestIdToRequestContext.Add(requestId.Id, new RequestContext(outgoingRequest.SenderEntityId, outgoingRequest));
            }

            private void SendCmdResponse(Connection connection, Cmd.OutgoingResponse outgoingResponse)
            {
                var responsePayload = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.ToSpatial(outgoingResponse.RawResponse);
                var response = new global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Commands.Cmd.Response(responsePayload);

                var requestId = new RequestId<IncomingCommandRequest<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Commands.Cmd>>(
                    outgoingResponse.RequestId);

                connection.SendCommandResponse(requestId, response);
            }

            private void SendCmdFailure(Connection connection, CommandFailure failure) {
                var requestId = new RequestId<IncomingCommandRequest<global::Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Commands.Cmd>>(
                    failure.RequestId);

                connection.SendCommandFailure(requestId, failure.Message);
            }
            public override void SendCommands(Connection connection)
            {
                foreach (var request in CmdRequests)
                {
                    SendCmdRequest(connection, request);
                }
                CmdRequests.Clear();

                foreach (var response in CmdResponses)
                {
                    SendCmdResponse(connection, response);
                }
                CmdResponses.Clear();

                foreach (var failure in CmdFailure)
                {
                    SendCmdFailure(connection, failure);
                }
                CmdFailure.Clear();
            }

            public static ComponentWithNoFieldsWithCommands.Translation GetTranslation(uint internalHandleToTranslation)
            {
                return (ComponentWithNoFieldsWithCommands.Translation) ComponentTranslation.HandleToTranslation[internalHandleToTranslation];
            }
        }
    }

    public static class SpatialOSComponentWithNoFieldsWithCommandsCommandRequestHandlers
    {
        public static void SendCmdRequest(this CommandRequestSender<SpatialOSComponentWithNoFieldsWithCommands> commandRequestSender,
            long targetEntityId, global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty request)
        {
            var translation = ComponentWithNoFieldsWithCommands.Translation.GetTranslation(commandRequestSender.InternalHandleToTranslation);

            translation.CmdRequests.Add(new ComponentWithNoFieldsWithCommands.Cmd.OutgoingRequest(targetEntityId,
                commandRequestSender.EntityId, request));
        }

    }

}

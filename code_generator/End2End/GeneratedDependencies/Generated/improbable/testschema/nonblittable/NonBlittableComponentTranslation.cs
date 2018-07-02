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
using Improbable.TestSchema.Nonblittable;

namespace Generated.Improbable.TestSchema.Nonblittable
{
    public partial class NonBlittableComponent
    {
        public class Translation : ComponentTranslation, IDispatcherCallbacks<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent>
        {
            public override ComponentType TargetComponentType => targetComponentType;
            private static readonly ComponentType targetComponentType = typeof(SpatialOSNonBlittableComponent);

            public override ComponentType[] ReplicationComponentTypes => replicationComponentTypes;
            private static readonly ComponentType[] replicationComponentTypes = { typeof(SpatialOSNonBlittableComponent), typeof(Authoritative<SpatialOSNonBlittableComponent>), typeof(SpatialEntityId)};

            public override ComponentType[] CleanUpComponentTypes => cleanUpComponentTypes;
            private static readonly ComponentType[] cleanUpComponentTypes = 
            { 
                typeof(ComponentsUpdated<SpatialOSNonBlittableComponent>), typeof(AuthoritiesChanged<SpatialOSNonBlittableComponent>),
                typeof(CommandRequests<FirstCommand.Request>), typeof(CommandResponses<FirstCommand.Response>),
                typeof(CommandRequests<SecondCommand.Request>), typeof(CommandResponses<SecondCommand.Response>),
                typeof(EventsReceived<FirstEventEvent>),
                typeof(EventsReceived<SecondEventEvent>),
            };

            private readonly Dictionary<uint, RequestContext> RequestIdToRequestContext = new Dictionary<uint, RequestContext>();

            internal List<FirstCommand.OutgoingRequest> FirstCommandRequests = new List<FirstCommand.OutgoingRequest>();
            internal List<FirstCommand.OutgoingResponse> FirstCommandResponses = new List<FirstCommand.OutgoingResponse>();
            private static readonly ComponentPool<CommandRequests<FirstCommand.Request>> FirstCommandRequestPool =
                new ComponentPool<CommandRequests<FirstCommand.Request>>(
                    () => new CommandRequests<FirstCommand.Request>(),
                    (component) => component.Buffer.Clear());
            private static readonly ComponentPool<CommandResponses<FirstCommand.Response>> FirstCommandResponsePool =
                new ComponentPool<CommandResponses<FirstCommand.Response>>(
                    () => new CommandResponses<FirstCommand.Response>(),
                    (component) => component.Buffer.Clear());
            internal List<SecondCommand.OutgoingRequest> SecondCommandRequests = new List<SecondCommand.OutgoingRequest>();
            internal List<SecondCommand.OutgoingResponse> SecondCommandResponses = new List<SecondCommand.OutgoingResponse>();
            private static readonly ComponentPool<CommandRequests<SecondCommand.Request>> SecondCommandRequestPool =
                new ComponentPool<CommandRequests<SecondCommand.Request>>(
                    () => new CommandRequests<SecondCommand.Request>(),
                    (component) => component.Buffer.Clear());
            private static readonly ComponentPool<CommandResponses<SecondCommand.Response>> SecondCommandResponsePool =
                new ComponentPool<CommandResponses<SecondCommand.Response>>(
                    () => new CommandResponses<SecondCommand.Response>(),
                    (component) => component.Buffer.Clear());

            internal readonly Dictionary<long, List<global::Generated.Improbable.TestSchema.Nonblittable.FirstEventPayload>> EntityIdToFirstEventEvents = new Dictionary<long, List<global::Generated.Improbable.TestSchema.Nonblittable.FirstEventPayload>>();

            private static readonly ComponentPool<EventsReceived<FirstEventEvent>> FirstEventEventPool =
                new ComponentPool<EventsReceived<FirstEventEvent>>(
                    () => new EventsReceived<FirstEventEvent>(),
                    (component) => component.Buffer.Clear());
            internal readonly Dictionary<long, List<global::Generated.Improbable.TestSchema.Nonblittable.SecondEventPayload>> EntityIdToSecondEventEvents = new Dictionary<long, List<global::Generated.Improbable.TestSchema.Nonblittable.SecondEventPayload>>();

            private static readonly ComponentPool<EventsReceived<SecondEventEvent>> SecondEventEventPool =
                new ComponentPool<EventsReceived<SecondEventEvent>>(
                    () => new EventsReceived<SecondEventEvent>(),
                    (component) => component.Buffer.Clear());

            private static readonly ComponentPool<ComponentsUpdated<SpatialOSNonBlittableComponent>> UpdatesPool =
                new ComponentPool<ComponentsUpdated<SpatialOSNonBlittableComponent>>(
                    () => new ComponentsUpdated<SpatialOSNonBlittableComponent>(),
                    (component) => component.Buffer.Clear());

            private static readonly ComponentPool<AuthoritiesChanged<SpatialOSNonBlittableComponent>> AuthsPool =
                new ComponentPool<AuthoritiesChanged<SpatialOSNonBlittableComponent>>(
                    () => new AuthoritiesChanged<SpatialOSNonBlittableComponent>(),
                    (component) => component.Buffer.Clear());

            public Translation(MutableView view) : base(view)
            {
            }

            public override void RegisterWithDispatcher(Dispatcher dispatcher)
            {
                dispatcher.OnAddComponent<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent>(OnAddComponent);
                dispatcher.OnComponentUpdate<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent>(OnComponentUpdate);
                dispatcher.OnRemoveComponent<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent>(OnRemoveComponent);
                dispatcher.OnAuthorityChange<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent>(OnAuthorityChange);

                dispatcher.OnCommandRequest<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.FirstCommand>(OnCommandRequestFirstCommand);
                dispatcher.OnCommandResponse<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.FirstCommand>(OnCommandResponseFirstCommand);
                dispatcher.OnCommandRequest<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.SecondCommand>(OnCommandRequestSecondCommand);
                dispatcher.OnCommandResponse<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.SecondCommand>(OnCommandResponseSecondCommand);
            }

            public override void AddCommandRequestSender(Unity.Entities.Entity entity, long entityId)
            {
                view.AddComponent(entity, new CommandRequestSender<SpatialOSNonBlittableComponent>(entityId, translationHandle));
            }

            public void OnAddComponent(AddComponentOp<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                var data = op.Data.Get().Value;

                var spatialOSNonBlittableComponent = new SpatialOSNonBlittableComponent();
                spatialOSNonBlittableComponent.BoolField = data.boolField;
                spatialOSNonBlittableComponent.IntField = data.intField;
                spatialOSNonBlittableComponent.LongField = data.longField;
                spatialOSNonBlittableComponent.FloatField = data.floatField;
                spatialOSNonBlittableComponent.DoubleField = data.doubleField;
                spatialOSNonBlittableComponent.StringField = data.stringField;
                spatialOSNonBlittableComponent.OptionalField = data.optionalField.HasValue ? new global::System.Nullable<int>(data.optionalField.Value) : new global::System.Nullable<int>();
                spatialOSNonBlittableComponent.ListField = data.listField;
                spatialOSNonBlittableComponent.MapField = data.mapField.ToDictionary(entry => entry.Key, entry => entry.Value);
                spatialOSNonBlittableComponent.DirtyBit = false;

                view.SetComponentObject(entity, spatialOSNonBlittableComponent);
                view.AddComponent(entity, new NotAuthoritative<SpatialOSNonBlittableComponent>());
            }

            public void OnComponentUpdate(ComponentUpdateOp<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                var componentData = view.GetComponentObject<SpatialOSNonBlittableComponent>(entity);
                var update = op.Update.Get();

                if (view.HasComponent<NotAuthoritative<SpatialOSNonBlittableComponent>>(entity))
                {
                    if (update.boolField.HasValue)
                    {
                        componentData.BoolField = update.boolField.Value;
                    }
                    if (update.intField.HasValue)
                    {
                        componentData.IntField = update.intField.Value;
                    }
                    if (update.longField.HasValue)
                    {
                        componentData.LongField = update.longField.Value;
                    }
                    if (update.floatField.HasValue)
                    {
                        componentData.FloatField = update.floatField.Value;
                    }
                    if (update.doubleField.HasValue)
                    {
                        componentData.DoubleField = update.doubleField.Value;
                    }
                    if (update.stringField.HasValue)
                    {
                        componentData.StringField = update.stringField.Value;
                    }
                    if (update.optionalField.HasValue)
                    {
                        componentData.OptionalField = update.optionalField.Value.HasValue ? new global::System.Nullable<int>(update.optionalField.Value.Value) : new global::System.Nullable<int>();
                    }
                    if (update.listField.HasValue)
                    {
                        componentData.ListField = update.listField.Value;
                    }
                    if (update.mapField.HasValue)
                    {
                        componentData.MapField = update.mapField.Value.ToDictionary(entry => entry.Key, entry => entry.Value);
                    }
                }

                var firstEventEvents = update.firstEvent;
                foreach (var spatialEvent in firstEventEvents)
                {
                    var nativeEvent = new FirstEventEvent
                    {
                        Payload = global::Generated.Improbable.TestSchema.Nonblittable.FirstEventPayload.ToNative(spatialEvent)
                    };

                    view.AddEventReceived(entity, nativeEvent, FirstEventEventPool);
                }
                var secondEventEvents = update.secondEvent;
                foreach (var spatialEvent in secondEventEvents)
                {
                    var nativeEvent = new SecondEventEvent
                    {
                        Payload = global::Generated.Improbable.TestSchema.Nonblittable.SecondEventPayload.ToNative(spatialEvent)
                    };

                    view.AddEventReceived(entity, nativeEvent, SecondEventEventPool);
                }
                componentData.DirtyBit = false;
                view.UpdateComponentObject(entity, componentData, UpdatesPool);
            }

            public void OnRemoveComponent(RemoveComponentOp op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                view.RemoveComponent<SpatialOSNonBlittableComponent>(entity);
            }

            public void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = op.EntityId.Id;
                if (op.Authority == Authority.Authoritative)
                {
                    EntityIdToFirstEventEvents[entityId] = new List<global::Generated.Improbable.TestSchema.Nonblittable.FirstEventPayload>();
                    EntityIdToSecondEventEvents[entityId] = new List<global::Generated.Improbable.TestSchema.Nonblittable.SecondEventPayload>();
                    view.AddComponent(entityId, new EventSender<SpatialOSNonBlittableComponent>(entityId, translationHandle));
                }
                else if (op.Authority == Authority.NotAuthoritative)
                {
                    EntityIdToFirstEventEvents.Remove(entityId);
                    EntityIdToSecondEventEvents.Remove(entityId);
                    view.RemoveComponent<EventSender<SpatialOSNonBlittableComponent>>(entityId);
                }
                view.HandleAuthorityChange(entityId, op.Authority, AuthsPool);
            }

            public override void ExecuteReplication(Connection connection)
            {
                var componentDataArray = ReplicationComponentGroup.GetComponentArray<SpatialOSNonBlittableComponent>();
                var spatialEntityIdData = ReplicationComponentGroup.GetComponentDataArray<SpatialEntityId>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var componentData = componentDataArray[i];
                    var entityId = spatialEntityIdData[i].EntityId;
                    var hasPendingEvents = false;
                    var firstEventEvents = EntityIdToFirstEventEvents[entityId];
                    hasPendingEvents |= firstEventEvents.Count() > 0;
                    var secondEventEvents = EntityIdToSecondEventEvents[entityId];
                    hasPendingEvents |= secondEventEvents.Count() > 0;

                    if (componentData.DirtyBit || hasPendingEvents)
                    {
                        var update = new global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Update();
                        update.SetBoolField(componentData.BoolField);
                        update.SetIntField(componentData.IntField);
                        update.SetLongField(componentData.LongField);
                        update.SetFloatField(componentData.FloatField);
                        update.SetDoubleField(componentData.DoubleField);
                        update.SetStringField(componentData.StringField);
                        update.SetOptionalField(componentData.OptionalField.HasValue ? new global::Improbable.Collections.Option<int>(componentData.OptionalField.Value) : new global::Improbable.Collections.Option<int>());
                        update.SetListField(new global::Improbable.Collections.List<int>(componentData.ListField));
                        update.SetMapField(new global::Improbable.Collections.Map<int,string>(componentData.MapField.ToDictionary(entry => entry.Key, entry => entry.Value)));
                        foreach (var nativeEvent in firstEventEvents)
                        {
                            var spatialEvent = global::Generated.Improbable.TestSchema.Nonblittable.FirstEventPayload.ToSpatial(nativeEvent);
                            update.firstEvent.Add(spatialEvent);
                        }
                        foreach (var nativeEvent in secondEventEvents)
                        {
                            var spatialEvent = global::Generated.Improbable.TestSchema.Nonblittable.SecondEventPayload.ToSpatial(nativeEvent);
                            update.secondEvent.Add(spatialEvent);
                        }
                        SendComponentUpdate(connection, entityId, update);

                        componentData.DirtyBit = false;
                        view.SetComponentObject(entityId, componentData);

                        firstEventEvents.Clear();
                        secondEventEvents.Clear();
                    }
                }
            }

            public static void SendComponentUpdate(Connection connection, long entityId, global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Update update)
            {
                connection.SendComponentUpdate(new global::Improbable.EntityId(entityId), update);
            }

            public override void CleanUpComponents(ref EntityCommandBuffer entityCommandBuffer)
            {
                RemoveComponents(ref entityCommandBuffer, UpdatesPool, groupIndex: 0);
                RemoveComponents(ref entityCommandBuffer, AuthsPool, groupIndex: 1);
                RemoveComponents(ref entityCommandBuffer, FirstCommandRequestPool, groupIndex: 2);
                RemoveComponents(ref entityCommandBuffer, FirstCommandResponsePool, groupIndex: 3);
                RemoveComponents(ref entityCommandBuffer, SecondCommandRequestPool, groupIndex: 4);
                RemoveComponents(ref entityCommandBuffer, SecondCommandResponsePool, groupIndex: 5);
                RemoveComponents(ref entityCommandBuffer, FirstEventEventPool, groupIndex: 6);
                RemoveComponents(ref entityCommandBuffer, SecondEventEventPool, groupIndex: 7);
            }

            public void OnCommandRequestFirstCommand(CommandRequestOp<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.FirstCommand> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.CannotFindEntityForCommandRequest, op.EntityId.Id,
                        "FirstCommand");
                    return;
                }

                var requestPayload = op.Request.Get().Value;
                var unityRequestPayload = global::Generated.Improbable.TestSchema.Nonblittable.FirstCommandRequest.ToNative(requestPayload);
                var request = new FirstCommand.Request(op.RequestId.Id, this, op.CallerWorkerId, op.CallerAttributeSet, unityRequestPayload);

                view.AddCommandRequest(entity, request, FirstCommandRequestPool);
            }

            public void OnCommandResponseFirstCommand(CommandResponseOp<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.FirstCommand> op)
            {
                var requestId = op.RequestId.Id;
                RequestContext requestContext;
                if (!RequestIdToRequestContext.TryGetValue(requestId, out requestContext))
                {
                    Debug.LogErrorFormat(TranslationErrors.CannotFindEntityForCommandResponse, op.EntityId.Id,
                        "FirstCommand");
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
                    ? global::Generated.Improbable.TestSchema.Nonblittable.FirstCommandResponse.ToNative(op.Response.Value.Get().Value) 
                    : (global::Generated.Improbable.TestSchema.Nonblittable.FirstCommandResponse?) null;
                var outgoingRequest = (FirstCommand.OutgoingRequest) requestContext.Request;
                var response = new FirstCommand.Response(op.EntityId.Id, op.Message, (CommandStatusCode) op.StatusCode, unityResponsePayload, outgoingRequest.RawRequest);

                view.AddCommandResponse(entity, response, FirstCommandResponsePool);
            }

            private void SendFirstCommandRequest(Connection connection, FirstCommand.OutgoingRequest outgoingRequest)
            {
                var requestPayload = global::Generated.Improbable.TestSchema.Nonblittable.FirstCommandRequest.ToSpatial(outgoingRequest.RawRequest);
                var request = new global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.FirstCommand.Request(requestPayload);

                var requestId = connection.SendCommandRequest(new global::Improbable.EntityId(outgoingRequest.TargetEntityId), request, null);

                RequestIdToRequestContext.Add(requestId.Id, new RequestContext(outgoingRequest.SenderEntityId, outgoingRequest));
            }

            private void SendFirstCommandResponse(Connection connection, FirstCommand.OutgoingResponse outgoingResponse)
            {
                var responsePayload = global::Generated.Improbable.TestSchema.Nonblittable.FirstCommandResponse.ToSpatial(outgoingResponse.RawResponse);
                var response = new global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.FirstCommand.Response(responsePayload);

                var requestId = new RequestId<IncomingCommandRequest<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.FirstCommand>>(
                    outgoingResponse.RequestId);

                connection.SendCommandResponse(requestId, response);
            }

            public void OnCommandRequestSecondCommand(CommandRequestOp<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.SecondCommand> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.CannotFindEntityForCommandRequest, op.EntityId.Id,
                        "SecondCommand");
                    return;
                }

                var requestPayload = op.Request.Get().Value;
                var unityRequestPayload = global::Generated.Improbable.TestSchema.Nonblittable.SecondCommandRequest.ToNative(requestPayload);
                var request = new SecondCommand.Request(op.RequestId.Id, this, op.CallerWorkerId, op.CallerAttributeSet, unityRequestPayload);

                view.AddCommandRequest(entity, request, SecondCommandRequestPool);
            }

            public void OnCommandResponseSecondCommand(CommandResponseOp<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.SecondCommand> op)
            {
                var requestId = op.RequestId.Id;
                RequestContext requestContext;
                if (!RequestIdToRequestContext.TryGetValue(requestId, out requestContext))
                {
                    Debug.LogErrorFormat(TranslationErrors.CannotFindEntityForCommandResponse, op.EntityId.Id,
                        "SecondCommand");
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
                    ? global::Generated.Improbable.TestSchema.Nonblittable.SecondCommandResponse.ToNative(op.Response.Value.Get().Value) 
                    : (global::Generated.Improbable.TestSchema.Nonblittable.SecondCommandResponse?) null;
                var outgoingRequest = (SecondCommand.OutgoingRequest) requestContext.Request;
                var response = new SecondCommand.Response(op.EntityId.Id, op.Message, (CommandStatusCode) op.StatusCode, unityResponsePayload, outgoingRequest.RawRequest);

                view.AddCommandResponse(entity, response, SecondCommandResponsePool);
            }

            private void SendSecondCommandRequest(Connection connection, SecondCommand.OutgoingRequest outgoingRequest)
            {
                var requestPayload = global::Generated.Improbable.TestSchema.Nonblittable.SecondCommandRequest.ToSpatial(outgoingRequest.RawRequest);
                var request = new global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.SecondCommand.Request(requestPayload);

                var requestId = connection.SendCommandRequest(new global::Improbable.EntityId(outgoingRequest.TargetEntityId), request, null);

                RequestIdToRequestContext.Add(requestId.Id, new RequestContext(outgoingRequest.SenderEntityId, outgoingRequest));
            }

            private void SendSecondCommandResponse(Connection connection, SecondCommand.OutgoingResponse outgoingResponse)
            {
                var responsePayload = global::Generated.Improbable.TestSchema.Nonblittable.SecondCommandResponse.ToSpatial(outgoingResponse.RawResponse);
                var response = new global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.SecondCommand.Response(responsePayload);

                var requestId = new RequestId<IncomingCommandRequest<global::Improbable.TestSchema.Nonblittable.NonBlittableComponent.Commands.SecondCommand>>(
                    outgoingResponse.RequestId);

                connection.SendCommandResponse(requestId, response);
            }

            public override void SendCommands(Connection connection)
            {
                foreach (var request in FirstCommandRequests)
                {
                    SendFirstCommandRequest(connection, request);
                }
                FirstCommandRequests.Clear();

                foreach (var response in FirstCommandResponses)
                {
                    SendFirstCommandResponse(connection, response);
                }
                FirstCommandResponses.Clear();

                foreach (var request in SecondCommandRequests)
                {
                    SendSecondCommandRequest(connection, request);
                }
                SecondCommandRequests.Clear();

                foreach (var response in SecondCommandResponses)
                {
                    SendSecondCommandResponse(connection, response);
                }
                SecondCommandResponses.Clear();

            }

            public static NonBlittableComponent.Translation GetTranslation(uint internalHandleToTranslation)
            {
                return (NonBlittableComponent.Translation) ComponentTranslation.HandleToTranslation[internalHandleToTranslation];
            }
        }
    }

    public static class SpatialOSNonBlittableComponentCommandRequestHandlers
    {
        public static void SendFirstCommandRequest(this CommandRequestSender<SpatialOSNonBlittableComponent> commandRequestSender,
            long targetEntityId, global::Generated.Improbable.TestSchema.Nonblittable.FirstCommandRequest request)
        {
            var translation = NonBlittableComponent.Translation.GetTranslation(commandRequestSender.InternalHandleToTranslation);

            translation.FirstCommandRequests.Add(new NonBlittableComponent.FirstCommand.OutgoingRequest(targetEntityId,
                commandRequestSender.EntityId, request));
        }

        public static void SendSecondCommandRequest(this CommandRequestSender<SpatialOSNonBlittableComponent> commandRequestSender,
            long targetEntityId, global::Generated.Improbable.TestSchema.Nonblittable.SecondCommandRequest request)
        {
            var translation = NonBlittableComponent.Translation.GetTranslation(commandRequestSender.InternalHandleToTranslation);

            translation.SecondCommandRequests.Add(new NonBlittableComponent.SecondCommand.OutgoingRequest(targetEntityId,
                commandRequestSender.EntityId, request));
        }

    }

    public static class SpatialOSNonBlittableComponentEventHandlers
    {
        public static void SendFirstEventEvent(this EventSender<SpatialOSNonBlittableComponent> eventSender,
            global::Generated.Improbable.TestSchema.Nonblittable.FirstEventPayload eventData)
        {
            var translation = NonBlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            translation.EntityIdToFirstEventEvents[eventSender.EntityId].Add(eventData);
        }

        public static List<global::Generated.Improbable.TestSchema.Nonblittable.FirstEventPayload> GetFirstEventEvents(this EventSender<SpatialOSNonBlittableComponent> eventSender)
        {
            var translation = NonBlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            return translation.EntityIdToFirstEventEvents[eventSender.EntityId];
        }

        public static void ClearFirstEventEvents(this EventSender<SpatialOSNonBlittableComponent> eventSender)
        {
            var translation = NonBlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            translation.EntityIdToFirstEventEvents[eventSender.EntityId].Clear();
        }

        public static void SendSecondEventEvent(this EventSender<SpatialOSNonBlittableComponent> eventSender,
            global::Generated.Improbable.TestSchema.Nonblittable.SecondEventPayload eventData)
        {
            var translation = NonBlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            translation.EntityIdToSecondEventEvents[eventSender.EntityId].Add(eventData);
        }

        public static List<global::Generated.Improbable.TestSchema.Nonblittable.SecondEventPayload> GetSecondEventEvents(this EventSender<SpatialOSNonBlittableComponent> eventSender)
        {
            var translation = NonBlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            return translation.EntityIdToSecondEventEvents[eventSender.EntityId];
        }

        public static void ClearSecondEventEvents(this EventSender<SpatialOSNonBlittableComponent> eventSender)
        {
            var translation = NonBlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            translation.EntityIdToSecondEventEvents[eventSender.EntityId].Clear();
        }

    }
}

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
using Improbable.TestSchema.Blittable;

namespace Generated.Improbable.TestSchema.Blittable
{
    public partial class BlittableComponent
    {
        public class Translation : ComponentTranslation, IDispatcherCallbacks<global::Improbable.TestSchema.Blittable.BlittableComponent>
        {
            public override ComponentType TargetComponentType => targetComponentType;
            private static readonly ComponentType targetComponentType = typeof(SpatialOSBlittableComponent);

            public override ComponentType[] ReplicationComponentTypes => replicationComponentTypes;
            private static readonly ComponentType[] replicationComponentTypes = { typeof(SpatialOSBlittableComponent), typeof(Authoritative<SpatialOSBlittableComponent>), typeof(SpatialEntityId)};

            public override ComponentType[] CleanUpComponentTypes => cleanUpComponentTypes;
            private static readonly ComponentType[] cleanUpComponentTypes = 
            { 
                typeof(ComponentsUpdated<SpatialOSBlittableComponent>), typeof(AuthoritiesChanged<SpatialOSBlittableComponent>),
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

            internal readonly Dictionary<long, List<global::Generated.Improbable.TestSchema.Blittable.FirstEventPayload>> EntityIdToFirstEventEvents = new Dictionary<long, List<global::Generated.Improbable.TestSchema.Blittable.FirstEventPayload>>();

            private static readonly ComponentPool<EventsReceived<FirstEventEvent>> FirstEventEventPool =
                new ComponentPool<EventsReceived<FirstEventEvent>>(
                    () => new EventsReceived<FirstEventEvent>(),
                    (component) => component.Buffer.Clear());
            internal readonly Dictionary<long, List<global::Generated.Improbable.TestSchema.Blittable.SecondEventPayload>> EntityIdToSecondEventEvents = new Dictionary<long, List<global::Generated.Improbable.TestSchema.Blittable.SecondEventPayload>>();

            private static readonly ComponentPool<EventsReceived<SecondEventEvent>> SecondEventEventPool =
                new ComponentPool<EventsReceived<SecondEventEvent>>(
                    () => new EventsReceived<SecondEventEvent>(),
                    (component) => component.Buffer.Clear());

            private static readonly ComponentPool<ComponentsUpdated<SpatialOSBlittableComponent>> UpdatesPool =
                new ComponentPool<ComponentsUpdated<SpatialOSBlittableComponent>>(
                    () => new ComponentsUpdated<SpatialOSBlittableComponent>(),
                    (component) => component.Buffer.Clear());

            private static readonly ComponentPool<AuthoritiesChanged<SpatialOSBlittableComponent>> AuthsPool =
                new ComponentPool<AuthoritiesChanged<SpatialOSBlittableComponent>>(
                    () => new AuthoritiesChanged<SpatialOSBlittableComponent>(),
                    (component) => component.Buffer.Clear());

            public Translation(MutableView view) : base(view)
            {
            }

            public override void RegisterWithDispatcher(Dispatcher dispatcher)
            {
                dispatcher.OnAddComponent<global::Improbable.TestSchema.Blittable.BlittableComponent>(OnAddComponent);
                dispatcher.OnComponentUpdate<global::Improbable.TestSchema.Blittable.BlittableComponent>(OnComponentUpdate);
                dispatcher.OnRemoveComponent<global::Improbable.TestSchema.Blittable.BlittableComponent>(OnRemoveComponent);
                dispatcher.OnAuthorityChange<global::Improbable.TestSchema.Blittable.BlittableComponent>(OnAuthorityChange);

                dispatcher.OnCommandRequest<global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.FirstCommand>(OnCommandRequestFirstCommand);
                dispatcher.OnCommandResponse<global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.FirstCommand>(OnCommandResponseFirstCommand);
                dispatcher.OnCommandRequest<global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.SecondCommand>(OnCommandRequestSecondCommand);
                dispatcher.OnCommandResponse<global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.SecondCommand>(OnCommandResponseSecondCommand);
            }

            public override void AddCommandRequestSender(Unity.Entities.Entity entity, long entityId)
            {
                view.AddComponent(entity, new CommandRequestSender<SpatialOSBlittableComponent>(entityId, translationHandle));
            }

            public void OnAddComponent(AddComponentOp<global::Improbable.TestSchema.Blittable.BlittableComponent> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                var data = op.Data.Get().Value;

                var spatialOSBlittableComponent = new SpatialOSBlittableComponent();
                spatialOSBlittableComponent.BoolField = data.boolField;
                spatialOSBlittableComponent.IntField = data.intField;
                spatialOSBlittableComponent.LongField = data.longField;
                spatialOSBlittableComponent.FloatField = data.floatField;
                spatialOSBlittableComponent.DoubleField = data.doubleField;
                spatialOSBlittableComponent.DirtyBit = false;

                view.AddComponent(entity, spatialOSBlittableComponent);
                view.AddComponent(entity, new NotAuthoritative<SpatialOSBlittableComponent>());
            }

            public void OnComponentUpdate(ComponentUpdateOp<global::Improbable.TestSchema.Blittable.BlittableComponent> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                var componentData = view.GetComponent<SpatialOSBlittableComponent>(entity);
                var update = op.Update.Get();

                if (view.HasComponent<NotAuthoritative<SpatialOSBlittableComponent>>(entity))
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
                }

                var firstEventEvents = update.firstEvent;
                foreach (var spatialEvent in firstEventEvents)
                {
                    var nativeEvent = new FirstEventEvent
                    {
                        Payload = global::Generated.Improbable.TestSchema.Blittable.FirstEventPayload.ToNative(spatialEvent)
                    };

                    view.AddEventReceived(entity, nativeEvent, FirstEventEventPool);
                }
                var secondEventEvents = update.secondEvent;
                foreach (var spatialEvent in secondEventEvents)
                {
                    var nativeEvent = new SecondEventEvent
                    {
                        Payload = global::Generated.Improbable.TestSchema.Blittable.SecondEventPayload.ToNative(spatialEvent)
                    };

                    view.AddEventReceived(entity, nativeEvent, SecondEventEventPool);
                }
                componentData.DirtyBit = false;
                view.UpdateComponent(entity, componentData, UpdatesPool);
            }

            public void OnRemoveComponent(RemoveComponentOp op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.OpReceivedButNoEntity, op.GetType().Name, op.EntityId.Id);
                    return;
                }

                view.RemoveComponent<SpatialOSBlittableComponent>(entity);
            }

            public void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entityId = op.EntityId.Id;
                if (op.Authority == Authority.Authoritative)
                {
                    EntityIdToFirstEventEvents[entityId] = new List<global::Generated.Improbable.TestSchema.Blittable.FirstEventPayload>();
                    EntityIdToSecondEventEvents[entityId] = new List<global::Generated.Improbable.TestSchema.Blittable.SecondEventPayload>();
                    view.AddComponent(entityId, new EventSender<SpatialOSBlittableComponent>(entityId, translationHandle));
                }
                else if (op.Authority == Authority.NotAuthoritative)
                {
                    EntityIdToFirstEventEvents.Remove(entityId);
                    EntityIdToSecondEventEvents.Remove(entityId);
                    view.RemoveComponent<EventSender<SpatialOSBlittableComponent>>(entityId);
                }
                view.HandleAuthorityChange(entityId, op.Authority, AuthsPool);
            }

            public override void ExecuteReplication(Connection connection)
            {
                var componentDataArray = ReplicationComponentGroup.GetComponentDataArray<SpatialOSBlittableComponent>();
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
                        var update = new global::Improbable.TestSchema.Blittable.BlittableComponent.Update();
                        update.SetBoolField(componentData.BoolField);
                        update.SetIntField(componentData.IntField);
                        update.SetLongField(componentData.LongField);
                        update.SetFloatField(componentData.FloatField);
                        update.SetDoubleField(componentData.DoubleField);
                        foreach (var nativeEvent in firstEventEvents)
                        {
                            var spatialEvent = global::Generated.Improbable.TestSchema.Blittable.FirstEventPayload.ToSpatial(nativeEvent);
                            update.firstEvent.Add(spatialEvent);
                        }
                        foreach (var nativeEvent in secondEventEvents)
                        {
                            var spatialEvent = global::Generated.Improbable.TestSchema.Blittable.SecondEventPayload.ToSpatial(nativeEvent);
                            update.secondEvent.Add(spatialEvent);
                        }
                        SendComponentUpdate(connection, entityId, update);

                        componentData.DirtyBit = false;
                        componentDataArray[i] = componentData;

                        firstEventEvents.Clear();
                        secondEventEvents.Clear();
                    }
                }
            }

            public static void SendComponentUpdate(Connection connection, long entityId, global::Improbable.TestSchema.Blittable.BlittableComponent.Update update)
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

            public void OnCommandRequestFirstCommand(CommandRequestOp<global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.FirstCommand> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.CannotFindEntityForCommandRequest, op.EntityId.Id,
                        "FirstCommand");
                    return;
                }

                var requestPayload = op.Request.Get().Value;
                var unityRequestPayload = global::Generated.Improbable.TestSchema.Blittable.FirstCommandRequest.ToNative(requestPayload);
                var request = new FirstCommand.Request(op.RequestId.Id, this, op.CallerWorkerId, op.CallerAttributeSet, unityRequestPayload);

                view.AddCommandRequest(entity, request, FirstCommandRequestPool);
            }

            public void OnCommandResponseFirstCommand(CommandResponseOp<global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.FirstCommand> op)
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
                    ? global::Generated.Improbable.TestSchema.Blittable.FirstCommandResponse.ToNative(op.Response.Value.Get().Value) 
                    : (global::Generated.Improbable.TestSchema.Blittable.FirstCommandResponse?) null;
                var outgoingRequest = (FirstCommand.OutgoingRequest) requestContext.Request;
                var response = new FirstCommand.Response(op.EntityId.Id, op.Message, (CommandStatusCode) op.StatusCode, unityResponsePayload, outgoingRequest.RawRequest);

                view.AddCommandResponse(entity, response, FirstCommandResponsePool);
            }

            private void SendFirstCommandRequest(Connection connection, FirstCommand.OutgoingRequest outgoingRequest)
            {
                var requestPayload = global::Generated.Improbable.TestSchema.Blittable.FirstCommandRequest.ToSpatial(outgoingRequest.RawRequest);
                var request = new global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.FirstCommand.Request(requestPayload);

                var requestId = connection.SendCommandRequest(new global::Improbable.EntityId(outgoingRequest.TargetEntityId), request, null);

                RequestIdToRequestContext.Add(requestId.Id, new RequestContext(outgoingRequest.SenderEntityId, outgoingRequest));
            }

            private void SendFirstCommandResponse(Connection connection, FirstCommand.OutgoingResponse outgoingResponse)
            {
                var responsePayload = global::Generated.Improbable.TestSchema.Blittable.FirstCommandResponse.ToSpatial(outgoingResponse.RawResponse);
                var response = new global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.FirstCommand.Response(responsePayload);

                var requestId = new RequestId<IncomingCommandRequest<global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.FirstCommand>>(
                    outgoingResponse.RequestId);

                connection.SendCommandResponse(requestId, response);
            }

            public void OnCommandRequestSecondCommand(CommandRequestOp<global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.SecondCommand> op)
            {
                Unity.Entities.Entity entity;
                if (!view.TryGetEntity(op.EntityId.Id, out entity))
                {
                    Debug.LogErrorFormat(TranslationErrors.CannotFindEntityForCommandRequest, op.EntityId.Id,
                        "SecondCommand");
                    return;
                }

                var requestPayload = op.Request.Get().Value;
                var unityRequestPayload = global::Generated.Improbable.TestSchema.Blittable.SecondCommandRequest.ToNative(requestPayload);
                var request = new SecondCommand.Request(op.RequestId.Id, this, op.CallerWorkerId, op.CallerAttributeSet, unityRequestPayload);

                view.AddCommandRequest(entity, request, SecondCommandRequestPool);
            }

            public void OnCommandResponseSecondCommand(CommandResponseOp<global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.SecondCommand> op)
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
                    ? global::Generated.Improbable.TestSchema.Blittable.SecondCommandResponse.ToNative(op.Response.Value.Get().Value) 
                    : (global::Generated.Improbable.TestSchema.Blittable.SecondCommandResponse?) null;
                var outgoingRequest = (SecondCommand.OutgoingRequest) requestContext.Request;
                var response = new SecondCommand.Response(op.EntityId.Id, op.Message, (CommandStatusCode) op.StatusCode, unityResponsePayload, outgoingRequest.RawRequest);

                view.AddCommandResponse(entity, response, SecondCommandResponsePool);
            }

            private void SendSecondCommandRequest(Connection connection, SecondCommand.OutgoingRequest outgoingRequest)
            {
                var requestPayload = global::Generated.Improbable.TestSchema.Blittable.SecondCommandRequest.ToSpatial(outgoingRequest.RawRequest);
                var request = new global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.SecondCommand.Request(requestPayload);

                var requestId = connection.SendCommandRequest(new global::Improbable.EntityId(outgoingRequest.TargetEntityId), request, null);

                RequestIdToRequestContext.Add(requestId.Id, new RequestContext(outgoingRequest.SenderEntityId, outgoingRequest));
            }

            private void SendSecondCommandResponse(Connection connection, SecondCommand.OutgoingResponse outgoingResponse)
            {
                var responsePayload = global::Generated.Improbable.TestSchema.Blittable.SecondCommandResponse.ToSpatial(outgoingResponse.RawResponse);
                var response = new global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.SecondCommand.Response(responsePayload);

                var requestId = new RequestId<IncomingCommandRequest<global::Improbable.TestSchema.Blittable.BlittableComponent.Commands.SecondCommand>>(
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

            public static BlittableComponent.Translation GetTranslation(uint internalHandleToTranslation)
            {
                return (BlittableComponent.Translation) ComponentTranslation.HandleToTranslation[internalHandleToTranslation];
            }
        }
    }

    public static class SpatialOSBlittableComponentCommandRequestHandlers
    {
        public static void SendFirstCommandRequest(this CommandRequestSender<SpatialOSBlittableComponent> commandRequestSender,
            long targetEntityId, global::Generated.Improbable.TestSchema.Blittable.FirstCommandRequest request)
        {
            var translation = BlittableComponent.Translation.GetTranslation(commandRequestSender.InternalHandleToTranslation);

            translation.FirstCommandRequests.Add(new BlittableComponent.FirstCommand.OutgoingRequest(targetEntityId,
                commandRequestSender.EntityId, request));
        }

        public static void SendSecondCommandRequest(this CommandRequestSender<SpatialOSBlittableComponent> commandRequestSender,
            long targetEntityId, global::Generated.Improbable.TestSchema.Blittable.SecondCommandRequest request)
        {
            var translation = BlittableComponent.Translation.GetTranslation(commandRequestSender.InternalHandleToTranslation);

            translation.SecondCommandRequests.Add(new BlittableComponent.SecondCommand.OutgoingRequest(targetEntityId,
                commandRequestSender.EntityId, request));
        }

    }

    public static class SpatialOSBlittableComponentEventHandlers
    {
        public static void SendFirstEventEvent(this EventSender<SpatialOSBlittableComponent> eventSender,
            global::Generated.Improbable.TestSchema.Blittable.FirstEventPayload eventData)
        {
            var translation = BlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            translation.EntityIdToFirstEventEvents[eventSender.EntityId].Add(eventData);
        }

        public static List<global::Generated.Improbable.TestSchema.Blittable.FirstEventPayload> GetFirstEventEvents(this EventSender<SpatialOSBlittableComponent> eventSender)
        {
            var translation = BlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            return translation.EntityIdToFirstEventEvents[eventSender.EntityId];
        }

        public static void ClearFirstEventEvents(this EventSender<SpatialOSBlittableComponent> eventSender)
        {
            var translation = BlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            translation.EntityIdToFirstEventEvents[eventSender.EntityId].Clear();
        }

        public static void SendSecondEventEvent(this EventSender<SpatialOSBlittableComponent> eventSender,
            global::Generated.Improbable.TestSchema.Blittable.SecondEventPayload eventData)
        {
            var translation = BlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            translation.EntityIdToSecondEventEvents[eventSender.EntityId].Add(eventData);
        }

        public static List<global::Generated.Improbable.TestSchema.Blittable.SecondEventPayload> GetSecondEventEvents(this EventSender<SpatialOSBlittableComponent> eventSender)
        {
            var translation = BlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            return translation.EntityIdToSecondEventEvents[eventSender.EntityId];
        }

        public static void ClearSecondEventEvents(this EventSender<SpatialOSBlittableComponent> eventSender)
        {
            var translation = BlittableComponent.Translation.GetTranslation(eventSender.InternalHandleToTranslation);
            translation.EntityIdToSecondEventEvents[eventSender.EntityId].Clear();
        }

    }
}

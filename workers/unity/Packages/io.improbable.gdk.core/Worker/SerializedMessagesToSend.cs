using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Query;

namespace Improbable.Gdk.Core
{
    public class SerializedMessagesToSend
    {
        private const uint PositionComponentId = 54;

        private static readonly UpdateParameters UpdateParams = new UpdateParameters
            { Loopback = ComponentUpdateLoopback.ShortCircuited };

        private static List<Type> componentTypes;
        private static List<Type> commandTypes;

        private readonly MessageList<UpdateToSend> updates = new MessageList<UpdateToSend>();
        private readonly MessageList<RequestToSend> requests = new MessageList<RequestToSend>();
        private readonly MessageList<ResponseToSend> responses = new MessageList<ResponseToSend>();
        private readonly MessageList<FailureToSend> failures = new MessageList<FailureToSend>();

        private readonly MessageList<ReserveEntityIdsRequestToSend> reserveEntityIdsRequests =
            new MessageList<ReserveEntityIdsRequestToSend>();

        private readonly MessageList<CreateEntityRequestToSend> createEntityRequests =
            new MessageList<CreateEntityRequestToSend>();

        private readonly MessageList<DeleteEntityRequestToSend> deleteEntityRequests =
            new MessageList<DeleteEntityRequestToSend>();

        private readonly MessageList<EntityQueryRequestToSend> entityQueryRequests =
            new MessageList<EntityQueryRequestToSend>();

        private readonly MessageList<LogMessageToSend> logMessages =
            new MessageList<LogMessageToSend>();

        private readonly List<Metrics> metricsToSend = new List<Metrics>();

        private readonly MessageList<EntityComponent> authorityLossAcks =
            new MessageList<EntityComponent>();

        private readonly List<IComponentSerializer> componentSerializers = new List<IComponentSerializer>();
        private readonly List<ICommandSerializer> commandSerializers = new List<ICommandSerializer>();

        private CommandMetaData metaData;

        public SerializedMessagesToSend()
        {
            if (componentTypes == null)
            {
                componentTypes = ReflectionUtility.GetNonAbstractTypes(typeof(IComponentSerializer));
            }

            if (commandTypes == null)
            {
                commandTypes = ReflectionUtility.GetNonAbstractTypes(typeof(ICommandSerializer));
            }

            foreach (var type in componentTypes)
            {
                var instance = (IComponentSerializer) Activator.CreateInstance(type);
                componentSerializers.Add(instance);
            }

            foreach (var type in commandTypes)
            {
                var instance = (ICommandSerializer) Activator.CreateInstance(type);
                commandSerializers.Add(instance);
            }

            // Move the position serializer to the end of the queue so that the updates get sent last
            // This is to prevent an authority change before other updates have been applied from the same frame
            for (int i = 0; i < componentSerializers.Count; ++i)
            {
                if (componentSerializers[i].GetComponentId() == PositionComponentId)
                {
                    var positionSerializer = componentSerializers[i];
                    componentSerializers.RemoveAt(i);
                    componentSerializers.Add(positionSerializer);
                    break;
                }
            }
        }

        public void SerializeFrom(MessagesToSend messages, CommandMetaData commandMetaData)
        {
            metaData = commandMetaData;

            foreach (var serializer in commandSerializers)
            {
                serializer.Serialize(messages, this, metaData);
            }

            foreach (var metrics in messages.GetMetrics())
            {
                metricsToSend.Add(metrics);
            }

            messages.GetLogMessages().CopyTo(logMessages);

            messages.GetAuthorityLossAcknowledgements().CopyTo(authorityLossAcks);

            foreach (var serializer in componentSerializers)
            {
                serializer.Serialize(messages, this);
            }
        }

        public void Clear()
        {
            updates.Clear();
            requests.Clear();
            responses.Clear();
            failures.Clear();
            reserveEntityIdsRequests.Clear();
            createEntityRequests.Clear();
            deleteEntityRequests.Clear();
            entityQueryRequests.Clear();
            metricsToSend.Clear();
            logMessages.Clear();
            authorityLossAcks.Clear();
        }

        public CommandMetaData SendAndClear(Connection connection)
        {
            for (int i = 0; i < updates.Count; ++i)
            {
                ref readonly var update = ref updates[i];
                connection.SendComponentUpdate(update.EntityId, update.Update, UpdateParams);
            }

            for (int i = 0; i < requests.Count; ++i)
            {
                ref readonly var request = ref requests[i];
                var id = connection.SendCommandRequest(request.EntityId, request.Request, request.CommandId, request.Timeout);
                metaData.AddInternalRequestId(request.Request.ComponentId, request.CommandId, request.RequestId, id);
            }

            for (int i = 0; i < responses.Count; ++i)
            {
                ref readonly var response = ref responses[i];
                connection.SendCommandResponse(response.RequestId, response.Response);
            }

            for (int i = 0; i < failures.Count; ++i)
            {
                ref readonly var failure = ref failures[i];
                connection.SendCommandFailure(failure.RequestId, failure.Reason);
            }

            for (int i = 0; i < createEntityRequests.Count; ++i)
            {
                ref readonly var request = ref createEntityRequests[i];
                var id = connection.SendCreateEntityRequest(request.Entity, request.EntityId, request.Timeout);
                metaData.AddInternalRequestId(0, 0, request.RequestId, id);
            }

            for (int i = 0; i < deleteEntityRequests.Count; ++i)
            {
                ref readonly var request = ref deleteEntityRequests[i];
                var id = connection.SendDeleteEntityRequest(request.EntityId, request.Timeout);
                metaData.AddInternalRequestId(0, 0, request.RequestId, id);
            }

            for (int i = 0; i < reserveEntityIdsRequests.Count; ++i)
            {
                ref readonly var request = ref reserveEntityIdsRequests[i];
                var id = connection.SendReserveEntityIdsRequest(request.NumberOfEntityIds, request.Timeout);
                metaData.AddInternalRequestId(0, 0, request.RequestId, id);
            }

            for (int i = 0; i < entityQueryRequests.Count; ++i)
            {
                ref readonly var request = ref entityQueryRequests[i];
                var id = connection.SendEntityQueryRequest(request.Query, request.Timeout);
                metaData.AddInternalRequestId(0, 0, request.RequestId, id);
            }

            for (int i = 0; i < metricsToSend.Count; ++i)
            {
                connection.SendMetrics(metricsToSend[i]);
            }

            for (int i = 0; i < logMessages.Count; ++i)
            {
                ref readonly var logMessage = ref logMessages[i];
                connection.SendLogMessage(logMessage.LogLevel, logMessage.LoggerName, logMessage.Message,
                    logMessage.EntityId);
            }

            for (int i = 0; i < authorityLossAcks.Count; ++i)
            {
                ref readonly var entityComponent = ref authorityLossAcks[i];
                connection.SendAuthorityLossImminentAcknowledgement(entityComponent.EntityId, entityComponent.ComponentId);
            }

            Clear();

            return metaData;
        }

        public void AddComponentUpdate(ComponentUpdate update, long entityId)
        {
            updates.Add(new UpdateToSend(update, entityId));
        }

        public void AddRequest(CommandRequest request, uint commandId, long entityId, uint? timeout, long requestId)
        {
            requests.Add(new RequestToSend(request, commandId, entityId, timeout, requestId));
        }

        public void AddResponse(CommandResponse response, uint requestId)
        {
            responses.Add(new ResponseToSend(response, requestId));
        }

        public void AddFailure(string reason, uint requestId)
        {
            failures.Add(new FailureToSend(reason, requestId));
        }

        public void AddCreateEntityRequest(Entity entity, long? entityId, uint? timeout, long requestId)
        {
            createEntityRequests.Add(new CreateEntityRequestToSend(entity, entityId, timeout, requestId));
        }

        public void AddDeleteEntityRequest(long entityId, uint? timeout, long requestId)
        {
            deleteEntityRequests.Add(new DeleteEntityRequestToSend(entityId, timeout, requestId));
        }

        public void AddReserveEntityIdsRequest(uint numberOfEntityIds, uint? timeout, long requestId)
        {
            reserveEntityIdsRequests.Add(new ReserveEntityIdsRequestToSend(numberOfEntityIds, timeout, requestId));
        }

        public void AddEntityQueryRequest(EntityQuery query, uint? timeout, long requestId)
        {
            entityQueryRequests.Add(new EntityQueryRequestToSend(query, timeout, requestId));
        }

        internal void DestroyUnsentMessages()
        {
            for (int i = 0; i < updates.Count; ++i)
            {
                ref readonly var update = ref updates[i];
                update.Update.SchemaData.Value.Destroy();
            }

            for (int i = 0; i < requests.Count; ++i)
            {
                ref readonly var request = ref requests[i];
                request.Request.SchemaData.Value.Destroy();
            }

            for (int i = 0; i < responses.Count; ++i)
            {
                ref readonly var response = ref responses[i];
                response.Response.SchemaData.Value.Destroy();
            }
        }

        #region Containers

        private readonly struct UpdateToSend
        {
            public readonly ComponentUpdate Update;
            public readonly long EntityId;

            public UpdateToSend(ComponentUpdate update, long entityId)
            {
                Update = update;
                EntityId = entityId;
            }
        }

        private readonly struct RequestToSend
        {
            public readonly CommandRequest Request;
            public readonly uint CommandId;
            public readonly long EntityId;
            public readonly uint? Timeout;
            public readonly long RequestId;

            public RequestToSend(CommandRequest request, uint commandId, long entityId, uint? timeout, long requestId)
            {
                Request = request;
                CommandId = commandId;
                EntityId = entityId;
                Timeout = timeout;
                RequestId = requestId;
            }
        }

        private readonly struct ResponseToSend
        {
            public readonly CommandResponse Response;
            public readonly uint RequestId;

            public ResponseToSend(CommandResponse response, uint requestId)
            {
                Response = response;
                RequestId = requestId;
            }
        }

        private readonly struct FailureToSend
        {
            public readonly string Reason;
            public readonly uint RequestId;

            public FailureToSend(string reason, uint requestId)
            {
                Reason = reason;
                RequestId = requestId;
            }
        }

        private readonly struct ReserveEntityIdsRequestToSend
        {
            public readonly uint NumberOfEntityIds;
            public readonly uint? Timeout;
            public readonly long RequestId;

            public ReserveEntityIdsRequestToSend(uint numberOfEntityIds, uint? timeout, long requestId)
            {
                NumberOfEntityIds = numberOfEntityIds;
                Timeout = timeout;
                RequestId = requestId;
            }
        }

        private readonly struct CreateEntityRequestToSend
        {
            public readonly Entity Entity;
            public readonly long? EntityId;
            public readonly uint? Timeout;
            public readonly long RequestId;

            public CreateEntityRequestToSend(Entity entity, long? entityId, uint? timeout, long requestId)
            {
                Entity = entity;
                EntityId = entityId;
                Timeout = timeout;
                RequestId = requestId;
            }
        }

        private readonly struct DeleteEntityRequestToSend
        {
            public readonly long EntityId;
            public readonly uint? Timeout;
            public readonly long RequestId;

            public DeleteEntityRequestToSend(long entityId, uint? timeout, long requestId)
            {
                EntityId = entityId;
                Timeout = timeout;
                RequestId = requestId;
            }
        }

        private readonly struct EntityQueryRequestToSend
        {
            public readonly EntityQuery Query;
            public readonly uint? Timeout;
            public readonly long RequestId;

            public EntityQueryRequestToSend(EntityQuery query, uint? timeout, long requestId)
            {
                Query = query;
                Timeout = timeout;
                RequestId = requestId;
            }
        }

        #endregion
    }
}

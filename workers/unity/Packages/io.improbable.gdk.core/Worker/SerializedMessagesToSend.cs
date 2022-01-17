using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core.NetworkStats;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Query;

namespace Improbable.Gdk.Core
{
    public class SerializedMessagesToSend
    {
        private static readonly UpdateParameters UpdateParams = new UpdateParameters
        {
            Loopback = ComponentUpdateLoopback.ShortCircuited
        };

        private static readonly List<Type> ComponentEventTypes;
        private static readonly List<Type> CommandTypes;

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

        private readonly MessageList<AddComponentRequestToSend> addComponentRequests =
            new MessageList<AddComponentRequestToSend>();

        private readonly MessageList<RemoveComponentRequestToSend> removeComponentRequests =
            new MessageList<RemoveComponentRequestToSend>();

        private readonly MessageList<LogMessageToSend> logMessages =
            new MessageList<LogMessageToSend>();

        private readonly List<Metrics> metricsToSend = new List<Metrics>();

        private readonly List<IComponentEventSerializer> componentEventSerializers = new List<IComponentEventSerializer>();
        private readonly List<ICommandSerializer> commandSerializers = new List<ICommandSerializer>();

        private NetFrameStats netFrameStats = new NetFrameStats();

        static SerializedMessagesToSend()
        {
            ComponentEventTypes = ComponentDatabase.Metaclasses
                .Select(pair => pair.Value.EventSerializer)
                .Where(type => type != null)
                .ToList();

            CommandTypes = ComponentDatabase.Metaclasses
                .SelectMany(pair => pair.Value.Commands)
                .Select(metaclass => metaclass.Serializer)
                .ToList();
        }

        public SerializedMessagesToSend()
        {
            foreach (var type in ComponentEventTypes)
            {
                var instance = (IComponentEventSerializer) Activator.CreateInstance(type);
                if (instance.HasEvents)
                {
                    componentEventSerializers.Add(instance);
                }
            }

            foreach (var type in CommandTypes)
            {
                var instance = (ICommandSerializer) Activator.CreateInstance(type);
                commandSerializers.Add(instance);
            }

            commandSerializers.Add(new WorldCommandSerializer());
        }

        public void SerializeFrom(MessagesToSend messages, CommandMetaData commandMetaData)
        {
            messages.GetSerializedComponentUpdates().CopyTo(updates);

            foreach (var serializer in commandSerializers)
            {
                serializer.Serialize(messages, this, commandMetaData);
            }

            foreach (var metrics in messages.GetMetrics())
            {
                metricsToSend.Add(metrics);
            }

            messages.GetAddComponentRequests().CopyTo(addComponentRequests);
            messages.GetRemoveComponentRequests().CopyTo(removeComponentRequests);

            messages.GetLogMessages().CopyTo(logMessages);

            foreach (var serializer in componentEventSerializers)
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
            addComponentRequests.Clear();
            removeComponentRequests.Clear();
            metricsToSend.Clear();
            logMessages.Clear();
            netFrameStats.Clear();
        }

        public void SendAndClear(Connection connection, CommandMetaData commandMetaData, NetFrameStats frameStats)
        {
            for (var i = 0; i < updates.Count; ++i)
            {
                ref readonly var update = ref updates[i];
                netFrameStats.AddUpdate(update.Update);
                connection.SendComponentUpdate(update.EntityId, update.Update, UpdateParams);
            }

            for (var i = 0; i < requests.Count; ++i)
            {
                ref readonly var request = ref requests[i];
                var commandParameters = new CommandParameters { AllowShortCircuit = request.AllowShortCircuit };
                var rawRequestId = connection.SendCommandRequest(request.EntityId, request.Request, request.Timeout, commandParameters);
                var requestId = new InternalCommandRequestId(rawRequestId);
                commandMetaData.AddInternalRequestId(request.Request.ComponentId, request.CommandId, request.RequestId,
                    requestId);
            }

            for (var i = 0; i < responses.Count; ++i)
            {
                ref readonly var response = ref responses[i];
                connection.SendCommandResponse(response.RequestId, response.Response);
            }

            for (var i = 0; i < failures.Count; ++i)
            {
                ref readonly var failure = ref failures[i];
                connection.SendCommandFailure(failure.RequestId, failure.Reason);
            }

            for (var i = 0; i < createEntityRequests.Count; ++i)
            {
                ref readonly var request = ref createEntityRequests[i];
                var rawRequestId = connection.SendCreateEntityRequest(request.Entity, request.EntityId, request.Timeout);
                var requestId = new InternalCommandRequestId(rawRequestId);
                commandMetaData.AddInternalRequestId(0, 0, request.RequestId, requestId);
            }

            for (var i = 0; i < deleteEntityRequests.Count; ++i)
            {
                ref readonly var request = ref deleteEntityRequests[i];
                var rawRequestId = connection.SendDeleteEntityRequest(request.EntityId, request.Timeout);
                var requestId = new InternalCommandRequestId(rawRequestId);
                commandMetaData.AddInternalRequestId(0, 0, request.RequestId, requestId);
            }

            for (var i = 0; i < reserveEntityIdsRequests.Count; ++i)
            {
                ref readonly var request = ref reserveEntityIdsRequests[i];
                var rawRequestId = connection.SendReserveEntityIdsRequest(request.NumberOfEntityIds, request.Timeout);
                var requestId = new InternalCommandRequestId(rawRequestId);
                commandMetaData.AddInternalRequestId(0, 0, request.RequestId, requestId);
            }

            for (var i = 0; i < entityQueryRequests.Count; ++i)
            {
                ref readonly var request = ref entityQueryRequests[i];
                var rawRequestId = connection.SendEntityQueryRequest(request.Query, request.Timeout);
                var requestId = new InternalCommandRequestId(rawRequestId);
                commandMetaData.AddInternalRequestId(0, 0, request.RequestId, requestId);
            }

            for (var i = 0; i < addComponentRequests.Count; ++i)
            {
                ref readonly var request = ref addComponentRequests[i];
                connection.SendAddComponent(request.EntityId.Id, request.ComponentData, request.Parameters);
            }

            for (var i = 0; i < removeComponentRequests.Count; ++i)
            {
                ref readonly var request = ref removeComponentRequests[i];
                connection.SendRemoveComponent(request.EntityId.Id, request.ComponentId, request.Parameters);
            }

            for (var i = 0; i < metricsToSend.Count; ++i)
            {
                connection.SendMetrics(metricsToSend[i]);
            }

            for (var i = 0; i < logMessages.Count; ++i)
            {
                ref readonly var logMessage = ref logMessages[i];
                connection.SendLogMessage(logMessage.LogLevel, logMessage.LoggerName, logMessage.Message,
                    logMessage.EntityId);
            }

            frameStats.Merge(netFrameStats);
            Clear();
        }

        public void AddComponentEvent(ComponentUpdate update, long entityId)
        {
            updates.Add(new UpdateToSend(update, entityId));
        }

        public void AddRequest(CommandRequest request, uint commandId, long entityId, uint? timeout, bool allowShortCircuit, CommandRequestId requestId)
        {
            requests.Add(new RequestToSend(request, commandId, entityId, timeout, allowShortCircuit, requestId));
            netFrameStats.AddCommandRequest(request);
        }

        public void AddResponse(CommandResponse response, long requestId)
        {
            responses.Add(new ResponseToSend(response, requestId));
            netFrameStats.AddCommandResponse(response, message: null);
        }

        public void AddFailure(uint componentId, uint commandIndex, string reason, long requestId)
        {
            failures.Add(new FailureToSend(reason, requestId));
            netFrameStats.AddCommandResponse(reason, componentId, commandIndex);
        }

        public void AddCreateEntityRequest(Entity entity, long? entityId, uint? timeout, CommandRequestId requestId)
        {
            createEntityRequests.Add(new CreateEntityRequestToSend(entity, entityId, timeout, requestId));
            netFrameStats.AddWorldCommandRequest(WorldCommand.CreateEntity);
        }

        public void AddDeleteEntityRequest(long entityId, uint? timeout, CommandRequestId requestId)
        {
            deleteEntityRequests.Add(new DeleteEntityRequestToSend(entityId, timeout, requestId));
            netFrameStats.AddWorldCommandRequest(WorldCommand.DeleteEntity);
        }

        public void AddReserveEntityIdsRequest(uint numberOfEntityIds, uint? timeout, CommandRequestId requestId)
        {
            reserveEntityIdsRequests.Add(new ReserveEntityIdsRequestToSend(numberOfEntityIds, timeout, requestId));
            netFrameStats.AddWorldCommandRequest(WorldCommand.ReserveEntityIds);
        }

        public void AddEntityQueryRequest(EntityQuery query, uint? timeout, CommandRequestId requestId)
        {
            entityQueryRequests.Add(new EntityQueryRequestToSend(query, timeout, requestId));
            netFrameStats.AddWorldCommandRequest(WorldCommand.EntityQuery);
        }

        internal void DestroyUnsentMessages()
        {
            for (var i = 0; i < updates.Count; ++i)
            {
                ref readonly var update = ref updates[i];
                update.Update.SchemaData.Value.Destroy();
            }

            for (var i = 0; i < requests.Count; ++i)
            {
                ref readonly var request = ref requests[i];
                request.Request.SchemaData.Value.Destroy();
            }

            for (var i = 0; i < responses.Count; ++i)
            {
                ref readonly var response = ref responses[i];
                response.Response.SchemaData.Value.Destroy();
            }
        }

        #region Containers

        public readonly struct UpdateToSend
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
            public readonly bool AllowShortCircuit;
            public readonly CommandRequestId RequestId;

            public RequestToSend(CommandRequest request, uint commandId, long entityId, uint? timeout, bool allowShortCircuit, CommandRequestId requestId)
            {
                Request = request;
                CommandId = commandId;
                EntityId = entityId;
                Timeout = timeout;
                AllowShortCircuit = allowShortCircuit;
                RequestId = requestId;
            }
        }

        private readonly struct ResponseToSend
        {
            public readonly CommandResponse Response;
            public readonly long RequestId;

            public ResponseToSend(CommandResponse response, long requestId)
            {
                Response = response;
                RequestId = requestId;
            }
        }

        private readonly struct FailureToSend
        {
            public readonly string Reason;
            public readonly long RequestId;

            public FailureToSend(string reason, long requestId)
            {
                Reason = reason;
                RequestId = requestId;
            }
        }

        private readonly struct ReserveEntityIdsRequestToSend
        {
            public readonly uint NumberOfEntityIds;
            public readonly uint? Timeout;
            public readonly CommandRequestId RequestId;

            public ReserveEntityIdsRequestToSend(uint numberOfEntityIds, uint? timeout, CommandRequestId requestId)
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
            public readonly CommandRequestId RequestId;

            public CreateEntityRequestToSend(Entity entity, long? entityId, uint? timeout, CommandRequestId requestId)
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
            public readonly CommandRequestId RequestId;

            public DeleteEntityRequestToSend(long entityId, uint? timeout, CommandRequestId requestId)
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
            public readonly CommandRequestId RequestId;

            public EntityQueryRequestToSend(EntityQuery query, uint? timeout, CommandRequestId requestId)
            {
                Query = query;
                Timeout = timeout;
                RequestId = requestId;
            }
        }

        #endregion
    }
}

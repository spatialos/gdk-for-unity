using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Core
{
    public class WorldCommandSerializer : ICommandSerializer
    {
        public void Serialize(MessagesToSend messages, SerializedMessagesToSend serializedMessages, CommandMetaData commandMetaData)
        {
            var storage = messages.GetWorldCommandStorage();

            SerializeCreateEntityRequests(storage, serializedMessages, commandMetaData);
            SerializeDeleteEntityRequests(storage, serializedMessages, commandMetaData);
            SerializeReserveEntityIdsRequests(storage, serializedMessages, commandMetaData);
            SerializeEntityQueryRequests(storage, serializedMessages, commandMetaData);
        }

        private void SerializeCreateEntityRequests(WorldCommandsToSendStorage storage, SerializedMessagesToSend serializedMessages, CommandMetaData commandMetaData)
        {
            var requests = storage.GetCreateEntityResponses();

            for (int i = 0; i < requests.Count; ++i)
            {
                ref readonly var request = ref requests[i];
                var context = new CommandContext<WorldCommands.CreateEntity.Request>(request.SendingEntity,
                    request.Request, request.Request.Context, request.RequestId);
                commandMetaData.AddRequest(0, 0, context);

                serializedMessages.AddCreateEntityRequest(request.Request.Entity, request.Request.EntityId?.Id,
                    request.Request.TimeoutMillis, request.RequestId);
            }
        }

        private void SerializeDeleteEntityRequests(WorldCommandsToSendStorage storage, SerializedMessagesToSend serializedMessages, CommandMetaData commandMetaData)
        {
            var requests = storage.GetDeleteEntityResponses();

            for (int i = 0; i < requests.Count; ++i)
            {
                ref readonly var request = ref requests[i];
                var context = new CommandContext<WorldCommands.DeleteEntity.Request>(request.SendingEntity,
                    request.Request, request.Request.Context, request.RequestId);
                commandMetaData.AddRequest(0, 0, context);

                serializedMessages.AddDeleteEntityRequest(request.Request.EntityId.Id, request.Request.TimeoutMillis,
                    request.RequestId);
            }
        }

        private void SerializeReserveEntityIdsRequests(WorldCommandsToSendStorage storage, SerializedMessagesToSend serializedMessages, CommandMetaData commandMetaData)
        {
            var requests = storage.GetReserveEntityIdResponses();

            for (int i = 0; i < requests.Count; ++i)
            {
                ref readonly var request = ref requests[i];
                var context = new CommandContext<WorldCommands.ReserveEntityIds.Request>(request.SendingEntity,
                    request.Request, request.Request.Context, request.RequestId);
                commandMetaData.AddRequest(0, 0, context);

                serializedMessages.AddReserveEntityIdsRequest(request.Request.NumberOfEntityIds,
                    request.Request.TimeoutMillis, request.RequestId);
            }
        }

        private void SerializeEntityQueryRequests(WorldCommandsToSendStorage storage, SerializedMessagesToSend serializedMessages, CommandMetaData commandMetaData)
        {
            var requests = storage.GetEntityQueryResponses();

            for (int i = 0; i < requests.Count; ++i)
            {
                ref readonly var request = ref requests[i];
                var context = new CommandContext<WorldCommands.EntityQuery.Request>(request.SendingEntity,
                    request.Request, request.Request.Context, request.RequestId);
                commandMetaData.AddRequest(0, 0, context);

                serializedMessages.AddEntityQueryRequest(request.Request.EntityQuery, request.Request.TimeoutMillis,
                    request.RequestId);
            }
        }
    }
}

using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public class WorldCommandsToSendStorage : ICommandSendStorage
        , ICommandRequestSendStorage<WorldCommands.CreateEntity.Request>
        , ICommandRequestSendStorage<WorldCommands.DeleteEntity.Request>
        , ICommandRequestSendStorage<WorldCommands.ReserveEntityIds.Request>
        , ICommandRequestSendStorage<WorldCommands.EntityQuery.Request>
    {
        private readonly MessageList<CommandRequestWithMetaData<WorldCommands.CreateEntity.Request>> createEntityResponses =
            new MessageList<CommandRequestWithMetaData<WorldCommands.CreateEntity.Request>>();

        private readonly MessageList<CommandRequestWithMetaData<WorldCommands.DeleteEntity.Request>> deleteEntityResponses =
            new MessageList<CommandRequestWithMetaData<WorldCommands.DeleteEntity.Request>>();

        private readonly MessageList<CommandRequestWithMetaData<WorldCommands.ReserveEntityIds.Request>> reserveEntityIdsResponses =
            new MessageList<CommandRequestWithMetaData<WorldCommands.ReserveEntityIds.Request>>();

        private readonly MessageList<CommandRequestWithMetaData<WorldCommands.EntityQuery.Request>> entityQueryResponses =
            new MessageList<CommandRequestWithMetaData<WorldCommands.EntityQuery.Request>>();

        public bool Dirty { get; private set; }

        public void Clear()
        {
            createEntityResponses.Clear();
            deleteEntityResponses.Clear();
            reserveEntityIdsResponses.Clear();
            entityQueryResponses.Clear();
            Dirty = false;
        }

        public void AddRequest(WorldCommands.CreateEntity.Request request, Entity sendingEntity, CommandRequestId requestId)
        {
            createEntityResponses.Add(
                new CommandRequestWithMetaData<WorldCommands.CreateEntity.Request>(request, sendingEntity,
                    requestId));
            Dirty = true;
        }

        public void AddRequest(WorldCommands.DeleteEntity.Request request, Entity sendingEntity, CommandRequestId requestId)
        {
            deleteEntityResponses.Add(
                new CommandRequestWithMetaData<WorldCommands.DeleteEntity.Request>(request, sendingEntity,
                    requestId));
            Dirty = true;
        }

        public void AddRequest(WorldCommands.ReserveEntityIds.Request request, Entity sendingEntity, CommandRequestId requestId)
        {
            reserveEntityIdsResponses.Add(
                new CommandRequestWithMetaData<WorldCommands.ReserveEntityIds.Request>(request, sendingEntity,
                    requestId));
            Dirty = true;
        }

        public void AddRequest(WorldCommands.EntityQuery.Request request, Entity sendingEntity, CommandRequestId requestId)
        {
            entityQueryResponses.Add(
                new CommandRequestWithMetaData<WorldCommands.EntityQuery.Request>(request, sendingEntity,
                    requestId));
            Dirty = true;
        }

        internal MessageList<CommandRequestWithMetaData<WorldCommands.CreateEntity.Request>> GetCreateEntityResponses()
        {
            return createEntityResponses;
        }

        internal MessageList<CommandRequestWithMetaData<WorldCommands.DeleteEntity.Request>> GetDeleteEntityResponses()
        {
            return deleteEntityResponses;
        }

        internal MessageList<CommandRequestWithMetaData<WorldCommands.ReserveEntityIds.Request>> GetReserveEntityIdResponses()
        {
            return reserveEntityIdsResponses;
        }

        internal MessageList<CommandRequestWithMetaData<WorldCommands.EntityQuery.Request>> GetEntityQueryResponses()
        {
            return entityQueryResponses;
        }
    }
}

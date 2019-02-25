using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Core
{
    public class WorldCommandsReceivedStorage : ICommandDiffStorage
        , IDiffCommandResponseStorage<WorldCommands.CreateEntity.ReceivedResponse>
        , IDiffCommandResponseStorage<WorldCommands.DeleteEntity.ReceivedResponse>
        , IDiffCommandResponseStorage<WorldCommands.ReserveEntityIds.ReceivedResponse>
        , IDiffCommandResponseStorage<WorldCommands.EntityQuery.ReceivedResponse>
    {
        private readonly MessageList<WorldCommands.CreateEntity.ReceivedResponse> createEntityResponses =
            new MessageList<WorldCommands.CreateEntity.ReceivedResponse>();

        private readonly MessageList<WorldCommands.DeleteEntity.ReceivedResponse> deleteEntityResponses =
            new MessageList<WorldCommands.DeleteEntity.ReceivedResponse>();

        private readonly MessageList<WorldCommands.ReserveEntityIds.ReceivedResponse> reserveEntityIdsResponses =
            new MessageList<WorldCommands.ReserveEntityIds.ReceivedResponse>();

        private readonly MessageList<WorldCommands.EntityQuery.ReceivedResponse> entityQueryResponses =
            new MessageList<WorldCommands.EntityQuery.ReceivedResponse>();

        public void Clear()
        {
            createEntityResponses.Clear();
            deleteEntityResponses.Clear();
            reserveEntityIdsResponses.Clear();
            entityQueryResponses.Clear();
        }

        public void AddResponse(WorldCommands.CreateEntity.ReceivedResponse response)
        {
            createEntityResponses.Add(response);
        }

        public void AddResponse(WorldCommands.DeleteEntity.ReceivedResponse response)
        {
            deleteEntityResponses.Add(response);
        }

        public void AddResponse(WorldCommands.ReserveEntityIds.ReceivedResponse response)
        {
            reserveEntityIdsResponses.Add(response);
        }

        public void AddResponse(WorldCommands.EntityQuery.ReceivedResponse response)
        {
            entityQueryResponses.Add(response);
        }

        ReceivedMessagesSpan<WorldCommands.CreateEntity.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.CreateEntity.ReceivedResponse>.GetResponses()
        {
            return new ReceivedMessagesSpan<WorldCommands.CreateEntity.ReceivedResponse>(createEntityResponses);
        }

        ReceivedMessagesSpan<WorldCommands.DeleteEntity.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.DeleteEntity.ReceivedResponse>.GetResponses()
        {
            return new ReceivedMessagesSpan<WorldCommands.DeleteEntity.ReceivedResponse>(deleteEntityResponses);
        }

        ReceivedMessagesSpan<WorldCommands.ReserveEntityIds.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.ReserveEntityIds.ReceivedResponse>.GetResponses()
        {
            return new ReceivedMessagesSpan<WorldCommands.ReserveEntityIds.ReceivedResponse>(reserveEntityIdsResponses);
        }

        ReceivedMessagesSpan<WorldCommands.EntityQuery.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.EntityQuery.ReceivedResponse>.GetResponses()
        {
            return new ReceivedMessagesSpan<WorldCommands.EntityQuery.ReceivedResponse>(entityQueryResponses);
        }
    }
}

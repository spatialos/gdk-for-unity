using System.Collections.Generic;
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

        private readonly Comparer comparer = new Comparer();

        private bool createEntitySorted;
        private bool deleteEntitySorted;
        private bool reserveEntityIdsSorted;
        private bool entityQueriesSorted;

        public void Clear()
        {
            createEntityResponses.Clear();
            deleteEntityResponses.Clear();
            reserveEntityIdsResponses.Clear();
            entityQueryResponses.Clear();
            createEntitySorted = false;
            deleteEntitySorted = false;
            reserveEntityIdsSorted = false;
            entityQueriesSorted = false;
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

        ReceivedMessagesSpan<WorldCommands.CreateEntity.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.CreateEntity.ReceivedResponse>.GetResponse(long requestId)
        {
            if (!createEntitySorted)
            {
                createEntityResponses.Sort(comparer);
                createEntitySorted = true;
            }

            var responseIndex = createEntityResponses.GetResponseIndex(requestId);
            if (responseIndex < 0)
            {
                return ReceivedMessagesSpan<WorldCommands.CreateEntity.ReceivedResponse>.Empty();
            }

            return new ReceivedMessagesSpan<WorldCommands.CreateEntity.ReceivedResponse>(createEntityResponses,
                responseIndex, 1);
        }

        ReceivedMessagesSpan<WorldCommands.DeleteEntity.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.DeleteEntity.ReceivedResponse>.GetResponse(long requestId)
        {
            if (!deleteEntitySorted)
            {
                deleteEntityResponses.Sort(comparer);
                deleteEntitySorted = true;
            }

            var responseIndex = deleteEntityResponses.GetResponseIndex(requestId);
            if (responseIndex < 0)
            {
                return ReceivedMessagesSpan<WorldCommands.DeleteEntity.ReceivedResponse>.Empty();
            }

            return new ReceivedMessagesSpan<WorldCommands.DeleteEntity.ReceivedResponse>(deleteEntityResponses,
                responseIndex, 1);
        }

        ReceivedMessagesSpan<WorldCommands.ReserveEntityIds.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.ReserveEntityIds.ReceivedResponse>.GetResponse(long requestId)
        {
            if (!reserveEntityIdsSorted)
            {
                reserveEntityIdsResponses.Sort(comparer);
                reserveEntityIdsSorted = true;
            }

            var responseIndex = reserveEntityIdsResponses.GetResponseIndex(requestId);
            if (responseIndex < 0)
            {
                return ReceivedMessagesSpan<WorldCommands.ReserveEntityIds.ReceivedResponse>.Empty();
            }

            return new ReceivedMessagesSpan<WorldCommands.ReserveEntityIds.ReceivedResponse>(reserveEntityIdsResponses,
                responseIndex, 1);
        }

        ReceivedMessagesSpan<WorldCommands.EntityQuery.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.EntityQuery.ReceivedResponse>.GetResponse(long requestId)
        {
            if (!entityQueriesSorted)
            {
                entityQueryResponses.Sort(comparer);
                entityQueriesSorted = true;
            }

            var responseIndex = entityQueryResponses.GetResponseIndex(requestId);
            if (responseIndex < 0)
            {
                return ReceivedMessagesSpan<WorldCommands.EntityQuery.ReceivedResponse>.Empty();
            }

            return new ReceivedMessagesSpan<WorldCommands.EntityQuery.ReceivedResponse>(entityQueryResponses,
                responseIndex, 1);
        }

        private class Comparer : IComparer<WorldCommands.CreateEntity.ReceivedResponse>,
            IComparer<WorldCommands.DeleteEntity.ReceivedResponse>,
            IComparer<WorldCommands.ReserveEntityIds.ReceivedResponse>,
            IComparer<WorldCommands.EntityQuery.ReceivedResponse>
        {
            public int Compare(WorldCommands.CreateEntity.ReceivedResponse x,
                WorldCommands.CreateEntity.ReceivedResponse y)
            {
                return x.RequestId.CompareTo(y.RequestId);
            }

            public int Compare(WorldCommands.DeleteEntity.ReceivedResponse x,
                WorldCommands.DeleteEntity.ReceivedResponse y)
            {
                return x.RequestId.CompareTo(y.RequestId);
            }

            public int Compare(WorldCommands.ReserveEntityIds.ReceivedResponse x,
                WorldCommands.ReserveEntityIds.ReceivedResponse y)
            {
                return x.RequestId.CompareTo(y.RequestId);
            }

            public int Compare(WorldCommands.EntityQuery.ReceivedResponse x,
                WorldCommands.EntityQuery.ReceivedResponse y)
            {
                return x.RequestId.CompareTo(y.RequestId);
            }
        }
    }
}

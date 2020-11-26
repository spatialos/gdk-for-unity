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
            new MessageList<WorldCommands.CreateEntity.ReceivedResponse>(new Comparer());

        private readonly MessageList<WorldCommands.DeleteEntity.ReceivedResponse> deleteEntityResponses =
            new MessageList<WorldCommands.DeleteEntity.ReceivedResponse>(new Comparer());

        private readonly MessageList<WorldCommands.ReserveEntityIds.ReceivedResponse> reserveEntityIdsResponses =
            new MessageList<WorldCommands.ReserveEntityIds.ReceivedResponse>(new Comparer());

        private readonly MessageList<WorldCommands.EntityQuery.ReceivedResponse> entityQueryResponses =
            new MessageList<WorldCommands.EntityQuery.ReceivedResponse>(new Comparer());

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

        MessagesSpan<WorldCommands.CreateEntity.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.CreateEntity.ReceivedResponse>.GetResponses()
        {
            return createEntityResponses.Slice();
        }

        MessagesSpan<WorldCommands.DeleteEntity.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.DeleteEntity.ReceivedResponse>.GetResponses()
        {
            return deleteEntityResponses.Slice();
        }

        MessagesSpan<WorldCommands.ReserveEntityIds.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.ReserveEntityIds.ReceivedResponse>.GetResponses()
        {
            return reserveEntityIdsResponses.Slice();
        }

        MessagesSpan<WorldCommands.EntityQuery.ReceivedResponse>
            IDiffCommandResponseStorage<WorldCommands.EntityQuery.ReceivedResponse>.GetResponses()
        {
            return entityQueryResponses.Slice();
        }

        WorldCommands.CreateEntity.ReceivedResponse? IDiffCommandResponseStorage<WorldCommands.CreateEntity.ReceivedResponse>.GetResponse(CommandRequestId requestId)
        {
            return createEntityResponses.GetResponseIndex(requestId);
        }

        WorldCommands.DeleteEntity.ReceivedResponse?
            IDiffCommandResponseStorage<WorldCommands.DeleteEntity.ReceivedResponse>.GetResponse(CommandRequestId requestId)
        {
            return deleteEntityResponses.GetResponseIndex(requestId);
        }

        WorldCommands.ReserveEntityIds.ReceivedResponse?
            IDiffCommandResponseStorage<WorldCommands.ReserveEntityIds.ReceivedResponse>.GetResponse(CommandRequestId requestId)
        {
            return reserveEntityIdsResponses.GetResponseIndex(requestId);
        }

        WorldCommands.EntityQuery.ReceivedResponse?
            IDiffCommandResponseStorage<WorldCommands.EntityQuery.ReceivedResponse>.GetResponse(CommandRequestId requestId)
        {
            return entityQueryResponses.GetResponseIndex(requestId);
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

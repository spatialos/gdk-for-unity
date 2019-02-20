using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class WorldCommandStorage
    {
        private readonly MessageList<CreateEntityResponseOp> createEntityResponses =
            new MessageList<CreateEntityResponseOp>();

        private readonly MessageList<DeleteEntityResponseOp> deleteEntityResponses =
            new MessageList<DeleteEntityResponseOp>();

        private readonly MessageList<ReserveEntityIdsResponseOp> reserveEntityIdsResponses =
            new MessageList<ReserveEntityIdsResponseOp>();

        private readonly MessageList<EntityQueryResponseOp> entityQueryResponses =
            new MessageList<EntityQueryResponseOp>();

        public void Clear()
        {
            createEntityResponses.Clear();
            deleteEntityResponses.Clear();
            reserveEntityIdsResponses.Clear();
            entityQueryResponses.Clear();
        }

        public void AddResponse(CreateEntityResponseOp response)
        {
            createEntityResponses.Add(response);
        }

        public void AddResponse(DeleteEntityResponseOp response)
        {
            deleteEntityResponses.Add(response);
        }

        public void AddResponse(ReserveEntityIdsResponseOp response)
        {
            reserveEntityIdsResponses.Add(response);
        }

        public void AddResponse(EntityQueryResponseOp response)
        {
            entityQueryResponses.Add(response);
        }

        internal MessageList<CreateEntityResponseOp> GetCreateEntityResponses()
        {
            return createEntityResponses;
        }

        internal MessageList<DeleteEntityResponseOp> GetDeleteEntityResponses()
        {
            return deleteEntityResponses;
        }

        internal MessageList<ReserveEntityIdsResponseOp> GetReserveEntityIdsResponseOps()
        {
            return reserveEntityIdsResponses;
        }

        internal MessageList<EntityQueryResponseOp> GetEntityQueryResponses()
        {
            return entityQueryResponses;
        }
    }
}

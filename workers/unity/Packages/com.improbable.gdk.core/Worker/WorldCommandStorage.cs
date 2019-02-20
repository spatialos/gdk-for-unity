using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class WorldCommandStorage
    {
        private readonly ReceivedMessageList<CreateEntityResponseOp> createEntityResponses =
            new ReceivedMessageList<CreateEntityResponseOp>();

        private readonly ReceivedMessageList<DeleteEntityResponseOp> deleteEntityResponses =
            new ReceivedMessageList<DeleteEntityResponseOp>();

        private readonly ReceivedMessageList<ReserveEntityIdsResponseOp> reserveEntityIdsResponses =
            new ReceivedMessageList<ReserveEntityIdsResponseOp>();

        private readonly ReceivedMessageList<EntityQueryResponseOp> entityQueryResponses =
            new ReceivedMessageList<EntityQueryResponseOp>();

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

        internal ReceivedMessageList<CreateEntityResponseOp> GetCreateEntityResponses()
        {
            return createEntityResponses;
        }

        internal ReceivedMessageList<DeleteEntityResponseOp> GetDeleteEntityResponses()
        {
            return deleteEntityResponses;
        }

        internal ReceivedMessageList<ReserveEntityIdsResponseOp> GetReserveEntityIdsResponseOps()
        {
            return reserveEntityIdsResponses;
        }

        internal ReceivedMessageList<EntityQueryResponseOp> GetEntityQueryResponses()
        {
            return entityQueryResponses;
        }
    }
}

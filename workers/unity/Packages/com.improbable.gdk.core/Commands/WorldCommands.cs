
using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;

namespace Improbable.Gdk.Core.Commands
{
    public class CreateEntity
    {
        public struct Request
        {
            public Worker.Core.Entity Entity;
            public EntityId? EntityId;
            public uint? TimeoutMillis;
            public uint SenderEntityId;

            internal RequestId<CreateEntityRequest> Send(Connection connection)
            {
                return connection.SendCreateEntityRequest(Entity, EntityId, TimeoutMillis);
            } 
        }

        public struct Response : IIncomingCommandResponse
        {
            public StatusCode StatusCode { get; }
            public string Message { get; }
            public EntityId EntityId { get; }
        }
    }

    public class DeleteEntity
    {
        public struct Request {
            public EntityId EntityId;
            public uint? TimeoutMillis;
            public long SenderEntityId;

            internal RequestId<DeleteEntityRequest> Send(Connection connection)
            {
                return connection.SendDeleteEntityRequest(EntityId, TimeoutMillis);
            }
        }

        public struct Response : IIncomingCommandResponse
        {
            public StatusCode StatusCode { get; }
            public string Message { get; }
            public EntityId EntityId { get; }
        }
    }

    public class ReserveEntityIds
    {
        public struct Request
        {
            public uint NumberOfEntityIds;
            public uint? TimeoutMillis;
            public long SenderEntityId;
        }

        public struct Response : IIncomingCommandResponse
        {
            public StatusCode StatusCode { get; }
            public string Message { get; }
            public EntityId FirstEntityId { get; }
            public int NumberOfEntityIds { get; }
        }
    }

    public class EntityQuery
    {
        public struct Request
        {
            public Worker.Query.EntityQuery EntityQuery;
            public uint? TimeoutMillis;
            public long SenderEntityId;
        }

        public struct Response : IIncomingCommandResponse
        {
            public CommandStatusCode StatusCode { get; }
            public string Message { get; }
            public int ResultCount { get; }
            public Dictionary<long, Worker.Core.Entity> Result { get; }
        }
    }
}

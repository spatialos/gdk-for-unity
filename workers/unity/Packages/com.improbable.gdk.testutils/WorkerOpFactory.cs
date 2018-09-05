using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;
using UnityEngine;

namespace Improbable.Gdk.TestUtils
{
    public static class WorkerOpFactory
    {
        public static AddEntityOp CreateAddEntityOp(long entityId)
        {
            return new AddEntityOp
            {
                EntityId = new EntityId(entityId)
            };
        }

        public static RemoveEntityOp CreateRemoveEntityOp(long entityId)
        {
            return new RemoveEntityOp
            {
                EntityId = new EntityId(entityId)
            };
        }

        public static AddComponentOp CreateAddComponentOp(long entityId, uint componentId)
        {
            var schemaComponentData = new SchemaComponentData(componentId);

            return new AddComponentOp
            {
                Data = new ComponentData(schemaComponentData),
                EntityId = new EntityId(entityId)
            };
        }

        public static ComponentUpdateOp CreateComponentUpdateOp(long entityId, uint componentId)
        {
            var schemaComponentUpdate = new SchemaComponentUpdate(componentId);

            return new ComponentUpdateOp
            {
                Update = new ComponentUpdate(schemaComponentUpdate),
                EntityId = new EntityId(entityId)
            };
        }

        public static RemoveComponentOp CreateRemoveComponentOp(long entityId, uint componentId)
        {
            return new RemoveComponentOp
            {
                EntityId = new EntityId(entityId),
                ComponentId = componentId
            };
        }

        public static AuthorityChangeOp CreateAuthorityChangeOp(long entityId, uint componentId)
        {
            return new AuthorityChangeOp
            {
                EntityId = new EntityId(entityId),
                ComponentId = componentId
            };
        }

        public static CommandRequestOp CreateCommandRequestOp(uint componentId, uint commandIndex, long requestId)
        {
            var schemaRequest = new SchemaCommandRequest(componentId, commandIndex);

            return new CommandRequestOp
            {
                Request = new CommandRequest(schemaRequest),
                RequestId = new RequestId<IncomingCommandRequest>(requestId)
            };
        }

        public static CommandResponseOp CreateCommandResponseOp(uint componentId, uint commandIndex, long requestId)
        {
            var schemaResponse = new SchemaCommandResponse(componentId, commandIndex);

            return new CommandResponseOp
            {
                Response = new CommandResponse(schemaResponse),
                RequestId = new RequestId<OutgoingCommandRequest>(requestId)
            };
        }

        public static DisconnectOp CreateDisconnectOp(string reason)
        {
            return new DisconnectOp { Reason = reason};
        }

        public static CreateEntityResponseOp CreateCreateEntityResponseOp(long requestId)
        {
            return new CreateEntityResponseOp
            {
                RequestId = new RequestId<CreateEntityRequest>(requestId)
            };
        }

        public static DeleteEntityResponseOp CreateDeleteEntityResponseOp(long requestId)
        {
            return new DeleteEntityResponseOp
            {
                RequestId = new RequestId<DeleteEntityRequest>(requestId)
            };
        }

        public static ReserveEntityIdsResponseOp CreateReserveEntityIdsResponseOp(long requestId)
        {
            return new ReserveEntityIdsResponseOp
            {
                RequestId = new RequestId<ReserveEntityIdsRequest>(requestId)
            };
        }

        public static EntityQueryResponseOp CreatEntityQueryResponseOp(long requestId)
        {
            return new EntityQueryResponseOp
            {
                RequestId = new RequestId<EntityQueryRequest>(requestId)
            };
        }
    }
}

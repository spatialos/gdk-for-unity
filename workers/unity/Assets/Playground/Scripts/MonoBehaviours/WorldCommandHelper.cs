using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Worker;
using Improbable.Worker.Core;
using JetBrains.Annotations;

namespace Playground.MonoBehaviours
{
    public class WorldCommandHelper : IDisposable
    {
        private readonly WorldCommandsRequirables.WorldCommandResponseHandler responseHandler;
        private readonly WorldCommandsRequirables.WorldCommandRequestSender requestSender;

        private readonly HashSet<long> sentRequestIds = new HashSet<long>();

        public WorldCommandHelper(
            WorldCommandsRequirables.WorldCommandRequestSender requestSender,
            WorldCommandsRequirables.WorldCommandResponseHandler responseHandler)
        {
            this.requestSender = requestSender;
            this.responseHandler = responseHandler;

            responseHandler.OnReserveEntityIdsResponse += OnReserveEntityIdsResponse;
            responseHandler.OnCreateEntityResponse += OnCreateEntityResponse;
            responseHandler.OnDeleteEntityResponse += OnDeleteEntityResponse;
        }

        public void Dispose()
        {
            responseHandler.OnDeleteEntityResponse -= OnDeleteEntityResponse;
            responseHandler.OnCreateEntityResponse -= OnCreateEntityResponse;
            responseHandler.OnReserveEntityIdsResponse -= OnReserveEntityIdsResponse;
        }

        private void OnDeleteEntityResponse(WorldCommands.DeleteEntity.ReceivedResponse response)
        {
            var responseRequestId = response.RequestId;

            if (sentRequestIds.Remove(responseRequestId)
                && response.Context is Action<DeleteEntityResponseOp> callback)
            {
                callback(response.Op);
            }
        }

        private void OnCreateEntityResponse(WorldCommands.CreateEntity.ReceivedResponse response)
        {
            var responseRequestId = response.RequestId;

            if (sentRequestIds.Remove(responseRequestId)
                && response.Context is Action<CreateEntityResponseOp> callback)
            {
                callback(response.Op);
            }
        }

        private void OnReserveEntityIdsResponse(WorldCommands.ReserveEntityIds.ReceivedResponse response)
        {
            var responseRequestId = response.RequestId;

            if (sentRequestIds.Remove(responseRequestId) &&
                response.Context is Action<ReserveEntityIdsResponseOp> callback)
            {
                callback(response.Op);
            }
        }

        public void ReserveEntityIds(uint numberOfEntityIds, [NotNull] Action<ReserveEntityIdsResponseOp> action)
        {
            sentRequestIds.Add(requestSender.ReserveEntityIds(numberOfEntityIds, action).RequestId);
        }

        public void CreateEntity(Entity entityTemplate, EntityId? entityId,
            [NotNull] Action<CreateEntityResponseOp> action)
        {
            sentRequestIds.Add(requestSender.CreateEntity(entityTemplate, entityId, action).RequestId);
        }

        public void DeleteEntity(EntityId entityId, [NotNull] Action<DeleteEntityResponseOp> action)
        {
            sentRequestIds.Add(requestSender.DeleteEntity(entityId, action).RequestId);
        }
    }
}

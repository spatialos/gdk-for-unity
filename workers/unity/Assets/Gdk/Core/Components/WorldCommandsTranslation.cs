using System.Collections.Generic;
using Improbable.Gdk.Core.Components;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;
using TimeoutOption = Improbable.Collections.Option<uint>;

namespace Improbable.Gdk.Core
{
    internal struct CreateEntityRequest
    {
        public Worker.Entity Entity;
        public Collections.Option<EntityId> EntityId;
        public uint TimeoutMillis;
        public long SenderEntityId;
    }

    internal struct DeleteEntityRequest
    {
        public long EntityId;
        public uint TimeoutMillis;
        public long SenderEntityId;
    }

    internal struct ReserveEntityIdsRequest
    {
        public uint NumberOfEntityIds;
        public uint TimeoutMillis;
        public long SenderEntityId;
    }

    internal struct EntityQueryRequest
    {
        public Worker.Query.EntityQuery EntityQuery;
        public uint TimeoutMillis;
        public long SenderEntityId;
    }

    public struct CreateEntityResponse : IIncomingCommandResponse
    {
        public CommandStatusCode StatusCode { get; }
        public string Message { get; }
        public long EntityId { get; }

        internal CreateEntityResponse(CommandStatusCode statusCode, string message, long entityId)
        {
            StatusCode = statusCode;
            Message = message;
            EntityId = entityId;
        }
    }

    public struct DeleteEntityResponse : IIncomingCommandResponse
    {
        public CommandStatusCode StatusCode { get; }
        public string Message { get; }
        public long EntityId { get; }

        internal DeleteEntityResponse(CommandStatusCode statusCode, string message, long entityId)
        {
            StatusCode = statusCode;
            Message = message;
            EntityId = entityId;
        }
    }

    public struct ReserveEntityIdsResponse : IIncomingCommandResponse
    {
        public CommandStatusCode StatusCode { get; }
        public string Message { get; }
        public long FirstEntityId { get; }
        public int NumberOfEntityIds { get; }

        internal ReserveEntityIdsResponse(CommandStatusCode statusCode, string message, long firstEntityId,
            int numberOfEntityIds)
        {
            StatusCode = statusCode;
            Message = message;
            FirstEntityId = firstEntityId;
            NumberOfEntityIds = numberOfEntityIds;
        }
    }

    public struct EntityQueryResponse : IIncomingCommandResponse
    {
        public CommandStatusCode StatusCode { get; }
        public string Message { get; }
        public int ResultCount { get; }
        public Dictionary<long, Worker.Entity> Result { get; }

        internal EntityQueryResponse(CommandStatusCode statusCode, string message, int resultCount,
            Dictionary<long, Worker.Entity> result)
        {
            StatusCode = statusCode;
            Message = message;
            ResultCount = resultCount;
            Result = result;
        }
    }

    public struct WorldCommandSender : IComponentData
    {
        private long EntityId { get; }

        private uint HandleToTranslation { get; }

        public WorldCommandSender(long entityId, uint handleToTranslation)
        {
            EntityId = entityId;
            HandleToTranslation = handleToTranslation;
        }

        public void SendCreateEntityRequest(Worker.Entity entity, long entityId = 0, uint timeoutMillis = 0)
        {
            WorldCommandsTranslation translation =
                (WorldCommandsTranslation) ComponentTranslation.HandleToTranslation[HandleToTranslation];

            var entityIdOption = entityId != 0
                ? new Collections.Option<EntityId>(new EntityId(entityId))
                : new Collections.Option<EntityId>();

            translation.CreateEntityRequests.Add(new CreateEntityRequest
            {
                Entity = entity,
                EntityId = entityIdOption,
                TimeoutMillis = timeoutMillis,
                SenderEntityId = EntityId
            });
        }

        public void SendDeleteEntityRequest(long entityId, uint timeoutMillis = 0)
        {
            WorldCommandsTranslation translation =
                (WorldCommandsTranslation) ComponentTranslation.HandleToTranslation[HandleToTranslation];
            translation.DeleteEntityRequests.Add(new DeleteEntityRequest
            {
                EntityId = entityId,
                TimeoutMillis = timeoutMillis,
                SenderEntityId = EntityId
            });
        }

        public void SendReserveEntityIdsRequest(uint numberOfEntities, uint timeoutMillis = 0)
        {
            WorldCommandsTranslation translation =
                (WorldCommandsTranslation) ComponentTranslation.HandleToTranslation[HandleToTranslation];
            translation.ReserveEntityIdsRequests.Add(new ReserveEntityIdsRequest
            {
                NumberOfEntityIds = numberOfEntities,
                TimeoutMillis = timeoutMillis,
                SenderEntityId = EntityId
            });
        }

        public void SendEntityQueryRequest(Worker.Query.EntityQuery entityQuery, uint timeoutMillis = 0)
        {
            WorldCommandsTranslation translation =
                (WorldCommandsTranslation) ComponentTranslation.HandleToTranslation[HandleToTranslation];
            translation.EntityQueryRequests.Add(new EntityQueryRequest
            {
                EntityQuery = entityQuery,
                TimeoutMillis = timeoutMillis,
                SenderEntityId = EntityId
            });
        }
    }

    public class WorldCommandsTranslation : ComponentTranslation
    {
        public override ComponentType TargetComponentType => targetComponentType;
        private static readonly ComponentType targetComponentType = null;

        public override ComponentType[] ReplicationComponentTypes => replicationComponentTypes;
        private static readonly ComponentType[] replicationComponentTypes = { };

        public override ComponentType[] CleanUpComponentTypes => cleanUpComponentTypes;

        private static readonly ComponentType[] cleanUpComponentTypes =
        {
            typeof(CommandResponses<CreateEntityResponse>),
            typeof(CommandResponses<DeleteEntityResponse>),
            typeof(CommandResponses<ReserveEntityIdsResponse>),
            typeof(CommandResponses<EntityQueryResponse>),
        };

        private Dictionary<uint, long> RequestIdToEntityId = new Dictionary<uint, long>();

        internal List<CreateEntityRequest> CreateEntityRequests = new List<CreateEntityRequest>();
        internal List<DeleteEntityRequest> DeleteEntityRequests = new List<DeleteEntityRequest>();
        internal List<ReserveEntityIdsRequest> ReserveEntityIdsRequests = new List<ReserveEntityIdsRequest>();
        internal List<EntityQueryRequest> EntityQueryRequests = new List<EntityQueryRequest>();

        private static readonly ComponentPool<CommandResponses<CreateEntityResponse>> createEntityResponsePool =
            new ComponentPool<CommandResponses<CreateEntityResponse>>(
                () => new CommandResponses<CreateEntityResponse>(),
                component => component.Buffer.Clear());

        private static readonly ComponentPool<CommandResponses<DeleteEntityResponse>> deleteEntityResponsePool =
            new ComponentPool<CommandResponses<DeleteEntityResponse>>(
                () => new CommandResponses<DeleteEntityResponse>(),
                component => component.Buffer.Clear());

        private static readonly ComponentPool<CommandResponses<ReserveEntityIdsResponse>> reserveEntityIdsResponsesPool
            = new ComponentPool<CommandResponses<ReserveEntityIdsResponse>>(
                () => new CommandResponses<ReserveEntityIdsResponse>(),
                component => component.Buffer.Clear());

        private static readonly ComponentPool<CommandResponses<EntityQueryResponse>> entityQueryResponsePool =
            new ComponentPool<CommandResponses<EntityQueryResponse>>(
                () => new CommandResponses<EntityQueryResponse>(),
                component => component.Buffer.Clear());

        public WorldCommandsTranslation(MutableView view) : base(view)
        {
        }

        public override void AddCommandRequestSender(Entity entity, long EntityId)
        {
            view.AddComponent(entity, new WorldCommandSender(EntityId, translationHandle));
        }

        public override void ExecuteReplication(Connection connection)
        {
        }

        public override void SendCommands(Connection connection)
        {
            foreach (var request in CreateEntityRequests)
            {
                var requestId = connection.SendCreateEntityRequest(request.Entity, request.EntityId,
                    new TimeoutOption(request.TimeoutMillis));

                RequestIdToEntityId.Add(requestId.Id, request.SenderEntityId);
            }

            CreateEntityRequests.Clear();

            foreach (var request in DeleteEntityRequests)
            {
                var requestId = connection.SendDeleteEntityRequest(new EntityId(request.EntityId),
                    new TimeoutOption(request.TimeoutMillis));

                RequestIdToEntityId.Add(requestId.Id, request.SenderEntityId);
            }

            DeleteEntityRequests.Clear();

            foreach (var request in ReserveEntityIdsRequests)
            {
                var requestId = connection.SendReserveEntityIdsRequest(request.NumberOfEntityIds,
                    new TimeoutOption(request.TimeoutMillis));

                RequestIdToEntityId.Add(requestId.Id, request.SenderEntityId);
            }

            ReserveEntityIdsRequests.Clear();

            foreach (var request in EntityQueryRequests)
            {
                var requestId = connection.SendEntityQueryRequest(request.EntityQuery,
                    new TimeoutOption(request.TimeoutMillis));

                RequestIdToEntityId.Add(requestId.Id, request.SenderEntityId);
            }

            EntityQueryRequests.Clear();
        }

        public override void CleanUpComponents(ref EntityCommandBuffer entityCommandBuffer)
        {
            RemoveComponents<CommandResponses<CreateEntityResponse>>(ref entityCommandBuffer, 0);
            RemoveComponents<CommandResponses<DeleteEntityResponse>>(ref entityCommandBuffer, 1);
            RemoveComponents<CommandResponses<ReserveEntityIdsResponse>>(ref entityCommandBuffer, 2);
            RemoveComponents<CommandResponses<EntityQueryResponse>>(ref entityCommandBuffer, 3);
        }

        public override void RegisterWithDispatcher(Dispatcher dispatcher)
        {
            dispatcher.OnCreateEntityResponse(OnCreateEntityResponse);
            dispatcher.OnDeleteEntityResponse(OnDeleteEntityResponse);
            dispatcher.OnReserveEntityIdsResponse(OnReserveEntityIdResponse);
            dispatcher.OnEntityQueryResponse(OnEntityQueryResponse);
        }

        public void OnCreateEntityResponse(CreateEntityResponseOp op)
        {
            Entity entity;
            if (!TryGetEntityFromRequestId(op.RequestId.Id, "CreateEntity", out entity))
            {
                return;
            }

            var response =
                new CreateEntityResponse((CommandStatusCode) op.StatusCode, op.Message, op.EntityId.Value.Id);

            view.AddCommandResponse(entity, response, createEntityResponsePool);
        }

        public void OnDeleteEntityResponse(DeleteEntityResponseOp op)
        {
            Entity entity;
            if (!TryGetEntityFromRequestId(op.RequestId.Id, "DeleteEntity", out entity))
            {
                return;
            }

            var response =
                new DeleteEntityResponse((CommandStatusCode) op.StatusCode, op.Message, op.EntityId.Id);

            view.AddCommandResponse(entity, response, deleteEntityResponsePool);
        }

        public void OnReserveEntityIdResponse(ReserveEntityIdsResponseOp op)
        {
            Entity entity;
            if (!TryGetEntityFromRequestId(op.RequestId.Id, "ReserveEntityIds", out entity))
            {
                return;
            }

            var response = new ReserveEntityIdsResponse((CommandStatusCode) op.StatusCode, op.Message,
                op.FirstEntityId.Value.Id, op.NumberOfEntityIds);

            view.AddCommandResponse(entity, response, reserveEntityIdsResponsesPool);
        }

        public void OnEntityQueryResponse(EntityQueryResponseOp op)
        {
            Entity entity;
            if (!TryGetEntityFromRequestId(op.RequestId.Id, "EntityQuery", out entity))
            {
                return;
            }

            var result = new Dictionary<long, Worker.Entity>();
            foreach (var pair in op.Result)
            {
                result.Add(pair.Key.Id, pair.Value);
            }

            var response =
                new EntityQueryResponse((CommandStatusCode) op.StatusCode, op.Message, op.ResultCount, result);

            view.AddCommandResponse(entity, response, entityQueryResponsePool);
        }

        private bool TryGetEntityFromRequestId(uint requestId, string responseName, out Entity entity)
        {
            entity = Entity.Null;

            long entityId;
            if (!RequestIdToEntityId.TryGetValue(requestId, out entityId))
            {
                Debug.LogErrorFormat(TranslationErrors.RequestDoesNotExist, requestId, responseName);
                return false;
            }

            RequestIdToEntityId.Remove(requestId);

            if (entityId == MutableView.WorkerEntityId)
            {
                entity = view.WorkerEntity;
            }
            else if (!view.TryGetEntity(entityId, out entity))
            {
                Debug.LogWarningFormat(TranslationErrors.CannotFindEntityForWorldCommandResponse, entityId, responseName);
                return false;
            }

            return true;
        }
    }
}

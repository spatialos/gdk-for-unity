using System.Collections.Generic;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Query;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.Commands
{
    public static partial class WorldCommands
    {
        public static class EntityQuery
        {
            /// <summary>
            ///     An object that is a EntityQuery command request.
            /// </summary>
            public struct Request : ICommandRequest
            {
                public Improbable.Worker.CInterop.Query.EntityQuery EntityQuery;
                public uint? TimeoutMillis;
                public object Context;

                /// <summary>
                ///     Method to create an EntityQuery command request payload.
                /// </summary>
                /// <param name="entityQuery">The EntityQuery object defining the constraints and query type.</param>
                /// <param name="timeoutMillis">
                ///     (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds.
                /// </param>
                /// <param name="context">
                ///    (Optional) A context object that will be returned with the command response.
                /// </param>
                /// <returns></returns>
                public Request(Improbable.Worker.CInterop.Query.EntityQuery entityQuery,
                    uint? timeoutMillis = null, object context = null)
                {
                    EntityQuery = entityQuery;
                    TimeoutMillis = timeoutMillis;
                    Context = context;
                }
            }

            /// <summary>
            ///     An object that is the response of an EntityQuery command from the SpatialOS runtime.
            /// </summary>
            public readonly struct ReceivedResponse : IReceivedCommandResponse
            {
                public readonly Entity SendingEntity;

                /// <summary>
                ///     The status code of the command response. If equal to <see cref="StatusCode"/>.Success then
                ///     the command succeeded.
                /// </summary>
                public readonly StatusCode StatusCode;

                /// <summary>
                ///     The failure message of the command. Will only be non-null if the command failed.
                /// </summary>
                public readonly string Message;

                /// <summary>
                ///     A dictionary that represents the results of a <see cref="SnapshotResultType"/> entity query.
                /// </summary>
                public readonly Dictionary<EntityId, EntitySnapshot> Result;

                /// <summary>
                ///     The number of entities that matched the entity query constraints.
                /// </summary>
                public int ResultCount => Result.Count;

                /// <summary>
                ///     The request payload that was originally sent with this command.
                /// </summary>
                public readonly Request RequestPayload;

                /// <summary>
                ///     The context object that was provided when sending the command.
                /// </summary>
                public readonly object Context;

                /// <summary>
                ///     The unique request ID of this command. Will match the request ID in the corresponding request.
                /// </summary>
                public readonly CommandRequestId RequestId;

                internal ReceivedResponse(EntityQueryResponseOp op, Entity sendingEntity, Request req, CommandRequestId requestId)
                {
                    SendingEntity = sendingEntity;
                    StatusCode = op.StatusCode;
                    Message = op.Message;
                    RequestPayload = req;
                    Context = req.Context;
                    RequestId = requestId;

                    Result = new Dictionary<EntityId, EntitySnapshot>();
                    foreach (var entityIdToEntity in op.Result)
                    {
                        Result.Add(new EntityId(entityIdToEntity.Key), new EntitySnapshot(entityIdToEntity.Value));
                    }
                }

                CommandRequestId IReceivedCommandResponse.RequestId => RequestId;
            }
        }
    }
}

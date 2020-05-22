using Improbable.Worker.CInterop;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.Commands
{
    public static partial class WorldCommands
    {
        public static class ReserveEntityIds
        {
            /// <summary>
            ///     An object that is a ReserveEntityIds command request.
            /// </summary>
            public struct Request : ICommandRequest
            {
                public uint NumberOfEntityIds;
                public uint? TimeoutMillis;
                public object Context;

                /// <summary>
                ///     Used to create a ReserveEntityIds command request payload.
                /// </summary>
                /// <param name="numberOfEntityIds">The number of entity IDs to reserve.</param>
                /// <param name="timeoutMillis">
                ///     (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds.
                /// </param>
                /// <param name="context">
                ///    (Optional) A context object that will be returned with the command response.
                /// </param>
                /// <returns>The ReserveEntityIds command request payload.</returns>
                public Request(uint numberOfEntityIds, uint? timeoutMillis = null,
                    object context = null)
                {
                    NumberOfEntityIds = numberOfEntityIds;
                    TimeoutMillis = timeoutMillis;
                    Context = context;
                }
            }

            /// <summary>
            ///     An object that is the response of a ReserveEntityIds command from the SpatialOS runtime.
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
                ///     The first entity ID in the range that was reserved. Will only be non-null if the command
                ///     succeeded.
                /// </summary>
                public readonly EntityId? FirstEntityId;

                /// <summary>
                ///     The number of entity IDs that were reserved.
                /// </summary>
                public readonly int NumberOfEntityIds;

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

                internal ReceivedResponse(ReserveEntityIdsResponseOp op, Entity sendingEntity, Request req, CommandRequestId requestId)
                {
                    SendingEntity = sendingEntity;
                    StatusCode = op.StatusCode;
                    Message = op.Message;
                    NumberOfEntityIds = op.NumberOfEntityIds;
                    RequestPayload = req;
                    Context = req.Context;
                    RequestId = requestId;

                    FirstEntityId = op.FirstEntityId.HasValue
                        ? new EntityId(op.FirstEntityId.Value)
                        : (EntityId?) null;
                }

                CommandRequestId IReceivedCommandResponse.RequestId => RequestId;
            }
        }
    }
}

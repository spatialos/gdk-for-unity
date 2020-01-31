using Improbable.Worker.CInterop;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.Commands
{
    public static partial class WorldCommands
    {
        public static class CreateEntity
        {
            /// <summary>
            ///     An object that is a CreateEntity command request.
            /// </summary>
            public struct Request : ICommandRequest
            {
                public Improbable.Worker.CInterop.Entity Entity;
                public EntityId? EntityId;
                public uint? TimeoutMillis;
                public object Context;

                /// <summary>
                ///     Constructor to create a CreateEntity command request payload.
                /// </summary>
                /// <param name="template">
                ///     The EntityTemplate object that defines the SpatialOS components on the to-be-created entity.
                /// </param>
                /// <param name="entityId">
                ///     (Optional) The EntityId that the to-be-created entity should take.
                ///     This should only be provided if received as the result of a ReserveEntityIds command.
                /// </param>
                /// <param name="timeoutMillis">
                ///     (Optional) The command timeout in milliseconds. If not specified, will default to 5 seconds.
                /// </param>
                /// <param name="context">
                ///    (Optional) A context object that will be returned with the command response.
                /// </param>
                /// <returns>The CreateEntity command request payload.</returns>
                public Request(EntityTemplate template, EntityId? entityId = null,
                    uint? timeoutMillis = null, object context = null)
                {
                    Entity = template.GetEntity();
                    EntityId = entityId;
                    TimeoutMillis = timeoutMillis;
                    Context = context;
                }
            }

            /// <summary>
            ///     An object that is the response of a CreateEntity command from the SpatialOS runtime.
            /// </summary>
            public readonly struct ReceivedResponse : IReceivedCommandResponse
            {
                public readonly Unity.Entities.Entity SendingEntity;

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
                ///     The Entity ID of the created entity. Will only be non-null if the command succeeded.
                /// </summary>
                public readonly EntityId? EntityId;

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
                public readonly long RequestId;

                internal ReceivedResponse(CreateEntityResponseOp op, Entity sendingEntity, Request req, long requestId)
                {
                    SendingEntity = sendingEntity;
                    StatusCode = op.StatusCode;
                    Message = op.Message;
                    RequestPayload = req;
                    Context = req.Context;
                    RequestId = requestId;

                    EntityId = op.EntityId.HasValue
                        ? new EntityId(op.EntityId.Value)
                        : (EntityId?) null;
                }

                long IReceivedCommandResponse.RequestId => RequestId;
            }
        }
    }
}

using Improbable.Worker;

namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandRequest
    {
    }

    public interface IReceivedCommandRequest
    {
        /// <summary>
        ///     Gets the request ID from the request. For use in generic methods.
        /// </summary>
        /// <returns> The request ID associated with the request </returns>
        long GetRequestId();

        /// <summary>
        ///     Gets the target entity ID from the request. For use in generic methods.
        /// </summary>
        /// <returns> The target entity ID associated with the request </returns>
        EntityId GetTargetEntityId();
    }

    public interface ICommandResponse
    {
    }

    public interface IReceivedCommandResponse
    {
        /// <summary>
        ///     Gets the request ID from the request. For use in generic methods.
        /// </summary>
        /// <returns> The request ID associated with the request </returns>
        long GetRequestId();
    }
}

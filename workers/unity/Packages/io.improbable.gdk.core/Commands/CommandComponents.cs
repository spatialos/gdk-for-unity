namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandRequest
    {
    }

    public interface IReceivedCommandRequest : IReceivedEntityMessage
    {
        /// <summary>
        ///     Gets the request ID from the request. For use in generic methods.
        /// </summary>
        /// <returns> The request ID associated with the request </returns>
        long RequestId { get; }
        EntityId CallerWorkerEntityId { get; }
    }

    public interface ICommandResponse
    {
    }

    public interface IReceivedCommandResponse : IReceivedMessage
    {
        /// <summary>
        ///     Gets the request ID from the request. For use in generic methods.
        /// </summary>
        /// <returns> The request ID associated with the request </returns>
        CommandRequestId RequestId { get; }
    }

    public interface IRawReceivedCommandResponse
    {
    }
}

namespace Improbable.Gdk.Core
{
    public interface ICommandMetaDataStorage
    {
        uint GetComponentId();
        uint GetCommandId();

        void RemoveMetaData(uint internalRequestId);

        void AddRequestId(uint internalRequestId, long requestId);
        long GetRequestId(uint internalRequestId);
    }

    public interface ICommandPayloadStorage<T>
    {
        CommandContext<T> GetPayload(long requestId);
        void AddRequest(CommandContext<T> context, long requestId);
    }
}

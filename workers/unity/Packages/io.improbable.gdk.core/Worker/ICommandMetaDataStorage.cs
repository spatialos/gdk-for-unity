namespace Improbable.Gdk.Core
{
    public interface ICommandMetaDataStorage
    {
        uint GetComponentId();
        uint GetCommandId();

        void RemoveMetaData(long internalRequestId);

        void SetInternalRequestId(long internalRequestId, long requestId);
    }

    public interface ICommandPayloadStorage<T>
    {
        CommandContext<T> GetPayload(long internalRequestId);
        void AddRequest(in CommandContext<T> context);
    }
}

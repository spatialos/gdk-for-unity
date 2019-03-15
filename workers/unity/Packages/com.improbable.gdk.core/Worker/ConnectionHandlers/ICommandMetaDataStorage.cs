namespace Improbable.Gdk.Core
{
    public interface ICommandMetaDataStorage
    {
        uint GetComponentId();
        uint GetCommandId();

        void RemoveMetaData(uint internalRequestId);

        void SetInternalRequestId(uint internalRequestId, long requestId);
    }

    public interface ICommandPayloadStorage<T>
    {
        CommandContext<T> GetPayload(uint internalRequestId);
        void AddRequest(in CommandContext<T> context);
    }
}

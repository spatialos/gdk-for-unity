using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public interface IComponentDiffDeserializer
    {
        uint GetComponentId();

        void AddUpdateToDiff(ComponentUpdateOp op, ViewDiff diff, uint updateId);
        void AddComponentToDiff(AddComponentOp op, ViewDiff diff, uint updateId);
    }

    public interface IComponentEventSerializer
    {
        uint GetComponentId();

        void Serialize(MessagesToSend messages, SerializedMessagesToSend serializedMessages);
    }
}

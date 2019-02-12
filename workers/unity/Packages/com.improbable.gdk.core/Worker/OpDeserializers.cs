using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public interface IComponentDiffDeserializer
    {
        uint GetComponentId();

        void AddUpdate(ComponentUpdateOp op, ViewDiff diff, uint updateId);
        void AddComponent(AddComponentOp op, ViewDiff diff);
    }

    public interface ICommandDiffDeserializer
    {
        uint GetComponentId();
        uint GetCommandId();

        void AddRequest(CommandRequestOp op, ViewDiff diff);
        void AddResponse(CommandResponseOp op, ViewDiff diff);
    }
}

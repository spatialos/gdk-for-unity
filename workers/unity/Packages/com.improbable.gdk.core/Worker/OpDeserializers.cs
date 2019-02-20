using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public interface IComponentDiffDeserializer
    {
        uint GetComponentId();

        void AddUpdateToDiff(ComponentUpdateOp op, ViewDiff diff, uint updateId);
        void AddComponentToDiff(AddComponentOp op, ViewDiff diff);
    }

    public interface ICommandDiffDeserializer
    {
        uint GetComponentId();
        uint GetCommandId();

        void AddRequestToDiff(CommandRequestOp op, ViewDiff diff);
        void AddResponseToDiff(CommandResponseOp op, ViewDiff diff);
    }
}

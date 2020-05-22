using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandDiffDeserializer
    {
        void AddRequestToDiff(CommandRequestOp op, ViewDiff diff);
        void AddResponseToDiff(CommandResponseOp op, ViewDiff diff, CommandMetaData commandMetaData);
    }
}

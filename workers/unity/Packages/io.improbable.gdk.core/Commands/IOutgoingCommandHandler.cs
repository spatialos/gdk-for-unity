using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public interface IOutgoingCommandHandler
    {
        CommandRequestId SendCommand<T>(T request, Entity sendingEntity = default) where T : ICommandRequest;

        void SendResponse<T>(T response) where T : ICommandResponse;
    }
}

using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandRequestSendStorage<in T> : ICommandSendStorage
        where T : ICommandRequest
    {
        void AddRequest(T request, Entity sendingEntity, CommandRequestId requestId);
    }
}

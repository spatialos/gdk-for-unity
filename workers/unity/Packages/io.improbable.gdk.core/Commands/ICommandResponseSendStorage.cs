namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandResponseSendStorage<in T> : ICommandSendStorage
        where T : ICommandResponse
    {
        void AddResponse(T response);
    }
}

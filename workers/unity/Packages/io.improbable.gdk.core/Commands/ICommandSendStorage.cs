namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandSendStorage
    {
        bool Dirty { get; }
        void Clear();
    }
}

using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandSender
    {
        void SendAll();
        void Init(World world);
    }
}

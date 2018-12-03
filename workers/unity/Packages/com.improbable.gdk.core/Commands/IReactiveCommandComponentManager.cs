using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public interface IReactiveCommandComponentManager
    {
        void PopulateReactiveCommandComponents(CommandSystem commandSystem, EntityManager entityManger,
            WorkerSystem workerSystem, World world);

        void Clean(World world);
    }
}

using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface IReactiveComponentManager
    {
        void PopulateReactiveComponents(ComponentUpdateSystem updateSystem, EntityManager entityManger, WorkerSystem workerSystem, World world);
        void Clean(World world);
    }
}

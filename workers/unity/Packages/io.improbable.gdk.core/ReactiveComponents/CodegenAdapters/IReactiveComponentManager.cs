#if USE_LEGACY_REACTIVE_COMPONENTS
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    public interface IReactiveComponentManager
    {
        void PopulateReactiveComponents(ComponentUpdateSystem updateSystem, EntityManager entityManger, WorkerSystem workerSystem, World world);
        void Clean(World world);
    }
}
#endif

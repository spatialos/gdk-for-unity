using Unity.Entities;

namespace Improbable.Gdk.Core
{
    internal class WorkerSystem : ComponentSystem
    {
        public readonly Worker Worker;

        public WorkerSystem(Worker worker)
        {
            Worker = worker;
        }

        protected override void OnUpdate()
        {
        }
    }
}

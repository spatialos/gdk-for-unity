
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    internal class WorkerSystem : ComponentSystem
    {
        public Worker Worker { get; internal set; }

        protected override void OnUpdate()
        {
        }
    }
}

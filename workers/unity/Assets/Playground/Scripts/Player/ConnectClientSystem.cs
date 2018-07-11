using Improbable.Gdk.Core;
using Unity.Entities;

namespace Playground
{
    [UpdateBefore(typeof(SpatialOSUpdateGroup))]
    internal class ConnectClientSystem : ComponentSystem
    {
        public struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
            public ComponentDataArray<ConnectButtonClicked> DenotesClickedConnectButton;
        }

        [Inject] private Data data;

        private WorkerBase worker;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            worker = WorkerRegistry.GetWorkerForWorld(World);
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var entity = data.Entity[i];
                worker.Connect(worker.ConnectionConfig);
                PostUpdateCommands.RemoveComponent<ConnectButtonClicked>(entity);
            }
        }
    }
}

using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class DisconnectSystem : ComponentSystem
    {
        public struct DisconnectData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public SharedComponentDataArray<OnDisconnected> DisconnectMessage;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
        }

        [Inject] private DisconnectData data;

        private Worker worker;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            worker = Worker.TryGetWorker(World);
        }

        protected override void OnUpdate()
        {
            worker.LogDispatcher.HandleLog(
                LogType.Warning,
                new LogEvent($"Disconnected from SpatialOS with reason: \"{data.DisconnectMessage[0].ReasonForDisconnect}\""
                ));
            Worker.Disconnect(worker);
        }
    }
}

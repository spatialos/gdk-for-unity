using System;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    public class WorkerDisconnectCallbackSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public SharedComponentDataArray<OnDisconnected> DisconnectedMessage;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
        }

        public event Action<string> OnDisconnected;

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            OnDisconnected?.Invoke(data.DisconnectedMessage[0].ReasonForDisconnect);
        }
    }
}

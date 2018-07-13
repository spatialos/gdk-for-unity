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
            [ReadOnly] public SharedComponentDataArray<OnDisconnected> DisconnectMessage;

            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
        }

        [Inject] private DisconnectData data;

        protected override void OnUpdate()
        {
            Debug.LogWarningFormat("Diconnected from SpatialOS with reason: \"{0}\"",
                data.DisconnectMessage[0].ReasonForDisconnect);
        }
    }
}

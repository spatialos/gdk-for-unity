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
#pragma warning disable 649
            public readonly int Length;
            [ReadOnly] public SharedComponentDataArray<OnDisconnected> DisconnectMessage;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
#pragma warning restore 649
        }

#pragma warning disable 649
        [Inject] private DisconnectData data;
#pragma warning restore 649

        protected override void OnUpdate()
        {
            Debug.LogWarningFormat("Diconnected from SpatialOS with reason: \"{0}\"",
                data.DisconnectMessage[0].ReasonForDisconnect);
        }
    }
}

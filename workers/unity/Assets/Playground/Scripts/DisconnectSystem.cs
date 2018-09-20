using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

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
            Debug.LogWarningFormat("Disconnected from SpatialOS with reason: \"{0}\"",
                data.DisconnectMessage[0].ReasonForDisconnect);
        }
    }
}

using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Enables users to add a callback onto the disconnection event.
    /// </summary>
    [DisableAutoCreation]
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
        [Inject] private WorkerSystem worker;

        protected override void OnUpdate()
        {
            if (data.Length != 1)
            {
                worker.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent($"{typeof(WorkerEntityTag)} should only be present on a single entity"));
            }

            OnDisconnected?.Invoke(data.DisconnectedMessage[0].ReasonForDisconnect);
        }
    }
}

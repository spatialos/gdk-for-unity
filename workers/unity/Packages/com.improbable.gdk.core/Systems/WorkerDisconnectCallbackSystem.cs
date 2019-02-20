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
#pragma warning disable 649
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public SharedComponentDataArray<OnDisconnected> DisconnectedMessage;
            [ReadOnly] public ComponentDataArray<WorkerEntityTag> DenotesWorkerEntity;
        }
#pragma warning restore 649

        public event Action<string> OnDisconnected;

        private WorkerSystem worker;

#pragma warning disable 649
        [Inject] private Data data;
#pragma warning restore 649

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();
        }

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

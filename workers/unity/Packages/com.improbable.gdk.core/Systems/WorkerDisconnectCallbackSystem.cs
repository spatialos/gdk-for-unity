using System;
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
        public event Action<string> OnDisconnected;

        private WorkerSystem worker;
        private ComponentGroup group;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            group = GetComponentGroup(
                ComponentType.ReadOnly<OnDisconnected>(),
                ComponentType.ReadOnly<WorkerEntityTag>()
            );

            worker = World.GetExistingManager<WorkerSystem>();
        }

        protected override void OnUpdate()
        {
            if (group.CalculateLength() != 1)
            {
                worker.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent($"{typeof(WorkerEntityTag)} should only be present on a single entity"));
            }

            Entities.With(group).ForEach((OnDisconnected data) =>
            {
                OnDisconnected?.Invoke(data.ReasonForDisconnect);
            });
        }
    }
}

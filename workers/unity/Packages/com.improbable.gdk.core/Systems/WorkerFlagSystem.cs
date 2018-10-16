using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Updates worker flags on the worker entity and provides a callback on worker flag changes.
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    public class WorkerFlagSystem : ComponentSystem
    {
        /// <summary>
        ///     An event which is triggered when a worker flag changes
        /// </summary>
        /// <remarks>
        ///     Note that this is not fired when the Dispatcher callback is fired, but after the local view is updated
        ///     completely.
        /// </remarks>
        public event Action<string, string> OnWorkerFlagChange;

        // Okay to keep a copy of the struct as the reference to the dictionary inside will still be valid.
        private WorkerFlags flags;

        private readonly List<(string, string)> queuedFlagChanges = new List<(string, string)>();

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            var receiveSystem = World.GetOrCreateManager<SpatialOSReceiveSystem>();
            receiveSystem.Dispatcher.OnFlagUpdate(OnFlagUpdate);

            var workerEntity = World.GetExistingManager<WorkerSystem>().WorkerEntity;
            flags = EntityManager.GetSharedComponentData<WorkerFlags>(workerEntity);
        }

        protected override void OnUpdate()
        {
            if (OnWorkerFlagChange == null)
            {
                return;
            }

            foreach (var (name, value) in queuedFlagChanges)
            {
                OnWorkerFlagChange.Invoke(name, value);
            }

            queuedFlagChanges.Clear();
        }

        private void OnFlagUpdate(FlagUpdateOp op)
        {
            flags.Flags[op.Name] = op.Value;
            queuedFlagChanges.Add((op.Name, op.Value));
        }
    }

}



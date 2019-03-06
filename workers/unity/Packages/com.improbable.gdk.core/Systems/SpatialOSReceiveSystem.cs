using System;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Receives incoming messages from the SpatialOS runtime.
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class SpatialOSReceiveSystem : ComponentSystem
    {
        private WorkerSystem worker;
        private EcsViewSystem ecsViewSystem;
        private EntitySystem entitySystem;

        private View view;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();
            ecsViewSystem = World.GetOrCreateManager<EcsViewSystem>();
            entitySystem = World.GetOrCreateManager<EntitySystem>();

            view = worker.View;
        }

        protected override void OnUpdate()
        {
            try
            {
                worker.GetMessages();

                var diff = worker.Diff;

                ecsViewSystem.ApplyDiff(diff);
                entitySystem.ApplyDiff(diff);
                view.ApplyDiff(diff);
            }
            catch (Exception e)
            {
                worker.LogDispatcher.HandleLog(LogType.Exception, new LogEvent("Exception:")
                    .WithException(e));
            }
        }
    }
}

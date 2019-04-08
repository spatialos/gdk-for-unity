using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;

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
                Profiler.BeginSample("GetMessages");
                worker.GetMessages();
                Profiler.EndSample();

                var diff = worker.Diff;

                Profiler.BeginSample("ApplyDiff ECS");
                ecsViewSystem.ApplyDiff(diff);
                Profiler.EndSample();
                Profiler.BeginSample("ApplyDiff Entity");
                entitySystem.ApplyDiff(diff);
                Profiler.EndSample();
                Profiler.BeginSample("ApplyDiff View");
                view.ApplyDiff(diff);
                Profiler.EndSample();
            }
            catch (Exception e)
            {
                worker.LogDispatcher.HandleLog(LogType.Exception, new LogEvent("Exception:")
                    .WithException(e));
            }
        }
    }
}

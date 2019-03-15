using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Profiling;

namespace Improbable.Gdk.ReactiveComponents
{
    /// <summary>
    ///     Removes GDK reactive components from all entities
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSCleanGroup))]
    public class CleanReactiveComponentsSystem : ComponentSystem
    {
        private readonly List<ComponentCleanup> componentCleanups = new List<ComponentCleanup>();
        private NativeArray<ArchetypeChunk>[] chunkArrayCache;
        private NativeArray<JobHandle> gatheringJobs;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            GenerateComponentGroups();
            chunkArrayCache = new NativeArray<ArchetypeChunk>[componentCleanups.Count];
            gatheringJobs = new NativeArray<JobHandle>(componentCleanups.Count, Allocator.Persistent);
        }

        protected override void OnDestroyManager()
        {
            base.OnDestroyManager();
            gatheringJobs.Dispose();
        }

        private void GenerateComponentGroups()
        {
            // Find all ComponentCleanupHandlers
            foreach (var type in ReflectionUtility.GetNonAbstractTypes(typeof(ComponentCleanupHandler)))
            {
                var componentCleanupHandler = (ComponentCleanupHandler) Activator.CreateInstance(type);

                componentCleanups.Add(new ComponentCleanup
                {
                    Handler = componentCleanupHandler,
                    Group = GetComponentGroup(componentCleanupHandler.CleanupArchetypeQuery)
                });
            }
        }

        protected override void OnUpdate()
        {
            Profiler.BeginSample("GatherChunks");
            for (var i = 0; i < componentCleanups.Count; i++)
            {
                var replicator = componentCleanups[i];
                chunkArrayCache[i] = replicator.Group.CreateArchetypeChunkArray(Allocator.TempJob, out var jobHandle);
                gatheringJobs[i] = jobHandle;
            }

            Profiler.EndSample();

            JobHandle.CompleteAll(gatheringJobs);

            for (var i = 0; i < componentCleanups.Count; i++)
            {
                var cleanup = componentCleanups[i];
                var chunkArray = chunkArrayCache[i];

                Profiler.BeginSample("CleanReactiveComponents");
                cleanup.Handler.CleanComponents(chunkArray, this, PostUpdateCommands);
                Profiler.EndSample();

                chunkArray.Dispose();
            }
        }

        private struct ComponentCleanup
        {
            public ComponentCleanupHandler Handler;
            public ComponentGroup Group;
        }
    }
}

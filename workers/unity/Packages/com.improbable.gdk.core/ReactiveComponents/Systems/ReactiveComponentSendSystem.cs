using System;
using System.Collections.Generic;
using System.Reflection;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Profiling;

namespace Improbable.Gdk.ReactiveComponents
{
    /// <summary>
    ///     Executes the default replication logic for each SpatialOS component.
    /// </summary>
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    [UpdateBefore(typeof(ComponentUpdateSystem))]
    public class ReactiveComponentSendSystem : ComponentSystem
    {
        private readonly List<ComponentReplicator> componentReplicators = new List<ComponentReplicator>();
        private NativeArray<ArchetypeChunk>[] chunkArrayCache;

        private IConnectionHandler connection;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            connection = World.GetExistingManager<WorkerSystem>().ConnectionHandler;

            PopulateDefaultComponentReplicators();
            chunkArrayCache = new NativeArray<ArchetypeChunk>[componentReplicators.Count];
        }

        protected override void OnUpdate()
        {
            if (!connection.IsConnected())
            {
                return;
            }

            ReplicateEvents();
            ReplicateCommands();
        }

        private void ReplicateEvents()
        {
            var componentUpdateSystem = World.GetExistingManager<ComponentUpdateSystem>();

            Profiler.BeginSample("GatherChunks_Events");
            var gatheringJobs = new NativeArray<JobHandle>(componentReplicators.Count, Allocator.Temp);
            for (var i = 0; i < componentReplicators.Count; i++)
            {
                var replicator = componentReplicators[i];
                chunkArrayCache[i] = replicator.EventGroup.CreateArchetypeChunkArray(Allocator.TempJob, out var jobHandle);
                gatheringJobs[i] = jobHandle;
            }

            JobHandle.CompleteAll(gatheringJobs);
            gatheringJobs.Dispose();
            Profiler.EndSample();

            for (var i = 0; i < componentReplicators.Count; i++)
            {
                Profiler.BeginSample("SendEvents");
                componentReplicators[i].Handler.SendEvents(chunkArrayCache[i], this, componentUpdateSystem);
                chunkArrayCache[i].Dispose();
                Profiler.EndSample();
            }
        }

        private void ReplicateCommands()
        {
            var commandSystem = World.GetExistingManager<CommandSystem>();

            Profiler.BeginSample("GatherChunks_Commands");
            var gatheringJobs = new NativeArray<JobHandle>(componentReplicators.Count, Allocator.Temp);
            for (var i = 0; i < componentReplicators.Count; i++)
            {
                var replicator = componentReplicators[i];
                chunkArrayCache[i] = replicator.EventGroup.CreateArchetypeChunkArray(Allocator.TempJob, out var jobHandle);
                gatheringJobs[i] = jobHandle;
            }

            JobHandle.CompleteAll(gatheringJobs);
            gatheringJobs.Dispose();
            Profiler.EndSample();

            for (var i = 0; i < componentReplicators.Count; i++)
            {
                Profiler.BeginSample("SendCommands");
                componentReplicators[i].Handler.SendCommands(chunkArrayCache[i], this, commandSystem);
                chunkArrayCache[i].Dispose();
                Profiler.EndSample();
            }
        }

        internal void AddComponentReplicator(IReactiveComponentReplicationHandler reactiveComponentReplicationHandler)
        {
            componentReplicators.Add(new ComponentReplicator
            {
                Handler = reactiveComponentReplicationHandler,
                EventGroup = GetComponentGroup(reactiveComponentReplicationHandler.EventQuery),
                CommandGroup = GetComponentGroup(reactiveComponentReplicationHandler.CommandQueries),
            });
        }

        private void PopulateDefaultComponentReplicators()
        {
            // Find all component specific replicators and create an instance.
            var types = ReflectionUtility.GetNonAbstractTypes(typeof(IReactiveComponentReplicationHandler));

            foreach (var type in types)
            {
                if (type.GetCustomAttribute(typeof(DisableAutoRegisterAttribute)) != null)
                {
                    continue;
                }

                var componentReplicationHandler = (IReactiveComponentReplicationHandler) Activator.CreateInstance(type);
                AddComponentReplicator(componentReplicationHandler);
            }
        }

        private struct ComponentReplicator
        {
            public IReactiveComponentReplicationHandler Handler;
            public ComponentGroup EventGroup;
            public ComponentGroup CommandGroup;
        }
    }
}

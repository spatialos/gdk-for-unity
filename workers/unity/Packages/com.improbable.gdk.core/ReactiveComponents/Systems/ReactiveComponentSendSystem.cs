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
    public class ReactiveComponentSendSystem : ComponentSystem
    {
        private readonly List<ComponentReplicator> componentReplicators = new List<ComponentReplicator>();
        private NativeArray<ArchetypeChunk>[] chunkArrayCache;
        private NativeArray<JobHandle> gatheringJobs;

        private IConnectionHandler connection;

        protected override void OnCreate()
        {
            base.OnCreate();

            connection = World.GetExistingSystem<WorkerSystem>().ConnectionHandler;

            PopulateDefaultComponentReplicators();
            chunkArrayCache = new NativeArray<ArchetypeChunk>[componentReplicators.Count * 2];
            gatheringJobs = new NativeArray<JobHandle>(componentReplicators.Count * 2, Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            gatheringJobs.Dispose();
            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
            if (!connection.IsConnected())
            {
                return;
            }

            Profiler.BeginSample("GatherChunks");
            for (var i = 0; i < componentReplicators.Count; i++)
            {
                var replicator = componentReplicators[i];
                var eventIndex = i;
                var commandIndex = componentReplicators.Count + i;

                var eventJobHandle = new JobHandle();
                if (replicator.EventGroup != null)
                {
                    chunkArrayCache[eventIndex] =
                        replicator.EventGroup.CreateArchetypeChunkArray(Allocator.TempJob, out eventJobHandle);
                }
                else
                {
                    chunkArrayCache[eventIndex] = new NativeArray<ArchetypeChunk>();
                }

                var commandJobHandle = new JobHandle();
                if (replicator.CommandGroup != null)
                {
                    chunkArrayCache[commandIndex] =
                        replicator.CommandGroup.CreateArchetypeChunkArray(Allocator.TempJob, out commandJobHandle);
                }
                else
                {
                    chunkArrayCache[commandIndex] = new NativeArray<ArchetypeChunk>();
                }

                gatheringJobs[eventIndex] = eventJobHandle;
                gatheringJobs[commandIndex] = commandJobHandle;
            }

            Profiler.EndSample();

            JobHandle.CompleteAll(gatheringJobs);

            ReplicateEvents();
            ReplicateCommands();
        }

        private void ReplicateEvents()
        {
            var componentUpdateSystem = World.GetExistingSystem<ComponentUpdateSystem>();

            for (var i = 0; i < componentReplicators.Count; i++)
            {
                var eventIndex = i;
                if (chunkArrayCache[eventIndex].IsCreated)
                {
                    Profiler.BeginSample("SendEvents");
                    componentReplicators[i].Handler
                        .SendEvents(chunkArrayCache[eventIndex], this, componentUpdateSystem);
                    chunkArrayCache[eventIndex].Dispose();
                    Profiler.EndSample();
                }
            }
        }

        private void ReplicateCommands()
        {
            var commandSystem = World.GetExistingSystem<CommandSystem>();

            for (var i = 0; i < componentReplicators.Count; i++)
            {
                var commandIndex = componentReplicators.Count + i;
                if (chunkArrayCache[commandIndex].IsCreated)
                {
                    Profiler.BeginSample("SendCommands");
                    componentReplicators[i].Handler.SendCommands(chunkArrayCache[commandIndex], this, commandSystem);
                    chunkArrayCache[commandIndex].Dispose();
                    Profiler.EndSample();
                }
            }
        }

        internal void AddComponentReplicator(IReactiveComponentReplicationHandler reactiveComponentReplicationHandler)
        {
            var replicator = new ComponentReplicator
            {
                Handler = reactiveComponentReplicationHandler,
            };

            if (reactiveComponentReplicationHandler.EventQuery != null)
            {
                replicator.EventGroup = GetEntityQuery(reactiveComponentReplicationHandler.EventQuery);
            }

            if (reactiveComponentReplicationHandler.CommandQueries != null)
            {
                replicator.CommandGroup = GetEntityQuery(reactiveComponentReplicationHandler.CommandQueries);
            }

            componentReplicators.Add(replicator);
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
            public EntityQuery EventGroup;
            public EntityQuery CommandGroup;
        }
    }
}

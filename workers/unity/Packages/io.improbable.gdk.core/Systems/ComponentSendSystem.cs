using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Core.CodegenAdapters;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.Profiling;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Executes the default replication logic for each SpatialOS component.
    /// </summary>
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    [UpdateBefore(typeof(SpatialOSSendSystem))]
    public class ComponentSendSystem : ComponentSystem
    {
        private readonly List<ComponentReplicator> componentReplicators = new List<ComponentReplicator>();
        private NativeArray<ArchetypeChunk>[] chunkArrayCache;
        private NativeArray<JobHandle> gatheringJobs;

        protected override void OnCreate()
        {
            base.OnCreate();

            PopulateDefaultComponentReplicators();
            chunkArrayCache = new NativeArray<ArchetypeChunk>[componentReplicators.Count];
            gatheringJobs = new NativeArray<JobHandle>(componentReplicators.Count, Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            gatheringJobs.Dispose();
            base.OnDestroy();
        }

        public bool TryRegisterCustomReplicationSystem(uint componentId)
        {
            if (componentReplicators.All(componentReplicator => componentReplicator.ComponentId != componentId))
            {
                return false;
            }

            // The default replication system is removed, instead the custom one is responsible for replication.
            return componentReplicators.Remove(componentReplicators.First(
                componentReplicator => componentReplicator.ComponentId == componentId));
        }

        protected override void OnUpdate()
        {
            var componentUpdateSystem = World.GetExistingSystem<ComponentUpdateSystem>();

            Profiler.BeginSample("GatherChunks");
            for (var i = 0; i < componentReplicators.Count; i++)
            {
                var replicator = componentReplicators[i];
                chunkArrayCache[i] = replicator.Group.CreateArchetypeChunkArray(Allocator.TempJob, out var jobHandle);
                gatheringJobs[i] = jobHandle;
            }

            Profiler.EndSample();

            for (var i = 0; i < componentReplicators.Count; i++)
            {
                Profiler.BeginSample("ExecuteReplication");

                gatheringJobs[i].Complete();
                var replicator = componentReplicators[i];
                var chunkArray = chunkArrayCache[i];

                replicator.Handler.SendUpdates(chunkArray, this, EntityManager, componentUpdateSystem);
                chunkArray.Dispose();

                Profiler.EndSample();
            }
        }

        internal void AddComponentReplicator(IComponentReplicationHandler componentReplicationHandler)
        {
            componentReplicators.Add(new ComponentReplicator
            {
                ComponentId = componentReplicationHandler.ComponentId,
                Handler = componentReplicationHandler,
                Group = GetEntityQuery(componentReplicationHandler.ComponentUpdateQuery)
            });
        }

        private void PopulateDefaultComponentReplicators()
        {
            // Find all component specific replicators and create an instance.
            var types = ComponentDatabase.Metaclasses.Select(type => type.Value.ReplicationHandler);
            foreach (var type in types)
            {
                if (type.GetCustomAttribute<DisableAutoRegisterAttribute>() != null)
                {
                    continue;
                }

                var handler = (IComponentReplicationHandler) Activator.CreateInstance(type);

                AddComponentReplicator(handler);
            }
        }

        private struct ComponentReplicator
        {
            public uint ComponentId;
            public IComponentReplicationHandler Handler;
            public EntityQuery Group;
        }
    }
}

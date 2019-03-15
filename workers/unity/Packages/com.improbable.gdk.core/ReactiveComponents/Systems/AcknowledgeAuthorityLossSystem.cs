using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Profiling;

namespace Improbable.Gdk.ReactiveComponents
{
    /// <summary>
    ///    Checks for entities that have acknowledged authority loss during soft-handover and forwards this to SpatialOS.
    /// </summary>
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class AcknowledgeAuthorityLossSystem : ComponentSystem
    {
        private readonly List<ComponentAuthorityLossDetails> authorityLossDetails =
            new List<ComponentAuthorityLossDetails>();

        private NativeArray<ArchetypeChunk>[] chunkArrayCache;
        private NativeArray<JobHandle> gatheringJobs;

        private ComponentUpdateSystem updateSystem;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            updateSystem = World.GetExistingManager<ComponentUpdateSystem>();
            GenerateComponentGroups();
            chunkArrayCache = new NativeArray<ArchetypeChunk>[authorityLossDetails.Count];
            gatheringJobs = new NativeArray<JobHandle>(authorityLossDetails.Count, Allocator.Persistent);
        }

        protected override void OnDestroyManager()
        {
            base.OnDestroyManager();
            gatheringJobs.Dispose();
        }

        private void GenerateComponentGroups()
        {
            foreach (var type in ReflectionUtility.GetNonAbstractTypes(typeof(AbstractAcknowledgeAuthorityLossHandler)))
            {
                var authorityLossHandler = (AbstractAcknowledgeAuthorityLossHandler) Activator.CreateInstance(type);

                authorityLossDetails.Add(new ComponentAuthorityLossDetails
                {
                    Handler = authorityLossHandler,
                    Group = GetComponentGroup(authorityLossHandler.Query)
                });
            }
        }

        protected override void OnUpdate()
        {
            Profiler.BeginSample("GatherChunks");
            for (var i = 0; i < authorityLossDetails.Count; i++)
            {
                var replicator = authorityLossDetails[i];
                chunkArrayCache[i] = replicator.Group.CreateArchetypeChunkArray(Allocator.TempJob, out var jobHandle);
                gatheringJobs[i] = jobHandle;
            }

            Profiler.EndSample();

            JobHandle.CompleteAll(gatheringJobs);

            for (var i = 0; i < authorityLossDetails.Count; i++)
            {
                var details = authorityLossDetails[i];
                var chunkArray = chunkArrayCache[i];


                Profiler.BeginSample("AcknowledgingAuthorityLoss");
                details.Handler.AcknowledgeAuthorityLoss(chunkArray, this, updateSystem);
                Profiler.EndSample();

                chunkArray.Dispose();
            }
        }

        private struct ComponentAuthorityLossDetails
        {
            public AbstractAcknowledgeAuthorityLossHandler Handler;
            public ComponentGroup Group;
        }
    }
}

using System.Collections.Generic;
using Improbable.Gdk.Core.NetworkStats;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Profiling;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class SpatialOSSendSystem : ComponentSystem
    {
        private WorkerSystem worker;
        private NetworkStatisticsSystem networkStatisticsSystem;
        private NetFrameStats netFrameStats = new NetFrameStats();

        private NativeList<JobHandle> replicationHandles;
        private List<NativeQueue<SerializedMessagesToSend.UpdateToSend>> componentQueues;

        private ProfilerMarker updateQueueMarker = new ProfilerMarker("QueueSerializedMessages");

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();
            networkStatisticsSystem = World.GetOrCreateSystem<NetworkStatisticsSystem>();
            replicationHandles = new NativeList<JobHandle>(Allocator.Persistent);
            componentQueues = new List<NativeQueue<SerializedMessagesToSend.UpdateToSend>>();
        }

        protected override void OnUpdate()
        {
            JobHandle.CompleteAll(replicationHandles);
            replicationHandles.Clear();

            using (updateQueueMarker.Auto())
            {
                foreach (var componentQueue in componentQueues)
                {
                    while (componentQueue.TryDequeue(out var updateToSend))
                    {
                        worker.MessagesToSend.AddSerializedComponentUpdate(updateToSend);
                    }
                }
            }

            worker.SendMessages(netFrameStats);

            networkStatisticsSystem.AddOutgoingSample(netFrameStats);
            netFrameStats.Clear();

            componentQueues.Clear();
        }

        public void AddReplicationJobProducer(JobHandle job, NativeQueue<SerializedMessagesToSend.UpdateToSend> componentQueue)
        {
            replicationHandles.Add(job);
            componentQueues.Add(componentQueue);
        }
    }
}

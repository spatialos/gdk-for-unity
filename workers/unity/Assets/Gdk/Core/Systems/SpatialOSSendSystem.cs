using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class SpatialOSSendSystem : ComponentSystem
    {
        internal static class Warnings
        {
            public const string CustomReplicationSystemAlreadyExists =
                "Custom Replication System for type {0} already exists!";
        }

        private WorkerBase worker;
        private MutableView view;

        private readonly List<int> registeredReplicators = new List<int>();
        private readonly List<int> commandSenders = new List<int>();

        private int ticks = 0;
        private const int METRIC_TICKS_UPDATE = 200;

        public delegate Worker.Metrics MetricsComputation();
        private MetricsComputation metricComputedEverySend;

        protected override void OnCreateManager(int capacity)
        {
            OnCreateManager(capacity, delegate()
            {
                float fps = 1.0f / Time.deltaTime;
                return new Worker.Metrics
                {
                    // Load is 0 is fps is at most 20, 1 if it is at least 30, and linear in between.
                    Load = Mathf.Min(1.0f, Mathf.Max(0.0f, 3.0f - 0.1f * fps))
                };
            });
        }

        protected void OnCreateManager(int capacity, MetricsComputation metricsComputation)
        {
            base.OnCreateManager(capacity);

            worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;

            metricComputedEverySend = metricsComputation;

            GenerateComponentGroups();
        }

        private void GenerateComponentGroups()
        {
            foreach (var componentTranslatorPair in view.TranslationUnits)
            {
                var componentIndex = componentTranslatorPair.Key;
                var componentTranslator = componentTranslatorPair.Value;

                var replicationComponentGroup = GetComponentGroup(componentTranslator.ReplicationComponentTypes);
                componentTranslator.ReplicationComponentGroup = replicationComponentGroup;

                registeredReplicators.Add(componentIndex);
                commandSenders.Add(componentIndex);
            }
        }

        public void RegisterCustomReplicationSystem(ComponentType type)
        {
            if (!registeredReplicators.Contains(type.TypeIndex))
            {
                Debug.LogWarningFormat(Warnings.CustomReplicationSystemAlreadyExists, type);
                return;
            }

            registeredReplicators.Remove(type.TypeIndex);
        }

        protected override void OnUpdate()
        {
            if (worker.Connection == null)
            {
                return;
            }

            var connection = worker.Connection;

            foreach (var componentTypeIndex in registeredReplicators)
            {
                view.TranslationUnits[componentTypeIndex].ExecuteReplication(connection);
            }

            foreach (var componentTypeIndex in commandSenders)
            {
                view.TranslationUnits[componentTypeIndex].SendCommands(connection);
            }

            ticks++;
            if (ticks == METRIC_TICKS_UPDATE)
            {
                ticks = 0;
                connection.SendMetrics(metricComputedEverySend());
            }
        }
    }
}

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

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;

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
        }
    }
}

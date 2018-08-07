using System;
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class SpatialOSSendSystem : SpatialOSSystem
    {
        private readonly List<int> registeredReplicators = new List<int>();
        private readonly List<int> commandSenders = new List<int>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            GenerateComponentGroups();
        }

        private void GenerateComponentGroups()
        {
            foreach (var componentTranslatorPair in Worker.TranslationUnits)
            {
                var componentIndex = componentTranslatorPair.Key;
                var componentTranslator = componentTranslatorPair.Value;

                var replicationComponentGroup = GetComponentGroup(componentTranslator.ReplicationComponentTypes);
                componentTranslator.ReplicationComponentGroup = replicationComponentGroup;

                registeredReplicators.Add(componentIndex);
                commandSenders.Add(componentIndex);
            }
        }

        public bool TryRegisterCustomReplicationSystem(ComponentType type)
        {
            if (!registeredReplicators.Contains(type.TypeIndex))
            {
                return false;
            }

            // The default replication system is removed, instead the custom one is responsible for replication.
            return registeredReplicators.Remove(type.TypeIndex);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var componentTypeIndex in registeredReplicators)
            {
                Worker.TranslationUnits[componentTypeIndex].ExecuteReplication(Connection);
            }

            foreach (var componentTypeIndex in commandSenders)
            {
                Worker.TranslationUnits[componentTypeIndex].SendCommands(Connection);
            }
        }
    }
}

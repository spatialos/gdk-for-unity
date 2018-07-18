using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Worker.Core;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class SpatialOSSendSystem : ComponentSystem
    {
        private WorkerBase worker;

        private readonly List<SpecificComponentReplicator> specificComponentReplicators =
            new List<SpecificComponentReplicator>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = WorkerRegistry.GetWorkerForWorld(World);

            PopulateDefaultComponentReplicators();
        }

        private void PopulateDefaultComponentReplicators()
        {
            // Find all component specific replicators and create an instance.
            var componentReplicationTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentReplicationHandler).IsAssignableFrom(type) && !type.IsAbstract);

            foreach (var componentReplicationType in componentReplicationTypes)
            {
                var componentReplicationHandler =
                    Activator.CreateInstance(componentReplicationType, new { EntityManager }) as
                        ComponentReplicationHandler;

                specificComponentReplicators.Add(new SpecificComponentReplicator
                {
                    ComponentId = componentReplicationHandler.ComponentId,
                    Handler = componentReplicationHandler,
                    ReplicationComponentGroup =
                        GetComponentGroup(componentReplicationHandler.ReplicationComponentTypes),
                    CommandReplicationGroups = componentReplicationHandler.CommandTypes
                        .Select(componentType => GetComponentGroup(componentType)).ToList()
                });
            }
        }

        public bool TryRegisterCustomReplicationSystem(uint componentId)
        {
            if (specificComponentReplicators.All(componentReplicator => componentReplicator.ComponentId != componentId))
            {
                return false;
            }

            // The default replication system is removed, instead the custom one is responsible for replication.
            return specificComponentReplicators.Remove(specificComponentReplicators.First(
                componentReplicator => componentReplicator.ComponentId == componentId));
        }

        protected override void OnUpdate()
        {
            if (worker.Connection == null)
            {
                return;
            }

            foreach (var replicator in specificComponentReplicators)
            {
                replicator.Execute(worker.Connection);
            }
        }

        private struct SpecificComponentReplicator
        {
            public uint ComponentId;
            public ComponentReplicationHandler Handler;
            public ComponentGroup ReplicationComponentGroup;
            public List<ComponentGroup> CommandReplicationGroups;

            public void Execute(Connection connection)
            {
                Handler.ExecuteReplication(ReplicationComponentGroup, connection);
                Handler.SendCommands(CommandReplicationGroups, connection);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Worker.Core;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class SpatialOSSendSystem : ComponentSystem
    {
        private Connection connection;

        private readonly List<ComponentReplicator> componentReplicators =
            new List<ComponentReplicator>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            connection = World.GetExistingManager<WorkerSystem>().Connection;

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
                    (ComponentReplicationHandler) Activator.CreateInstance(componentReplicationType,
                        new object[] { EntityManager, World });

                componentReplicators.Add(new ComponentReplicator
                {
                    ComponentId = componentReplicationHandler.ComponentId,
                    Handler = componentReplicationHandler,
                    ReplicationComponentGroup =
                        GetComponentGroup(componentReplicationHandler.ReplicationComponentTypes),
                });
            }
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
            if (connection == null)
            {
                return;
            }

            foreach (var replicator in componentReplicators)
            {
                replicator.Execute(this, connection);
            }
        }

        private struct ComponentReplicator
        {
            public uint ComponentId;
            public ComponentReplicationHandler Handler;
            public ComponentGroup ReplicationComponentGroup;
            public List<ComponentGroup> CommandReplicationGroups;

            public void Execute(SpatialOSSendSystem sendSystem, Connection connection)
            {
                Handler.ExecuteReplication(ReplicationComponentGroup, connection);
                Handler.SendCommands(sendSystem, connection);
            }
        }
    }
}

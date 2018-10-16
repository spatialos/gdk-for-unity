using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine.Profiling;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Executes the default replication logic for each SpatialOS component.
    /// </summary>
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class SpatialOSSendSystem : ComponentSystem
    {
        // Can't access the generated component ID in Core code.
        private const uint PositionComponentId = 54;

        private Connection connection;

        private readonly List<ComponentReplicator> componentReplicators =
            new List<ComponentReplicator>();

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            connection = World.GetExistingManager<WorkerSystem>().Connection;

            PopulateDefaultComponentReplicators();
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
                Profiler.BeginSample("ExecuteReplication");
                replicator.Handler.ExecuteReplication(replicator.UpdateGroup, this, connection);
                Profiler.EndSample();

                Profiler.BeginSample("SendCommands");
                replicator.Handler.SendCommands(replicator.CommandGroup, this, connection);
                Profiler.EndSample();
            }
        }

        internal void AddComponentReplicator(ComponentReplicationHandler componentReplicationHandler)
        {
            componentReplicators.Add(new ComponentReplicator
            {
                ComponentId = componentReplicationHandler.ComponentId,
                Handler = componentReplicationHandler,
                UpdateGroup = GetComponentGroup(componentReplicationHandler.ComponentUpdateQuery),
                CommandGroup = GetComponentGroup(componentReplicationHandler.CommandQueries),
            });
        }

        private void PopulateDefaultComponentReplicators()
        {
            // Find all component specific replicators and create an instance.
            var componentReplicationTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentReplicationHandler).IsAssignableFrom(type) && !type.IsAbstract
                    && type.GetCustomAttribute(typeof(DisableAutoRegisterAttribute)) == null);

            foreach (var componentReplicationType in componentReplicationTypes)
            {
                var componentReplicationHandler =
                    (ComponentReplicationHandler) Activator.CreateInstance(componentReplicationType,
                        EntityManager, World);

                AddComponentReplicator(componentReplicationHandler);
            }

            // Force the position component to be replicated last. A position update can trigger an authority
            // change, which could cause subsequent updates to be dropped.
            var positionReplicatorIndex =
                componentReplicators.FindIndex(replicator => replicator.ComponentId == PositionComponentId);
            var positionReplicator = componentReplicators[positionReplicatorIndex];

            componentReplicators.RemoveAt(positionReplicatorIndex);
            componentReplicators.Add(positionReplicator);
        }

        private struct ComponentReplicator
        {
            public uint ComponentId;
            public ComponentReplicationHandler Handler;
            public ComponentGroup UpdateGroup;
            public ComponentGroup CommandGroup;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Worker.CInterop;
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
    [UpdateBefore(typeof(ComponentUpdateSystem))]
    public class ReactiveComponentSendSystem : ComponentSystem
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
            var commandSystem = World.GetExistingManager<CommandSystem>();
            var componentUpdateSystem = World.GetExistingManager<ComponentUpdateSystem>();

            if (connection == null)
            {
                return;
            }

            foreach (var replicator in componentReplicators)
            {
                Profiler.BeginSample("SendEvents");
                replicator.Handler.SendEvents(replicator.EventGroup, this, componentUpdateSystem);
                Profiler.EndSample();

                Profiler.BeginSample("SendCommands");
                replicator.Handler.SendCommands(replicator.CommandGroup, this, commandSystem);
                Profiler.EndSample();
            }
        }

        internal void AddComponentReplicator(IReactiveComponentReplicationHandler reactiveComponentReplicationHandler)
        {
            componentReplicators.Add(new ComponentReplicator
            {
                ComponentId = reactiveComponentReplicationHandler.ComponentId,
                Handler = reactiveComponentReplicationHandler,
                EventGroup = GetComponentGroup(reactiveComponentReplicationHandler.EventQuery),
                CommandGroup = GetComponentGroup(reactiveComponentReplicationHandler.CommandQueries),
            });
        }

        private void PopulateDefaultComponentReplicators()
        {
            // Find all component specific replicators and create an instance.
            var componentReplicationTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IReactiveComponentReplicationHandler).IsAssignableFrom(type) && !type.IsAbstract
                    && type.GetCustomAttribute(typeof(DisableAutoRegisterAttribute)) == null);

            foreach (var componentReplicationType in componentReplicationTypes)
            {
                var componentReplicationHandler =
                    (IReactiveComponentReplicationHandler) Activator.CreateInstance(componentReplicationType);

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
            public IReactiveComponentReplicationHandler Handler;
            public ComponentGroup EventGroup;
            public ComponentGroup CommandGroup;
        }
    }
}

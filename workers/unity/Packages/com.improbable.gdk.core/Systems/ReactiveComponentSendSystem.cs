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
        private readonly List<ComponentReplicator> componentReplicators = new List<ComponentReplicator>();

        private Connection connection;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            connection = World.GetExistingManager<WorkerSystem>().Connection;

            PopulateDefaultComponentReplicators();
        }

        protected override void OnUpdate()
        {
            if (connection == null)
            {
                return;
            }

            var commandSystem = World.GetExistingManager<CommandSystem>();
            var componentUpdateSystem = World.GetExistingManager<ComponentUpdateSystem>();

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
                .Where(type =>
                {
                    return typeof(IReactiveComponentReplicationHandler).IsAssignableFrom(type)
                        && !type.IsAbstract
                        && type.GetCustomAttribute(typeof(DisableAutoRegisterAttribute)) == null;
                });

            foreach (var componentReplicationType in componentReplicationTypes)
            {
                var componentReplicationHandler =
                    (IReactiveComponentReplicationHandler) Activator.CreateInstance(componentReplicationType);

                AddComponentReplicator(componentReplicationHandler);
            }
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

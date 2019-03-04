using System;
using System.Collections.Generic;
using System.Reflection;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine.Profiling;

namespace Improbable.Gdk.ReactiveComponents
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

        private IConnectionHandler connection;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            connection = World.GetExistingManager<WorkerSystem>().ConnectionHandler;

            PopulateDefaultComponentReplicators();
        }

        protected override void OnUpdate()
        {
            if (!connection.IsConnected())
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
                Handler = reactiveComponentReplicationHandler,
                EventGroup = GetComponentGroup(reactiveComponentReplicationHandler.EventQuery),
                CommandGroup = GetComponentGroup(reactiveComponentReplicationHandler.CommandQueries),
            });
        }

        private void PopulateDefaultComponentReplicators()
        {
            // Find all component specific replicators and create an instance.
            var types = ReflectionUtility.GetNonAbstractTypes(typeof(IReactiveComponentReplicationHandler));

            foreach (var type in types)
            {
                if (type.GetCustomAttribute(typeof(DisableAutoRegisterAttribute)) != null)
                {
                    continue;
                }

                var componentReplicationHandler = (IReactiveComponentReplicationHandler) Activator.CreateInstance(type);
                AddComponentReplicator(componentReplicationHandler);
            }
        }

        private struct ComponentReplicator
        {
            public IReactiveComponentReplicationHandler Handler;
            public ComponentGroup EventGroup;
            public ComponentGroup CommandGroup;
        }
    }
}

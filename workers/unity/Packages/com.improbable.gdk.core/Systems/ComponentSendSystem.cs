using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Core.CodegenAdapters;
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
    [UpdateBefore(typeof(SpatialOSSendSystem))]
    public class ComponentSendSystem : ComponentSystem
    {
        private readonly List<ComponentReplicator> componentReplicators = new List<ComponentReplicator>();

        private IConnectionHandler connection;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            connection = World.GetExistingManager<WorkerSystem>().ConnectionHandler;

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
            if (!connection.IsConnected())
            {
                return;
            }

            var componentUpdateSystem = World.GetExistingManager<ComponentUpdateSystem>();

            foreach (var replicator in componentReplicators)
            {
                Profiler.BeginSample("ExecuteReplication");
                replicator.Handler.SendUpdates(replicator.Group, this, EntityManager, componentUpdateSystem);
                Profiler.EndSample();
            }
        }

        internal void AddComponentReplicator(IComponentReplicationHandler componentReplicationHandler)
        {
            componentReplicators.Add(new ComponentReplicator
            {
                ComponentId = componentReplicationHandler.ComponentId,
                Handler = componentReplicationHandler,
                Group = GetComponentGroup(componentReplicationHandler.ComponentUpdateQuery)
            });
        }

        private void PopulateDefaultComponentReplicators()
        {
            // Find all component specific replicators and create an instance.
            var types = ReflectionUtility.GetNonAbstractTypes(typeof(IComponentReplicationHandler));
            foreach (var type in types)
            {
                if (type.GetCustomAttribute(typeof(DisableAutoRegisterAttribute)) != null)
                {
                    continue;
                }

                var handler = (IComponentReplicationHandler) Activator.CreateInstance(type);

                AddComponentReplicator(handler);
            }
        }

        private struct ComponentReplicator
        {
            public uint ComponentId;
            public IComponentReplicationHandler Handler;
            public ComponentGroup Group;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Components;
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

        internal struct ComponentReplicator
        {
            public ComponentReplication ComponentReplication;
            public ComponentGroup BasicReplicationGroup;
            public List<ComponentGroup> CommandReplicationGroups;
        }

        private WorkerBase worker;
        private MutableView view;


        private readonly Dictionary<uint, ComponentReplicator> componentReplicators = new Dictionary<uint, ComponentReplicator>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;

            GenerateComponentGroups();
        }

        private void GenerateComponentGroups()
        {
            var replicators = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentReplication).IsAssignableFrom(type) && !type.IsAbstract).ToList();

            foreach (var replicator in replicators)
            {
                var r = (ComponentReplication)Activator.CreateInstance(replicator, this);
                var basicReplicationGroup = GetComponentGroup(r.BasicReplicationComponentTypes);
                var commandReplicationGroup = r.CommandTypes.Select(commandType => GetComponentGroup(commandType)).ToList();

                componentReplicators.Add(r.ComponentId, new ComponentReplicator {
                    ComponentReplication = r,
                    BasicReplicationGroup = basicReplicationGroup,
                    CommandReplicationGroups = commandReplicationGroup,
                });
            }
        }

        public void RegisterCustomReplicationSystem(uint componentId)
        {
            if (!componentReplicators.ContainsKey(componentId))
            {
                Debug.LogWarningFormat(Warnings.CustomReplicationSystemAlreadyExists, componentId);
                return;
            }

            componentReplicators.Remove(componentId);
        }

        protected override void OnUpdate()
        {
            if (worker.Connection == null)
            {
                return;
            }

            var connection = worker.Connection;
            foreach (var replicator in componentReplicators)
            {
                var r = replicator.Value;
                r.ComponentReplication.ExecuteReplication(r.BasicReplicationGroup, connection);
                r.ComponentReplication.SendCommands(r.CommandReplicationGroups, connection);
            }
        }
    }
}

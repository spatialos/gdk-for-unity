using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Base class for custom replication.
    /// </summary>
    /// <remarks>
    ///    Adding a system that inherits from this class will disable automatic replication for component type T.
    /// </remarks>
    /// <typeparam name="T">The SpatialOS component to define custom replication logic for.</typeparam>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSSendGroup.CustomSpatialOSSendGroup))]
    public abstract class CustomSpatialOSSendSystem<T> : ComponentSystem where T : ISpatialComponentData, new()
    {
        private const string LoggerName = "CustomSpatialOSSendSystem";

        private const string CustomReplicationSystemAlreadyExists =
            "Custom Replication System for this component already exists.";

        protected WorkerSystem WorkerSystem;

        private ComponentSendSystem componentSendSystem;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            WorkerSystem = World.GetExistingManager<WorkerSystem>();

            componentSendSystem = World.GetOrCreateManager<ComponentSendSystem>();

            var component = new T();

            if (!componentSendSystem.TryRegisterCustomReplicationSystem(component.ComponentId))
            {
                WorkerSystem.LogDispatcher.HandleLog(LogType.Error, new LogEvent(CustomReplicationSystemAlreadyExists)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("ComponentType", typeof(T)));
            }
        }
    }
}

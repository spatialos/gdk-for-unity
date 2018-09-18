using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSSendGroup.CustomSpatialOSSendGroup))]
    public abstract class CustomSpatialOSSendSystem<T> : ComponentSystem where T : ISpatialComponentData, new()
    {
        private const string LoggerName = "CustomSpatialOSSendSystem";

        private const string CustomReplicationSystemAlreadyExists =
            "Custom Replication System for this component already exists.";

        protected WorkerSystem WorkerSystem;

        private SpatialOSSendSystem spatialOSSendSystem;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            WorkerSystem = World.GetExistingManager<WorkerSystem>();

            spatialOSSendSystem = World.GetOrCreateManager<SpatialOSSendSystem>();

            var component = new T();

            if (!spatialOSSendSystem.TryRegisterCustomReplicationSystem(component.ComponentId))
            {
                WorkerSystem.LogDispatcher.HandleLog(LogType.Error, new LogEvent(CustomReplicationSystemAlreadyExists)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField("ComponentType", typeof(T)));
            }
        }
    }
}

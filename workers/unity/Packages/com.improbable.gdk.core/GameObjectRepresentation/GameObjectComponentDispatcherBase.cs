using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    internal abstract class GameObjectComponentDispatcherBase
    {
        public abstract ComponentType[] ComponentAddedComponentTypes { get; }
        public ComponentGroup ComponentAddedComponentGroup { get; set; }
        public abstract ComponentType[] ComponentRemovedComponentTypes { get; }
        public ComponentGroup ComponentRemovedComponentGroup { get; set; }
        public abstract ComponentType[] AuthoritiesChangedComponentTypes { get; }
        public ComponentGroup AuthoritiesChangedComponentGroup { get; set; }

        public abstract ComponentType[] ComponentsUpdatedComponentTypes { get; }
        public ComponentGroup ComponentsUpdatedComponentGroup { get; set; }
        public abstract ComponentType[][] EventsReceivedComponentTypeArrays { get; }
        public ComponentGroup[] EventsReceivedComponentGroups { get; set; }
        public abstract ComponentType[][] CommandRequestsComponentTypeArrays { get; }
        public ComponentGroup[] CommandRequestsComponentGroups { get; set; }

        protected abstract uint GetComponentId();

        public void InvokeOnAddComponentLifecycleCallbacks(Dictionary<int, SpatialOSBehaviourManager> entityIndexToManagers)
        {
            var entities = ComponentAddedComponentGroup.GetEntityArray();
            for (var i = 0; i < entities.Length; i++)
            {
                var spatialOSBehaviourManager = entityIndexToManagers[entities[i].Index];
                spatialOSBehaviourManager.AddComponent(GetComponentId());
            }
        }

        public void InvokeOnRemoveComponentLifecycleCallbacks(Dictionary<int, SpatialOSBehaviourManager> entityIndexToManagers)
        {
            var entities = ComponentRemovedComponentGroup.GetEntityArray();
            for (var i = 0; i < entities.Length; i++)
            {
                var spatialOSBehaviourManager = entityIndexToManagers[entities[i].Index];
                spatialOSBehaviourManager.RemoveComponent(GetComponentId());
            }
        }

        public abstract void InvokeOnAuthorityChangeLifecycleCallbacks(Dictionary<int, SpatialOSBehaviourManager> entityIndexToManagers);

        public abstract void InvokeOnComponentUpdateUserCallbacks(ReaderWriterStore readerWriterStore);
        public abstract void InvokeOnEventUserCallbacks(ReaderWriterStore readerWriterStore);
        public abstract void InvokeOnCommandRequestUserCallbacks(ReaderWriterStore readerWriterStore);
        public abstract void InvokeOnAuthorityChangeUserCallbacks(ReaderWriterStore readerWriterStore);
    }
}

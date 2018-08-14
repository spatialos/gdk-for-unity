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

        public void InvokeOnAddComponentLifecycleCallbacks(Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers)
        {
            var entities = ComponentAddedComponentGroup.GetEntityArray();
            for (var i = 0; i < entities.Length; i++)
            {
                var activationManager = entityIndexToManagers[entities[i].Index];
                activationManager.AddComponent(GetComponentId());
            }
        }

        public void InvokeOnRemoveComponentLifecycleCallbacks(Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers)
        {
            var entities = ComponentRemovedComponentGroup.GetEntityArray();
            for (var i = 0; i < entities.Length; i++)
            {
                var activationManager = entityIndexToManagers[entities[i].Index];
                activationManager.RemoveComponent(GetComponentId());
            }
        }

        public abstract void InvokeOnAuthorityChangeLifecycleCallbacks(Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers);

        public abstract void InvokeOnComponentUpdateUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores);
        public abstract void InvokeOnEventUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores);
        public abstract void InvokeOnCommandRequestUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores);
        public abstract void InvokeOnAuthorityChangeUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores);
    }
}

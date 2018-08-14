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

        public abstract void InvokeOnAddComponentLifecycleMethods
            (Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers);
        public abstract void InvokeOnRemoveComponentLifecycleMethods
            (Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers);
        public abstract void InvokeOnAuthorityChangeLifecycleMethods
            (Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers);

        public abstract void InvokeOnComponentUpdateCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores);
        public abstract void InvokeOnEventCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores);
        public abstract void InvokeOnCommandRequestCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores);
        public abstract void InvokeOnAuthorityChangeCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores);
    }
}

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

        public abstract void MarkComponentsAddedForActivation
            (Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers);
        public abstract void MarkComponentsRemovedForDeactivation
            (Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers);
        public abstract void MarkAuthorityChangesForActivation
            (Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers);

        public abstract void InvokeOnComponentUpdateCallbacks(Dictionary<int, InjectableStore> readerWriterStores);
        public abstract void InvokeOnEventCallbacks(Dictionary<int, InjectableStore> readerWriterStores);
        public abstract void InvokeOnCommandRequestCallbacks(Dictionary<int, InjectableStore> readerWriterStores);
        public abstract void InvokeOnAuthorityChangeCallbacks(Dictionary<int, InjectableStore> readerWriterStores);
    }
}

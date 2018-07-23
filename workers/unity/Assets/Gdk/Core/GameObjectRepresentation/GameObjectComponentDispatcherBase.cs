using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public abstract class GameObjectComponentDispatcherBase
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

        public abstract void InvokeOnAddComponentCallbacks();
        public abstract void InvokeOnRemoveComponentCallbacks();
        public abstract void InvokeOnAuthorityChangeCallbacks();
        public abstract void InvokeOnComponentUpdateCallbacks();
        public abstract void InvokeOnEventCallbacks();
        public abstract void InvokeOnCommandRequestCallbacks();
    }
}

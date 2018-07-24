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

        public abstract void InvokeOnAddComponentCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnRemoveComponentCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnAuthorityChangeCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnComponentUpdateCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnEventCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnCommandRequestCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
    }
}

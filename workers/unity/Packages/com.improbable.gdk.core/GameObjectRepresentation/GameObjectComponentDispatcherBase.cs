using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
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

        public abstract void InvokeOnAddComponentLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnRemoveComponentLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnAuthorityChangeLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);

        public abstract void InvokeOnAuthorityChangeUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnComponentUpdateUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnEventUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnCommandRequestUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
    }
}

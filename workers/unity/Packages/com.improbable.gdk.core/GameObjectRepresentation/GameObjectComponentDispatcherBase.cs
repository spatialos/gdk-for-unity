using System;
using System.Collections.Generic;
using System.ComponentModel;
using Improbable.Gdk.Core.MonoBehaviours;
using Unity.Entities;

namespace Improbable.Gdk.Core
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

        public void InvokeOnAddComponentLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
        {
            var entities = ComponentAddedComponentGroup.GetEntityArray();
            for (var i = 0; i < entities.Length; i++)
            {
                var spatialOSBehaviourManager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                spatialOSBehaviourManager.AddComponent(GetComponentId());
            }
        }

        public void InvokeOnRemoveComponentLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
        {
            var entities = ComponentRemovedComponentGroup.GetEntityArray();
            for (var i = 0; i < entities.Length; i++)
            {
                var spatialOSBehaviourManager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                spatialOSBehaviourManager.RemoveComponent(GetComponentId());
            }
        }

        public abstract void InvokeOnComponentUpdateUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnEventUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnCommandRequestUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnAuthorityChangeLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
        public abstract void InvokeOnAuthorityChangeUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem);
    }
}

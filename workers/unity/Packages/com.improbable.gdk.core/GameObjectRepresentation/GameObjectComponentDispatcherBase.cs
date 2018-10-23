using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Base class for a GameObjectComponentDispatcher. Used in code generation.
    /// </summary>
    public abstract class GameObjectComponentDispatcherBase
    {
        public abstract ComponentType[] ComponentAddedComponentTypes { get; }
        public ComponentGroup ComponentAddedComponentGroup { get; set; }
        public abstract ComponentType[] ComponentRemovedComponentTypes { get; }
        public ComponentGroup ComponentRemovedComponentGroup { get; set; }
        public abstract ComponentType[] AuthorityGainedComponentTypes { get; }
        public ComponentGroup AuthorityGainedComponentGroup { get; set; }
        public abstract ComponentType[] AuthorityLostComponentTypes { get; }
        public ComponentGroup AuthorityLostComponentGroup { get; set; }
        public abstract ComponentType[] AuthorityLossImminentComponentTypes { get; }
        public ComponentGroup AuthorityLossImminentComponentGroup { get; set; }

        public abstract ComponentType[] ComponentsUpdatedComponentTypes { get; }
        public ComponentGroup ComponentsUpdatedComponentGroup { get; set; }
        public abstract ComponentType[][] EventsReceivedComponentTypeArrays { get; }
        public ComponentGroup[] EventsReceivedComponentGroups { get; set; }
        public abstract ComponentType[][] CommandRequestsComponentTypeArrays { get; }
        public ComponentGroup[] CommandRequestsComponentGroups { get; set; }
        public abstract ComponentType[][] CommandResponsesComponentTypeArrays { get; }
        public ComponentGroup[] CommandResponsesComponentGroups { get; set; }

        public abstract void MarkComponentsAddedForActivation
            (Dictionary<Entity, MonoBehaviourActivationManager> entityIndexToManagers);

        public abstract void MarkComponentsRemovedForDeactivation
            (Dictionary<Entity, MonoBehaviourActivationManager> entityIndexToManagers);

        public abstract void MarkAuthorityGainedForActivation
            (Dictionary<Entity, MonoBehaviourActivationManager> entityIndexToManagers);

        public abstract void MarkAuthorityLostForDeactivation
            (Dictionary<Entity, MonoBehaviourActivationManager> entityIndexToManagers);

        public abstract void InvokeOnComponentUpdateCallbacks(Dictionary<Entity, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnEventCallbacks(Dictionary<Entity, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnCommandRequestCallbacks(Dictionary<Entity, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnCommandResponseCallbacks(Dictionary<Entity, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnAuthorityGainedCallbacks(Dictionary<Entity, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnAuthorityLostCallbacks(Dictionary<Entity, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnAuthorityLossImminentCallbacks(Dictionary<Entity, InjectableStore> entityIndexToInjectableStore);
    }
}

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

        public abstract void MarkComponentsAddedForActivation
            (Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers);
        public abstract void MarkComponentsRemovedForDeactivation
            (Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers);
        public abstract void MarkAuthorityGainedForActivation
            (Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers);
        public abstract void MarkAuthorityLostForDeactivation
            (Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers);

        public abstract void InvokeOnComponentUpdateCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnEventCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnCommandRequestCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnCommandResponseCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnAuthorityGainedCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnAuthorityLostCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore);
        public abstract void InvokeOnAuthorityLossImminentCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore);
    }
}

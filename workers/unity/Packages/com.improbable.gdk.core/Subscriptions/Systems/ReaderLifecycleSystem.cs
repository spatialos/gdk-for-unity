using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions
{
    // todo this should really just subsume the callback systems
    // or the order of events should be decided by system order rather than in this system
    // would enable users changing readers more easily maybe
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    public class ReaderLifecycleSystem : ComponentSystem
    {
        private readonly List<MonoBehaviour> behavioursToEnable = new List<MonoBehaviour>();
        private readonly List<MonoBehaviour> behavioursToDisable = new List<MonoBehaviour>();

        // todo make this order the behaviours somehow
        internal void EnableMonoBehaviour(MonoBehaviour behaviour)
        {
            behavioursToEnable.Add(behaviour);
        }

        internal void DisableMonoBehaviour(MonoBehaviour behaviour)
        {
            behavioursToDisable.Add(behaviour);
        }

        [Inject] private CommandCallbackSystem commandCallbackSystem;
        [Inject] private ComponentCallbackSystem componentCallbackSystem;
        [Inject] private ComponentConstraintsCallbackSystem componentConstraintsCallbackSystem;

        [Inject] private CommandSystem commandSystem;
        [Inject] private ComponentUpdateSystem componentUpdateSystem;

        protected override void OnUpdate()
        {
            componentConstraintsCallbackSystem.Invoke(componentUpdateSystem);
            foreach (var behaviour in behavioursToDisable)
            {
                behaviour.enabled = false;
            }

            componentCallbackSystem.InvokeNoLossImminent(componentUpdateSystem);

            foreach (var behaviour in behavioursToEnable)
            {
                behaviour.enabled = true;
            }

            componentCallbackSystem.InvokeLossImminent(componentUpdateSystem);

            commandCallbackSystem.InvokeCallbacks(commandSystem);
        }
    }
}

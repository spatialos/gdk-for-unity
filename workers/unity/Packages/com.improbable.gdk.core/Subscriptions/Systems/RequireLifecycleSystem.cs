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
    public class RequireLifecycleSystem : ComponentSystem
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
            behavioursToEnable.Remove(behaviour);
        }

        [Inject] private CommandCallbackSystem commandCallbackSystem;
        [Inject] private ComponentCallbackSystem componentCallbackSystem;
        [Inject] private ComponentConstraintsCallbackSystem componentConstraintsCallbackSystem;

        [Inject] private CommandSystem commandSystem;

        protected override void OnUpdate()
        {
            componentConstraintsCallbackSystem.Invoke();
            foreach (var behaviour in behavioursToDisable)
            {
                if (behaviour == null)
                {
                    continue;
                }

                behaviour.enabled = false;
            }

            behavioursToDisable.Clear();

            componentCallbackSystem.InvokeNoLossImminent();

            foreach (var behaviour in behavioursToEnable)
            {
                if (behaviour == null)
                {
                    continue;
                }

                behaviour.enabled = true;
            }

            behavioursToEnable.Clear();

            componentCallbackSystem.InvokeLossImminent();

            commandCallbackSystem.InvokeCallbacks(commandSystem);
        }
    }
}

using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(RequireLifecycleGroup))]
    public class RequireLifecycleSystem : ComponentSystem
    {
        private readonly List<MonoBehaviour> behavioursToEnable = new List<MonoBehaviour>();

        private CommandCallbackSystem commandCallbackSystem;
        private ComponentCallbackSystem componentCallbackSystem;
        private ComponentConstraintsCallbackSystem componentConstraintsCallbackSystem;
        private WorkerFlagCallbackSystem workerFlagCallbackSystem;

        // todo make this order the behaviours somehow
        internal void EnableMonoBehaviour(MonoBehaviour behaviour)
        {
            behavioursToEnable.Add(behaviour);
        }

        internal void DisableMonoBehaviour(MonoBehaviour behaviour)
        {
            // Behaviours can be disabled immediately, as conflicting information can not arrive in the same frame.
            behaviour.enabled = false;
            behavioursToEnable.Remove(behaviour);
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            commandCallbackSystem = World.GetExistingManager<CommandCallbackSystem>();
            componentCallbackSystem = World.GetExistingManager<ComponentCallbackSystem>();
            componentConstraintsCallbackSystem = World.GetExistingManager<ComponentConstraintsCallbackSystem>();
            workerFlagCallbackSystem = World.GetExistingManager<WorkerFlagCallbackSystem>();
        }

        protected override void OnUpdate()
        {
            componentConstraintsCallbackSystem.Invoke();

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

            commandCallbackSystem.InvokeCallbacks();
            workerFlagCallbackSystem.InvokeCallbacks();
        }
    }
}

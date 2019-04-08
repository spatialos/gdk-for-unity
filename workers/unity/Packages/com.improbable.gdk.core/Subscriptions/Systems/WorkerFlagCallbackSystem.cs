using System;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [DisableAutoCreation]
    public class WorkerFlagCallbackSystem : ComponentSystem
    {
        private WorkerFlagCallbackManager manager;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            Enabled = false;

            manager = new WorkerFlagCallbackManager(World);
        }

        public ulong RegisterWorkerFlagChangeCallback(Action<(string, string)> callback)
        {
            return manager.RegisterCallback(callback);
        }

        public bool UnregisterWorkerFlagChangeCallback(ulong callbackKey)
        {
            return manager.UnregisterCallback(callbackKey);
        }

        internal void InvokeCallbacks()
        {
            manager.InvokeCallbacks();
        }

        protected override void OnUpdate()
        {
        }
    }
}

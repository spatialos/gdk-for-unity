using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public class WorkerFlagCallbackManager : ICallbackManager
    {
        private readonly CallbackCollection<(string, string)> callbackCollection = new CallbackCollection<(string, string)>();
        private readonly WorkerSystem workerSystem;

        private ulong nextCallbackId = 1;

        public WorkerFlagCallbackManager(World world)
        {
            workerSystem = world.GetExistingSystem<WorkerSystem>();
        }

        public ulong RegisterCallback(Action<(string, string)> callback)
        {
            callbackCollection.Add(nextCallbackId, callback);
            return nextCallbackId++;
        }

        public bool UnregisterCallback(ulong callbackKey)
        {
            return callbackCollection.Remove(callbackKey);
        }

        public void InvokeCallbacks()
        {
            var workerFlagChanges = workerSystem.Diff.GetWorkerFlagChanges();
            for (var i = 0; i < workerFlagChanges.Count; ++i)
            {
                var pair = workerFlagChanges[i];
                callbackCollection.InvokeAll(pair);
            }
        }
    }
}

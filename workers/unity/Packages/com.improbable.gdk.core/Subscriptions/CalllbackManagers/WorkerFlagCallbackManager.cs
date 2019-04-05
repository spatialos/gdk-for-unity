using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public class WorkerFlagCallbackManager : ICallbackManager
    {
        private readonly Callbacks<(string, string)> callbacks = new Callbacks<(string, string)>();
        private readonly WorkerSystem workerSystem;

        private ulong nextCallbackId = 1;

        public WorkerFlagCallbackManager(World world)
        {
            workerSystem = world.GetExistingManager<WorkerSystem>();
        }

        public ulong RegisterCallback(Action<string, string> callback)
        {
            callbacks.Add(nextCallbackId, pair => callback(pair.Item1, pair.Item2));
            return nextCallbackId++;
        }

        public bool UnregisterCallback(ulong callbackKey)
        {
            return callbacks.Remove(callbackKey);
        }

        public void InvokeCallbacks()
        {
            var workerFlagChanges = workerSystem.Diff.GetWorkerFlagChanges();
            for (int i = 0; i < workerFlagChanges.Count; ++i)
            {
                var pair = workerFlagChanges[i];
                callbacks.InvokeAll(pair);
            }
        }
    }
}

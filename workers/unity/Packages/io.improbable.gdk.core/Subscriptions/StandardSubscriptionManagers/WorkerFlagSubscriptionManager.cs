using System;
using System.Collections.Generic;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [AutoRegisterSubscriptionManager]
    public class WorkerFlagSubscriptionManager : SubscriptionManager<WorkerFlagReader>
    {
        private World world;

        private HashSet<Subscription<WorkerFlagReader>> readerSubscriptions = new HashSet<Subscription<WorkerFlagReader>>();

        public WorkerFlagSubscriptionManager(World world)
        {
            this.world = world;
        }

        public override void Cancel(ISubscription subscription)
        {
            var readerSubscription = (Subscription<WorkerFlagReader>) subscription;

            readerSubscription.Value.IsValid = false;
            readerSubscription.Value.RemoveAllCallbacks();
            readerSubscriptions.Remove(readerSubscription);
        }

        public override void ResetValue(ISubscription subscription)
        {
            var readerSubscription = (Subscription<WorkerFlagReader>) subscription;

            readerSubscription.Value.RemoveAllCallbacks();
        }

        public override Subscription<WorkerFlagReader> Subscribe(EntityId entityId)
        {
            var subscription = new Subscription<WorkerFlagReader>(this, new EntityId(0));
            readerSubscriptions.Add(subscription);

            subscription.SetAvailable(new WorkerFlagReader(world));

            return subscription;
        }
    }

    public class WorkerFlagReader
    {
        public bool IsValid;

        private readonly WorkerFlagCallbackSystem callbackSystem;
        private readonly View view;

        private Dictionary<Action<string, string>, ulong> callbackToKey;

        public WorkerFlagReader(World world)
        {
            IsValid = true;
            callbackSystem = world.GetExistingSystem<WorkerFlagCallbackSystem>();
            view = world.GetExistingSystem<WorkerSystem>().View;
        }

        public event Action<string, string> OnWorkerFlagChange
        {
            add
            {
                if (callbackToKey == null)
                {
                    callbackToKey = new Dictionary<Action<string, string>, ulong>();
                }

                var key = callbackSystem.RegisterWorkerFlagChangeCallback(pair =>
                {
                    if (!IsValid)
                    {
                        return;
                    }

                    value(pair.Item1, pair.Item2);
                });

                callbackToKey.Add(value, key);
            }
            remove
            {
                if (callbackToKey == null)
                {
                    return;
                }

                if (!callbackToKey.TryGetValue(value, out var key))
                {
                    return;
                }

                callbackSystem.UnregisterWorkerFlagChangeCallback(key);
                callbackToKey.Remove(value);
            }
        }

        public string GetFlag(string name)
        {
            return view.GetWorkerFlag(name);
        }

        internal void RemoveAllCallbacks()
        {
            if (callbackToKey == null)
            {
                return;
            }

            foreach (var valuePair in callbackToKey)
            {
                callbackSystem.UnregisterWorkerFlagChangeCallback(valuePair.Value);
            }

            callbackToKey.Clear();
        }
    }
}

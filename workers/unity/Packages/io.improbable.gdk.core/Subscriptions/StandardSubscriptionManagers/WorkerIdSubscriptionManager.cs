using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [AutoRegisterSubscriptionManager]
    public class WorkerIdSubscriptionManager : SubscriptionManager<WorkerId>
    {
        private readonly World world;

        public WorkerIdSubscriptionManager(World world)
        {
            this.world = world;
        }

        public override Subscription<WorkerId> Subscribe(EntityId entityId)
        {
            var subscription = new Subscription<WorkerId>(this, new EntityId(0));
            var workerId = world.GetExistingSystem<WorkerSystem>().WorkerId;
            subscription.SetAvailable(new WorkerId(workerId));

            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
        }

        public override void ResetValue(ISubscription subscription)
        {
        }
    }

    public readonly struct WorkerId : IEquatable<string>, IEquatable<WorkerId>
    {
        public readonly string Id;

        public WorkerId(string id)
        {
            Id = id;
        }

        public bool Equals(WorkerId other)
        {
            if (Id == null)
            {
                throw new NullReferenceException();
            }

            return other.Id != null && Id.Equals(other.Id);
        }

        public bool Equals(string other)
        {
            if (Id == null)
            {
                throw new NullReferenceException();
            }

            return other != null && Id.Equals(other);
        }

        public override string ToString()
        {
            return Id;
        }

        public static implicit operator string(WorkerId workerId)
        {
            return workerId.Id;
        }
    }
}

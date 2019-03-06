using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    public interface ISubscription
    {
        // todo not all of these need to be here
        bool HasValue { get; }
        EntityId EntityId { get; }

        // todo put these in their own interface
        void SetAvailabilityHandler(ISubscriptionAvailabilityHandler handler);
        ISubscriptionAvailabilityHandler GetAvailabilityHandler();
        object GetErasedValue();

        void Cancel();
        void ResetValue();
    }

    // possible this can be made a struct
    // might need to make HasValue a property though and give it an ID
    public class Subscription<T> : ISubscription
    {
        // Want a custom event thing that gives an add and invoke option
        public bool HasValue { get; private set; }
        public EntityId EntityId { get; }

        private readonly SubscriptionManagerBase manager;

        private ISubscriptionAvailabilityHandler availabilityHandler;

        private List<Action<T>> onAvailable;

        public event Action<T> OnAvailable
        {
            add
            {
                if (HasValue)
                {
                    value(this.value);
                }

                if (onAvailable == null)
                {
                    onAvailable = new List<Action<T>>();
                }

                onAvailable.Add(value);
            }
            remove => onAvailable.Remove(value);
        }

        public event Action<T> OnUnavailable;

        private T value;

        public T Value
        {
            get
            {
                if (HasValue)
                {
                    return value;
                }

                throw new InvalidOperationException("Subscribed object not available");
            }
        }

        public Subscription(SubscriptionManagerBase manager, EntityId entityId)
        {
            this.manager = manager;
            this.EntityId = entityId;
        }

        public void Cancel()
        {
            manager.Cancel(this);
        }

        // Would like to find a way to make these internal and have managers update via a proxy
        // Could also do this via explicit interface although it means more boxing
        public void SetAvailable(T subscribedObject)
        {
            value = subscribedObject;
            HasValue = true;

            availabilityHandler?.OnAvailable();

            if (onAvailable != null)
            {
                foreach (var callback in onAvailable)
                {
                    callback(value);
                }
            }
        }

        public void SetUnavailable()
        {
            HasValue = false;
            value = default(T);

            availabilityHandler?.OnUnavailable();

            OnUnavailable?.Invoke(value);
        }

        // I'm still unsure about this way of doing things.
        // But nothing in here strictly has to be hidden from users but it's not the stuff they want their IDEs showing them

        #region type erased and internal things that should really be somewhere else. This will probably be fixed by making Subscription just an ID and a view onto a manager.

        void ISubscription.SetAvailabilityHandler(ISubscriptionAvailabilityHandler handler)
        {
            availabilityHandler = handler;
            if (HasValue)
            {
                handler.OnAvailable();
            }
        }

        ISubscriptionAvailabilityHandler ISubscription.GetAvailabilityHandler()
        {
            return availabilityHandler;
        }

        object ISubscription.GetErasedValue()
        {
            return Value;
        }

        void ISubscription.ResetValue()
        {
            manager.ResetValue(this);
        }

        #endregion
    }
}

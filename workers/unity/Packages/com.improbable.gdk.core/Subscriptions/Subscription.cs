using System;
using System.Collections.Generic;
using Improbable.Worker;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions
{
    public interface ITypeErasedSubscription
    {
        bool HasValue { get; }
        EntityId EntityId { get; }

        // todo think of an actual name
        event Action OnAvailable_Internal;
        event Action OnUnavailable_Internal;

        // todo see if there is a way to get away without this
        object GetErasedValue();

        void Cancel();
        void ResetValue();
    }

    // possible this can be made a struct
    // might need to make HasValue a property though and give it an ID
    public class Subscription<T> : ITypeErasedSubscription
    {
        // Want a custom event thing that gives an add and invoke option
        public bool HasValue { get; private set; }
        public EntityId EntityId { get; }

        private readonly SubscriptionManagerBase manager;

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

            if (onAvailable_internal != null)
            {
                foreach (var callback in onAvailable_internal)
                {
                    callback();
                }
            }

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

            if (onUnavailable_internal != null)
            {
                foreach (var callback in onUnavailable_internal)
                {
                    callback();
                }
            }

            OnUnavailable?.Invoke(value);
        }

        // I'm still unsure about this way of doing things.
        // But nothing in here strictly has to be hidden from users but it's not the stuff they want their IDEs showing them

        #region ITypeErasedSubscription implementation

        private List<Action> onAvailable_internal;

        event Action ITypeErasedSubscription.OnAvailable_Internal
        {
            add
            {
                if (HasValue)
                {
                    value();
                }

                if (onAvailable_internal == null)
                {
                    onAvailable_internal = new List<Action>();
                }

                onAvailable_internal.Add(value);
            }
            remove => onAvailable_internal.Remove(value);
        }

        private List<Action> onUnavailable_internal;

        event Action ITypeErasedSubscription.OnUnavailable_Internal
        {
            add
            {
                if (onUnavailable_internal == null)
                {
                    onUnavailable_internal = new List<Action>();
                }

                onUnavailable_internal.Add(value);
            }
            remove => onUnavailable_internal.Remove(value);
        }

        object ITypeErasedSubscription.GetErasedValue()
        {
            return Value;
        }

        void ITypeErasedSubscription.ResetValue()
        {
            manager.ResetValue(this);
        }

        #endregion
    }
}

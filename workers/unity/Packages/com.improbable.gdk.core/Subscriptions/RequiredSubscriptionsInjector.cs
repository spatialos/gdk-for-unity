using System;
using Improbable.Worker;

namespace Improbable.Gdk.Subscriptions
{
    // todo this should probably be an interface to be able to remove reflection via baking
    public class RequiredSubscriptionsInjector
    {
        private readonly SubscriptionAggregate subscriptions;
        private readonly RequiredSubscriptionsInfo info;
        private readonly object target;

        private Action onEnable;
        private Action onDisable;

        // todo should either special case monobehaviours or not use callbacks
        // for non monobehaviours we could use functors
        public RequiredSubscriptionsInjector(object target, EntityId entityId, SubscriptionSystem subscriptionSystem,
            Action onEnable = null, Action onDisable = null)
        {
            info = RequiredSubscriptionsDatabase.GetOrCreateRequiredSubscriptionsInfo(target.GetType());
            if (info.RequiredTypes.Length == 0)
            {
                return;
            }

            this.target = target;
            this.onEnable = onEnable;
            this.onDisable = onDisable;

            subscriptions = new SubscriptionAggregate(subscriptionSystem, entityId, info.RequiredTypes);
            subscriptions.OnSubscriptionsSatisfied += HandleSubscriptionsSatisfied;
            subscriptions.OnSubscriptionsNoLongerSatisfied += HandleSubscriptionsNoLongerSatisfied;
        }

        public void CancelSubscriptions()
        {
            subscriptions.Cancel();

            if (target == null)
            {
                return;
            }

            foreach (var field in info.RequiredFields)
            {
                if (!field.FieldType.IsValueType)
                {
                    field.SetValue(target, null);
                }
            }
        }

        private void HandleSubscriptionsSatisfied()
        {
            foreach (var field in info.RequiredFields)
            {
                // todo should really do this as they become available rather than all at once
                field.SetValue(target, subscriptions.GetErasedValue(field.FieldType));
            }

            onEnable?.Invoke();
        }

        private void HandleSubscriptionsNoLongerSatisfied()
        {
            onDisable?.Invoke();
        }
    }
}

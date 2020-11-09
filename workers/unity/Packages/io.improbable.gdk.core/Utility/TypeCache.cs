using System;
using System.Collections.Generic;
using Improbable.Gdk.Subscriptions;

namespace Improbable.Gdk.Core
{
    public static class TypeCache
    {
        private static List<Type> subscriptionManagerTypes;

        public static List<Type> GetSubscriptionManagerTypes()
        {
            if (subscriptionManagerTypes == null)
            {
                subscriptionManagerTypes = ReflectionUtility.GetNonAbstractTypes(typeof(SubscriptionManagerBase),
                    typeof(AutoRegisterSubscriptionManagerAttribute));
            }

            return subscriptionManagerTypes;
        }

        public static List<Type> GetComponentMetaClassTypes()
        {
            return ReflectionUtility.GetNonAbstractTypes(typeof(IComponentMetaclass));
        }
    }
}

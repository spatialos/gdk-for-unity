using System;
using System.Collections.Generic;
using Improbable.Gdk.Subscriptions;

namespace Improbable.Gdk.Core
{
    public static class TypeCache
    {
        private static List<Type> subscriptionManagerTypes;
        private static List<Type> componentMetaClassTypes;

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
            if (componentMetaClassTypes == null)
            {
                componentMetaClassTypes = ReflectionUtility.GetNonAbstractTypes(typeof(IComponentMetaclass));
            }

            return componentMetaClassTypes;
        }
        public static readonly Lazy<Type> ComponentSetManager =
            new Lazy<Type>(() => ReflectionUtility.GetNonAbstractTypes(typeof(IComponentSetManager))[0]);
    }
}

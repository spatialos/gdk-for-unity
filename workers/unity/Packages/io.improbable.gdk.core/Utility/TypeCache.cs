using System;
using System.Collections.Generic;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public static class TypeCache
    {
        private static List<Type> subscriptionManagerTypes;
        private static List<Type> temporaryComponentDataTypes;
        private static List<Type> temporarySharedComponentDataTypes;

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

        public static List<Type> GetTemporaryComponentDataTypes()
        {
            if (temporaryComponentDataTypes == null)
            {
                temporaryComponentDataTypes = ReflectionUtility.GetNonAbstractTypes(typeof(IComponentData),
                    typeof(RemoveAtEndOfTickAttribute));
            }

            return temporaryComponentDataTypes;
        }

        public static List<Type> GetTemporarySharedComponentDataTypes()
        {
            if (temporarySharedComponentDataTypes == null)
            {
                temporarySharedComponentDataTypes = ReflectionUtility.GetNonAbstractTypes(typeof(ISharedComponentData),
                    typeof(RemoveAtEndOfTickAttribute));
            }

            return temporarySharedComponentDataTypes;
        }
    }
}

using System;
using System.Collections.Generic;
using Improbable.Gdk.Subscriptions;

namespace Improbable.Gdk.Core
{
    public static class TypeCache
    {
        public static readonly Lazy<IReadOnlyList<Type>> SubscriptionManagerTypes = new Lazy<IReadOnlyList<Type>>(() =>
            ReflectionUtility.GetNonAbstractTypes(typeof(SubscriptionManagerBase),
                typeof(AutoRegisterSubscriptionManagerAttribute)));

        public static readonly Lazy<IReadOnlyList<Type>> ComponentMetaClassTypes =
            new Lazy<IReadOnlyList<Type>>(() => ReflectionUtility.GetNonAbstractTypes(typeof(IComponentMetaclass)));

        public static readonly Lazy<Type> ComponentSetManager =
            new Lazy<Type>(() => ReflectionUtility.GetNonAbstractTypes(typeof(IComponentSetManager))[0]);
    }
}

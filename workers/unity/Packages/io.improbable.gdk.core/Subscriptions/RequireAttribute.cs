using System;
using JetBrains.Annotations;

namespace Improbable.Gdk.Subscriptions
{
    [AttributeUsage(AttributeTargets.Field), MeansImplicitUse]
    public class RequireAttribute : Attribute
    {
    }
}

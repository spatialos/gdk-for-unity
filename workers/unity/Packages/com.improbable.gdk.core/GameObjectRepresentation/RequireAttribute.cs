using System;

namespace Improbable.Gdk.Core
{
    /// <returns>
    ///     Marks fields/properties of MonoBehaviours that require a Reader or Writer to be injected into them.
    /// </returns>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RequireAttribute : Attribute
    {
    }
}

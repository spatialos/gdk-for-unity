using System;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Marks fields/properties of MonoBehaviours that require a Reader or Writer to be injected into them.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RequireAttribute : Attribute
    {
    }
}

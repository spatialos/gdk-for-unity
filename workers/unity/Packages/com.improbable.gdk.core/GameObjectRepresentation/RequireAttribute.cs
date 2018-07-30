using System;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    /// <summary>
    ///     Marks fields of MonoBehaviours that require a Reader or Writer to be injected into them.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class RequireAttribute : Attribute
    {
    }
}

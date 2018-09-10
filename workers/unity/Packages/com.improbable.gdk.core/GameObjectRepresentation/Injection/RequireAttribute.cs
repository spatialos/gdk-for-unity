using System;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Marks fields of MonoBehaviours that require an Injectable to be injected into them.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class RequireAttribute : Attribute
    {
    }
}

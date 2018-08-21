using System;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Any component with this attribute will be removed from all entities by the CleanReactiveComponentSystem
    ///     Can be added to components extending <see cref="IComponentData" /> or <see cref="ISharedComponentData" />
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct)]
    public class RemoveAtEndOfTickAttribute : Attribute
    {
    }
}

using System;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Any component which inherits this interface will be removed from all entities by the CleanReactiveComponentSystem.
    ///     Can be added to components extending <see cref="IComponentData" /> or <see cref="ISharedComponentData" />
    /// </summary>

    public interface IRemoveableComponent
    {
        void RemoveComponent(EntityCommandBuffer commands, Entity entity);
    }
}

using System;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Any component that inheriting this interrace will be removed from all entities
    ///     by the CleanReactiveComponentSystem.
    ///     Can be added to components extending <see cref="IComponentData" /> or <see cref="ISharedComponentData" />
    /// </summary>

    public interface RemoveAtEndOfTick
    {
        void RemoveComponent(EntityCommandBuffer commands, Entity entity);
    }
}

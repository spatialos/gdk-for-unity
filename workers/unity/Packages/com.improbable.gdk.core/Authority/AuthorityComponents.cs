using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     ECS component denotes that this worker is authoritative over <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The SpatialOS component.</typeparam>
    public struct Authoritative<T> : IComponentData where T : ISpatialComponentData
    {
    }

    /// <summary>
    ///     ECS component denotes that this worker is not authoritative over <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The SpatialOS component.</typeparam>
    public struct NotAuthoritative<T> : IComponentData where T : ISpatialComponentData
    {
    }

    /// <summary>
    ///     ECS component denotes that this worker will lose authority over <see cref="T"/> imminently.
    /// </summary>
    /// <remarks>
    ///     Note that this worker may still be authoritative over this component.
    /// </remarks>
    /// <typeparam name="T">The SpatialOS component.</typeparam>
    public struct AuthorityLossImminent<T> : IComponentData where T : ISpatialComponentData
    {
    }
}

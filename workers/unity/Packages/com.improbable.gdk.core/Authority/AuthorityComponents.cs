using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     This ECS component denotes that this worker is authoritative over T.
    /// </summary>
    /// <typeparam name="T">The SpatialOS component.</typeparam>
    public struct Authoritative<T> : IComponentData where T : ISpatialComponentData
    {
    }

    /// <summary>
    ///     This ECS component denotes that this worker is not authoritative over T.
    /// </summary>
    /// <typeparam name="T">The SpatialOS component.</typeparam>
    public struct NotAuthoritative<T> : IComponentData where T : ISpatialComponentData
    {
    }

    /// <summary>
    ///     This ECS component denotes that this worker will lose authority over T imminently.
    ///     Note that this worker is still authoritative over this component.
    /// </summary>
    /// <typeparam name="T">The SpatialOS component.</typeparam>
    public struct AuthorityLossImminent<T> : IComponentData where T : ISpatialComponentData
    {
    }
}

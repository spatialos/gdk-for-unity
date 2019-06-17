#if !DISABLE_REACTIVE_COMPONENTS
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
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
    ///     If AcknowledgeAuthorityLoss is set then authority handover will complete before the handover timeout.
    /// </summary>
    /// <remarks>
    ///     Note that this worker may still be authoritative over this component.
    /// </remarks>
    /// <typeparam name="T">The SpatialOS component.</typeparam>
    public struct AuthorityLossImminent<T> : IComponentData where T : ISpatialComponentData
    {
        public bool AcknowledgeAuthorityLoss;
    }
}
#endif

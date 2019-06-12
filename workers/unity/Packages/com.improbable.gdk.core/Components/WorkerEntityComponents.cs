using System;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     ECS Component denoting a worker entity
    /// </summary>
    public struct WorkerEntityTag : IComponentData
    {
    }

    /// <summary>
    ///     ECS Component added to the worker entity immediately after establishing a connection to a SpatialOS deployment.
    /// </summary>
    /// <remarks>
    ///    This is a temporary component and the <see cref="Improbable.Gdk.Core.CleanTemporaryComponentsSystem"/> will
    ///    remove it at the end of the frame.
    /// </remarks>
    [RemoveAtEndOfTick]
    public struct OnConnected : IComponentData
    {
    }

    /// <summary>
    ///     ECS Component added to the worker entity immediately after disconnecting from SpatialOS
    /// </summary>
    ///    This is a temporary component and the <see cref="Improbable.Gdk.Core.CleanTemporaryComponentsSystem"/> will
    ///    remove it at the end of the frame.
    [RemoveAtEndOfTick]
    public struct OnDisconnected : ISharedComponentData, IEquatable<OnDisconnected>
    {
        /// <summary>
        ///     The reported reason for disconnecting
        /// </summary>
        public string ReasonForDisconnect;

        public bool Equals(OnDisconnected other)
        {
            return string.Equals(ReasonForDisconnect, other.ReasonForDisconnect);
        }

        public override bool Equals(object obj)
        {
            return obj is OnDisconnected other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (ReasonForDisconnect != null ? ReasonForDisconnect.GetHashCode() : 0);
        }
    }
}

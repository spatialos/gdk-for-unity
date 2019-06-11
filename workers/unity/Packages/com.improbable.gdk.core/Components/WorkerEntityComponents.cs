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
    [RemoveAtEndOfTick]
    public struct OnConnected : IComponentData
    {
    }

    /// <summary>
    ///     ECS Component added to the worker entity immediately after disconnecting from SpatialOS
    /// </summary>
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

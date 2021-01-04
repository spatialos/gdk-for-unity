using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.NetworkStats;

namespace Improbable.Gdk.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     Represents a handler for a SpatialOS connection.
    /// </summary>
    public interface IConnectionHandler : IDisposable
    {
        /// <summary>
        ///     Gets the worker ID for this worker.
        /// </summary>
        /// <returns>The worker ID.</returns>
        string GetWorkerId();

        /// <summary>
        ///     Gets the worker <see cref="EntityId"/> for this worker.
        /// </summary>
        /// <returns>The worker EntityId.</returns>
        EntityId GetWorkerEntityId();

        /// <summary>
        ///     Populates the <see cref="ViewDiff"/> object using the messages received since
        ///     the last time this was called.
        /// </summary>
        /// <param name="viewDiff">The ViewDiff to populate.</param>
        void GetMessagesReceived(ref ViewDiff viewDiff);

        /// <summary>
        ///     Gets the current messages to send container.
        /// </summary>
        /// <returns>The messages to send container.</returns>
        MessagesToSend GetMessagesToSendContainer();

        /// <summary>
        ///     Adds a set of <see cref="MessagesToSend"/> to be sent.
        /// </summary>
        /// <remarks>
        ///     The messages may not be sent immediately. This is up to the implementer.
        /// </remarks>
        /// <param name="messages">The set of messages to send.</param>
        void PushMessagesToSend(MessagesToSend messages, NetFrameStats frameStats);

        /// <summary>
        ///     Gets a value indicating whether the underlying connection is connected.
        /// </summary>
        /// <returns>True if the underlying connection is connected, false otherwise.</returns>
        bool IsConnected();
    }
}

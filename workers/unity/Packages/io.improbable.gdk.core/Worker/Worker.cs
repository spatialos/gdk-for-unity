using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Gdk.Core.NetworkStats;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Represents a SpatialOS worker.
    /// </summary>
    public class Worker : IDisposable
    {
        /// <summary>
        ///     The type of the worker.
        /// </summary>
        public readonly string WorkerType;

        /// <summary>
        ///     The worker ID.
        /// </summary>
        /// <remarks>
        ///     Unique for a given SpatialOS deployment.
        /// </remarks>
        public readonly string WorkerId;

        /// <summary>
        ///     The worker attribute list
        /// </summary>
        public readonly List<string> Attributes;

        /// <summary>
        ///     The logger for this worker.
        /// </summary>
        public ILogDispatcher LogDispatcher;

        /// <summary>
        ///     Denotes whether this worker is connected or not.
        /// </summary>
        public bool IsConnected => ConnectionHandler.IsConnected();

        protected IConnectionHandler ConnectionHandler;

        // todo replace internal with real apis
        internal ViewDiff ViewDiff;

        internal MessagesToSend MessagesToSend;

        protected Worker(IConnectionHandler connectionHandler, string workerType, ILogDispatcher logDispatcher)
        {
            ConnectionHandler = connectionHandler;
            WorkerType = workerType;
            WorkerId = connectionHandler.GetWorkerId();
            Attributes = connectionHandler.GetWorkerAttributes();

            LogDispatcher = logDispatcher;
            logDispatcher.Worker = this;
            logDispatcher.WorkerType = workerType;

            MessagesToSend = connectionHandler.GetMessagesToSendContainer();
        }

        /// <summary>
        ///     Creates a <see cref="Worker"/> object asynchronously.
        /// </summary>
        /// <param name="connectionHandlerBuilder">
        ///     A builder which describes how to create the <see cref="IConnectionHandler"/> for this worker.
        /// </param>
        /// <param name="workerType">The type of worker to connect as.</param>
        /// <param name="logDispatcher">The logger to use for this worker.</param>
        /// <param name="token">A cancellation token which will cancel this asynchronous operation</param>
        /// <returns>A task which represents the asynchronous creation of a worker.</returns>
        public static async Task<Worker> CreateWorkerAsync(IConnectionHandlerBuilder connectionHandlerBuilder, string workerType,
            ILogDispatcher logDispatcher, CancellationToken? token = null)
        {
            var handler = await connectionHandlerBuilder.CreateAsync(token);
            return new Worker(handler, workerType, logDispatcher);
        }

        /// <summary>
        ///     Ticks the worker. Fetches all messages received since the last <see cref="Tick"/> call and
        ///     applies the diff.
        /// </summary>
        public void Tick()
        {
            ConnectionHandler.GetMessagesReceived(ref ViewDiff);
        }

        public void EnsureMessagesFlushed(NetFrameStats frameStats)
        {
            ConnectionHandler.PushMessagesToSend(MessagesToSend, frameStats);
            MessagesToSend = ConnectionHandler.GetMessagesToSendContainer();
        }


        /// <summary>
        ///     Sends a log message to SpatialOS from this worker.
        /// </summary>
        /// <param name="logLevel">The log verbosity level.</param>
        /// <param name="message">The log message.</param>
        /// <param name="loggerName">A name for the sender of the log.</param>
        /// <param name="entityId">
        ///     The <see cref="EntityId"/> to associate with the log message.
        ///     Set to null for no <see cref="EntityId"/>.
        /// </param>
        public void SendLogMessage(LogLevel logLevel, string message, string loggerName, EntityId? entityId)
        {
            MessagesToSend.AddLogMessage(new LogMessageToSend(message, loggerName, logLevel, entityId?.Id));
        }

        public virtual void Dispose()
        {
            ConnectionHandler?.Dispose();
            ConnectionHandler = null;
            LogDispatcher?.Dispose();
            LogDispatcher = null;
        }
    }
}

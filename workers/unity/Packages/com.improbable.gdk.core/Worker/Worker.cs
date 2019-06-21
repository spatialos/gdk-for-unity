using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        protected readonly IConnectionHandler ConnectionHandler;

        // todo replace internal with real apis
        internal readonly View View;
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
            View = new View();
        }

        public static async Task<Worker> CreateWorkerAsync(IConnectionHandlerBuilder connectionHandlerBuilder, string workerType,
            ILogDispatcher logDispatcher, CancellationToken? token = null)
        {
            var handler = await connectionHandlerBuilder.CreateAsync(token);
            return new Worker(handler, workerType, logDispatcher);
        }

        public void Advance()
        {
            ConnectionHandler.GetMessagesReceived(ref ViewDiff);
            View.ApplyDiff(ViewDiff);
        }

        public void EnsureMessagesFlushed()
        {
            ConnectionHandler.PushMessagesToSend(MessagesToSend);
            MessagesToSend = ConnectionHandler.GetMessagesToSendContainer();
        }

        public void SendLogMessage(LogLevel logLevel, string message, string loggerName, EntityId? entityId)
        {
            MessagesToSend.AddLogMessage(new LogMessageToSend(message, loggerName, logLevel, entityId?.Id));
        }

        public virtual void Dispose()
        {
            ConnectionHandler?.Dispose();
            LogDispatcher?.Dispose();
            LogDispatcher = null;
        }
    }
}

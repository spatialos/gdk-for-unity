using System;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class SpatialOSConnectionHandlerBuilder : IConnectionHandlerBuilder
    {
        /// <summary>
        ///     Denotes the threading mode for the <see cref="IConnectionHandler"/>.
        /// </summary>
        public enum ThreadingMode
        {
            /// <summary>
            ///     The <see cref="IConnectionHandler"/> is single-threaded.
            /// </summary>
            SingleThreaded,

            /// <summary>
            ///     The <see cref="IConnectionHandler"/> is multi-threaded.
            /// </summary>
            MultiThreaded
        }

        public string WorkerType { get; private set; } = string.Empty;

        private ConnectionParameters connectionParameters;
        private IConnectionFlow flow;

        private ThreadingMode threadingMode = ThreadingMode.SingleThreaded;

        /// <summary>
        ///     Sets the connection flow implementation to use when creating the underlying SpatialOS connection.
        /// </summary>
        /// <param name="flow">The connection flow implementation.</param>
        /// <returns>Itself</returns>
        public SpatialOSConnectionHandlerBuilder SetConnectionFlow(IConnectionFlow flow)
        {
            this.flow = flow;
            return this;
        }

        /// <summary>
        ///     Sets the threading mode for the resultant <see cref="IConnectionHandler"/>.
        /// </summary>
        /// <param name="mode">The desired threading mode.</param>
        /// <returns>Itself</returns>
        public SpatialOSConnectionHandlerBuilder SetThreadingMode(ThreadingMode mode)
        {
            threadingMode = mode;
            return this;
        }

        /// <summary>
        ///     Sets the connection parameters to use for the underlying SpatialOS connection.
        /// </summary>
        /// <param name="parameters">The connection parameters to use.</param>
        /// <returns>Itself</returns>
        public SpatialOSConnectionHandlerBuilder SetConnectionParameters(ConnectionParameters parameters)
        {
            connectionParameters = parameters;
            WorkerType = connectionParameters.WorkerType;
            return this;
        }

        public async Task<IConnectionHandler> CreateAsync(CancellationToken? token = null)
        {
            if (connectionParameters == null)
            {
                throw new InvalidOperationException("ConnectionParameters must be provided");
            }

            if (flow == null)
            {
                throw new InvalidOperationException("A connection flow implementation must be provided.");
            }

            var connection = await flow.CreateAsync(connectionParameters);

            switch (threadingMode)
            {
                case ThreadingMode.SingleThreaded:
                    return new SpatialOSConnectionHandler(connection);
                case ThreadingMode.MultiThreaded:
                    return new MultiThreadedSpatialOSConnectionHandler(connection);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

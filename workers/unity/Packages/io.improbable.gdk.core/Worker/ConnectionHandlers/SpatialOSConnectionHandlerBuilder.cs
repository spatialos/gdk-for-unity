using System;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class SpatialOSConnectionHandlerBuilder : IConnectionHandlerBuilder
    {
        public string WorkerType { get; private set; } = string.Empty;

        private ConnectionParameters connectionParameters;
        private IConnectionFlow flow;

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

            return new SpatialOSConnectionHandler(connection);
        }
    }
}

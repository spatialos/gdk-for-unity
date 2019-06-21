using System;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class SpatialOSConnectionHandlerBuilder : IConnectionHandlerBuilder
    {
        public enum ThreadingMode
        {
            SingleThreaded,
            MultiThreaded
        }

        public string WorkerType => connectionParameters?.WorkerType ?? string.Empty;

        private ConnectionParameters connectionParameters;
        private IConnectionFlow flow;

        private ThreadingMode threadingMode = ThreadingMode.SingleThreaded;

        public SpatialOSConnectionHandlerBuilder SetConnectionFlow(IConnectionFlow flow)
        {
            this.flow = flow;
            return this;
        }

        public SpatialOSConnectionHandlerBuilder SetThreadingMode(ThreadingMode mode)
        {
            threadingMode = mode;
            return this;
        }

        public SpatialOSConnectionHandlerBuilder SetConnectionParameters(ConnectionParameters parameters)
        {
            connectionParameters = parameters;
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
                throw new InvalidOperationException("An executor implementation must be provided.");
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

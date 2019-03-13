using System;
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

        private ReceptionistConfig? receptionistConfig;
        private LocatorConfig? locatorConfig;
        private AlphaLocatorConfig? alphaLocatorConfig;

        private ConnectionParameters connectionParameters;

        private ThreadingMode threadingMode = ThreadingMode.SingleThreaded;

        public SpatialOSConnectionHandlerBuilder SetConfig(ReceptionistConfig config)
        {
            CheckExistingConfigAndThrow();
            receptionistConfig = config;
            return this;
        }

        public SpatialOSConnectionHandlerBuilder SetConfig(LocatorConfig config)
        {
            CheckExistingConfigAndThrow();
            locatorConfig = config;
            return this;
        }

        public SpatialOSConnectionHandlerBuilder SetConfig(AlphaLocatorConfig config)
        {
            CheckExistingConfigAndThrow();
            alphaLocatorConfig = config;
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

        public async Task<IConnectionHandler> CreateAsync()
        {
            if (!(receptionistConfig.HasValue || locatorConfig.HasValue || alphaLocatorConfig.HasValue))
            {
                throw new InvalidOperationException("Must set exactly one of ReceptionistConfig, LocatorConfig, or AlphaLocatorConfig");
            }

            if (connectionParameters == null)
            {
                throw new InvalidOperationException("ConnectionParameters must be provided");
            }

            Connection connection;
            if (receptionistConfig.HasValue)
            {
                connection = await ConnectionUtils.CreateConnectionAsync(receptionistConfig.Value, connectionParameters);
            }
            else if (locatorConfig.HasValue)
            {
                connection = await ConnectionUtils.CreateConnectionAsync(locatorConfig.Value, connectionParameters);
            }
            else
            {
                connection = await ConnectionUtils.CreateConnectionAsync(alphaLocatorConfig.Value, connectionParameters);
            }

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

        private void CheckExistingConfigAndThrow()
        {
            if (receptionistConfig.HasValue || locatorConfig.HasValue || alphaLocatorConfig.HasValue)
            {
                throw new InvalidOperationException("Must set exactly one of ReceptionistConfig, LocatorConfig, or AlphaLocatorConfig");
            }
        }
    }
}

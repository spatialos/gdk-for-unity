using System.Collections.Generic;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Represents an object which can initialize the connection parameters.
    /// </summary>
    public interface IConnectionParameterInitializer
    {
        /// <summary>
        ///     Initializes the connection parameters.
        /// </summary>
        /// <param name="connectionParameters">The <see cref="ConnectionParameters"/> object to initialize.</param>
        void Initialize(ConnectionParameters connectionParameters);
    }

    public class CommandLineConnectionParameterInitializer : IConnectionParameterInitializer
    {
        private readonly CommandLineArgs commandLineArgs;

        public CommandLineConnectionParameterInitializer()
        {
            commandLineArgs = CommandLineArgs.FromCommandLine();
        }

        internal CommandLineConnectionParameterInitializer(Dictionary<string, string> args)
        {
            commandLineArgs = CommandLineArgs.From(args);
        }

        public void Initialize(ConnectionParameters connectionParameters)
        {
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.LinkProtocol,
                ref connectionParameters.Network.ConnectionType);
            connectionParameters.Network.UseExternalIp = commandLineArgs.Contains(RuntimeConfigNames.UseExternalIp);
        }
    }

    public class ConnectionParameterInitializer : IConnectionParameterInitializer
    {
        private readonly CommandLineConnectionParameterInitializer clInitializer;
        private readonly ProtocolLogController protocolLogController;
        private readonly string workerId;

        public ConnectionParameterInitializer(string workerId, ProtocolLogController protocolLogController, CommandLineConnectionParameterInitializer clInitializer = null)
        {
            this.clInitializer = clInitializer;
            this.protocolLogController = protocolLogController;
            this.workerId = workerId;
        }

        public void Initialize(ConnectionParameters connectionParameters)
        {
            clInitializer?.Initialize(connectionParameters);
            if (protocolLogController != null)
            {
                connectionParameters.Logsinks.Add(protocolLogController.GetLogsinkParameters(workerId));
            }
        }
    }
}

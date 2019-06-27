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

        public void Initialize(ConnectionParameters connectionParameters)
        {
            commandLineArgs.TryGetCommandLineValue(RuntimeConfigNames.LinkProtocol,
                ref connectionParameters.Network.ConnectionType);
        }
    }
}

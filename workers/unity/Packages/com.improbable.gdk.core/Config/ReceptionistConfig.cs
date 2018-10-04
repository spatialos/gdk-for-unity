using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     An implementation of <see cref="ConnectionConfig"/> which is used when connecting to the SpatialOS runtime
    ///     through the Receptionist.
    /// </summary>
    /// <remarks>
    ///     This is most often used when connecting workers to a local deployment or connecting server-workers to a cloud
    ///     deployment.
    /// </remarks>
    public class ReceptionistConfig : ConnectionConfig
    {
        /// <summary>
        ///     The IP address for connecting to the Receptionist.
        /// </summary>
        /// <remarks>
        ///     Default is "127.0.0.1".
        /// </remarks>
        public string ReceptionistHost = RuntimeConfigDefaults.ReceptionistHost;

        /// <summary>
        ///     The port for connecting to the Receptionist.
        /// </summary>
        /// <remarks>
        ///     Default is 7777.
        /// </remarks>
        public ushort ReceptionistPort = RuntimeConfigDefaults.ReceptionistPort;

        /// <summary>
        ///     Checks that the ReceptionistConfig instance is valid. This does not guarantee a successful connection.
        /// </summary>
        public override void Validate()
        {
            ValidateConfig(ReceptionistHost, RuntimeConfigNames.ReceptionistHost);
        }

        /// <summary>
        ///     Creates a <see cref="ReceptionistConfig"/> instance from a set of command line arguments.
        /// </summary>
        /// <param name="parsedArgs">A dictionary of command line argument to command line value.</param>
        /// <returns>A <see cref="ReceptionistConfig"/> instance.</returns>
        public static ReceptionistConfig CreateConnectionConfigFromCommandLine(Dictionary<string, string> parsedArgs)
        {
            var config = new ReceptionistConfig
            {
                ReceptionistHost = CommandLineUtility.GetCommandLineValue(
                    parsedArgs, RuntimeConfigNames.ReceptionistHost, RuntimeConfigDefaults.ReceptionistHost),
                ReceptionistPort = CommandLineUtility.GetCommandLineValue(
                    parsedArgs, RuntimeConfigNames.ReceptionistPort, RuntimeConfigDefaults.ReceptionistPort),
                LinkProtocol = CommandLineUtility.GetCommandLineValue(
                    parsedArgs, RuntimeConfigNames.LinkProtocol, RuntimeConfigDefaults.LinkProtocol),
                WorkerId = CommandLineUtility.GetCommandLineValue(
                    parsedArgs, RuntimeConfigNames.WorkerId, string.Empty),
                WorkerType = CommandLineUtility.GetCommandLineValue(
                    parsedArgs, RuntimeConfigNames.WorkerType, string.Empty)
            };
            return config;
        }
    }
}

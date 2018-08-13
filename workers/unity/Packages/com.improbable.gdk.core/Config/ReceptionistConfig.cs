using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public class ReceptionistConfig : ConnectionConfig
    {
        public string ReceptionistHost = RuntimeConfigDefaults.ReceptionistHost;
        public ushort ReceptionistPort = RuntimeConfigDefaults.ReceptionistPort;

        public override void Validate()
        {
            ValidateConfig(ReceptionistHost, RuntimeConfigNames.ReceptionistHost);
        }

        public static ReceptionistConfig CreateConnectionConfigFromCommandLine(Dictionary<string, string> parsedArgs)
        {
            var config = new ReceptionistConfig
            {
                ReceptionistHost = CommandLineUtility.GetCommandLineValue(
                    parsedArgs, RuntimeConfigNames.ReceptionistHost, RuntimeConfigDefaults.ReceptionistHost),
                ReceptionistPort = CommandLineUtility.GetCommandLineValue(
                    parsedArgs, RuntimeConfigNames.ReceptionistPort, RuntimeConfigDefaults.ReceptionistPort),
                LinkProtocol = CommandLineUtility.GetCommandLineValue(
                    parsedArgs, RuntimeConfigNames.LinkProtocol, RuntimeConfigDefaults.LinkProtocol)
            };
            return config;
        }
    }
}

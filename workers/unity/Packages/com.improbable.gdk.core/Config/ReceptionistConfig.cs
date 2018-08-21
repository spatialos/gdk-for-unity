using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public class ReceptionistConfig : ConnectionConfig
    {
        public string ReceptionistHost = RuntimeConfigDefaults.ReceptionistHost;
        public ushort ReceptionistPort = RuntimeConfigDefaults.ReceptionistPort;

        // Default Android emulator host IP alias
        private const string AndroidEmulatorHostDeviceIp = "10.0.2.2";

        public override void Validate()
        {
            ValidateConfig(ReceptionistHost, RuntimeConfigNames.ReceptionistHost);
        }

        public static ReceptionistConfig CreateConnectionConfigFromCommandLine(Dictionary<string, string> parsedArgs)
        {
            var config = new ReceptionistConfig();
            config.ReceptionistHost = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.ReceptionistHost, RuntimeConfigDefaults.ReceptionistHost);
            config.ReceptionistPort = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.ReceptionistPort, RuntimeConfigDefaults.ReceptionistPort);
            config.LinkProtocol = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.LinkProtocol, RuntimeConfigDefaults.LinkProtocol);
            config.WorkerId = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.WorkerId, string.Empty);
            config.WorkerType = CommandLineUtility.GetCommandLineValue(
                parsedArgs, RuntimeConfigNames.WorkerType, string.Empty);
            return config;
        }

        public static ReceptionistConfig CreateConnectionConfigForAndroidEmulator()
        {
            return new ReceptionistConfig
            {
                ReceptionistHost = AndroidEmulatorHostDeviceIp,
                UseExternalIp = true
            };
        }

        public static ReceptionistConfig CreateConnectionConfigForPhysicalAndroid(string ip)
        {
            return new ReceptionistConfig
            {
                ReceptionistHost = ip,
                UseExternalIp = true
            };
        }
    }
}

using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public class MobileConnectionFlowInitializer : IConnectionFlowInitializer<ReceptionistFlow>,
        IConnectionFlowInitializer<AlphaLocatorFlow>
    {
        private readonly IMobileSettingsProvider[] settingsProviders;

        public MobileConnectionFlowInitializer(params IMobileSettingsProvider[] settingsProviders)
        {
            this.settingsProviders = settingsProviders;
        }

        public ConnectionService GetConnectionService()
        {
            return settingsProviders
                .Select(provider => provider.GetConnectionService())
                .FilterOption()
                .FirstOrDefault();
        }

        public void Initialize(ReceptionistFlow receptionist)
        {
#if UNITY_ANDROID
            // Override for Android emulator.

            if (Application.isMobilePlatform && DeviceInfo.ActiveDeviceType == MobileDeviceType.Virtual)
            {
                receptionist.ReceptionistHost = DeviceInfo.AndroidEmulatorDefaultCallbackIp;
                return;
            }
#endif
            receptionist.ReceptionistHost = settingsProviders
                .Select(provider => provider.GetReceptionistHostIp())
                .FilterOption()
                .FirstOrDefault() ?? RuntimeConfigDefaults.ReceptionistHost;
        }

        public void Initialize(AlphaLocatorFlow alphaLocator)
        {
            alphaLocator.DevAuthToken = settingsProviders
                .Select(provider => provider.GetDevAuthToken())
                .FilterOption()
                .FirstOrDefault() ?? string.Empty;
        }

        public interface IMobileSettingsProvider
        {
            Option<string> GetReceptionistHostIp();
            Option<string> GetDevAuthToken();
            Option<ConnectionService> GetConnectionService();
        }

        public class CommandLineSettingsProvider : IMobileSettingsProvider
        {
            public Option<string> GetReceptionistHostIp()
            {
                var args = LaunchArguments.GetArguments();

                var hostIp = string.Empty;

                if (args.TryGetCommandLineValue(RuntimeConfigNames.ReceptionistHost, ref hostIp))
                {
                    PlayerPrefs.SetString(RuntimeConfigNames.ReceptionistHost, hostIp);
                    PlayerPrefs.Save();
                    return hostIp;
                }

                return Option<string>.Empty;
            }

            public Option<string> GetDevAuthToken()
            {
                var args = LaunchArguments.GetArguments();

                var devAuthToken = string.Empty;

                if (args.TryGetCommandLineValue(RuntimeConfigNames.DevAuthTokenKey, ref devAuthToken))
                {
                    PlayerPrefs.SetString(RuntimeConfigNames.DevAuthTokenKey, devAuthToken);
                    PlayerPrefs.Save();
                    return devAuthToken;
                }

                return Option<string>.Empty;
            }

            public Option<ConnectionService> GetConnectionService()
            {
                var args = LaunchArguments.GetArguments();

                var environment = string.Empty;

                if (args.TryGetCommandLineValue(RuntimeConfigNames.Environment, ref environment))
                {
                    PlayerPrefs.SetString(RuntimeConfigNames.Environment, environment);
                    PlayerPrefs.Save();

                    return environment == RuntimeConfigDefaults.LocalEnvironment
                        ? ConnectionService.Receptionist
                        : ConnectionService.AlphaLocator;
                }

                return Option<ConnectionService>.Empty;
            }
        }

        public class PlayerPrefsSettingsProvider : IMobileSettingsProvider
        {
            public Option<string> GetReceptionistHostIp()
            {
                return PlayerPrefs.HasKey(RuntimeConfigNames.ReceptionistHost)
                    ? new Option<string>(PlayerPrefs.GetString(RuntimeConfigNames.ReceptionistHost, string.Empty))
                    : Option<string>.Empty;
            }

            public Option<string> GetDevAuthToken()
            {
                return PlayerPrefs.HasKey(RuntimeConfigNames.DevAuthTokenKey)
                    ? new Option<string>(PlayerPrefs.GetString(RuntimeConfigNames.DevAuthTokenKey, string.Empty))
                    : Option<string>.Empty;
            }

            public Option<ConnectionService> GetConnectionService()
            {
                if (!PlayerPrefs.HasKey(RuntimeConfigNames.Environment))
                {
                    return Option<ConnectionService>.Empty;
                }

                var environment = PlayerPrefs.GetString(RuntimeConfigNames.Environment, string.Empty);

                return environment == RuntimeConfigDefaults.LocalEnvironment
                    ? ConnectionService.Receptionist
                    : ConnectionService.AlphaLocator;
            }
        }
    }
}

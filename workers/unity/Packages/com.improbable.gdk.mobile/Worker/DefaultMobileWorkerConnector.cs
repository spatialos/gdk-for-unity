using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public abstract class DefaultMobileWorkerConnector : WorkerConnector
    {
        private const string HostIpPlayerPrefsKey = "SpatialOSHostIp";

#pragma warning disable 649
        [SerializeField] private string IpAddress;
        [SerializeField] private bool ShouldConnectLocally;
#pragma warning restore 649

        private void Awake()
        {
            InitializeClient();
        }

        protected virtual void InitializeClient()
        {
            var arguments = LaunchArguments.GetArguments();
            var environment = CommandLineUtility.GetCommandLineValue(arguments, RuntimeConfigNames.Environment, string.Empty);
            if (string.IsNullOrEmpty(environment))
            {
                environment = PlayerPrefs.GetString(RuntimeConfigNames.Environment, string.Empty);
            }
            else
            {
                PlayerPrefs.SetString(RuntimeConfigNames.Environment, environment);
            }

            ShouldConnectLocally = environment == "local";
            if (ShouldConnectLocally)
            {
                IpAddress = GetHostIp();
                PlayerPrefs.SetString(HostIpPlayerPrefsKey, IpAddress);
            }
            else
            {
                PlayerPrefs.DeleteKey(HostIpPlayerPrefsKey);
            }

            PlayerPrefs.Save();
        }

        protected virtual string GetHostIp()
        {
            var arguments = LaunchArguments.GetArguments();
            var hostIp =
                CommandLineUtility.GetCommandLineValue(arguments, RuntimeConfigNames.ReceptionistHost, string.Empty);
            if (!string.IsNullOrEmpty(hostIp))
            {
                return hostIp;
            }

            if (Application.isMobilePlatform)
            {
                switch (DeviceInfo.ActiveDeviceType)
                {
                    case MobileDeviceType.Virtual:
                        return DeviceInfo.AndroidEmulatorDefaultCallbackIp;
                    case MobileDeviceType.Physical:
                        return PlayerPrefs.GetString(HostIpPlayerPrefsKey);
                }
            }

            if (!string.IsNullOrEmpty(IpAddress))
            {
                return IpAddress;
            }

            return RuntimeConfigDefaults.ReceptionistHost;
        }

        protected override ConnectionService GetConnectionService()
        {
            return ShouldConnectLocally ? ConnectionService.Receptionist : ConnectionService.AlphaLocator;
        }

        protected override ConnectionParameters GetConnectionParameters(string workerType, ConnectionService service)
        {
            return new ConnectionParameters
            {
                WorkerType = workerType,
                Network =
                {
                    ConnectionType = NetworkConnectionType.Kcp,
                    UseExternalIp = true,
                    Kcp = new KcpNetworkParameters
                    {
                        Heartbeat = new HeartbeatParameters()
                        {
                            IntervalMillis = 5000,
                            TimeoutMillis = 10000
                        }
                    }
                },
                EnableProtocolLoggingAtStartup = false,
                DefaultComponentVtable = new ComponentVtable(),
            };
        }

        protected override ReceptionistConfig GetReceptionistConfig(string workerType)
        {
            return new ReceptionistConfig
            {
                ReceptionistHost = GetHostIp(),
                ReceptionistPort = RuntimeConfigDefaults.ReceptionistPort,
                WorkerId = CreateNewWorkerId(workerType)
            };
        }

        protected override LocatorConfig GetLocatorConfig()
        {
            throw new NotImplementedException("The locator flow is not available for mobile workers.");
        }

        protected override AlphaLocatorConfig GetAlphaLocatorConfig(string workerType)
        {
            return GetAlphaLocatorConfigViaDevAuthFlow(workerType);
        }
    }
}

using System;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public enum DeviceType
    {
        AndroidDevice,
        AndroidEmulator,
        iOSDevice,
        iOSSimulator
    }

    public struct DeviceLaunchConfig
    {
        public readonly string deviceName;
        public readonly string deviceId;
        public readonly string prettyDeviceType;
        public readonly DeviceType deviceType;

        private readonly Action<DeviceLaunchConfig, MobileLaunchConfig> launchAction;

        public DeviceLaunchConfig(string deviceName, string deviceId, DeviceType deviceType, Action<DeviceLaunchConfig, MobileLaunchConfig> launchAction)
        {
            this.deviceName = deviceName;
            this.deviceId = deviceId;
            this.deviceType = deviceType;
            this.launchAction = launchAction;
            prettyDeviceType = GetPrettyDeviceType(deviceType);
        }

        public static string GetPrettyDeviceType(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.AndroidDevice:
                    return "Android Device";
                case DeviceType.AndroidEmulator:
                    return "Android Emulator";
                case DeviceType.iOSDevice:
                    return "iOS Device";
                case DeviceType.iOSSimulator:
                    return "iOS Simulator";
                default:
                    throw new ArgumentException($"Invalid Device Type: {deviceType}");
            }
        }

        public void Launch(MobileLaunchConfig mobileLaunchConfig) => launchAction(this, mobileLaunchConfig);
    }
}

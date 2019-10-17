using System;

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
        public readonly string DeviceName;
        public readonly string DeviceId;
        public readonly string PrettyDeviceType;
        public readonly DeviceType DeviceType;

        private readonly Action<DeviceLaunchConfig, MobileLaunchConfig> launchAction;

        public DeviceLaunchConfig(string deviceName, string deviceId, DeviceType deviceType, Action<DeviceLaunchConfig, MobileLaunchConfig> launchAction)
        {
            this.DeviceName = deviceName;
            this.DeviceId = deviceId;
            this.DeviceType = deviceType;
            this.launchAction = launchAction;
            PrettyDeviceType = deviceType.ToPrettyDeviceType();
        }

        public void Launch(MobileLaunchConfig mobileLaunchConfig) => launchAction(this, mobileLaunchConfig);
    }
}

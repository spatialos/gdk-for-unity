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

        public readonly bool IsAndroid;

        private readonly Action<DeviceLaunchConfig, MobileLaunchConfig> launchAction;

        public DeviceLaunchConfig(string deviceName, string deviceId, DeviceType deviceType, Action<DeviceLaunchConfig, MobileLaunchConfig> launchAction)
        {
            this.DeviceName = deviceName;
            this.DeviceId = deviceId;
            this.PrettyDeviceType = deviceType.ToPrettyDeviceType();
            this.DeviceType = deviceType;
            this.IsAndroid = deviceType == DeviceType.AndroidDevice || deviceType == DeviceType.AndroidEmulator;
            this.launchAction = launchAction;
        }

        public void Launch(MobileLaunchConfig mobileLaunchConfig) => launchAction(this, mobileLaunchConfig);
    }
}

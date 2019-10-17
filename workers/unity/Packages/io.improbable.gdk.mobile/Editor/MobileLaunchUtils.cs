using System;

namespace Improbable.Gdk.Mobile
{
    public static class MobileLaunchUtils
    {
        public static string ToPrettyDeviceType(this DeviceType deviceType)
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
                    throw new ArgumentException($"Unknown Device Type: {deviceType}");
            }
        }
    }
}

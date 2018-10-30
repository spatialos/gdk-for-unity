using UnityEngine;

namespace Improbable.Gdk.Mobile.Android
{
    public static class AndroidDeviceInfo
    {
        public const string EmulatorDefaultCallbackIp = "10.0.2.2";

        private static MobileDeviceType activeDeviceType = MobileDeviceType.Unknown;

        public static MobileDeviceType ActiveDeviceType
        {
            get
            {
                if (activeDeviceType == MobileDeviceType.Unknown)
                {
                    using (var build = new AndroidJavaObject("android.os.Build"))
                    {
                        var fingerprint = build.GetStatic<string>("FINGERPRINT");
                        var model = build.GetStatic<string>("MODEL");
                        var brand = build.GetStatic<string>("BRAND");
                        var device = build.GetStatic<string>("DEVICE");
                        var product = build.GetStatic<string>("PRODUCT");

                        // How to detect an Android emulator by Stackoverflow:
                        // https://stackoverflow.com/questions/2799097/how-can-i-detect-when-an-android-application-is-running-in-the-emulator/21505193#21505193
                        activeDeviceType = (fingerprint.StartsWith("generic") || fingerprint.StartsWith("unknown") ||
                            model.Contains("google_sdk") || model.Contains("Emulator") ||
                            model.Contains("Android SDK built for x86") ||
                            brand.StartsWith("generic") && device.StartsWith("generic") ||
                            product.Equals("google_sdk"))
                            ? MobileDeviceType.Virtual
                            : MobileDeviceType.Physical;
                    }
                }

                return activeDeviceType;
            }
        }
    }
}

using UnityEngine;

namespace Improbable.Gdk.Mobile.Android
{
    public class DeviceInfo
    {
        public const string AndroidStudioEmulatorDefaultCallbackIp = "10.0.2.2";

        private static readonly AndroidJavaObject Build = new AndroidJavaObject("android.os.Build");
        private static readonly string Fingerprint = GetValue("FINGERPRINT");
        private static readonly string Model = GetValue("MODEL");
        private static readonly string Brand = GetValue("BRAND");
        private static readonly string Device = GetValue("DEVICE");
        private static readonly string Product = GetValue("PRODUCT");

        public static bool IsAndroidStudioEmulator()
        {
            return Fingerprint.StartsWith("generic") || Fingerprint.StartsWith("unknown") ||
                Model.Contains("google_sdk") || Model.Contains("Emulator") ||
                Model.Contains("Android SDK built for x86") ||
                Brand.StartsWith("generic") && Device.StartsWith("generic") ||
                Product.Equals("google_sdk");
        }

        private static string GetValue(string parameter)
        {
            return Build.GetStatic<string>(parameter);
        }
    }
}

using UnityEngine;

namespace Improbable.Gdk.Mobile.Android
{
    public static class AndroidDeviceInfo
    {
        public const string AndroidStudioEmulatorDefaultCallbackIp = "10.0.2.2";

        private static bool isInitialised = false;
        private static bool isEmulator = false;

        public static bool IsAndroidStudioEmulator()
        {
            if (!isInitialised)
            {
                AndroidJavaObject build = new AndroidJavaObject("android.os.Build");
                string fingerprint = GetValue(build, "FINGERPRINT");
                string model = GetValue(build, "MODEL");
                string brand = GetValue(build, "BRAND");
                string device = GetValue(build, "DEVICE");
                string product = GetValue(build, "PRODUCT");

                isEmulator = fingerprint.StartsWith("generic") || fingerprint.StartsWith("unknown") ||
                    model.Contains("google_sdk") || model.Contains("Emulator") ||
                    model.Contains("Android SDK built for x86") ||
                    brand.StartsWith("generic") && device.StartsWith("generic") ||
                    product.Equals("google_sdk");
                isInitialised = true;
            }

            return isEmulator;
        }

        private static string GetValue(AndroidJavaObject build, string parameter)
        {
            return build.GetStatic<string>(parameter);
        }
    }
}

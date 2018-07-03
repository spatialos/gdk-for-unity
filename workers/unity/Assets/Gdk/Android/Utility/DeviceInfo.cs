using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Improbable.Gdk.Android
{
    public class DeviceInfo
    {
        private static readonly AndroidJavaObject Build = new AndroidJavaObject("android.os.Build");
        private static readonly string Fingerprint = GetValue("FINGERPRINT");
        private static readonly string Model = GetValue("MODEL");
        private static readonly string Manufacturer = GetValue("MANUFACTURER");
        private static readonly string Brand = GetValue("BRAND");
        private static readonly string Device = GetValue("DEVICE");
        private static readonly string Product = GetValue("PRODUCT");

        public static bool isEmulator()
        {
            return Fingerprint.StartsWith("generic") || Fingerprint.StartsWith("unknown") ||
                Model.Contains("google_sdk") || Model.Contains("Emulator") ||
                Model.Contains("Android SDK built for x86") || Manufacturer.Contains("Genymotion") ||
                (Brand.StartsWith("generic") && Device.StartsWith("generic")) ||
                "google_sdk".Equals(Product);
        }

        private static string GetValue(string parameter)
        {
            return Build.GetStatic<string>(parameter);
        }
    }
}

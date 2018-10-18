using UnityEngine;

namespace Improbable.Gdk.Mobile.iOS
{
    public static class iOSDeviceInfo
    {
        public static bool IsSimulator()
        {
            return SystemInfo.deviceModel.Equals("x86_64");
        }
    }
}

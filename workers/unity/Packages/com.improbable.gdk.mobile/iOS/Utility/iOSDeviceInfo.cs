using UnityEngine;

namespace Improbable.Gdk.Mobile.Ios
{
    public static class iOSDeviceInfo
    {
        public static bool IsiOSSimulator()
        {
            return SystemInfo.deviceModel.Equals("x86_64");
        }
    }
}

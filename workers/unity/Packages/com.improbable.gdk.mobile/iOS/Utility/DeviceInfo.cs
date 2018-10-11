using UnityEngine;

namespace Improbable.Gdk.Mobile.Ios
{
    public class DeviceInfo
    {
        public static bool IsIosSimulator()
        {
            return SystemInfo.deviceModel.Equals("x86_64");
        }
    }
}

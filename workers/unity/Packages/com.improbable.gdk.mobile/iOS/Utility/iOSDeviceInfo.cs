using UnityEngine;

namespace Improbable.Gdk.Mobile.iOS
{
    public static class iOSDeviceInfo
    {
        private static MobileDeviceType activeDeviceType = MobileDeviceType.Unknown;

        public static MobileDeviceType ActiveDeviceType
        {
            get
            {
                if (activeDeviceType == MobileDeviceType.Unknown)
                {
                    activeDeviceType = SystemInfo.deviceModel.Equals("x86_64")
                        ? MobileDeviceType.Virtual
                        : MobileDeviceType.Physical;
                }

                return activeDeviceType;
            }
        }
    }
}

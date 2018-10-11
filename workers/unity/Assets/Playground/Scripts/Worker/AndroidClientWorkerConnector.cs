using System;
using UnityEngine;
#if UNITY_ANDROID
using Improbable.Gdk.Mobile.Android;
#endif

namespace Playground.Worker
{
    public class AndroidClientWorkerConnector : AbstractMobileClientWorker
    {
        protected override string GetHostIp()
        {
#if UNITY_ANDROID
            var hostIp = IpAddress;
            if (Application.isMobilePlatform && DeviceInfo.IsAndroidStudioEmulator() && hostIp.Equals(string.Empty))
            {
                return DeviceInfo.AndroidStudioEmulatorDefaultCallbackIp;
            }

            return hostIp;
#else
            throw new NotImplementedException("Incompatible platform: Please use Android");
#endif
        }
    }
}

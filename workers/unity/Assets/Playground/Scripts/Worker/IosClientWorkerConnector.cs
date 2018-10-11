using System;
using UnityEngine;
#if UNITY_IOS
using Improbable.Gdk.Mobile.Ios;
#endif
using Improbable.Gdk.Core;

namespace Playground.Worker
{
    public class IosClientWorkerConnector : AbstractMobileClientWorker
    {
        protected override string GetHostIp()
        {
#if UNITY_IOS
            var hostIp = GetIpFromField();
            if ((Application.isEditor || DeviceInfo.IsIosSimulator()) && hostIp.Equals(string.Empty))
            {
                return RuntimeConfigDefaults.ReceptionistHost;
            }

            return hostIp;
#else
            throw new NotImplementedException("Incompatible platform: Please use iOS");
#endif
        }
    }
}

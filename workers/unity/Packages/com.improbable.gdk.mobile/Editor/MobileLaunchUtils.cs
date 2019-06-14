using System.Text;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tools;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public class MobileLaunchUtils
    {
        private const string argStructure = "+{0} {1} ";

        public static bool TryPrepareArguments(bool shouldConnectLocally, string runtimeIp, out string builtArguments)
        {
            var arguments = new StringBuilder();
            if (shouldConnectLocally)
            {
                if (string.IsNullOrEmpty(runtimeIp))
                {
                    Debug.LogWarning("No local runtime IP was specified. Ensure you set one in SpatialOS > GDK tools configuration.");
                }

                arguments.AppendFormat(argStructure, RuntimeConfigNames.Environment, RuntimeConfigDefaults.LocalEnvironment);
                arguments.AppendFormat(argStructure, RuntimeConfigNames.ReceptionistHost, runtimeIp);
            }
            else
            {
                arguments.AppendFormat(argStructure, RuntimeConfigNames.Environment, RuntimeConfigDefaults.CloudEnvironment);

                // Return error if no DevAuthToken is set AND fails to generate new DevAuthToken.
                if (!PlayerPrefs.HasKey(RuntimeConfigNames.DevAuthTokenKey) && !DevAuthTokenUtils.TryGenerate())
                {
                    Debug.LogError("Failed to generate a Dev Auth Token to launch mobile client.");
                    builtArguments = string.Empty;
                    return false;
                }

                arguments.AppendFormat(argStructure, RuntimeConfigNames.DevAuthTokenKey, DevAuthTokenUtils.DevAuthToken);
            }

            builtArguments = arguments.ToString();
            return true;
        }
    }
}

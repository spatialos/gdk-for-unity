using System.Text;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tools;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public class MobileLaunchUtils
    {
        private const string argStructure = "+{0} {1} ";

        public static string PrepareArguments(bool shouldConnectLocally, string runtimeIp)
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
                    throw new PlayerPrefsException("Failed to retrieve a Dev Auth Token to launch a mobile client.");
                }

                arguments.AppendFormat(argStructure, RuntimeConfigNames.DevAuthTokenKey, DevAuthTokenUtils.DevAuthToken);
            }

            return arguments.ToString();
        }
    }
}

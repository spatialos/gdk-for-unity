using System.Text;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tools;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public class MobileLaunchHelper
    {
        public const string MenuLaunchMobile = "SpatialOS/Launch mobile client";
        private const string argStructure = "+{0} {1} ";

        public static bool TryPrepareArguments(bool shouldConnectLocally, out string builtArguments)
        {
            var gdkToolsConfig = GdkToolsConfiguration.GetOrCreateInstance();
            var arguments = new StringBuilder();
            if (shouldConnectLocally)
            {
                if (string.IsNullOrEmpty(gdkToolsConfig.RuntimeIp))
                {
                    Debug.LogWarning("No local runtime IP was specified. Ensure you set one in SpatialOS > GDK tools configuration.");
                }

                arguments.Append(string.Format(argStructure, RuntimeConfigNames.Environment, RuntimeConfigDefaults.LocalEnvironment));
                arguments.Append(string.Format(argStructure, RuntimeConfigNames.ReceptionistHost, gdkToolsConfig.RuntimeIp));
            }
            else
            {
                arguments.Append(string.Format(argStructure, RuntimeConfigNames.Environment, RuntimeConfigDefaults.CloudEnvironment));

                // Return error if no DevAuthToken is set AND fails to generate new DevAuthToken.
                if (!PlayerPrefs.HasKey(RuntimeConfigNames.DevAuthTokenKey) && !DevAuthTokenUtils.TryGenerate())
                {
                    Debug.LogError("Failed to generate a Dev Auth Token to launch mobile client.");
                    builtArguments = string.Empty;
                    return false;
                }

                arguments.Append(string.Format(argStructure, RuntimeConfigNames.DevAuthTokenKey, DevAuthTokenUtils.DevAuthToken));
                arguments.Append($"+{RuntimeConfigNames.DevAuthTokenKey} {DevAuthTokenUtils.DevAuthToken} ");
            }

            builtArguments = arguments.ToString();
            return true;
        }
    }
}

using System;
using System.Text;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    [Serializable]
    public struct MobileLaunchConfig
    {
        private const string DevelopmentTeamIdEditorPrefKey = "DevelopmentTeam";
        private const string RuntimeIpEditorPrefKey = "RuntimeIp";

        public bool ShouldConnectLocally;

        public string RuntimeIp
        {
            get => EditorPrefs.GetString(RuntimeIpEditorPrefKey);
            set => EditorPrefs.SetString(RuntimeIpEditorPrefKey, value);
        }

        public string DevelopmentTeamId
        {
            get => EditorPrefs.GetString(DevelopmentTeamIdEditorPrefKey);
            set => EditorPrefs.SetString(DevelopmentTeamIdEditorPrefKey, value);
        }

        private const string ArgStructure = "+{0} {1} ";

        public string ToLaunchArgs()
        {
            var arguments = new StringBuilder();
            if (ShouldConnectLocally)
            {
                if (string.IsNullOrEmpty(RuntimeIp))
                {
                    Debug.LogWarning("No local runtime IP was specified. Ensure you set one in SpatialOS > GDK tools configuration.");
                }

                arguments.AppendFormat(ArgStructure, RuntimeConfigNames.Environment, RuntimeConfigDefaults.LocalEnvironment);
                arguments.AppendFormat(ArgStructure, RuntimeConfigNames.ReceptionistHost, RuntimeIp);
            }
            else
            {
                arguments.AppendFormat(ArgStructure, RuntimeConfigNames.Environment, RuntimeConfigDefaults.CloudEnvironment);

                // Return error if no DevAuthToken is set AND fails to generate new DevAuthToken.
                if (!PlayerPrefs.HasKey(RuntimeConfigNames.DevAuthTokenKey) && !DevAuthTokenUtils.TryGenerate())
                {
                    throw new PlayerPrefsException("Failed to retrieve a Dev Auth Token to launch a mobile client.");
                }

                arguments.AppendFormat(ArgStructure, RuntimeConfigNames.DevAuthTokenKey, DevAuthTokenUtils.DevAuthToken);
            }

            return arguments.ToString();
        }
    }
}

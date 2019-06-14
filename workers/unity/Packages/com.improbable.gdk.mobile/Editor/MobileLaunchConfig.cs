using System.Collections.Generic;
using System.Net;
using UnityEditor;

namespace Improbable.Gdk.Mobile
{
    public class MobileLaunchConfig
    {
        public bool shouldConnectLocally;
        public string RuntimeIp => EditorPrefs.GetString(RuntimeIpEditorPrefKey);
        public string DevelopmentTeamId => EditorPrefs.GetString(DevelopmentTeamIdEditorPrefKey);
        
        internal Dictionary<string, string> availableSimulators = new Dictionary<string, string>();
        internal Dictionary<string, string> availableDevices = new Dictionary<string, string>();

        internal string DevelopmentTeamIdEditorPrefKey = "DevelopmentTeam";
        internal string RuntimeIpEditorPrefKey = "RuntimeIp";

        public MobileLaunchConfig()
        {
            availableSimulators = iOSLaunchUtils.RetrieveAvailableiOSSimulators();
            availableDevices = iOSLaunchUtils.RetrieveAvailableiOSDevices();
        }
    }
}

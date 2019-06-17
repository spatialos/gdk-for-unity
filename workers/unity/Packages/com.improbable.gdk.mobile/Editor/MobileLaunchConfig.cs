using System.Collections.Generic;
using System.Net;
using UnityEditor;

namespace Improbable.Gdk.Mobile
{
    public class MobileLaunchConfig
    {
        private const string DevelopmentTeamIdEditorPrefKey = "DevelopmentTeam";
        private const string RuntimeIpEditorPrefKey = "RuntimeIp";

        public bool shouldConnectLocally;

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
    }
}

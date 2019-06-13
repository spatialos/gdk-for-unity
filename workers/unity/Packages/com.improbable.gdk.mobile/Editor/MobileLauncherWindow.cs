using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public class MobileLauncherWindow : EditorWindow
    {
        private const string MobileSectionLabel = "Mobile Settings";
        private const string iOSSectionLabel = "iOS";
        private const string AndroidSectionLabel = "Android";
        private const string DevelopmentTeamIdLabel = "Development Team Id";
        private const string RuntimeIpLabel = "Local runtime IP";
        private const string ConnectLocallyLabel = "Should connect locally?";

        private MobileLaunchConfig launchConfig;

        private string[] simulatorNames;
        private string[] deviceNames;
        
        private int simulatorNameIndex;
        private int deviceNameIndex;

        [MenuItem("SpatialOS/Mobile launcher", false, 70)]
        public static void ShowWindow()
        {
            var inspectorWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            var deploymentWindow = GetWindow<MobileLauncherWindow>(inspectorWindowType);
            deploymentWindow.titleContent.text = "Mobile Launcher";
            deploymentWindow.titleContent.tooltip = "A tab for managing your mobile client-workers.";
            deploymentWindow.Show();
        }

        private void OnEnable()
        {
            launchConfig = new MobileLaunchConfig();
            deviceNames = launchConfig.availableDevices.Keys.ToArray();
            simulatorNames = launchConfig.availableSimulators.Keys.ToArray();
        }
        
        public void OnGUI()
        {
            GUILayout.Label(MobileSectionLabel, EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorPrefs.SetString(
                    launchConfig.RuntimeIpEditorPrefKey,
                    EditorGUILayout.TextField(RuntimeIpLabel, launchConfig.RuntimeIp));
                launchConfig.shouldConnectLocally = EditorGUILayout.Toggle(ConnectLocallyLabel, launchConfig.shouldConnectLocally);
            }
            
            DrawHorizontalLine(10, new Color(0.7f, 0.7f, 0.7f, 1));
            GUILayout.Label(AndroidSectionLabel, EditorStyles.boldLabel);

            if (GUILayout.Button("Launch Android app"))
            {
                AndroidLaunchUtils.Launch(launchConfig.shouldConnectLocally);
            }

#if UNITY_EDITOR_OSX

            DrawHorizontalLine(10, new Color(0.7f, 0.7f, 0.7f, 1));

            GUILayout.Label(iOSSectionLabel, EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                EditorPrefs.SetString(
                    launchConfig.DevelopmentTeamIdEditorPrefKey,
                    EditorGUILayout.TextField(DevelopmentTeamIdLabel, launchConfig.DevelopmentTeamId));
                
                using (new EditorGUI.DisabledScope(false)) // todo check xcode project
                {
                    if (GUILayout.Button("Build XCode project"))
                    {
                        iOSLaunchUtils.Build(launchConfig.DevelopmentTeamId);
                    }
                }
                
                DrawHorizontalLine(8, new Color(0.4f, 0.4f, 0.4f, 1));

                using (new GUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledScope(simulatorNames.Length == 0))
                    {
                        simulatorNameIndex =
                            EditorGUILayout.Popup("Simulator Model", simulatorNameIndex, simulatorNames);
                    }

                    var buttonIcon = new GUIContent(EditorGUIUtility.IconContent("Refresh"))
                    {
                        tooltip = "Refresh your simulator list."
                    };

                    if (GUILayout.Button(buttonIcon, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                    {
                        launchConfig.availableSimulators = iOSLaunchUtils.RetrieveAvailableiOSSimulators();
                        simulatorNames = launchConfig.availableSimulators.Keys.ToArray();
                    }
                }

                using (new EditorGUI.DisabledScope(simulatorNames.Length == 0))
                {
                    if (GUILayout.Button("Launch iOS app on Simulator"))
                    {
                        var simulatorUID =
                            launchConfig.availableSimulators[simulatorNames[simulatorNameIndex]];
                        iOSLaunchUtils.Launch(true, simulatorUID, true);
                    }
                }
                
                DrawHorizontalLine(8, new Color(0.4f, 0.4f, 0.4f, 1));

                using (new GUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledScope(deviceNames.Length == 0))
                    {
                        deviceNameIndex = EditorGUILayout.Popup("Device Model", deviceNameIndex, deviceNames);
                    }

                    var buttonIcon = new GUIContent(EditorGUIUtility.IconContent("Refresh"))
                    {
                        tooltip = "Refresh your device list."
                    };

                    if (GUILayout.Button(buttonIcon, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                    {
                        launchConfig.availableDevices = iOSLaunchUtils.RetrieveAvailableiOSDevices();
                        deviceNames = launchConfig.availableDevices.Keys.ToArray();
                        deviceNameIndex = 0;
                    }
                }


                using (new EditorGUI.DisabledScope(deviceNames.Length == 0))
                {
                    if (GUILayout.Button("Launch iOS app on device"))
                    {
                        var deviceUID = launchConfig.availableDevices[deviceNames[deviceNameIndex]];
                        iOSLaunchUtils.Launch(true, deviceUID, false);
                    }
                }
            }
#endif
        }
        
        private void DrawHorizontalLine(int height, Color color)
        {
            var rect = EditorGUILayout.GetControlRect(false, height);
            rect.height = height;
            using (new Handles.DrawingScope(color))
            {
                Handles.DrawLine(new Vector2(rect.x, rect.yMax), new Vector2(rect.xMax, rect.yMax));
            }

            GUILayout.Space(rect.height);
        }
    }
}

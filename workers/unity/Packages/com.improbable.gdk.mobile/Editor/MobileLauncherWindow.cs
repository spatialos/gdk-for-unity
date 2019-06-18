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

        private static readonly Color DarkGrey = new Color(0.4f, 0.4f, 0.4f, 1);
        private static readonly Color LightGrey = new Color(0.7f, 0.7f, 0.7f, 1);

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
            
#if UNITY_EDITOR_OSX
            deviceNames = iOSLaunchUtils.RetrieveAvailableiOSDevices().Keys.ToArray();
            simulatorNames = iOSLaunchUtils.RetrieveAvailableiOSSimulators().Keys.ToArray();
#endif
        }

        public void OnGUI()
        {
            GUILayout.Label(MobileSectionLabel, EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                launchConfig.RuntimeIp = EditorGUILayout.TextField(RuntimeIpLabel, launchConfig.RuntimeIp);
                launchConfig.ShouldConnectLocally = EditorGUILayout.Toggle(ConnectLocallyLabel, launchConfig.ShouldConnectLocally);
            }
            
            DrawHorizontalLine(10, LightGrey);
            GUILayout.Label(AndroidSectionLabel, EditorStyles.boldLabel);

            if (GUILayout.Button("Launch Android app"))
            {
                AndroidLaunchUtils.Launch(launchConfig.ShouldConnectLocally, launchConfig.RuntimeIp);
            }

#if UNITY_EDITOR_OSX

            DrawHorizontalLine(10, LightGrey);

            GUILayout.Label(iOSSectionLabel, EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                launchConfig.DevelopmentTeamId = EditorGUILayout.TextField(DevelopmentTeamIdLabel, launchConfig.DevelopmentTeamId);
                
                if (GUILayout.Button("Build XCode project"))
                {
                    iOSLaunchUtils.Build(launchConfig.DevelopmentTeamId);
                }

                DrawHorizontalLine(8, DarkGrey);

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
                        simulatorNames = iOSLaunchUtils.RetrieveAvailableiOSSimulators().Keys.ToArray();
                        simulatorNameIndex = 0;
                    }
                }

                using (new EditorGUI.DisabledScope(simulatorNames.Length == 0))
                {
                    if (GUILayout.Button("Launch iOS app in Simulator"))
                    {
                        var availableSimulators = iOSLaunchUtils.RetrieveAvailableiOSSimulators();
                        if (availableSimulators.TryGetValue(simulatorNames[simulatorNameIndex], out var simulatorUID))
                        {
                            iOSLaunchUtils.Launch(launchConfig.ShouldConnectLocally, simulatorUID, launchConfig.RuntimeIp, true);
                        }
                        else
                        {
                            simulatorNames = availableSimulators.Keys.ToArray();
                            simulatorNameIndex = 0;
                            Debug.LogError("Failed to launch app on selected simulator.");
                        }
                    }
                }
                
                DrawHorizontalLine(8, DarkGrey);

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
                        deviceNames = iOSLaunchUtils.RetrieveAvailableiOSDevices().Keys.ToArray();
                        deviceNameIndex = 0;
                    }
                }

                using (new EditorGUI.DisabledScope(deviceNames.Length == 0))
                {
                    if (GUILayout.Button("Launch iOS app on device"))
                    {
                        var availableDevices = iOSLaunchUtils.RetrieveAvailableiOSDevices();
                        if (availableDevices.TryGetValue(deviceNames[deviceNameIndex], out var deviceUID))
                        {
                            iOSLaunchUtils.Launch(launchConfig.ShouldConnectLocally, deviceUID, launchConfig.RuntimeIp, false);
                        }
                        else
                        {
                            deviceNames = availableDevices.Keys.ToArray();
                            deviceNameIndex = 0;
                            Debug.LogError("Failed to launch app on selected device. Is the device still connected?");
                        }
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

using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Editor;
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
        private const string ConnectLocallyLabel = "Connect locally";

        private static readonly Color DarkGrey = new Color(0.4f, 0.4f, 0.4f, 1);
        private static readonly Color LightGrey = new Color(0.7f, 0.7f, 0.7f, 1);

        private MobileLaunchConfig launchConfig;

        private Dictionary<string, string> androidEmulators;
        private Dictionary<string, string> androidDevices;

        private string[] androidEmulatorNames;
        private string[] androidDeviceNames;

        private int androidEmulatorNameIndex;
        private int androidDeviceNameIndex;

        private Dictionary<string, string> iOSSimulators;
        private Dictionary<string, string> iOSDevices;

        private string[] iOSSimulatorNames;
        private string[] iOSDeviceNames;

        private int iOSSimulatorNameIndex;
        private int iOSDeviceNameIndex;

        [MenuItem("SpatialOS/Mobile Launcher", false, 52)]
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
            RefreshAndroidEmulatorsAndDevices();

#if UNITY_EDITOR_OSX
            iOSDeviceNames = iOSLaunchUtils.RetrieveAvailableiOSDevices().Keys.ToArray();
            iOSSimulatorNames = iOSLaunchUtils.RetrieveAvailableiOSSimulators().Keys.ToArray();
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

            DisplayAndroidMenu();

#if UNITY_EDITOR_OSX
            DisplayiOSMenu();
#endif
        }

        private void RefreshAndroidEmulatorsAndDevices()
        {
            var availableEmulatorsAndDevices = AndroidLaunchUtils.RetrieveAvailableEmulatorsAndDevices();

            androidEmulators = availableEmulatorsAndDevices.Emulators;
            androidDevices = availableEmulatorsAndDevices.Devices;

            androidEmulatorNames = androidEmulators.Keys.ToArray();
            androidDeviceNames = androidDevices.Keys.ToArray();

            androidEmulatorNameIndex = 0;
            androidDeviceNameIndex = 0;
        }

        private void DisplayAndroidMenu()
        {
            CommonUIElements.DrawHorizontalLine(10, LightGrey);

            GUILayout.Label(AndroidSectionLabel, EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                DisplayAndroidDeviceList(ref androidEmulatorNameIndex, ref androidEmulatorNames, true);
                DisplayAndroidLaunchButton(ref androidEmulatorNameIndex, ref androidEmulatorNames, ref androidEmulators, true);

                CommonUIElements.DrawHorizontalLine(8, DarkGrey);

                DisplayAndroidDeviceList(ref androidDeviceNameIndex, ref androidDeviceNames, false);
                DisplayAndroidLaunchButton(ref androidDeviceNameIndex, ref androidDeviceNames, ref androidDevices, false);
            }
        }

        private void DisplayAndroidDeviceList(ref int index, ref string[] names, bool isEmulator)
        {
            var deviceOrEmulator = isEmulator ? "Emulator" : "Device";

            using (new GUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(names.Length == 0))
                {
                    index = EditorGUILayout.Popup($"{deviceOrEmulator} Model", index, names);
                }

                var buttonIcon = new GUIContent(EditorGUIUtility.IconContent("Refresh"))
                {
                    tooltip = $"Refresh your {deviceOrEmulator.ToLower()} list."
                };

                if (GUILayout.Button(buttonIcon, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                {
                    RefreshAndroidEmulatorsAndDevices();
                }
            }
        }

        private void DisplayAndroidLaunchButton(ref int index, ref string[] names, ref Dictionary<string, string> devices, bool isEmulator)
        {
            var deviceOrEmulator = isEmulator ? "Emulator" : "Device";

            using (new EditorGUI.DisabledScope(names.Length == 0))
            {
                if (GUILayout.Button($"Launch Android app on {deviceOrEmulator}"))
                {
                    if (devices.TryGetValue(names[index], out var deviceId))
                    {
                        AndroidLaunchUtils.Launch(launchConfig.ShouldConnectLocally, deviceId, launchConfig.RuntimeIp, isEmulator);
                    }
                    else
                    {
                        RefreshAndroidEmulatorsAndDevices();
                        Debug.LogError($"Failed to launch app on selected {deviceOrEmulator}. Is the {deviceOrEmulator} still connected?");
                    }
                }
            }
        }

#if UNITY_EDITOR_OSX
        private void DisplayiOSMenu()
        {
            CommonUIElements.DrawHorizontalLine(10, LightGrey);

            GUILayout.Label(iOSSectionLabel, EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                launchConfig.DevelopmentTeamId = EditorGUILayout.TextField(DevelopmentTeamIdLabel, launchConfig.DevelopmentTeamId);

                if (GUILayout.Button("Build XCode project"))
                {
                    iOSLaunchUtils.Build(launchConfig.DevelopmentTeamId);
                }

                CommonUIElements.DrawHorizontalLine(8, DarkGrey);

                using (new GUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledScope(iOSSimulatorNames.Length == 0))
                    {
                        iOSSimulatorNameIndex =
                            EditorGUILayout.Popup("Simulator Model", iOSSimulatorNameIndex, iOSSimulatorNames);
                    }

                    var buttonIcon = new GUIContent(EditorGUIUtility.IconContent("Refresh"))
                    {
                        tooltip = "Refresh your simulator list."
                    };

                    if (GUILayout.Button(buttonIcon, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                    {
                        iOSSimulatorNames = iOSLaunchUtils.RetrieveAvailableiOSSimulators().Keys.ToArray();
                        iOSSimulatorNameIndex = 0;
                    }
                }

                using (new EditorGUI.DisabledScope(iOSSimulatorNames.Length == 0))
                {
                    if (GUILayout.Button("Launch iOS app in Simulator"))
                    {
                        var availableSimulators = iOSLaunchUtils.RetrieveAvailableiOSSimulators();
                        if (availableSimulators.TryGetValue(iOSSimulatorNames[iOSSimulatorNameIndex], out var simulatorUID))
                        {
                            iOSLaunchUtils.Launch(launchConfig.ShouldConnectLocally, simulatorUID, launchConfig.RuntimeIp, true);
                        }
                        else
                        {
                            iOSSimulatorNames = availableSimulators.Keys.ToArray();
                            iOSSimulatorNameIndex = 0;
                            Debug.LogError("Failed to launch app on selected simulator.");
                        }
                    }
                }

                CommonUIElements.DrawHorizontalLine(8, DarkGrey);

                using (new GUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledScope(iOSDeviceNames.Length == 0))
                    {
                        iOSDeviceNameIndex = EditorGUILayout.Popup("Device Model", iOSDeviceNameIndex, iOSDeviceNames);
                    }

                    var buttonIcon = new GUIContent(EditorGUIUtility.IconContent("Refresh"))
                    {
                        tooltip = "Refresh your device list."
                    };

                    if (GUILayout.Button(buttonIcon, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                    {
                        iOSDeviceNames = iOSLaunchUtils.RetrieveAvailableiOSDevices().Keys.ToArray();
                        iOSDeviceNameIndex = 0;
                    }
                }

                using (new EditorGUI.DisabledScope(iOSDeviceNames.Length == 0))
                {
                    if (GUILayout.Button("Launch iOS app on device"))
                    {
                        var availableDevices = iOSLaunchUtils.RetrieveAvailableiOSDevices();
                        if (availableDevices.TryGetValue(iOSDeviceNames[iOSDeviceNameIndex], out var deviceUID))
                        {
                            iOSLaunchUtils.Launch(launchConfig.ShouldConnectLocally, deviceUID, launchConfig.RuntimeIp, false);
                        }
                        else
                        {
                            iOSDeviceNames = availableDevices.Keys.ToArray();
                            iOSDeviceNameIndex = 0;
                            Debug.LogError("Failed to launch app on selected device. Is the device still connected?");
                        }
                    }
                }
            }
        }
#endif
    }
}

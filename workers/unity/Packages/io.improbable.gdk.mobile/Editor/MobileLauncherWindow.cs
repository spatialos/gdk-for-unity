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
            RefreshiOSEmulatorsAndDevices();
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
            (androidEmulators, androidDevices) = AndroidLaunchUtils.RetrieveAvailableEmulatorsAndDevices();

            androidEmulatorNames = androidEmulators.Keys.ToArray();
            androidDeviceNames = androidDevices.Keys.ToArray();

            androidEmulatorNameIndex = 0;
            androidDeviceNameIndex = 0;
        }

        private void RefreshiOSEmulatorsAndDevices()
        {
            (iOSSimulators, iOSDevices) = iOSLaunchUtils.RetrieveAvailableEmulatorsAndDevices();

            iOSSimulatorNames = iOSSimulators.Keys.ToArray();
            iOSDeviceNames = iOSDevices.Keys.ToArray();

            iOSSimulatorNameIndex = 0;
            iOSDeviceNameIndex = 0;
        }

        private void DisplayAndroidMenu()
        {
            CommonUIElements.DrawHorizontalLine(10, LightGrey);

            GUILayout.Label(AndroidSectionLabel, EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                DisplayDevicesAndLaunchButton(true, true);

                CommonUIElements.DrawHorizontalLine(8, DarkGrey);

                DisplayDevicesAndLaunchButton(true, false);
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

                DisplayDevicesAndLaunchButton(false, true);

                CommonUIElements.DrawHorizontalLine(8, DarkGrey);

                DisplayDevicesAndLaunchButton(false, false);
            }
        }
#endif

        private void DisplayDevicesAndLaunchButton(bool isAndroid, bool isEmulator)
        {
            var androidOriOs = isAndroid ? "Android" : "iOS";
            var deviceOrEmulator = isEmulator
                ? isAndroid
                    ? "Emulator"
                    : "Simulator"
                : "Device";

            ref var index = ref isAndroid
                ? ref isEmulator
                    ? ref androidEmulatorNameIndex
                    : ref androidDeviceNameIndex
                : ref isEmulator
                    ? ref iOSSimulatorNameIndex
                    : ref iOSDeviceNameIndex;

            var names = isAndroid
                ? isEmulator
                    ? androidEmulatorNames
                    : androidDeviceNames
                : isEmulator
                    ? iOSSimulatorNames
                    : iOSDeviceNames;

            var devices = isAndroid
                ? isEmulator
                    ? androidEmulators
                    : androidDevices
                : isEmulator
                    ? iOSSimulators
                    : iOSDevices;

            // List of devices/emulators with refresh button linked to correct platform
            using (new GUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(names.Length == 0))
                {
                    index = EditorGUILayout.Popup($"{deviceOrEmulator} Model", index, names);
                }

                var buttonIcon = new GUIContent(EditorGUIUtility.IconContent("Refresh"))
                {
                    tooltip = $"Refresh your {(!isAndroid && isEmulator ? "Simulator" : deviceOrEmulator.ToLower())} list."
                };

                if (GUILayout.Button(buttonIcon, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                {
                    if (isAndroid)
                    {
                        RefreshAndroidEmulatorsAndDevices();
                    }
                    else
                    {
                        RefreshiOSEmulatorsAndDevices();
                    }
                }
            }

            // Launch button linked to the correct platform and chosen device
            using (new EditorGUI.DisabledScope(names.Length == 0))
            {
                if (!GUILayout.Button($"Launch {androidOriOs} app on {deviceOrEmulator}"))
                {
                    return;
                }

                if (devices.TryGetValue(names[index], out var deviceId))
                {
                    if (isAndroid)
                    {
                        AndroidLaunchUtils.Launch(launchConfig.ShouldConnectLocally, deviceId, launchConfig.RuntimeIp, isEmulator);
                    }
                    else
                    {
                        iOSLaunchUtils.Launch(launchConfig.ShouldConnectLocally, deviceId, launchConfig.RuntimeIp, isEmulator);
                    }
                }
                else
                {
                    if (isAndroid)
                    {
                        RefreshAndroidEmulatorsAndDevices();
                    }
                    else
                    {
                        RefreshiOSEmulatorsAndDevices();
                    }

                    Debug.LogError($"Failed to launch {androidOriOs} app on selected {deviceOrEmulator}. Is the {deviceOrEmulator} still connected?");
                }
            }
        }
    }
}

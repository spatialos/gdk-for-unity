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

        private MobileLaunchConfig mobileLaunchConfig;

        private (int currentIndex, List<DeviceLaunchConfig> devices) androidEmulators;
        private (int currentIndex, List<DeviceLaunchConfig> devices) androidDevices;

        private (int currentIndex, List<DeviceLaunchConfig> devices) iOSSimulators;
        private (int currentIndex, List<DeviceLaunchConfig> devices) iOSDevices;

        private readonly string[] emptyDeviceNameList = { "No devices found" };

        private bool androidPlaybackEngineInstalled;

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
            androidPlaybackEngineInstalled = AndroidUtils.IsAndroidPlaybackEngineInstalled();

            if (androidPlaybackEngineInstalled)
            {
                RefreshAndroidEmulatorsAndDevices();
            }

#if UNITY_EDITOR_OSX
            RefreshiOSEmulatorsAndDevices();
#endif
        }

        public void OnGUI()
        {
            GUILayout.Label(MobileSectionLabel, EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                mobileLaunchConfig.RuntimeIp = EditorGUILayout.TextField(RuntimeIpLabel, mobileLaunchConfig.RuntimeIp);
                mobileLaunchConfig.ShouldConnectLocally = EditorGUILayout.Toggle(ConnectLocallyLabel, mobileLaunchConfig.ShouldConnectLocally);
            }

            if (androidPlaybackEngineInstalled)
            {
                DisplayAndroidMenu();
            }

#if UNITY_EDITOR_OSX
            DisplayiOSMenu();
#endif
        }

        private void RefreshEmulatorsAndDevices(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.AndroidDevice:
                case DeviceType.AndroidEmulator:
                    RefreshAndroidEmulatorsAndDevices();
                    break;
                case DeviceType.iOSDevice:
                case DeviceType.iOSSimulator:
                    RefreshiOSEmulatorsAndDevices();
                    break;
                default:
                    Debug.LogError($"Unknown Device Type: {deviceType}");
                    break;
            }
        }

        private void RefreshAndroidEmulatorsAndDevices()
        {
            var (emulators, devices) = AndroidUtils.RetrieveAvailableEmulatorsAndDevices();

            androidEmulators = (0, emulators);
            androidDevices = (0, devices);
        }

        private void RefreshiOSEmulatorsAndDevices()
        {
            var (emulators, devices) = iOSUtils.RetrieveAvailableEmulatorsAndDevices();

            iOSSimulators = (0, emulators);
            iOSDevices = (0, devices);
        }

        private void DisplayAndroidMenu()
        {
            CommonUIElements.DrawHorizontalLine(10, LightGrey);

            GUILayout.Label(AndroidSectionLabel, EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                DisplayDevicesAndLaunchButton(DeviceType.AndroidEmulator);

                CommonUIElements.DrawHorizontalLine(8, DarkGrey);

                DisplayDevicesAndLaunchButton(DeviceType.AndroidDevice);
            }
        }

#if UNITY_EDITOR_OSX
        private void DisplayiOSMenu()
        {
            CommonUIElements.DrawHorizontalLine(10, LightGrey);

            GUILayout.Label(iOSSectionLabel, EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                mobileLaunchConfig.DevelopmentTeamId = EditorGUILayout.TextField(DevelopmentTeamIdLabel, mobileLaunchConfig.DevelopmentTeamId);

                if (GUILayout.Button("Build XCode project"))
                {
                    iOSUtils.Build(mobileLaunchConfig.DevelopmentTeamId);
                }

                CommonUIElements.DrawHorizontalLine(8, DarkGrey);

                DisplayDevicesAndLaunchButton(DeviceType.iOSSimulator);

                CommonUIElements.DrawHorizontalLine(8, DarkGrey);

                DisplayDevicesAndLaunchButton(DeviceType.iOSDevice);
            }
        }
#endif

        private void DisplayDevicesAndLaunchButton(DeviceType deviceType)
        {
            int index;
            List<DeviceLaunchConfig> devices;

            switch (deviceType)
            {
                case DeviceType.AndroidDevice:
                    (index, devices) = androidDevices;
                    break;
                case DeviceType.AndroidEmulator:
                    (index, devices) = androidEmulators;
                    break;
                case DeviceType.iOSDevice:
                    (index, devices) = iOSDevices;
                    break;
                case DeviceType.iOSSimulator:
                    (index, devices) = iOSSimulators;
                    break;
                default:
                    throw new ArgumentException($"Unknown Device Type: {deviceType}");
            }

            var prettyDeviceType = deviceType.ToPrettyDeviceType();
            var names = devices.Select(config => config.DeviceName).ToArray();
            var noDevicesFound = devices.Count == 0;

            // List of devices/emulators with refresh button linked to correct platform
            using (new GUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(noDevicesFound))
                {
                    SetDeviceIndex(deviceType,
                        noDevicesFound
                            ? EditorGUILayout.Popup(prettyDeviceType, 0, emptyDeviceNameList)
                            : EditorGUILayout.Popup(prettyDeviceType, index, names));
                }

                var buttonIcon = new GUIContent(EditorGUIUtility.IconContent("Refresh"))
                {
                    tooltip = $"Refresh your {prettyDeviceType} list."
                };

                if (GUILayout.Button(buttonIcon, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                {
                    RefreshEmulatorsAndDevices(deviceType);
                }
            }

            // Draw a launch button that will launch the app on the correct platform and chosen device
            using (new EditorGUI.DisabledScope(noDevicesFound))
            {
                if (!GUILayout.Button($"Launch app on {prettyDeviceType}"))
                {
                    return;
                }

                devices[index].Launch(mobileLaunchConfig);
            }
        }

        private void SetDeviceIndex(DeviceType deviceType, int index)
        {
            switch (deviceType)
            {
                case DeviceType.AndroidDevice:
                    androidDevices.currentIndex = index;
                    break;
                case DeviceType.AndroidEmulator:
                    androidEmulators.currentIndex = index;
                    break;
                case DeviceType.iOSDevice:
                    iOSDevices.currentIndex = index;
                    break;
                case DeviceType.iOSSimulator:
                    iOSSimulators.currentIndex = index;
                    break;
                default:
                    throw new ArgumentException($"Unknown Device Type: {deviceType}");
            }
        }
    }
}

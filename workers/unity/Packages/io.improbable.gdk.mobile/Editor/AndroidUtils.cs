using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Mobile
{
    public static class AndroidUtils
    {
        /*
            The regex below operates on output similar to the following:
                List of devices attached
                adb server version (40) doesn't match this client (39); killing...
                * daemon started successfully *
                    emulator-5556         device product:sdk_gphone_x86 model:Android_SDK_built_for_x86 device:generic_x86
                    p4u1b414jiwa5h3re     device product:starltexx model:SM_G960F device:starlte

            There are four capture groups: id, product, model, device. The "id" group is the emulator name or device
            serial number. For example, "emulator-5556" or "p4u1b414jiwa5h3re".

            The other capture groups are used to extract their respective fields in the device metadata. For example,
            the "product" capture would be "starltexx", "model" would be "SM_G960F" and "device" would be "starlte".
        */
        private static readonly Regex DeviceMatchRegex = new Regex(pattern:
            "(?:(?<id>[\\w\\d_-]+)\\s*device).*" +
            "(?:product:(?<product>[\\w\\d_-]+))\\s*" +
            "(?:model:(?<model>[\\w\\d_-]+))\\s*" +
            "(?:device:(?<device>[\\w\\d_-]+)).*");

        public static (List<DeviceLaunchConfig> emulators, List<DeviceLaunchConfig> devices) RetrieveAvailableEmulatorsAndDevices()
        {
            var availableEmulators = new List<DeviceLaunchConfig>();
            var availableDevices = new List<DeviceLaunchConfig>();

            if (!TryGetAdbPath(out var adbPath))
            {
                Debug.LogError(
                    $"Could not find Android SDK. Please set the SDK location in your editor preferences.");
                return (availableEmulators, availableDevices);
            }

            // List connected devices
            // adb devices -l
            var result = RedirectedProcess.Command(adbPath)
                .WithArgs("devices", "-l")
                .AddOutputProcessing(message =>
                {
                    if (!DeviceMatchRegex.IsMatch(message))
                    {
                        return;
                    }

                    var match = DeviceMatchRegex.Match(message);
                    var deviceId = match.Groups["id"].Value;

                    if (deviceId.Contains("emulator"))
                    {
                        availableEmulators.Add(new DeviceLaunchConfig(
                            deviceName: $"{match.Groups["product"].Value} ({deviceId})",
                            deviceId: deviceId,
                            deviceType: DeviceType.AndroidEmulator,
                            launchAction: Launch));
                    }
                    else
                    {
                        availableDevices.Add(new DeviceLaunchConfig(
                            deviceName: $"{match.Groups["model"].Value} ({deviceId})",
                            deviceId: deviceId,
                            deviceType: DeviceType.AndroidDevice,
                            launchAction: Launch));
                    }
                })
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .Run();

            if (result.ExitCode == 0)
            {
                return (availableEmulators, availableDevices);
            }

            Debug.LogError($"Failed to find Android emulators or devices:\n {string.Join("\n", result.Stderr)}");

            availableEmulators.Clear();
            availableDevices.Clear();

            return (availableEmulators, availableDevices);
        }

        public static void Launch(DeviceLaunchConfig deviceLaunchConfig, MobileLaunchConfig mobileLaunchConfig)
        {
            // Throw if device type is neither AndroidDevice nor AndroidEmulator
            if (!deviceLaunchConfig.IsAndroid)
            {
                throw new ArgumentException($"Device must of be of type {DeviceType.AndroidDevice} or {DeviceType.AndroidEmulator}.");
            }

            try
            {
                // Find adb
                if (!TryGetAdbPath(out var adbPath))
                {
                    Debug.LogError(
                        $"Could not find Android SDK. Please set the SDK location in your editor preferences.");
                    return;
                }

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Installing APK", 0.3f);

                // Find apk to install
                if (!TryGetApkPath(Common.BuildScratchDirectory, out var apkPath))
                {
                    Debug.LogError($"Could not find a built out Android binary in \"{Common.BuildScratchDirectory}\" to launch.");
                    return;
                }

                // Check if chosen emulator/device is connected
                // adb -s <device id> get-state
                if (RedirectedProcess.Command(adbPath)
                    .InDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, "..")))
                    .WithArgs($"-s {deviceLaunchConfig.DeviceId}", "get-state").Run().ExitCode != 0)
                {
                    Debug.LogError($"Chosen {deviceLaunchConfig.PrettyDeviceType} ({deviceLaunchConfig.DeviceId}) not found.");
                    return;
                }

                // Install apk on chosen emulator/device
                // adb -s <device id> install -r <apk>
                if (RedirectedProcess.Command(adbPath)
                    .InDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, "..")))
                    .WithArgs($"-s {deviceLaunchConfig.DeviceId}", "install", "-r", $"\"{apkPath}\"").Run().ExitCode != 0)
                {
                    Debug.LogError(
                        $"Failed to install the apk on the {deviceLaunchConfig.PrettyDeviceType}. " +
                        $"If the application is already installed on your {deviceLaunchConfig.PrettyDeviceType}, " +
                        "try uninstalling it before launching the mobile client.");
                    return;
                }

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Launching Client", 0.9f);

                // Get GDK-related mobile launch arguments
                var arguments = mobileLaunchConfig.ToLaunchArgs();

                // Get bundle identifier
                var bundleId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);

                // Launch the bundle on chosen device
                // Use -S force stops target app before launching again
                // adb -s <device id>
                //    shell am start -S -n <unity package path> -e arguments <mobile launch arguments>
                RedirectedProcess.Command(adbPath)
                    .WithArgs($"-s {deviceLaunchConfig.DeviceId}", "shell", "am", "start", "-S",
                        "-n", $"{bundleId}/com.unity3d.player.UnityPlayerActivity",
                        "-e", "\"arguments\"", $"\\\"{arguments}\\\"")
                    .InDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, "..")))
                    .Run();

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Done", 1.0f);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static bool TryGetAdbPath(out string adbPath)
        {
            var sdkRootPath = GetSDKDirectory();
            if (string.IsNullOrEmpty(sdkRootPath))
            {
                adbPath = null;
                return false;
            }

            adbPath = Path.Combine(sdkRootPath, "platform-tools", "adb");
            return true;
        }

        private static bool TryGetApkPath(string rootPath, out string apkPath)
        {
            if (Directory.Exists(rootPath))
            {
                foreach (var file in Directory.GetFiles(rootPath, "*.apk", SearchOption.AllDirectories))
                {
                    apkPath = file;
                    return true;
                }
            }

            apkPath = string.Empty;
            return false;
        }

        private static bool UseEmbeddedSDK()
        {
            const string SDKPrefKey = "SdkUseEmbedded";
            return !EditorPrefs.HasKey(SDKPrefKey) || EditorPrefs.GetBool(SDKPrefKey);
        }

        private static string GetSDKDirectory()
        {
            if (UseEmbeddedSDK())
            {
                return Path.Combine(BuildPipeline.GetPlaybackEngineDirectory(BuildTarget.Android, BuildOptions.None), "SDK");
            }
            else
            {
                return EditorPrefs.GetString("AndroidSdkRoot");
            }
        }

        public static bool IsAndroidPlaybackEngineInstalled()
        {
            var targetGroup = BuildPipeline.GetBuildTargetGroup(BuildTarget.Android);
            return BuildPipeline.IsBuildTargetSupported(targetGroup, BuildTarget.Android);
        }
    }
}

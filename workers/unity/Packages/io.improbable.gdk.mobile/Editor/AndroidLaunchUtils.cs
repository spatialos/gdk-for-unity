using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Mobile
{
    public static class AndroidLaunchUtils
    {
        private static readonly Regex DeviceMatchRegex = new Regex("(?:(?<id>[\\w_\\d-]+)\\s*device).*"
            + "(?:product:(?<product>[\\w_\\d]+))\\s*"
            + "(?:model:(?<model>[\\w_\\d]+))\\s*"
            + "(?:device:(?<device>[\\w_\\d]+)).*");

        public static (Dictionary<string, string>, Dictionary<string, string>) RetrieveAvailableEmulatorsAndDevices()
        {
            var available = false;
            var availableEmulators = new Dictionary<string, string>();
            var availableDevices = new Dictionary<string, string>();

            if (!TryGetAdbPath(out var adbPath))
            {
                Debug.LogError(
                    $"Could not find Android SDK. Please set the SDK location in your editor preferences.");
                return (availableEmulators, availableDevices);
            }

            var result = RedirectedProcess.Command(adbPath)
                .WithArgs("devices", "-l")
                .AddOutputProcessing(message =>
                {
                    if (!DeviceMatchRegex.IsMatch(message))
                    {
                        return;
                    }

                    available = true;

                    var match = DeviceMatchRegex.Match(message);
                    var deviceId = match.Groups["id"].Value;

                    if (deviceId.Contains("emulator"))
                    {
                        availableEmulators[$"{match.Groups["product"].Value} ({deviceId})"] = deviceId;
                    }
                    else
                    {
                        availableDevices[$"{match.Groups["model"].Value} ({deviceId})"] = deviceId;
                    }
                })
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .Run();

            if (result.ExitCode == 0)
            {
                if (!available)
                {
                    Debug.Log("No Android emulators or devices found.");
                }

                return (availableEmulators, availableDevices);
            }

            Debug.LogError("Failed to find Android emulators or devices.");
            availableEmulators.Clear();
            availableDevices.Clear();
            return (availableEmulators, availableDevices);
        }

        public static void Launch(bool shouldConnectLocally, string deviceId, string runtimeIp, bool useEmulator)
        {
            try
            {
                // Useful for personalised output
                var deviceOrEmulator = useEmulator ? "emulator" : "device";

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
                if (RedirectedProcess.Command(adbPath)
                    .InDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, "..")))
                    .WithArgs($"-s {deviceId}", "get-state").Run().ExitCode != 0)
                {
                    Debug.LogError($"Chosen {deviceOrEmulator} ({deviceId}) not found.");
                    return;
                }

                // Install apk on chosen emulator/device
                if (RedirectedProcess.Command(adbPath)
                    .InDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, "..")))
                    .WithArgs($"-s {deviceId}", "install", "-r", $"\"{apkPath}\"").Run().ExitCode != 0)
                {
                    Debug.LogError(
                        $"Failed to install the apk on the {deviceOrEmulator}. If the application is already installed on your {deviceOrEmulator}, " +
                        "try uninstalling it before launching the mobile client.");
                    return;
                }

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Launching Client", 0.9f);

                var arguments = MobileLaunchUtils.PrepareArguments(shouldConnectLocally, runtimeIp);

                // Get chosen android package id and launch
                var bundleId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
                RedirectedProcess.Command(adbPath)
                    .WithArgs($"-s {deviceId}", "shell", "am", "start", "-S",
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
            var sdkRootPath = EditorPrefs.GetString("AndroidSdkRoot");
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
    }
}

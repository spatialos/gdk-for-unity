using System.IO;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Mobile
{
    // TODO UTY-2095: Improve Android workflow and refactor the code
    public static class AndroidLaunchUtils
    {
        public static void Launch(bool shouldConnectLocally, string runtimeIp)
        {
            try
            {
                // Find ADB tool
                var sdkRootPath = EditorPrefs.GetString("AndroidSdkRoot");
                if (string.IsNullOrEmpty(sdkRootPath))
                {
                    Debug.LogError(
                        $"Could not find Android SDK. Please set the SDK location in your editor preferences.");
                    return;
                }

                var adbPath = Path.Combine(sdkRootPath, "platform-tools", "adb");

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Installing APK", 0.3f);

                // Find apk to install
                if (!TryGetApkPath(Common.BuildScratchDirectory, out var apkPath))
                {
                    Debug.LogError($"Could not find a built out Android binary in \"{Common.BuildScratchDirectory}\" to launch.");
                    return;
                }

                // Ensure an android device/emulator is present
                if (RedirectedProcess.Command(adbPath)
                    .InDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, "..")))
                    .WithArgs("get-state").Run() != 0)
                {
                    Debug.LogError("No Android device/emulator detected.");
                    return;
                }

                // Install apk on connected phone / emulator
                if (RedirectedProcess.Command(adbPath)
                    .InDirectory(Path.GetFullPath(Path.Combine(Application.dataPath, "..")))
                    .WithArgs("install", "-r", $"\"{apkPath}\"").Run() != 0)
                {
                    Debug.LogError(
                        "Failed to install the apk on the device/emulator. If the application is already installed on your device/emulator, " +
                        "try uninstalling it before launching the mobile client.");
                    return;
                }

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Launching Client", 0.9f);

                var arguments = MobileLaunchUtils.PrepareArguments(shouldConnectLocally, runtimeIp);

                // Get chosen android package id and launch
                var bundleId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
                RedirectedProcess.Command(adbPath)
                    .WithArgs("shell", "am", "start", "-S", "-n", $"{bundleId}/com.unity3d.player.UnityPlayerActivity",
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

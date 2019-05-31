using System.IO;
using System.Text;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public static class LaunchMenu
    {
        private const string rootApkPath = "build";
        private static string AbsoluteApkPath => Path.GetFullPath(Path.Combine(Application.dataPath, Path.Combine("..", rootApkPath)));

        private const string MenuLaunchMobile = "SpatialOS/Launch mobile client";
        private const string MenuLaunchAndroidLocal = "/Android for local";
        private const string MenuLaunchAndroidCloud = "/Android for cloud";

        [MenuItem(MenuLaunchMobile + MenuLaunchAndroidLocal, false, 73)]
        private static void LaunchAndroidLocal()
        {
            LaunchAndroid(true);
        }

        [MenuItem(MenuLaunchMobile + MenuLaunchAndroidCloud, false, 74)]
        private static void LaunchAndroidCloud()
        {
            LaunchAndroid(false);
        }

        private static void LaunchAndroid(bool shouldConnectLocally)
        {
            try
            {
                // Find ADB tool
                var sdkRootPath = EditorPrefs.GetString("AndroidSdkRoot");
                if (string.IsNullOrEmpty(sdkRootPath))
                {
                    Debug.LogError($"Could not find Android SDK. Please set the SDK location in your editor preferences.");
                    return;
                }

                var adbPath = Path.Combine(sdkRootPath, "platform-tools", "adb");

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Installing APK", 0.3f);

                // Find apk to install
                if (!TryGetApkPath(AbsoluteApkPath, out var apkPath))
                {
                    Debug.LogError($"Could not find a built out Android binary in \"{AbsoluteApkPath}\" to launch.");
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
                    Debug.LogError("Failed to install the apk on the device/emulator. If the application is already installed on your device/emulator, " +
                        "try uninstalling it before launching the mobile client.");
                    return;
                }

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Launching Client", 0.9f);

                // Optional arguments to be passed, same as standalone
                // Use this to pass through the local ip to connect to
                var runtimeIp = GdkToolsConfiguration.GetOrCreateInstance().RuntimeIp;
                var arguments = new StringBuilder();
                if (shouldConnectLocally)
                {
                    if (string.IsNullOrEmpty(runtimeIp))
                    {
                        Debug.LogWarning("No local runtime IP was specified. Ensure you set one in SpatialOS > GDK tools configuration.");
                    }

                    arguments.Append($"+{RuntimeConfigNames.Environment} {RuntimeConfigDefaults.LocalEnvironment} ");
                    arguments.Append($"+{RuntimeConfigNames.ReceptionistHost} {runtimeIp} ");
                }
                else
                {
                    arguments.Append($"+{RuntimeConfigNames.Environment} {RuntimeConfigDefaults.CloudEnvironment} ");

                    var gdkToolsConfig = GdkToolsConfiguration.GetOrCreateInstance();

                    // Return error if no DevAuthToken is set AND fails to generate new DevAuthToken.
                    if (!PlayerPrefs.HasKey(RuntimeConfigNames.DevAuthTokenKey) && !DevAuthTokenUtils.TryGenerate())
                    {
                        Debug.LogError("Failed to generate a Dev Auth Token to launch mobile client.");
                        return;
                    }

                    arguments.Append($"+{RuntimeConfigNames.DevAuthTokenKey} {DevAuthTokenUtils.DevAuthToken} ");
                }

                // Get chosen android package id and launch
                var bundleId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
                RedirectedProcess.Command(adbPath)
                    .WithArgs("shell", "am", "start", "-S", "-n", $"{bundleId}/com.unity3d.player.UnityPlayerActivity",
                        "-e", "\"arguments\"", $"\\\"{arguments.ToString()}\\\"")
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
            foreach (var file in Directory.GetFiles(rootPath, "*.apk", SearchOption.AllDirectories))
            {
                apkPath = file;
                return true;
            }

            apkPath = string.Empty;
            return false;
        }
    }
}

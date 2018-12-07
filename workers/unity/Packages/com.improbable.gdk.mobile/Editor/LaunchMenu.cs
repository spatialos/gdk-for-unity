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
        private const string MenuLaunchAndroid = "SpatialOS/Launch mobile client/Android Device";

        [MenuItem(MenuLaunchAndroid, false, 73)]
        private static void LaunchMobileClient()
        {
            try
            {
                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Installing APK", 0.3f);

                // Find apk to install
                var rootPath = Path.Combine(Application.dataPath, Path.Combine("..", rootApkPath));
                if (!TryGetApkPath(rootPath, out var apkPath))
                {
                    Debug.LogError($"Could not find a built out Android binary in \"{rootPath}\" to launch.");
                    return;
                }

                // Install apk on connected phone / emulator
                RedirectedProcess.Run("adb", "install", "-r", apkPath);

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Launching Client", 0.9f);

                // Optional arguments to be passed, same as standalone
                // Use this to pass through the local ip to connect to
                var runtimeIp = GdkToolsConfiguration.GetOrCreateInstance().RuntimeIp;
                var arguments = new StringBuilder();
                if (!string.IsNullOrEmpty(runtimeIp))
                {
                    arguments.Append($"+{RuntimeConfigNames.ReceptionistHost} {runtimeIp}");
                }

                // Get chosen android package id and launch
                var bundleId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
                RedirectedProcess.Run("adb", "shell", "am", "start", "-S",
                    "-n", $"{bundleId}/com.unity3d.player.UnityPlayerActivity",
                    "-e", "\"arguments\"", $"\\\"{arguments.ToString()}\\\"");

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Done", 1.0f);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        [MenuItem(MenuLaunchAndroid, true)]
        private static bool LaunchMobileClientValidate()
        {
            var rootPath = Path.Combine(Application.dataPath, Path.Combine("..", rootApkPath));
            return TryGetApkPath(rootPath, out _);
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

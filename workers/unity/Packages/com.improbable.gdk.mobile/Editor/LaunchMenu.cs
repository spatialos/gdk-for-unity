using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Mobile
{
    public static class LaunchMenu
    {
        private const string rootApkPath = "build";
        private static string AbsoluteApkPath => Path.GetFullPath(Path.Combine(Application.dataPath, Path.Combine("..", rootApkPath)));
        private static string XCodeProjectPath = Path.GetFullPath(Path.Combine(Path.Combine(Application.dataPath, "..", "build", "worker", "MobileClient\\@iOS", "MobileClient\\@iOS")));
        private static string XCodeProjectFile = "Unity-iPhone.xcodeproj";
        private static string DerivedDataPath = Path.GetFullPath(Path.Combine(XCodeProjectPath, "..", "..", "build"));

        private const string MenuLaunchMobile = "SpatialOS/Launch mobile client";
        private const string MenuLaunchAndroidLocal = "/Android for local";
        private const string MenuLaunchAndroidCloud = "/Android for cloud";
        private const string MenuLaunchiOSLocal = "/iOS for local";
        private const string MenuLaunchiOSCloud = "/iOS for cloud";


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

#if UNITY_EDITOR_OSX
        [MenuItem(MenuLaunchMobile + MenuLaunchiOSLocal, false, 75)]
        private static void LaunchiOSLocal()
        {
            LaunchiOS(true);
        }

        [MenuItem(MenuLaunchMobile + MenuLaunchiOSCloud, false, 76)]
        private static void LaunchiOSCloud()
        {
            LaunchiOS(false);
        }
#endif

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

                if (!TryPrepareArguments(shouldConnectLocally, out var arguments))
                {
                    Debug.LogError("Failed to generate arguments");
                    return;
                }

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
            foreach (var file in Directory.GetFiles(rootPath, "*.apk", SearchOption.AllDirectories))
            {
                apkPath = file;
                return true;
            }

            apkPath = string.Empty;
            return false;
        }

        private static void LaunchiOS(bool shouldConnectLocally)
        {
            try
            {
                EditorUtility.DisplayProgressBar("Preparing your Mobile Client", "Building your XCode project", 0f);

                var toolsConfig = GdkToolsConfiguration.GetOrCreateInstance();
                if (string.IsNullOrEmpty(toolsConfig.DevelopmentTeamId))
                {
                    Debug.LogError("Development Team Id was not specified. Unable to build the XCode project.");
                    return;
                }

                if (RedirectedProcess.Command("xcodebuild")
                    .WithArgs("build-for-testing",
                        "-project", Path.Combine(XCodeProjectPath, XCodeProjectFile),
                        "-derivedDataPath", DerivedDataPath,
                        "-scheme", "Unity-iPhone",
                        $"DEVELOPMENT_TEAM={toolsConfig.DevelopmentTeamId}",
                        "-allowProvisioningUpdates")
                    .Run() != 0)
                {
                    Debug.LogError(
                        $"Failed to build your XCode project. Make sure you have xcodebuild installed and check the logs.");
                    return;
                }

                var deviceUID = string.Empty;
                var xcTestRunPath = string.Empty;

                // Check if we have a physical device connected
                var availableDevices = new List<string>();
                var deviceRegex = new Regex("\\[([a-z]|[0-9])+\\]");
                RedirectedProcess.Command("instruments")
                    .WithArgs("-s", "devices")
                    .AddOutputProcessing(message =>
                    {
                        if (deviceRegex.IsMatch(message))
                        {
                            availableDevices.Add(deviceRegex.Match(message).Value);
                        }
                    })
                    .Run();

                if (!TryGetXCTestRunPath(availableDevices.Count == 0, out xcTestRunPath))
                {
                    Debug.LogError("Unable to fund a xctestrun file for the correct architecture. Did you built it for the correct platform?");
                    return;
                }

                if (!TryPrepareArguments(shouldConnectLocally, out var arguments))
                {
                    Debug.LogError("Failed to generate arguments");
                    return;
                }

                // add SpatialOS arguments to iOS environment variables
                var doc = new XmlDocument();
                doc.Load(xcTestRunPath);
                XmlNode spatialNode = null;
                var rootNode = doc.DocumentElement.ChildNodes[0].ChildNodes[1];
                for (var i = 0; i < rootNode.ChildNodes.Count; i++)
                {
                    var node = rootNode.ChildNodes[i];
                    if (node.InnerText != "EnvironmentVariables")
                    {
                        continue;
                    }

                    var envValueNode = rootNode.ChildNodes[i + 1];
                    for (var j = 0; j < envValueNode.ChildNodes.Count; j++)
                    {
                        var envNode = envValueNode.ChildNodes[j];
                        if (envNode.InnerText != LaunchArguments.iOSEnvironmentKey)
                        {
                            continue;
                        }

                        spatialNode = node.ChildNodes[j + 1];
                        break;
                    }

                    if (spatialNode != null)
                    {
                        spatialNode.InnerText = arguments;
                    }
                    else
                    {
                        var spatialKeyNode = doc.CreateNode("element", "key", string.Empty);
                        spatialKeyNode.InnerText = LaunchArguments.iOSEnvironmentKey;
                        var spatialValueNode = doc.CreateNode("element", "string", string.Empty);
                        spatialValueNode.InnerText = arguments;
                        envValueNode.AppendChild(spatialKeyNode);
                        envValueNode.AppendChild(spatialValueNode);
                    }

                    break;
                }

                doc.Save(xcTestRunPath);

                // fix document after updating it
                var text = File.ReadAllText(xcTestRunPath);
                text = text.Replace("[]>", ">");
                File.WriteAllText(xcTestRunPath, text);

                if (availableDevices.Count > 0)
                {
                    deviceUID = availableDevices[0].Trim('[', ']');
                }
                else
                {
                    // start simulator
                    deviceUID = toolsConfig.GetSimulatorUID().Trim('[', ']');

                    if (RedirectedProcess.Command("xcrun")
                        .WithArgs("instruments", "-w", deviceUID, "-t", "Blank")
                        .Run() != 0)
                    {
                        Debug.LogError("Was unable to start iOS Simulator.");
                        return;
                    }
                }

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Installing your app", 0.7f);
                var command = "osascript";
                var commandArgs = $@"-e 'tell application ""Terminal""
                                     activate
                                     do script ""xcodebuild test-without-building -destination id={deviceUID} -xctestrun {xcTestRunPath}""
                                     end tell'";

                var processInfo = new ProcessStartInfo(command, commandArgs)
                {
                    CreateNoWindow = false,
                    UseShellExecute = true,
                    WorkingDirectory = Common.SpatialProjectRootDir
                };

                var process = Process.Start(processInfo);

                if (process == null)
                {
                    Debug.LogError("Failed to start app on iOS Simulator.");
                    return;
                }

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Done", 1.0f);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static bool TryGetXCTestRunPath(bool forSimulator, out string xctestrunPath)
        {
            foreach (var file in Directory.GetFiles(DerivedDataPath, "*.xctestrun", SearchOption.AllDirectories))
            {
                if (!forSimulator && file.Contains("iphoneos"))
                {
                    xctestrunPath = file;
                    return true;
                }

                if (forSimulator && file.Contains("iphonesimulator"))
                {
                    xctestrunPath = file;
                    return true;
                }
            }

            xctestrunPath = string.Empty;
            return false;
        }

        private static bool TryPrepareArguments(bool shouldConnectLocally, out string builtArguments)
        {
            var gdkToolsConfig = GdkToolsConfiguration.GetOrCreateInstance();
            var arguments = new StringBuilder();
            builtArguments = string.Empty;
            if (shouldConnectLocally)
            {
                if (string.IsNullOrEmpty(gdkToolsConfig.RuntimeIp))
                {
                    Debug.LogWarning("No local runtime IP was specified. Ensure you set one in SpatialOS > GDK tools configuration.");
                }

                arguments.Append($"+{RuntimeConfigNames.Environment} {RuntimeConfigDefaults.LocalEnvironment} ");
                arguments.Append($"+{RuntimeConfigNames.ReceptionistHost} {gdkToolsConfig.RuntimeIp} ");
            }
            else
            {
                arguments.Append($"+{RuntimeConfigNames.Environment} {RuntimeConfigDefaults.CloudEnvironment} ");


                // Return error if no DevAuthToken is set AND fails to generate new DevAuthToken.
                if (!PlayerPrefs.HasKey(RuntimeConfigNames.DevAuthTokenKey) && !DevAuthTokenUtils.TryGenerate())
                {
                    Debug.LogError("Failed to generate a Dev Auth Token to launch mobile client.");
                    return false;
                }

                arguments.Append($"+{RuntimeConfigNames.DevAuthTokenKey} {DevAuthTokenUtils.DevAuthToken} ");
            }

            builtArguments = arguments.ToString();
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Mobile
{
    public static class iOSUtils
    {
        private static readonly string XCodeProjectPath = Path.GetFullPath(Path.Combine(Common.BuildScratchDirectory, "MobileClient@iOS", "MobileClient@iOS"));
        private static readonly string DerivedDataPath = Path.GetFullPath(Path.Combine(Common.BuildScratchDirectory, "ios-build"));
        private static readonly string XCodeProjectFile = "Unity-iPhone.xcodeproj";

        private static readonly Regex NameRegex = new Regex("^(.+) \\[");
        private static readonly Regex SimulatorUidRegex = new Regex("\\[([a-zA-Z0-9\\-]+)\\] \\(Simulator\\)$");
        private static readonly Regex DeviceUidRegex = new Regex("\\[([a-zA-Z0-9\\-]+)\\]$");

        public static (List<DeviceLaunchConfig> emulators, List<DeviceLaunchConfig> devices) RetrieveAvailableEmulatorsAndDevices()
        {
            var availableSimulators = new List<DeviceLaunchConfig>();
            var availableDevices = new List<DeviceLaunchConfig>();

            // List connected devices
            // instruments -s devices
            var result = RedirectedProcess.Command("instruments")
                .WithArgs("-s", "devices")
                .AddOutputProcessing(message =>
                {
                    // Simulators
                    if (message.Contains("iPhone") || message.Contains("iPad"))
                    {
                        if (SimulatorUidRegex.IsMatch(message))
                        {
                            var simulatorName = NameRegex.Match(message).Groups[1].Value;
                            var simulatorUid = SimulatorUidRegex.Match(message).Groups[1].Value;

                            availableSimulators.Add(new DeviceLaunchConfig(
                                deviceName: simulatorName,
                                deviceId: simulatorUid,
                                deviceType: DeviceType.iOSSimulator,
                                launchAction: Launch));
                            return;
                        }
                    }

                    // Devices
                    if (DeviceUidRegex.IsMatch(message))
                    {
                        var deviceName = NameRegex.Match(message).Groups[1].Value;
                        var deviceUid = DeviceUidRegex.Match(message).Groups[1].Value;
                        availableDevices.Add(new DeviceLaunchConfig(
                            deviceName: deviceName,
                            deviceId: deviceUid,
                            deviceType: DeviceType.iOSDevice,
                            launchAction: Launch));
                    }
                })
                .RedirectOutputOptions(OutputRedirectBehaviour.None)
                .Run();

            if (result.ExitCode == 0)
            {
                return (availableSimulators, availableDevices);
            }

            Debug.LogError("Failed to find iOS Simulators or devices. " +
                "Make sure you have the Command line tools for XCode (https://developer.apple.com/download/more/) " +
                $"installed and check the logs:\n {string.Join("\n", result.Stderr)}");

            availableSimulators.Clear();
            availableDevices.Clear();

            return (availableSimulators, availableDevices);
        }

        public static void MenuBuild(string developmentTeamId)
        {
            try
            {
                EditorUtility.DisplayProgressBar("Preparing your Mobile Client", "Building your XCode project", 0f);
                Build(developmentTeamId);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public static void Build(string developmentTeamId = "")
        {
            if (!Directory.Exists(XCodeProjectPath))
            {
                throw new BuildFailedException(
                    $"Unable to find an XCode project in {XCodeProjectPath}. " +
                    "Did you build your iOS worker?");
            }

            if (string.IsNullOrEmpty(developmentTeamId) && IsXCodeProjectForDevice())
            {
                throw new BuildFailedException(
                    "Development Team Id was not specified. Can't build this XCode project for device. " +
                    "Either enter a Development Team Id or select the Simulator SDK as the target sdk.");
            }

            if (!TryBuildXCodeProject(developmentTeamId, out var xcBuildErrors))
            {
                throw new BuildFailedException("Failed to build your XCode project. " +
                    "Make sure you have the Command line tools for XCode (https://developer.apple.com/download/more/) " +
                    $"installed and check the logs:\n{string.Join("\n", xcBuildErrors)}");
            }
        }

        public static void Launch(DeviceLaunchConfig deviceLaunchConfig, MobileLaunchConfig mobileLaunchConfig)
        {
            // Throw if device type is neither iOSDevice nor iOSSimulator
            if (deviceLaunchConfig.IsAndroid)
            {
                throw new ArgumentException($"Device must of be of type {DeviceType.iOSDevice} or {DeviceType.iOSSimulator}.");
            }

            try
            {
                var useEmulator = deviceLaunchConfig.DeviceType == DeviceType.iOSSimulator;

                EditorUtility.DisplayProgressBar("Preparing your Mobile Client", "Preparing launch arguments", 0.0f);

                if (!TryGetXCodeTestRunPath(useEmulator, out var xcTestRunPath))
                {
                    Debug.LogError(
                        "Unable to find a xctestrun file for the correct architecture. Did you build your client using the correct Target SDK? " +
                        "Go to Project Settings > Player > iOS > Other Settings > Target SDK to select the correct one before building your iOS worker.");
                    return;
                }

                var arguments = mobileLaunchConfig.ToLaunchArgs();

                if (!TryModifyEnvironmentVariables(xcTestRunPath, arguments))
                {
                    Debug.LogError($"Was unable to read and modify {xcTestRunPath}.");
                    return;
                }

                if (useEmulator)
                {
                    EditorUtility.DisplayProgressBar("Launching Mobile Client", "Start iOS Simulator", 0.5f);

                    // Need to start Simulator before launching application on it
                    // instruments -w <device id> -t <profiling template>
                    var result = RedirectedProcess.Command("xcrun")
                        .WithArgs("instruments", "-w", deviceLaunchConfig.DeviceId, "-t", "Blank")
                        .Run();

                    if (result.ExitCode != 0)
                    {
                        Debug.LogError($"Unable to start iOS Simulator:\n{string.Join("\n", result.Stderr)}");
                        return;
                    }
                }

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Installing your app", 0.7f);

                if (!TryLaunchApplication(deviceLaunchConfig.DeviceId, xcTestRunPath))
                {
                    Debug.LogError("Failed to start app on iOS device.");
                }

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Done", 1.0f);
            }
            finally
            {
                var traceDirectories = Directory
                    .GetDirectories(Path.Combine(Application.dataPath, ".."), "*.trace")
                    .Where(s => s.EndsWith(".trace"));
                foreach (var directory in traceDirectories)
                {
                    Directory.Delete(directory, true);
                }

                EditorUtility.ClearProgressBar();
            }
        }

        private static bool IsXCodeProjectForDevice()
        {
            /*
             * Unity adds multiple libraries to the XCode project. One of them is libiPhone-lib.
             * This library will be a dylib if the Unity project was built for simulator.
             * Otherwise it will be a static library.
             */
            return File.Exists(Path.Combine(XCodeProjectPath, "Library", "libiPhone-lib.a"));
        }

        private static bool TryBuildXCodeProject(string developmentTeamId, out IEnumerable<string> xcBuildErrors)
        {
            var teamIdArgs = string.Empty;
            if (!string.IsNullOrEmpty(developmentTeamId))
            {
                teamIdArgs = $"DEVELOPMENT_TEAM={developmentTeamId}";
            }

            var result = RedirectedProcess.Command("xcodebuild")
                .WithArgs("build-for-testing",
                    "-project", Path.Combine(XCodeProjectPath, XCodeProjectFile),
                    "-derivedDataPath", DerivedDataPath,
                    "-scheme", "Unity-iPhone",
                    teamIdArgs,
                    "-allowProvisioningUpdates")
                .Run();

            xcBuildErrors = result.Stderr.Concat(result.Stdout);
            return result.ExitCode == 0;
        }

        private static bool TryLaunchApplication(string deviceId, string filePath)
        {
            var command = "osascript";
            var commandArgs = $@"-e 'tell application ""Terminal""
                                     activate
                                     do script ""xcodebuild test-without-building -destination 'id={deviceId}' -xctestrun {filePath}""
                                     end tell'";

            var processInfo = new ProcessStartInfo(command, commandArgs)
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WorkingDirectory = Common.SpatialProjectRootDir
            };

            var process = Process.Start(processInfo);

            return process != null;
        }

        private static bool TryGetXCodeTestRunPath(bool useSimulator, out string xctestrunPath)
        {
            if (!Directory.Exists(DerivedDataPath))
            {
                xctestrunPath = string.Empty;
                return false;
            }

            var files = Directory.GetFiles(DerivedDataPath, "*.xctestrun", SearchOption.AllDirectories);
            xctestrunPath = useSimulator
                ? files.FirstOrDefault(file => file.Contains("iphonesimulator"))
                : files.FirstOrDefault(file => file.Contains("iphoneos"));

            return !string.IsNullOrEmpty(xctestrunPath);
        }

        private static bool TryModifyEnvironmentVariables(string filePath, string arguments)
        {
            /*
             * How to add SpatialOS arguments to the game as iOS environment variables
             * The xctestrun file contains the launch arguments for your game
             * However it is structured slightly different from most XML docs by only using names like
             * "key", "dict", "string" as their node names.
             * We need to iterate through the XML file and find the node that contains "EnvironmentVariables" as a text
             * and then add the values to the next node, containing the actual environment variables.
             *
             * <key>EnvironmentVariables</key>
             * <dict>
             *     <key>OS_ACTIVITY_DT_MODE</key>
             *     <string>YES</string>
             *     <key>SQLITE_ENABLE_THREAD_ASSERTIONS</key>
             *     <string>1</string>
             *     <key>SPATIALOS_ARGUMENTS</key>
             *     <string>+environment local +receptionistHost 192.168.0.10 </string>
             * </dict>
             */

            try
            {
                var doc = new XmlDocument();
                doc.Load(filePath);
                // Navigate to the <dict> node containing all the parameters to launch the client
                var rootNode = doc.DocumentElement.ChildNodes[0].ChildNodes[1];
                var envKeyNode = rootNode.ChildNodes.Cast<XmlNode>().FirstOrDefault(node => node.InnerText == "EnvironmentVariables");
                var envValueNode = envKeyNode.NextSibling;
                var spatialKeyNode = envValueNode.ChildNodes.Cast<XmlNode>()
                    .FirstOrDefault(node => node.InnerText == LaunchArguments.iOSEnvironmentKey);

                if (spatialKeyNode != null)
                {
                    var spatialValueNode = spatialKeyNode.NextSibling;
                    spatialValueNode.InnerText = arguments;
                }
                else
                {
                    spatialKeyNode = doc.CreateNode("element", "key", string.Empty);
                    spatialKeyNode.InnerText = LaunchArguments.iOSEnvironmentKey;
                    var spatialValueNode = doc.CreateNode("element", "string", string.Empty);
                    spatialValueNode.InnerText = arguments;
                    envValueNode.AppendChild(spatialKeyNode);
                    envValueNode.AppendChild(spatialValueNode);
                }

                doc.Save(filePath);

                // UTY-2068: We currently get invalid XML using the XMLDocument object, we need to identify why this happens and fix it.
                var text = File.ReadAllText(filePath);
                text = text.Replace("[]>", ">");
                File.WriteAllText(filePath, text);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}

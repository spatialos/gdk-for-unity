using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Improbable.Gdk.Tools;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Mobile
{
    public static class iOSLaunchUtils
    {
        private static readonly string XCodeProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "build", "worker", "MobileClient\\@iOS", "MobileClient\\@iOS"));
        private static readonly string DerivedDataPath = Path.GetFullPath(Path.Combine(XCodeProjectPath, "..", "..", "build"));
        private static readonly string XCodeProjectFile = "Unity-iPhone.xcodeproj";

        private static readonly Regex nameRegex = new Regex("^[a-z|A-Z|\\s|0-9]+");
        private static readonly Regex simulatorUIDRegex = new Regex("\\[([A-Z]|[0-9]|-)+\\]");
        private static readonly Regex deviceUIDRegex = new Regex("\\[([a-z]|[0-9])+\\]");
        
        public static Dictionary<string, string> RetrieveAvailableiOSSimulators()
        {
            var availableSimulators = new Dictionary<string, string>();

            // Check if we have a physical device connected
            RedirectedProcess.Command("instruments")
                .WithArgs("-s", "devices")
                .AddOutputProcessing(message =>
                {
                    // get all simulators
                    if (message.Contains("iPhone") | message.Contains("iPad"))
                    {
                        if (simulatorUIDRegex.IsMatch(message))
                        {
                            availableSimulators[nameRegex.Match(message).Value] = simulatorUIDRegex.Match(message).Value;
                        }
                    }
                })
                .Run();

            return availableSimulators;
        }

        public static Dictionary<string, string> RetrieveAvailableiOSDevices()
        {
            var availableDevices = new Dictionary<string, string>();
            RedirectedProcess.Command("instruments")
                .WithArgs("-s", "devices")
                .AddOutputProcessing(message =>
                {
                    if (deviceUIDRegex.IsMatch(message))
                    {
                        availableDevices[nameRegex.Match(message).Value] = deviceUIDRegex.Match(message).Value;
                    }
                })
                .Run();

            return availableDevices;
        }
        
        public static void Build(string developmentTeamId)
        {
            try
            {
                EditorUtility.DisplayProgressBar("Preparing your Mobile Client", "Building your XCode project", 0f);

                if (!Directory.Exists(Path.Combine(XCodeProjectPath, XCodeProjectFile)))
                {
                    Debug.LogError("Was not able to find an XCode project. Did you build your iOS worker?");
                    return;
                }

                if (string.IsNullOrEmpty(developmentTeamId))
                {
                    Debug.LogError("Development Team Id was not specified. Unable to build the XCode project.");
                    return;
                }

                if (!TryBuildXCodeProject(developmentTeamId))
                {
                    Debug.LogError(
                        $"Failed to build your XCode project. Make sure you have xcodebuild installed and check the logs.");
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
        
        public static void Launch(bool shouldConnectLocally, string deviceId, bool useSimulator)
        {
            try 
            {
                EditorUtility.DisplayProgressBar("Preparing your Mobile Client", "Preparing launch arguments", 0.0f);

                if (!TryGetXCTestRunPath(useSimulator, out var xcTestRunPath))
                {
                    Debug.LogError(
                        "Unable to find a xctestrun file for the correct architecture. Did you build your game using the correct Target SDK?");
                    return;
                }

                if (!MobileLaunchUtils.TryPrepareArguments(shouldConnectLocally, out var arguments))
                {
                    Debug.LogError("Failed to generate arguments");
                    return;
                }

                if (!TryModifyEnvironmentVariables(xcTestRunPath, arguments))
                {
                    Debug.LogError($"Was unable to read and modify {xcTestRunPath}.");
                    return;
                }
                
                if (useSimulator)
                {
                    EditorUtility.DisplayProgressBar("Launching Mobile Client", "Start iOS Simulator", 0.5f);

                    // Start simulator
                    if (RedirectedProcess.Command("xcrun")
                        .WithArgs("instruments", "-w", deviceId, "-t", "Blank")
                        .Run() != 0)
                    {
                        Debug.LogError("Was unable to start iOS Simulator.");
                        return;
                    }
                }

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Installing your app", 0.7f);

                if (!TryLaunchApplication(deviceId, xcTestRunPath))
                {
                    Debug.LogError("Failed to start app on iOS device.");
                }

                EditorUtility.DisplayProgressBar("Launching Mobile Client", "Done", 1.0f);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
        
        private static bool TryBuildXCodeProject(string developmentTeamId)
        {
            var process = RedirectedProcess.Command("xcodebuild")
                .WithArgs("build-for-testing",
                    "-project", Path.Combine(XCodeProjectPath, XCodeProjectFile),
                    "-derivedDataPath", DerivedDataPath,
                    "-scheme", "Unity-iPhone",
                    $"DEVELOPMENT_TEAM={developmentTeamId}",
                    "-allowProvisioningUpdates")
                .Run();
            
            return process == 0;
        }

        private static bool TryLaunchApplication(string deviceId, string filePath)
        {
            var command = "osascript";
            var commandArgs = $@"-e 'tell application ""Terminal""
                                     activate
                                     do script ""xcodebuild test-without-building -destination id={deviceId} -xctestrun {filePath}""
                                     end tell'";

            var processInfo = new ProcessStartInfo(command, commandArgs)
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WorkingDirectory = Common.SpatialProjectRootDir
            };

            var process = Process.Start(processInfo);

            return process == null;
        }

        private static bool TryGetXCTestRunPath(bool forSimulator, out string xctestrunPath)
        {
            var files = Directory.GetFiles(DerivedDataPath, "*.xctestrun", SearchOption.AllDirectories);
            xctestrunPath = forSimulator 
                ? files.FirstOrDefault(file => file.Contains("iphonesimulator")) 
                : files.FirstOrDefault(file => file.Contains("iphoneos"));

            return string.IsNullOrEmpty(xctestrunPath);
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
                XmlNode spatialNode = null;
                // Navigate to the <dict> node containing all the parameters to launch the game
                var rootNode = doc.DocumentElement.ChildNodes[0].ChildNodes[1];
                for (var i = 0; i < rootNode.ChildNodes.Count; i++)
                {
                    var node = rootNode.ChildNodes[i];
                    if (node.InnerText != "EnvironmentVariables")
                    {
                        continue;
                    }

                    // add new parameters to the environment variables <dict> node
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    public static class iOSLaunchMenu
    {
        private static string XCodeProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "build", "worker", "MobileClient\\@iOS", "MobileClient\\@iOS"));
        private static string DerivedDataPath = Path.GetFullPath(Path.Combine(XCodeProjectPath, "..", "..", "build"));
        private static string XCodeProjectFile = "Unity-iPhone.xcodeproj";

        private const string MenuLaunchiOSLocal = "/iOS for local";
        private const string MenuLaunchiOSCloud = "/iOS for cloud";

#if UNITY_EDITOR_OSX
        [MenuItem(MobileLaunchHelper.MenuLaunchMobile + MenuLaunchiOSLocal, false, 75)]
        private static void LaunchiOSLocal()
        {
            LaunchiOS(true);
        }

        [MenuItem(MobileLaunchHelper.MenuLaunchMobile + MenuLaunchiOSCloud, false, 76)]
        private static void LaunchiOSCloud()
        {
            LaunchiOS(false);
        }
#endif

        private static void LaunchiOS(bool shouldConnectLocally)
        {
            try
            {
                EditorUtility.DisplayProgressBar("Preparing your Mobile Client", "Building your XCode project", 0f);

                if (!Directory.Exists(Path.Combine(XCodeProjectPath, XCodeProjectFile)))
                {
                    Debug.LogError("Was not able to find an XCode project. Did you build your iOS worker?");
                    return;
                }
                
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
                    Debug.LogError("Unable to find a xctestrun file for the correct architecture. Did you build your game using the correct Target SDK?");
                    return;
                }

                if (!MobileLaunchHelper.TryPrepareArguments(shouldConnectLocally, out var arguments))
                {
                    Debug.LogError("Failed to generate arguments");
                    return;
                }

                /*
                 * How to add SpatialOS arguments to the game as iOS environment variables.
                 * The xctestrun file contains the launch arguments for your game.
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
                    doc.Load(xcTestRunPath);
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

                    doc.Save(xcTestRunPath);

                    // UTY-2068: We currently get invalid XML using the XMLDocument object, we need to identify why this happens and fix it.
                    var text = File.ReadAllText(xcTestRunPath);
                    text = text.Replace("[]>", ">");
                    File.WriteAllText(xcTestRunPath, text);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Was unable to read and modify {xcTestRunPath}: {e}");
                    return;
                }

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
            var files = Directory.GetFiles(DerivedDataPath, "*.xctestrun", SearchOption.AllDirectories);
            xctestrunPath = forSimulator 
                ? files.FirstOrDefault(file => file.Contains("iphonesimulator")) 
                : files.FirstOrDefault(file => file.Contains("iphoneos"));

            return string.IsNullOrEmpty(xctestrunPath);
        }
    }
}

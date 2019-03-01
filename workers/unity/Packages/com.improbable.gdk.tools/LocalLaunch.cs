using Improbable.Gdk.Tools.MiniJSON;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Tools
{
    public static class LocalLaunch
    {
        private const string InspectorUrl = "http://localhost:21000/inspector";

        private static readonly string DefaultLogFileName
            = Path.GetFullPath(Path.Combine(Common.SpatialProjectRootDir, "logs", "*unityclient.log"));

        private static readonly string
            BuildPath = Path.GetFullPath(Path.Combine(Common.SpatialProjectRootDir, "build", "assembly", "worker"));

        private static readonly string ClientConfigFilename = "spatialos.UnityClient.worker.json";

        // Windows: The exit code is 0xc000013a when the user closes the console window, or presses Ctrl+C.
        private const int WindowsCtrlCExitCode = -1073741510;

        // Unix-like: The exit code is 128 + SIGINT (2).
        private const int UnixSigIntExitCode = 128 + 2;

        [MenuItem("SpatialOS/Build worker configs", false, MenuPriorities.BuildWorkerConfigs)]
        private static void BuildConfigMenu()
        {
            Debug.Log("Building worker configs...");
            EditorApplication.delayCall += BuildConfig;
        }

        [MenuItem("SpatialOS/Local launch %l", false, MenuPriorities.LocalLaunch)]
        private static void LaunchMenu()
        {
            Debug.Log("Launching SpatialOS locally...");
            EditorApplication.delayCall += LaunchLocalDeployment;
        }

        [MenuItem("SpatialOS/Launch standalone client", false, MenuPriorities.LaunchStandaloneClient)]
        private static void LaunchStandaloneClient()
        {
            Debug.Log("Launching a standalone client");
            EditorApplication.delayCall += LaunchClient;
        }

        [MenuItem("SpatialOS/Open inspector", false, MenuPriorities.OpenInspector)]
        private static void OpenInspector()
        {
            Application.OpenURL(InspectorUrl);
        }

        public static void BuildConfig()
        {
            using (new ShowProgressBarScope("Building worker configs..."))
            {
                // Run from the root of the project to build all available worker configs.
                RedirectedProcess
                    .Command(Common.SpatialBinary)
                    .WithArgs("build", "build-config", "--json_output")
                    .InDirectory(Common.SpatialProjectRootDir)
                    .Run();
            }
        }

        public static void LaunchClient()
        {
            var command = Common.SpatialBinary;
            var commandArgs = "local worker launch UnityClient default";
            var unityClientZipName = "UnityClient@Windows.zip";

            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                command = "osascript";
                commandArgs = $@"-e 'tell application ""Terminal""
                                     activate
                                     do script ""cd {Common.SpatialProjectRootDir} && {Common.SpatialBinary} {commandArgs}""
                                     end tell'";
                unityClientZipName = "UnityClient@Mac.zip";
            }

            var processInfo = new ProcessStartInfo(command, commandArgs)
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WorkingDirectory = Common.SpatialProjectRootDir
            };

            var process = Process.Start(processInfo);

            if (process == null)
            {
                Debug.LogError("Failed to start a standalone client locally.");
                return;
            }

            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) =>
            {
                // N.B. This callback is run on a different thread.
                if (process.ExitCode == 0)
                {
                    return;
                }

                var latestLogFile = GetClientLogFileFullPath();
                var latestClientBuild = Path.GetFullPath(Path.Combine(BuildPath, unityClientZipName));

                if (!File.Exists(latestClientBuild))
                {
                    Debug.LogError($"Local client build missing. Couldn't find the Unity Client at {latestClientBuild}.");
                    return;
                }

                if (!File.Exists(latestLogFile))
                {
                    Debug.LogError($"Could not find a standalone client log file {latestLogFile}.");
                    return;
                }

                var message = $"For more information, check the Unity Standalone Client local launch logfile: {latestLogFile}";

                if (WasProcessKilled(process))
                {
                    Debug.Log(message);
                }
                else
                {
                    Debug.LogError($"Errors occured - {message}");
                }

                process.Dispose();
                process = null;
            };
        }

        private static string GetClientLogFileFullPath()
        {
            try
            {
                var logConfigPath = Path.Combine(Common.SpatialProjectRootDir, "workers", "unity");
                var configFileJson = File.ReadAllText(Path.Combine(logConfigPath, ClientConfigFilename));
                var configFileJsonDeserialized = Json.Deserialize(configFileJson);
                var currentOS = Application.platform == RuntimePlatform.OSXEditor ? "macos" : "windows";
                Dictionary<string, object> partialJsonDict;

                if (!configFileJsonDeserialized.TryGetValue("external", out var externalValue))
                {
                    Debug.LogError($"Config file {ClientConfigFilename} doesn't contain key 'external'.");
                    return DefaultLogFileName;
                }

                partialJsonDict = externalValue as Dictionary<string, object>;

                if (!partialJsonDict.TryGetValue("default", out var defaultValue))
                {
                    Debug.LogError($"Config file {ClientConfigFilename} doesn't contain key 'default' within 'external'.");
                    return DefaultLogFileName;
                }

                partialJsonDict = defaultValue as Dictionary<string, object>;

                if (!partialJsonDict.TryGetValue(currentOS, out var currentOSValue))
                {
                    Debug.LogError($"Config file {ClientConfigFilename} doesn't contain key '{currentOS}' within 'external' -> 'default'.");
                    return DefaultLogFileName;
                }

                partialJsonDict = currentOSValue as Dictionary<string, object>;

                if (!partialJsonDict.TryGetValue("arguments", out var argumentsValue))
                {
                    Debug.LogError($"Config file {ClientConfigFilename} doesn't contain key 'arguments' within 'external' -> 'default' -> '{currentOS}'.");
                    return DefaultLogFileName;
                }

                var arguments = (List<object>) argumentsValue;

                var logFileArg = arguments.SkipWhile(arg => !string.Equals(arg, "-logfile")).FirstOrDefault(arg => !string.Equals(arg, "-logfile"));

                // Logger file not found - using default one
                if (logFileArg == null)
                {
                    Debug.LogError($"Config file {ClientConfigFilename} doesn't contain '-logfile' argument within 'external' -> 'default' -> '{currentOS}' -> arguments.");
                    return DefaultLogFileName;
                }

                var logFileName = Path.GetFullPath(Path.Combine(logConfigPath, (string) logFileArg));
                return logFileName;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
                return DefaultLogFileName;
            }
        }

        public static void LaunchLocalDeployment()
        {
            BuildConfig();

            var command = Common.SpatialBinary;
            var commandArgs = "local launch";

            var runtimeIp = GdkToolsConfiguration.GetOrCreateInstance().RuntimeIp;
            if (!string.IsNullOrEmpty(runtimeIp))
            {
                commandArgs = $"{commandArgs} --runtime_ip={runtimeIp}";
            }

            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                command = "osascript";
                commandArgs = $@"-e 'tell application ""Terminal""
                                     activate
                                     do script ""cd {Common.SpatialProjectRootDir} && {Common.SpatialBinary} {commandArgs}""
                                     end tell'";
            }

            var processInfo = new ProcessStartInfo(command, commandArgs)
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WorkingDirectory = Common.SpatialProjectRootDir
            };

            var process = Process.Start(processInfo);

            if (process == null)
            {
                Debug.LogError("Failed to start SpatialOS locally.");
                return;
            }

            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) =>
            {
                // N.B. This callback is run on a different thread.
                if (process.ExitCode == 0)
                {
                    return;
                }

                var logPath = Path.Combine(Common.SpatialProjectRootDir, "logs");
                var latestLogFile = Directory.GetFiles(logPath, "spatial_*.log")
                    .Select(f => new FileInfo(f))
                    .OrderBy(f => f.LastWriteTimeUtc).LastOrDefault();

                if (latestLogFile == null)
                {
                    Debug.LogError($"Could not find a spatial log file in {logPath}.");
                    return;
                }

                var message = $"For more information, check the spatial local launch logfile: {latestLogFile.FullName}";

                if (WasProcessKilled(process))
                {
                    Debug.Log(message);
                }
                else
                {
                    Debug.LogError($"Errors occured - {message}");
                }

                process.Dispose();
                process = null;
            };
        }

        private static bool WasProcessKilled(Process process)
        {
            if (process == null)
            {
                return false;
            }

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    return process.ExitCode == WindowsCtrlCExitCode;
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.LinuxEditor:
                    return process.ExitCode == UnixSigIntExitCode;
            }

            return false;
        }
    }
}

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
        private static readonly string
            SpatialProjectRootDir = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", ".."));

        // Windows: The exit code is 0xc000013a when the user closes the console window, or presses Ctrl+C.
        private const int WindowsCtrlCExitCode = -1073741510;

        // Unix-like: The exit code is 128 + SIGINT (2).
        private const int UnixSigIntExitCode = 128 + 2;

        [MenuItem("SpatialOS/Build worker configs")]
        private static void BuildConfigMenu()
        {
            Debug.Log("Building worker configs...");
            EditorApplication.delayCall += BuildConfig;
        }

        [MenuItem("SpatialOS/Local launch %l")]
        private static void LaunchMenu()
        {
            Debug.Log("Launching SpatialOS locally...");
            EditorApplication.delayCall += LaunchLocalDeployment;
        }

        [MenuItem("SpatialOS/Launch standalone client")]
        private static void LaunchAdditionalClientMenu()
        {
            Debug.Log("Launching a standalone client locally...");
            EditorApplication.delayCall += LaunchClient;
        }

        public static void BuildConfig()
        {
            using (new ShowProgressBarScope("Building worker configs..."))
            {
                // Run from the root of the project to build all available worker configs.
                RedirectedProcess.RunIn(SpatialProjectRootDir, Common.SpatialBinary, "build", "build-config",
                    "--json_output");
            }
        }

        public static void LaunchClient()
        {
            GetClientLogFilename();
            Launch("Launch standalone client", "local worker launch UnityClient default", "*unityclient.log");
        }

        public static void LaunchLocalDeployment()
        {
            BuildConfig();
            Launch("Launch SpatialOS locally", "local launch", "spatial_*.log");
        }

        private static string GetClientLogFilename()
        {
            string clientConfigFilename = "spatialos.UnityClient.worker.json";

            var logConfigPath = Path.Combine(SpatialProjectRootDir, "workers", "unity", clientConfigFilename);
            var configFileJson = File.ReadAllText(logConfigPath);
            var dict = Json.Deserialize(configFileJson) as Dictionary<string, object>;

            return "WIP";
        }

        public static void Launch(string commandName, string commandArgs, string logFileName)
        {
            var command = GetCommand();
            var commandArgsGenerated = GenerateCommandArgs(commandArgs);

            var processInfo = new ProcessStartInfo(command, commandArgsGenerated)
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WorkingDirectory = SpatialProjectRootDir
            };

            var process = Process.Start(processInfo);

            if (process == null)
            {
                Debug.LogError($"Failed to start a process for the command: {commandName}");
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

                var logPath = Path.Combine(SpatialProjectRootDir, "logs");
                var latestLogFile = Directory.GetFiles(logPath, logFileName)
                    .Select(f => new FileInfo(f))
                    .OrderBy(f => f.LastWriteTimeUtc).LastOrDefault();

                if (latestLogFile == null)
                {
                    Debug.LogError($"Could not find a log file in {logPath}.");
                    return;
                }

                var message = $"Logfile for the {commandName} command: {latestLogFile.FullName}";

                if (WasProcessKilled(process))
                {
                    Debug.Log(message);
                }
                else
                {
                    var content = File.ReadAllText(latestLogFile.FullName);
                    Debug.LogError($"{message}\n{content}");
                }

                process.Dispose();
                process = null;
            };
        }

        private static string GetCommand()
        {
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                return "osascript";
            }
            return Common.SpatialBinary;
        }

        private static string GenerateCommandArgs(string command)
        {
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                return $@"-e 'tell application ""Terminal""
                        activate
                        do script ""cd {SpatialProjectRootDir} && {Common.SpatialBinary} {command}""
                        end tell'";
            }
            return command;
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

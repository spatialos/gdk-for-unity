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

        [MenuItem("Improbable/Spatial/Build worker configs")]
        private static void BuildConfigMenu()
        {
            Debug.Log("Building worker configs...");
            EditorApplication.delayCall += BuildConfig;
        }

        [MenuItem("Improbable/Spatial/Local launch")]
        private static void LaunchMenu()
        {
            Debug.Log("Launching SpatialOS locally...");
            EditorApplication.delayCall += Launch;
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

        public static void Launch()
        {
            BuildConfig();

            var command = Common.SpatialBinary;
            var commandArgs = "local launch";
            
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                command = "osascript";
                commandArgs = $"-e 'tell application \"Terminal\"\nactivate\ndo script \"cd {SpatialProjectRootDir} && {Common.SpatialBinary} local launch\"\nend tell'";
            }

            var processInfo = new ProcessStartInfo(command, commandArgs)
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WorkingDirectory = SpatialProjectRootDir
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

                var logPath = Path.Combine(SpatialProjectRootDir, "logs");
                var latestLogFile = Directory.GetFiles(logPath, "spatial_*.log")
                    .Select(f => new FileInfo(f))
                    .OrderBy(f => f.LastWriteTimeUtc).LastOrDefault();

                if (latestLogFile == null)
                {
                    Debug.LogError($"Could not find a spatial log file in {logPath}.");
                    return;
                }

                var message = $"Spatial local launch logfile: {latestLogFile.FullName}";

                if (WasProcessKilled(process))
                {
                    Debug.Log(message);
                }
                else
                {
                    var content = File.ReadAllText(latestLogFile.FullName);
                    message = $"{message}\n{content}";

                    Debug.LogError(message);
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

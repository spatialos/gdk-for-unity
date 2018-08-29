using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    public static class LocalLaunch
    {
        private static readonly string
            SpatialProjectRootDir = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", ".."));

        // Windows: The exit code is 0xc000013a when the user closes the console window, or presses Ctrl+C.
        private const int WindowsCtrlCExitCode = -1073741510;

        // Unixy: The exit code is 128 + SIGINT (2).
        private const int UnixSigIntExitCode = 128 + 2;

        public static void BuildConfig()
        {
            using (new ShowProgressBarScope("Building worker configs..."))
            {
                // Run from the root of the project to build all available worker configs.
                Common.RunProcessIn(SpatialProjectRootDir, Common.SpatialBinary, "build", "build-config",
                    "--json_output");
            }
        }

        public static void Launch()
        {
            BuildConfig();
            var processInfo = new ProcessStartInfo("spatial", "local launch")
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                WorkingDirectory = SpatialProjectRootDir
            };

            var process = Process.Start(processInfo);

            if (process == null)
            {
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
                    UnityEngine.Debug.LogError($"Could not find a spatial log file in {logPath}.");
                    return;
                }

                var content = File.ReadAllText(latestLogFile.FullName);
                var message = $"Loaded from {latestLogFile.FullName}\n{content}";

                if (WasProcessKilled(process))
                {
                    UnityEngine.Debug.Log(message);
                }
                else
                {
                    UnityEngine.Debug.LogError(message);
                }

                process.Dispose();
                process = null;
            };
        }

        [MenuItem("Improbable/Spatial/Build worker configs")]
        private static void BuildConfigMenu()
        {
            BuildConfig();
        }

        [MenuItem("Improbable/Spatial/Local launch")]
        private static void LauncMenu()
        {
            BuildConfig();
            Launch();
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

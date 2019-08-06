using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Improbable.Gdk.Tools;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.TestUtils.Editor
{
    /// <summary>
    ///     Manages a single `spatial local launch` instance.
    /// </summary>
    public class SpatialDeploymentManager : IDisposable
    {
        private Process spatialProcess;

        public static async Task<SpatialDeploymentManager> Start(string deploymentJsonPath, string snapshotPath)
        {
            if (!File.Exists(Path.Combine(Common.SpatialProjectRootDir, deploymentJsonPath)))
            {
                throw new ArgumentException($"Could not find deployment config at {deploymentJsonPath}");
            }

            if (!File.Exists(Path.Combine(Common.SpatialProjectRootDir, snapshotPath)))
            {
                throw new ArgumentException($"Could not find snapshot at {snapshotPath}");
            }

            await BuildWorkerConfigs().ConfigureAwait(false);
            var process = await StartSpatial(deploymentJsonPath, snapshotPath).ConfigureAwait(false);

            return new SpatialDeploymentManager
            {
                spatialProcess = process
            };
        }

        private static Task BuildWorkerConfigs()
        {
            var tcs = new TaskCompletionSource<bool>();

            var processInfo =
                new ProcessStartInfo("spatial", "build build-config")
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = Common.SpatialProjectRootDir
                };

            var process = new Process
            {
                StartInfo = processInfo,
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                if (process.ExitCode == 0)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.TrySetException(
                        new Exception($"Failed to build worker configs. Raw output:\n{process.StandardOutput.ReadToEnd()}\n Raw stderr: \n{process.StandardError.ReadToEnd()}"));
                }
            };

            process.Start();

            return tcs.Task;
        }

        private static Task<Process> StartSpatial(string deploymentJsonPath, string snapshotPath)
        {
            var tcs = new TaskCompletionSource<Process>();

            var processInfo =
                new ProcessStartInfo("spatial", $"local launch {deploymentJsonPath} --snapshot {snapshotPath} --enable_pre_run_check=false")
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = Common.SpatialProjectRootDir
                };

            var process = new Process
            {
                StartInfo = processInfo,
                EnableRaisingEvents = true
            };

            var output = new StringBuilder();

            process.Exited += (sender, args) =>
            {
                tcs.TrySetException(
                    new Exception($"Spatial process failed to start. Raw output:\n{output.ToString()}"));
            };

            process.OutputDataReceived += (sender, args) =>
            {
                if (string.IsNullOrEmpty(args.Data))
                {
                    return;
                }

                if (args.Data.Contains("localhost:21000/inspector-v2"))
                {
                    tcs.TrySetResult(process);
                }

                output.AppendLine(args.Data);
            };

            process.Start();
            process.BeginOutputReadLine();

            return tcs.Task;
        }


        public void Dispose()
        {
            var success = spatialProcess.KillTree();

            if (success != 0)
            {
                Debug.LogWarning("Failed to stop spatial process tree.");
            }
        }
    }

    // Copied from: https://raw.githubusercontent.com/dotnet/cli/master/test/Microsoft.DotNet.Tools.Tests.Utilities/Extensions/ProcessExtensions.cs
    // Licensed under MIT.
    public static class ProcessExtensions
    {
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(value: 30);

        public static int KillTree(this Process process)
        {
            return process.KillTree(DefaultTimeout);
        }

        public static int KillTree(this Process process, TimeSpan timeout)
        {
            if (IsWindows)
            {
                return RunProcessAndWaitForExit(
                    "taskkill",
                    $"/T /F /PID {process.Id}",
                    timeout,
                    out _,
                    out _);
            }

            var children = new HashSet<int>();
            if (GetAllChildIdsUnix(process.Id, children, timeout) != 0)
            {
                return 1;
            }

            foreach (var childId in children)
            {
                if (KillProcessUnix(childId, timeout) != 0)
                {
                    return 1;
                }
            }

            return KillProcessUnix(process.Id, timeout);
        }

        private static int GetAllChildIdsUnix(int parentId, ISet<int> children, TimeSpan timeout)
        {
            var exitCode = RunProcessAndWaitForExit(
                "pgrep",
                $"-P {parentId}",
                timeout,
                out var stdout,
                out _);

            if (exitCode == 0 && !string.IsNullOrEmpty(stdout))
            {
                using (var reader = new StringReader(stdout))
                {
                    while (true)
                    {
                        var text = reader.ReadLine();

                        if (string.IsNullOrEmpty(text))
                        {
                            break;
                        }

                        if (int.TryParse(text, out var id))
                        {
                            children.Add(id);
                            // Recursively get the children
                            GetAllChildIdsUnix(id, children, timeout);
                        }
                    }
                }
            }

            return exitCode;
        }

        private static int KillProcessUnix(int processId, TimeSpan timeout)
        {
            return RunProcessAndWaitForExit(
                "kill",
                $"-TERM {processId}",
                timeout,
                out _,
                out _);
        }

        private static int RunProcessAndWaitForExit(string fileName, string arguments, TimeSpan timeout,
            out string stdout, out string stderr)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            var process = Process.Start(startInfo);

            stdout = null;
            stderr = null;

            if (process.WaitForExit((int) timeout.TotalMilliseconds))
            {
                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();
            }
            else
            {
                process.Kill();
            }

            return process.ExitCode;
        }
    }
}

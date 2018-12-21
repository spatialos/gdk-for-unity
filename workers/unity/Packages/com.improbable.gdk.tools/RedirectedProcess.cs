using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Improbable.Gdk.Tools.MiniJSON;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Tools
{
    public class RedirectedProcessResult
    {
        public int ExitCode;
        public List<string> Stdout;
        public List<string> Stderr;
    }

    /// <summary>
    ///     Runs a windowless process with its stdout/stderr redirected to the Unity console as a single debug print at the
    ///     end.
    /// </summary>
    public class RedirectedProcess
    {
        /// <summary>
        ///     Runs the redirected process and waits for it to return.
        /// </summary>
        /// <param name="command">The filename to run.</param>
        /// <param name="arguments">Parameters that will be passed to the command.</param>
        /// <returns>The exit code.</returns>
        public static int Run(string command, params string[] arguments)
        {
            return RunIn(Path.GetFullPath(Path.Combine(Application.dataPath, "..")), command, arguments);
        }

        /// <summary>
        ///     Runs the redirected process and waits for it to return.
        /// </summary>
        /// <param name="workingDirectory">The directory to run the command from.</param>
        /// <param name="command">The filename to run.</param>
        /// <param name="arguments">Parameters that will be passed to the command.</param>
        /// <returns>The exit code.</returns>
        public static int RunIn(string workingDirectory, string command, params string[] arguments)
        {
            var info = new ProcessStartInfo(command, string.Join(" ", arguments))
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory
            };

            using (var process = Process.Start(info))
            {
                if (process == null)
                {
                    throw new Exception(
                        $"Failed to run {info.FileName} {info.Arguments}\nIs the .NET Core SDK installed?");
                }

                process.EnableRaisingEvents = true;

                var processOutput = new StringBuilder();

                void OnReceived(object sender, DataReceivedEventArgs args)
                {
                    if (string.IsNullOrEmpty(args.Data))
                    {
                        return;
                    }

                    lock (processOutput)
                    {
                        processOutput.AppendLine(ProcessSpatialOutput(args.Data));
                    }
                }

                process.OutputDataReceived += OnReceived;
                process.ErrorDataReceived += OnReceived;

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                // Ensure that the first line of the log is something useful in the Unity editor console.
                var trimmedOutput = processOutput.ToString().Trim();

                if (!string.IsNullOrEmpty(trimmedOutput))
                {
                    if (process.ExitCode == 0)
                    {
                        Debug.Log(trimmedOutput);
                    }
                    else
                    {
                        Debug.LogError(trimmedOutput);
                    }
                }

                return process.ExitCode;
            }
        }

        /// <summary>
        ///     Runs the redirected process and returns a task which can be waited on.
        /// </summary>
        /// <param name="workingDirectory">The directory to run the command from.</param>
        /// <param name="command">The filename to run.</param>
        /// <param name="arguments">Parameters that will be passed to the command.</param>
        /// <param name="redirectStdout">Redirect standard output to Debug.Log</param>
        /// <param name="redirectStderr">Redirect standard error to Debug.LogError</param>
        /// <returns>A task which would return the exit code and output.</returns>
        public static async Task<RedirectedProcessResult> RunInAsync(string workingDirectory, string command, string[] arguments, bool redirectStdout = false, bool redirectStderr = false)
        {
            var info = new ProcessStartInfo(command, string.Join(" ", arguments))
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory
            };

            return await Task.Run(() =>
            {
                using (var process = Process.Start(info))
                {
                    if (process == null)
                    {
                        throw new Exception(
                            $"Failed to run {info.FileName} {info.Arguments}\nIs the .NET Core SDK installed?");
                    }

                    process.EnableRaisingEvents = true;

                    var processStandardOutput = new List<string>();
                    var processStandardError = new List<string>();

                    void OnStandardOutput(object sender, DataReceivedEventArgs args)
                    {
                        if (!string.IsNullOrEmpty(args.Data))
                        {
                            if (redirectStdout)
                            {
                                Debug.Log(args.Data);
                            }
                            lock (processStandardOutput)
                            {
                                processStandardOutput.Add(args.Data);
                            }
                        }
                    }

                    void OnStandardError(object sender, DataReceivedEventArgs args)
                    {
                        if (!string.IsNullOrEmpty(args.Data))
                        {
                            if (redirectStdout)
                            {
                                Debug.LogError(args.Data);
                            }
                            lock (processStandardOutput)
                            {
                                processStandardError.Add(args.Data);
                            }
                        }
                    }

                    process.OutputDataReceived += OnStandardOutput;
                    process.ErrorDataReceived += OnStandardError;

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    return new RedirectedProcessResult
                    {
                        ExitCode = process.ExitCode,
                        Stdout = processStandardOutput,
                        Stderr = processStandardError
                    };
                }
            });
        }

        private static string ProcessSpatialOutput(string argsData)
        {
            if (!argsData.StartsWith("{") || !argsData.EndsWith("}"))
            {
                return argsData;
            }

            try
            {
                var logEvent = Json.Deserialize(argsData);
                if (logEvent.TryGetValue("msg", out var message))
                {
                    return (string) message;
                }
            }
            catch
            {
                return argsData;
            }

            return argsData;
        }
    }
}

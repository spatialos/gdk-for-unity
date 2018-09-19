using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Improbable.Gdk.Tools.MiniJSON;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Tools
{
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
        /// <param name="workingDirectory">The directory to run the filename from.</param>
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

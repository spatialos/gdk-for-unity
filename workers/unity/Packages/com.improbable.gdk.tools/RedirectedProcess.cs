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
        private string RunningCommand = string.Empty;
        private string[] Arguments;
        private string WorkingDirectory = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        private Action<string> OutputProcessor;
        private Action<string> ErrorProcessor;

        /// <summary>
        ///     Runs the redirected process and waits for it to return.
        /// </summary>
        /// <param name="command">The filename to run.</param>
        /// <param name="arguments">Parameters that will be passed to the command.</param>
        /// <returns>The exit code.</returns>
        // public static int Run(string command, params string[] arguments)
        // {
        //     return RunIn(Path.GetFullPath(Path.Combine(Application.dataPath, "..")), command, arguments);
        // }
        public static RedirectedProcess CommandWithArgs(string command, params string[] arguments)
        {
            var redirectedProcess = new RedirectedProcess();
            redirectedProcess.RunningCommand = command;
            redirectedProcess.Arguments = arguments;
            return redirectedProcess;
        }

        public RedirectedProcess InDirectory(string directory)
        {
            WorkingDirectory = directory;
            return this;
        }

        public RedirectedProcess ProcessOutput(Action<string> outputProcessor)
        {
            OutputProcessor = outputProcessor;
            return this;
        }

        public RedirectedProcess ProcessErrors(Action<string> errorProcessor)
        {
            ErrorProcessor = errorProcessor;
            return this;
        }

        /// <summary>
        ///     Runs the redirected process and waits for it to return.
        /// </summary>
        /// <param name="workingDirectory">The directory to run the command from.</param>
        /// <param name="command">The filename to run.</param>
        /// <param name="arguments">Parameters that will be passed to the command.</param>
        /// <returns>The exit code.</returns>
        // public static int RunIn(string workingDirectory, string command, params string[] arguments)
        public int Run()
        {
            var info = new ProcessStartInfo(RunningCommand, string.Join(" ", Arguments))
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = WorkingDirectory
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

                void OnReceived(string output)
                {
                    lock (processOutput)
                    {
                        processOutput.AppendLine(ProcessSpatialOutput(output));
                    }
                }

                OutputProcessor = OutputProcessor ?? OnReceived;
                ErrorProcessor = ErrorProcessor ?? OnReceived;

                void OnStandardOutput(object sender, DataReceivedEventArgs args)
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        OutputProcessor(args.Data);
                    }
                }

                void OnStandardError(object sender, DataReceivedEventArgs args)
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        ErrorProcessor(args.Data);
                    }
                }

                process.OutputDataReceived += OnStandardOutput;
                process.ErrorDataReceived += OnStandardError;

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
        public async Task<RedirectedProcessResult> RunInAsync(bool redirectStdout = false, bool redirectStderr = false)
        {
            return await Task.Run(() =>
            {
                var processStandardOutput = new List<string>();
                var processStandardError = new List<string>();

                void OnStandardOutput(string output)
                {
                    if (redirectStdout)
                    {
                        Debug.Log(output);
                    }

                    lock (processStandardOutput)
                    {
                        processStandardOutput.Add(output);
                    }
                }

                void OnStandardError(string error)
                {
                    if (redirectStderr)
                    {
                        Debug.LogError(error);
                    }

                    lock (processStandardOutput)
                    {
                        processStandardError.Add(error);
                    }
                }

                OutputProcessor = OnStandardOutput;
                ErrorProcessor = OnStandardError;

                var exitCode = Run();

                return new RedirectedProcessResult
                {
                    ExitCode = exitCode,
                    Stdout = processStandardOutput,
                    Stderr = processStandardError
                };
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

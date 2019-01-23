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
    ///     Runs a windowless process.
    /// </summary>
    public class RedirectedProcess
    {
        private string command = string.Empty;
        private string[] arguments;
        private string workingDirectory = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        private Action<string> outputProcessor;
        private Action<string> errorProcessor;

        /// <summary>
        ///     Creates the redirected process for the command.
        /// </summary>
        /// <param name="command">The filename to run.</param>
        public static RedirectedProcess Command(string command)
        {
            var redirectedProcess = new RedirectedProcess { command = command };
            return redirectedProcess;
        }

        /// <summary>
        ///     Adds arguments to process command call.
        /// </summary>
        /// <param name="arguments">Parameters that will be passed to the command.</param>
        public RedirectedProcess WithArgs(params string[] arguments)
        {
            this.arguments = arguments;
            return this;
        }

        /// <summary>
        ///     Sets which directory run the process in.
        /// </summary>
        /// <param name="directory">Working directory of the process.</param>
        public RedirectedProcess InDirectory(string directory)
        {
            workingDirectory = directory;
            return this;
        }

        /// <summary>
        ///     Sets custom processing for regular output of process.
        /// </summary>
        /// <param name="outputProcessor">Processing action for regular output.</param>
        public RedirectedProcess ProcessOutput(Action<string> outputProcessor)
        {
            this.outputProcessor = outputProcessor;
            return this;
        }

        /// <summary>
        ///     Sets custom processing for error output of process.
        /// </summary>
        /// <param name="errorProcessor">Processing action for error output.</param>
        public RedirectedProcess ProcessErrors(Action<string> errorProcessor)
        {
            this.errorProcessor = errorProcessor;
            return this;
        }

        /// <summary>
        ///     Runs the redirected process and waits for it to return.
        /// </summary>
        public int Run()
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

                StringBuilder outputLog = null;
                if (outputProcessor == null || errorProcessor == null)
                {
                    outputLog = new StringBuilder();
                }

                void OnReceived(string output)
                {
                    lock (outputLog)
                    {
                        outputLog.AppendLine(ProcessSpatialOutput(output));
                    }
                }

                outputProcessor = outputProcessor ?? OnReceived;
                errorProcessor = errorProcessor ?? OnReceived;

                process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs args)
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        outputProcessor(args.Data);
                    }
                };
                process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs args)
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        errorProcessor(args.Data);
                    }
                };

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                if (outputLog == null)
                {
                    return process.ExitCode;
                }

                // Ensure that the first line of the log is something useful in the Unity editor console.
                var trimmedOutput = outputLog.ToString().Trim();

                if (string.IsNullOrEmpty(trimmedOutput))
                {
                    return process.ExitCode;
                }

                if (process.ExitCode == 0)
                {
                    Debug.Log(trimmedOutput);
                }
                else
                {
                    Debug.LogError(trimmedOutput);
                }

                return process.ExitCode;
            }
        }

        /// <summary>
        ///     Runs the redirected process and returns a task which can be waited on.
        /// </summary>
        /// <param name="redirectStdout">Redirect standard output to Debug.Log</param>
        /// <param name="redirectStderr">Redirect standard error to Debug.LogError</param>
        /// <returns>A task which would return the exit code and output.</returns>
        public async Task<RedirectedProcessResult> RunAsync(bool redirectStdout = false, bool redirectStderr = false)
        {
            return await Task.Run(() =>
            {
                var processStandardOutput = new List<string>();
                var processStandardError = new List<string>();

                outputProcessor = delegate(string output)
                {
                    if (redirectStdout)
                    {
                        Debug.Log(output);
                    }

                    lock (processStandardOutput)
                    {
                        processStandardOutput.Add(output);
                    }
                };
                errorProcessor = delegate(string error)
                {
                    if (redirectStderr)
                    {
                        Debug.LogError(error);
                    }

                    lock (processStandardOutput)
                    {
                        processStandardError.Add(error);
                    }
                };

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

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

    [Flags]
    public enum OutputRedirectBehaviour
    {
        /// <summary>
        ///     <para>No redirected output, only custom outputProcessors are used</para>
        /// </summary>
        None = 0,

        /// <summary>
        ///     <para>Standard output is immediately redirected to Debug.Log</para>
        /// </summary>
        RedirectStdOut = 1,

        /// <summary>
        ///     <para>Error output is immediately redirected to Debug.LogError</para>
        /// </summary>
        RedirectStdErr = 2,

        /// <summary>
        ///     <para>All output is accumulated and then redirected to Debug.Log after the process has finished</para>
        /// </summary>
        RedirectAccumulatedOutput = 4,

        /// <summary>
        ///     <para>If set will process contained `spatial` output and extract it's messages from JSON</para>
        /// </summary>
        ProcessSpatialOutput = 8,
    }

    /// <summary>
    ///     Runs a windowless process.
    /// </summary>
    public class RedirectedProcess
    {
        private string command = string.Empty;
        private string[] arguments;
        private string workingDirectory = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        private readonly List<Action<string>> outputProcessors = new List<Action<string>>();
        private readonly List<Action<string>> errorProcessors = new List<Action<string>>();

        private OutputRedirectBehaviour outputRedirectBehaviour =
            OutputRedirectBehaviour.ProcessSpatialOutput |
            OutputRedirectBehaviour.RedirectAccumulatedOutput;

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
        ///     Adds custom processing for regular output of process.
        /// </summary>
        /// <param name="outputProcessor">Processing action for regular output.</param>
        public RedirectedProcess AddOutputProcessing(Action<string> outputProcessor)
        {
            outputProcessors.Add(outputProcessor);
            return this;
        }

        /// <summary>
        ///     Adds custom processing for error output of process.
        /// </summary>
        /// <param name="errorProcessor">Processing action for error output.</param>
        public RedirectedProcess AddErrorProcessing(Action<string> errorProcessor)
        {
            errorProcessors.Add(errorProcessor);
            return this;
        }

        /// <summary>
        ///     Adds custom processing for error output of process.
        /// </summary>
        /// <param name="redirectBehaviour">Options for redirecting process output to Debug.Log().</param>
        public RedirectedProcess RedirectOutputOptions(OutputRedirectBehaviour redirectBehaviour)
        {
            outputRedirectBehaviour = redirectBehaviour;
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

            using (var process = new Process())
            {
                process.StartInfo = info;


                StringBuilder outputLog = null;
                if ((outputRedirectBehaviour & OutputRedirectBehaviour.RedirectAccumulatedOutput) !=
                    OutputRedirectBehaviour.None)
                {
                    outputLog = new StringBuilder();
                }

                process.OutputDataReceived += (sender, args) =>
                {
                    var outputString = args.Data;
                    if (string.IsNullOrEmpty(outputString))
                    {
                        return;
                    }

                    if ((outputRedirectBehaviour & OutputRedirectBehaviour.ProcessSpatialOutput) != OutputRedirectBehaviour.None)
                    {
                        outputString = ProcessSpatialOutput(outputString);
                    }

                    if ((outputRedirectBehaviour & OutputRedirectBehaviour.RedirectStdOut) !=
                        OutputRedirectBehaviour.None)
                    {
                        Debug.Log(outputString);
                    }

                    if (outputLog != null)
                    {
                        lock (outputLog)
                        {
                            outputLog.AppendLine(ProcessSpatialOutput(outputString));
                        }
                    }

                    foreach (var outputProcessor in outputProcessors)
                    {
                        try
                        {
                            outputProcessor.Invoke(outputString);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                };

                process.ErrorDataReceived += (sender, args) =>
                {
                    var errorString = args.Data;
                    if (string.IsNullOrEmpty(errorString))
                    {
                        return;
                    }

                    if ((outputRedirectBehaviour & OutputRedirectBehaviour.ProcessSpatialOutput) != OutputRedirectBehaviour.None)
                    {
                        errorString = ProcessSpatialOutput(errorString);
                    }

                    if ((outputRedirectBehaviour & OutputRedirectBehaviour.RedirectStdErr) !=
                        OutputRedirectBehaviour.None)
                    {
                        Debug.LogError(errorString);
                    }

                    if (outputLog != null)
                    {
                        lock (outputLog)
                        {
                            outputLog.AppendLine(ProcessSpatialOutput(errorString));
                        }
                    }

                    foreach (var errorProcessor in errorProcessors)
                    {
                        try
                        {
                            errorProcessor.Invoke(errorString);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                };

                process.EnableRaisingEvents = true;

                if (!process.Start())
                {
                    throw new Exception(
                        $"Failed to run {info.FileName} {info.Arguments}");
                }

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                if (outputLog == null)
                {
                    return process.ExitCode;
                }

                // Ensure that the first line of the log is something useful in the Unity editor console.
                var trimmedOutput = outputLog.ToString().TrimStart();

                if (trimmedOutput == string.Empty)
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
        /// <returns>A task which would return the exit code and output.</returns>
        public Task<RedirectedProcessResult> RunAsync()
        {
            return Task.Run(() =>
            {
                var processStandardOutput = new List<string>();
                var processStandardError = new List<string>();

                AddOutputProcessing(output =>
                {
                    lock (processStandardOutput)
                    {
                        processStandardOutput.Add(output);
                    }
                });
                AddErrorProcessing(error =>
                {
                    lock (processStandardOutput)
                    {
                        processStandardError.Add(error);
                    }
                });

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

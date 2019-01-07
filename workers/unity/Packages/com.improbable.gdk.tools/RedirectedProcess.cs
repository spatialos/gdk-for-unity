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
    public enum ProcessOutputOptions
    {
        /// <summary>
        ///   <para>No redirected output, only custom outputProcessors are used</para>
        /// </summary>
        None = 0,

        /// <summary>
        ///   <para>Standard output is immediately redirected to Debug.Log</para>
        /// </summary>
        RedirectStdOut = 1,

        /// <summary>
        ///   <para>Error output is immediately redirected to Debug.LogError</para>
        /// </summary>
        RedirectStdErr = 2,

        /// <summary>
        ///   <para>All output is accumulated and then redirected to Debug.Log after the process has finished</para>
        /// </summary>
        RedirectAccumulatedOutput = 4,

        /// <summary>
        ///   <para>If set will process contained `spatial` output and extract it's messages from JSON</para>
        /// </summary>
        ProcessSpatialOutput = 8,
    }

    /// <summary>
    ///     Runs a windowless process.
    /// </summary>
    public static class RedirectedProcess
    {
        private string command = string.Empty;
        private string[] arguments;
        private string workingDirectory = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        private readonly List<Action<string>> outputProcessors = new List<Action<string>>();
        private readonly List<Action<string>> errorProcessors = new List<Action<string>>();

        private ProcessOutputOptions outputOptions =
            ProcessOutputOptions.ProcessSpatialOutput |
            ProcessOutputOptions.RedirectAccumulatedOutput;

        private bool returnImmediately = false;

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
        /// <param name="options">Options for redirecting process output to Debug.Log().</param>
        public RedirectedProcess RedirectOutputOptions(ProcessOutputOptions options)
        {
            outputOptions = options;
            return this;
        }

        /// <summary>
        ///     Informs the class to return without waiting for redirected process to finish
        /// </summary>
        public RedirectedProcess ReturnImmediately()
        {
            returnImmediately = true;
            return this;
        }

        /// <summary>
        ///     Runs redirected process in specified output directory with its stdout/stderr redirected to the
        ///     Unity console as a single debug print at the end.
        /// </summary>
        public int Run()
        {
            var processOutput = new StringBuilder();

            void ProcessOutput(string output)
            {
                lock (processOutput)
                {
                    processOutput.AppendLine(ProcessSpatialOutput(output));
                }
            }

            var exitCode = RunRedirectedProcess(workingDirectory, command, null, ProcessOutput, true, arguments);

            var trimmedOutput = processOutput.ToString().Trim();

            if (string.IsNullOrEmpty(trimmedOutput))
            {
                return exitCode;
            }

            if (exitCode == 0)
            {
                Debug.Log(trimmedOutput);
            }
            else
            {
                Debug.LogError(trimmedOutput);
            }

            return exitCode;
        }

        /// <summary>
        ///     Runs redirected process in Application root directory and extracts returns output in output string.
        /// </summary>
        /// <param name="command">The filename to run.</param>
        /// <param name="output">String containing output of the process.</param>
        /// <param name="arguments">Parameters that will be passed to the command.</param>
        /// <returns>The exit code.</returns>
        public static int RunExtractOutput(string command, out string output, params string[] arguments)
        {
            var processOutput = new StringBuilder();

            void ProcessOutput(string outputLine)
            {
                lock (processOutput)
                {
                    processOutput.AppendLine(outputLine);
                }
            }

            var exitCode = RunRedirectedProcess(AppDataPath, command, null, ProcessOutput, true, arguments);

            output = processOutput.ToString().Trim();
            return exitCode;
        }

        /// <summary>
        ///     Runs redirected process in Application root directory with its stdout/stderr redirected to the
        ///     Unity console immediately.
        /// </summary>
        /// <param name="command">The filename to run.</param>
        /// <param name="arguments">Parameters that will be passed to the command.</param>
        /// <returns>The exit code.</returns>
        public static int RunWithImmediateOutput(string command, params string[] arguments)
        {
            return RunWithEnvVars(command, null, arguments);
        }

        /// <summary>
        ///     Runs redirected process in Application root directory with given environment variables.
        ///     Stdout/stderr are redirected to the Unity console immediately.
        /// </summary>
        /// <param name="command">The filename to run.</param>
        /// <param name="envVars">Dictionary containing environment variables to be used.</param>
        /// <param name="arguments">Parameters that will be passed to the command.</param>
        /// <returns>The exit code.</returns>
        public static int RunWithEnvVars(string command, Dictionary<string, string> envVars,
            params string[] arguments)
        {
            void ProcessOutput(string output)
            {
                output = output.Trim();
                if (!string.IsNullOrEmpty(output))
                {
                    Debug.Log(output);
                }
            }

            return RunRedirectedProcess(AppDataPath, command, envVars, ProcessOutput, false, arguments);
        }

        /// <summary>
        ///     Runs the redirected process and waits for it to return.
        /// </summary>
        /// <param name="workingDirectory">The directory to run the filename from.</param>
        /// <param name="command">The filename to run.</param>
        /// <param name="envVars">Dictionary containing environment variables to be used.</param>
        /// <param name="outputProcessor">Action for processing output line by line.</param>
        /// <param name="arguments">Parameters that will be passed to the command.</param>
        /// <returns>The exit code.</returns>
        private static int RunRedirectedProcess(string workingDirectory, string command, Dictionary<string, string> envVars,
            Action<string> outputProcessor, bool waitForExit, params string[] arguments)
        {
            var info = new ProcessStartInfo(command, string.Join(" ", arguments))
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory
            };

            foreach (var envVarEntry in envVars ?? new Dictionary<string, string>())
            {
                info.EnvironmentVariables.Add(envVarEntry.Key, envVarEntry.Value);
            }

            var process = Process.Start(info);
            if (process == null)
            {
                throw new Exception(
                    $"Failed to run {info.FileName} {info.Arguments}\nIs the .NET Core SDK installed?");
            }

            StringBuilder outputLog = null;
            if ((outputOptions & ProcessOutputOptions.RedirectAccumulatedOutput) != ProcessOutputOptions.None)
            {
                outputLog = new StringBuilder();
            }

            process.OutputDataReceived += (sender, args) =>
            {
                var outputString = args.Data;
                if ((outputOptions & ProcessOutputOptions.ProcessSpatialOutput) != ProcessOutputOptions.None)
                {
                    outputString = ProcessSpatialOutput(outputString);
                }

                if (string.IsNullOrEmpty(outputString))
                {
                    return;
                }

                if ((outputOptions & ProcessOutputOptions.RedirectStdOut) != ProcessOutputOptions.None)
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
                    outputProcessor(outputString);
                }
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                var errorString = args.Data;
                if ((outputOptions & ProcessOutputOptions.ProcessSpatialOutput) != ProcessOutputOptions.None)
                {
                    errorString = ProcessSpatialOutput(errorString);
                }

                if (string.IsNullOrEmpty(errorString))
                {
                    return;
                }

                if ((outputOptions & ProcessOutputOptions.RedirectStdErr) != ProcessOutputOptions.None)
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
                    errorProcessor(errorString);
                }
            };

            process.EnableRaisingEvents = true;
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            if (returnImmediately)
            {
                process.Exited += (sender, args) =>
                {
                    process.Dispose();
                };
                return 0;
            }

            process.WaitForExit();
            var exitCode = process.ExitCode;
            process.Dispose();

            if (outputLog == null)
            {
                return exitCode;
            }

            // Ensure that the first line of the log is something useful in the Unity editor console.
            var trimmedOutput = outputLog.ToString().TrimStart();

            if (string.IsNullOrEmpty(trimmedOutput))
            {
                return exitCode;
            }

            if (exitCode == 0)
            {
                Debug.Log(trimmedOutput);
            }
            else
            {
                Debug.LogError(trimmedOutput);
            }

            return exitCode;
        }

        /// <summary>
        ///     Runs the redirected process and returns a task which can be waited on.
        /// </summary>
        /// <returns>A task which would return the exit code and output.</returns>
        public async Task<RedirectedProcessResult> RunAsync()
        {
            return await Task.Run(() =>
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

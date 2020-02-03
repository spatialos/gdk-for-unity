using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Utils
{
    public static class SystemTools
    {
        public static void EnsureDirectoryEmpty(string dir)
        {
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }

            Directory.CreateDirectory(dir);
        }

        public static void RunRedirected(string command, List<string> arguments)
        {
            var exe = Path.GetFileNameWithoutExtension(command);
            var logger = LogManager.GetLogger($"Command: {exe}");

            command = Environment.ExpandEnvironmentVariables(command);
            command = Path.GetFullPath(command);
            arguments = arguments.Select(Environment.ExpandEnvironmentVariables).ToList();

            var startInfo = new ProcessStartInfo(command, string.Join(" ", arguments.ToArray()))
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false
            };

            try
            {
                using (var process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        throw new NullReferenceException();
                    }

                    process.EnableRaisingEvents = true;
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            logger.Info(e.Data);
                        }
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            logger.Error(e.Data);
                        }
                    };

                    // Async print lines of output as they come in.
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Exit code {process.ExitCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                var stringArgs = string.Join("\n\t", arguments);
                throw new Exception($"{ex.Message} while running:\n{command}\n\t{stringArgs}");
            }
        }
    }
}

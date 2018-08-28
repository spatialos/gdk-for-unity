using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Improbable
{
    internal class Common
    {
        static Common()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                Console.WriteLine(((Exception) eventArgs.ExceptionObject).Message);
                Environment.Exit(1);
            };
        }

        public static void EnsureDirectoryEmpty(string dir)
        {
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }

            Directory.CreateDirectory(dir);
        }

        public static int RunRedirectedWithExitCode(string command, params string[] arguments)
        {
            command = Environment.ExpandEnvironmentVariables(command);
            arguments = arguments.Select(Environment.ExpandEnvironmentVariables).ToArray();

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
                    if (process != null)
                    {
                        process.EnableRaisingEvents = true;
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                Console.WriteLine("{0}", e.Data);
                            }
                        };
                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                Console.Error.WriteLine("{0}", e.Data);
                            }
                        };

                        // Async print lines of output as they come in.
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        process.WaitForExit();

                        return process.ExitCode;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message} while running:\n{command}\n\t{string.Join("\n\t", arguments)}");
            }

            return 1;
        }

        public static void RunRedirected(string command, params string[] arguments)
        {
            var exitCode = RunRedirectedWithExitCode(command, arguments);
            if (exitCode != 0)
            {
                throw new Exception(
                    $"Exit code {exitCode} while running:\n{command}\n\t{string.Join("\n\t", arguments)}");
            }
        }
    }
}

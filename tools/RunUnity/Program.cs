using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityPaths;

namespace RunUnity
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("--help") || args.Contains("/?") || args.Contains("/help"))
            {
                Paths.PrintHelp();
                Environment.Exit(0);
            }

            try
            {
                string location = string.Empty;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    location = "Unity.app/Contents/MacOS/Unity";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    location = "Editor\\Unity.exe";
                }
                else
                {
                    throw new Exception($"Platform '{RuntimeInformation.OSDescription}' is unsupported.");
                }

                var path = Path.Combine(Paths.TryGetUnityPath(), location);
                Console.WriteLine($"Found Unity in {path}");
                var arguments = string.Join(' ', args);
                using (var process = Process.Start(path, arguments))
                {
                    if (process == null)
                    {
                        throw new Exception("Process failed to start");
                    }

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Process exited with a non-zero error code ({process.ExitCode})");
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}

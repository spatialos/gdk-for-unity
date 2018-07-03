using System;
using System.IO;
using System.Linq;

namespace FindUnity
{
    internal static class Program
    {
        // If SpecialFolder.ProgramFiles returns the (x86) variant instead of "Program Files",
        // check that "Prefer 32 bit" is unticked in the project's Build settings.
        private static readonly string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private static readonly string UnityHubPath = Path.Combine(ProgramFiles, "Unity", "Hub");
        private static readonly string UnityPath = Path.Combine(ProgramFiles, "Unity");
        private const string UnityHomeEnvVar = "UNITY_HOME";

        private static void Main(string[] args)
        {
            if (args.Contains("--help") || args.Contains("/?") || args.Contains("/help"))
            {
                Console.Out.WriteLine("Looks for a path to Unity in the following locations, in order:");
                Console.Out.WriteLine($"  1) The path set in the environment variable {UnityHomeEnvVar}");
                Console.Out.WriteLine($"  2) The default installation path for the Unity Hub ({UnityHubPath})");
                Console.Out.WriteLine($"  3) The default installation path for Unity ({UnityPath})");
                Environment.Exit(0);
            }

            try
            {
                if (!Directory.Exists("Assets"))
                {
                    throw new Exception("Please run from within a Unity project");
                }

                var unityHome = Environment.GetEnvironmentVariable(UnityHomeEnvVar);
                if (!string.IsNullOrEmpty(unityHome))
                {
                    if (!Directory.Exists(unityHome))
                    {
                        throw new Exception($"{UnityHomeEnvVar}='{unityHome}', but this path does not exist.");
                    }

                    Console.Out.WriteLine(unityHome);
                    Environment.Exit(0);
                }

                var projectVersion = File.ReadAllText(Path.Combine("ProjectSettings", "ProjectVersion.txt"))
                    .Remove(0, "m_EditorVersion:".Length).Trim();

                var hubPath = Path.Combine(UnityHubPath, "Editor", projectVersion);
                if (Directory.Exists(hubPath))
                {
                    Console.Out.WriteLine(hubPath);
                    Environment.Exit(0);
                }

                if (Directory.Exists(Path.Combine(UnityPath, "Editor")))
                {
                    Console.Out.WriteLine(UnityPath);
                    Environment.Exit(0);
                }

                throw new Exception($"Could not find Unity in\n  {UnityHomeEnvVar}={unityHome}\n  {hubPath}\n  {UnityPath}");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}

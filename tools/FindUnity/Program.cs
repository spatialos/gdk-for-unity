using System;
using System.IO;
using System.Linq;

namespace FindUnity
{
    internal static class Program
    {
        private const string UnityHomeEnvVar = "UNITY_HOME";

        private static void Main(string[] args)
        {
            // If SpecialFolder.ProgramFiles returns the (x86) variant instead of "Program Files",
            // check that "Prefer 32 bit" is unticked in the project's Build settings.
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var unityHubPath = Path.Combine(programFiles, "Unity", "Hub");
            var unityPath = Path.Combine(programFiles, "Unity");
            var improbableUnityRootPath = programFiles == "/Applications" ? "/Applications" : Path.Combine(@"c:\", "Unity");

            if (args.Contains("--help") || args.Contains("/?") || args.Contains("/help"))
            {
                Console.Out.WriteLine("Looks for a path to Unity in the following locations, in order:");
                Console.Out.WriteLine($"  1) The path set in the environment variable {UnityHomeEnvVar}");
                Console.Out.WriteLine($"  2) The default installation path for the Unity Hub ({unityHubPath})");
                Console.Out.WriteLine($"  3) The default installation path for Unity ({unityPath})");
                Console.Out.WriteLine($"  4) An Improbable-specific installation path for Unity ({improbableUnityRootPath})");
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

                var hubPath = Path.Combine(unityHubPath, "Editor", projectVersion);
                if (Directory.Exists(hubPath))
                {
                    Console.Out.WriteLine(hubPath);
                    Environment.Exit(0);
                }

                if (Directory.Exists(Path.Combine(unityPath, "Editor")))
                {
                    // The UnityPath may contain other folders like "Hub", so sanity check that there's an Editor folder in it.
                    Console.Out.WriteLine(unityPath);
                    Environment.Exit(0);
                }

                var improbableUnityPath = Path.Combine(improbableUnityRootPath, $"Unity-{projectVersion}");
                if (Directory.Exists(improbableUnityPath))
                {
                    Console.Out.WriteLine(improbableUnityPath);
                    Environment.Exit(0);
                }

                throw new Exception($"Could not find Unity in\n  {UnityHomeEnvVar}={unityHome}\n  {hubPath}\n  {unityPath}");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}

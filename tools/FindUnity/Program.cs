using System;
using System.IO;
using System.Linq;

namespace FindUnity
{
    internal static class Program
    {
        // If SpecialFolder.ProgramFiles returns the (x86) variant instead of "Program Files",
        // check that "Prefer 32 bit" is unticked in the project's Build settings.
        // On MacOS, SpecialFolder.ProgramFiles maps to "/Applications"

        private static readonly string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private static readonly string UnityHubPath = Path.Combine(ProgramFiles, "Unity", "Hub");
        private static readonly string UnityPath = Path.Combine(ProgramFiles, "Unity");
        private const string UnityHomeEnvVar = "UNITY_HOME";

        private static void Main(string[] args)
        {
            // Adjust the root based on Windows or MacOS. Linux is currently unsupported.
            var improbableUnityRootPath = ProgramFiles == "/Applications" ? "/Applications" : Path.Combine(@"c:\", "Unity");

            if (args.Contains("--help") || args.Contains("/?") || args.Contains("/help"))
            {
                Console.Out.WriteLine("Looks for a path to Unity in the following locations, in order:");
                Console.Out.WriteLine($"  1) The path set in the environment variable {UnityHomeEnvVar}");
                Console.Out.WriteLine($"  2) An Improbable-specific installation path for Unity ({improbableUnityRootPath})");            
                Console.Out.WriteLine($"  3) The default installation path for the Unity Hub ({UnityHubPath})");
                Console.Out.WriteLine($"  4) The default installation path for Unity ({UnityPath})");
                Console.Out.WriteLine();
                Console.Out.WriteLine("If Unity is found, prints the path to standard out and return exit code 0.");
                Console.Out.WriteLine("If not, prints an error message to standard error and return exit code 1.");
                Environment.Exit(0);
            }

            try
            {
                if (!Directory.Exists("Assets"))
                {
                    throw new Exception($"Please run from within a Unity project folder. {Environment.CurrentDirectory} is not a Unity project.");
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


                var improbableUnityPath = Path.Combine(improbableUnityRootPath, $"Unity-{projectVersion}");
                if (Directory.Exists(improbableUnityPath))
                {
                    Console.Out.WriteLine(improbableUnityPath);
                    Environment.Exit(0);
                }

                var hubPath = Path.Combine(UnityHubPath, "Editor", projectVersion);
                if (Directory.Exists(hubPath))
                {
                    Console.Out.WriteLine(hubPath);
                    Environment.Exit(0);
                }

                if (Directory.Exists(Path.Combine(UnityPath, "Editor")))
                {
                    // The UnityPath may contain other folders like "Hub", so sanity check that there's an Editor folder in it.
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

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace UnityPaths
{
    public static class Paths
    {
        // If SpecialFolder.ProgramFiles returns the (x86) variant instead of "Program Files",
        // check that "Prefer 32 bit" is unticked in the project's Build settings.
        // On MacOS, SpecialFolder.ProgramFiles maps to "/Applications"
        public static readonly string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        public static readonly string UnityHubPath = Path.Combine(ProgramFiles, "Unity", "Hub");
        public static readonly string DefaultUnityPath = Path.Combine(ProgramFiles, "Unity");
        public const string UnityHomeEnvVar = "UNITY_HOME";

        public static string UnityHomeValue => Environment.GetEnvironmentVariable(UnityHomeEnvVar);

        // Adjust the root based on Windows or MacOS. Linux is currently unsupported.
        public static string ImprobableUnityRootPath =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "/Applications/Unity" : Path.Combine(@"c:\", "Unity");

        public static string TryGetUnityPath()
        {
            if (!Directory.Exists("Assets"))
            {
                throw new Exception(
                    $"Please run from within a Unity project folder. {Environment.CurrentDirectory} is not a Unity project.");
            }

            if (!string.IsNullOrEmpty(UnityHomeValue))
            {
                if (!Directory.Exists(UnityHomeValue))
                {
                    throw new Exception($"{UnityHomeEnvVar}='{UnityHomeValue}', but this path does not exist.");
                }

                return UnityHomeValue;
            }

            var projectVersion = File.ReadAllText(Path.Combine("ProjectSettings", "ProjectVersion.txt"))
                .Remove(0, "m_EditorVersion:".Length).Trim();

            var improbableUnityPath = Path.Combine(ImprobableUnityRootPath, $"{projectVersion}");
            if (Directory.Exists(improbableUnityPath))
            {
                return improbableUnityPath;
            }

            var hubPath = Path.Combine(UnityHubPath, "Editor", projectVersion);
            if (Directory.Exists(hubPath))
            {
                return hubPath;
            }

            if (Directory.Exists(Path.Combine(DefaultUnityPath, "Editor")))
            {
                // The DefaultUnityPath may contain other folders like "Hub", so sanity check that there's an Editor folder in it.
                return DefaultUnityPath;
            }

            throw new Exception(
                $"Could not find Unity in\n  {UnityHomeEnvVar}={UnityHomeValue}\n  {improbableUnityPath}\n  {hubPath}\n  {DefaultUnityPath}");
        }

        public static void PrintHelp()
        {
            Console.Out.WriteLine("Looks for a path to Unity in the following locations, in order:");
            Console.Out.WriteLine(
                $"  1) The path set in the environment variable {UnityHomeEnvVar} ({UnityHomeValue}");
            Console.Out.WriteLine(
                $"  2) An Improbable-specific installation path for Unity ({ImprobableUnityRootPath})");
            Console.Out.WriteLine($"  3) The default installation path for the Unity Hub ({UnityHubPath})");
            Console.Out.WriteLine($"  4) The default installation path for Unity ({DefaultUnityPath})");
            Console.Out.WriteLine();
            Console.Out.WriteLine("Must be run from a Unity project folder.");
            Console.Out.WriteLine();
            Console.Out.WriteLine("If Unity is found, prints the path to standard out and return exit code 0.");
            Console.Out.WriteLine("If not, prints an error message to standard error and return exit code 1.");
        }
    }
}

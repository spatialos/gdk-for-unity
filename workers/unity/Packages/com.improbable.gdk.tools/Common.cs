using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Tools
{
    static class Common
    {
        public static string CoreSdkVersion { get; }

        private const string PackagesDir = "Packages";

        static Common()
        {
            try
            {
                var versionFile = Path.Combine(GetThisPackagePath(), "core-sdk.version");
                CoreSdkVersion = File.ReadAllText(versionFile, Encoding.UTF8).Trim();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        ///     Finds the "file:" reference path from the package manifest.
        /// </summary>
        public static string GetThisPackagePath()
        {
            const string gdkTools = "com.improbable.gdk.tools";
            var manifest = GetManifestDependencies();

            if (!manifest.TryGetValue(gdkTools, out var path))
            {
                throw new Exception($"The project manifest must reference '{gdkTools}'.");
            }

            if (!path.StartsWith("file:"))
            {
                throw new Exception($"The '{gdkTools}' package must exist on disk.");
            }

            path = path.Replace("file:", string.Empty);
            path = $"{PackagesDir}/{path}";

            return path;
        }

        public static Dictionary<string, string> GetManifestDependencies()
        {
            try
            {
                var manifest =
                    MiniJSON.Json.Deserialize(File.ReadAllText($"{PackagesDir}/manifest.json", Encoding.UTF8));
                return ((Dictionary<string, object>) manifest["dependencies"]).ToDictionary(kv => kv.Key,
                    kv => (string) kv.Value);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Failed to parse manifest file: {e.Message}");
            }
        }

        public static int RunProcess(string command, params string[] arguments)
        {
            var info = new ProcessStartInfo(command, string.Join(" ", arguments))
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = Path.GetFullPath(Path.Combine(Application.dataPath, ".."))
            };

            using (var process = Process.Start(info))
            {
                if (process == null)
                {
                    throw new Exception(
                        $"Failed to run {info.FileName} {info.Arguments}\nIs the .NET Core SDK installed?");
                }

                process.EnableRaisingEvents = true;

                process.OutputDataReceived += OnReceived;
                process.ErrorDataReceived += OnErrorReceived;

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();
                return process.ExitCode;
            }
        }

        private static void OnReceived(object sender, DataReceivedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                Debug.Log(args.Data);
            }
        }

        private static void OnErrorReceived(object sender, DataReceivedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                Debug.LogError(args.Data);
            }
        }
    }
}

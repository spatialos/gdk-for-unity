using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Improbable.Gdk.Tools.MiniJSON;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Tools
{
    internal static class Common
    {
        public static string CoreSdkVersion { get; }

        private const string PackagesDir = "Packages";
        private const string UsrLocalBinDir = "/usr/local/bin";
        public static string SpatialBinary => DiscoverSpatialLocation();


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
                    Json.Deserialize(File.ReadAllText($"{PackagesDir}/manifest.json", Encoding.UTF8));
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
            return RunProcessIn(Path.GetFullPath(Path.Combine(Application.dataPath, "..")), command, arguments);
        }

        public static int RunProcessIn(string workingDirectory, string command, params string[] arguments)
        {
            var info = new ProcessStartInfo(command, string.Join(" ", arguments))
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory
            };

            using (var process = Process.Start(info))
            {
                if (process == null)
                {
                    throw new Exception(
                        $"Failed to run {info.FileName} {info.Arguments}\nIs the .NET Core SDK installed?");
                }

                process.EnableRaisingEvents = true;

                var processOutput = new StringBuilder();

                void OnReceived(object sender, DataReceivedEventArgs args)
                {
                    if (string.IsNullOrEmpty(args.Data))
                    {
                        return;
                    }

                    lock (processOutput)
                    {
                        processOutput.AppendLine(ProcessSpatialOutput(args.Data));
                    }
                }

                process.OutputDataReceived += OnReceived;
                process.ErrorDataReceived += OnReceived;

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Debug.Log(processOutput);
                }
                else
                {
                    Debug.LogError(processOutput);
                }


                return process.ExitCode;
            }
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

        private static string DiscoverSpatialLocation()
        {
            var pathValue = Environment.GetEnvironmentVariable("PATH");
            if (pathValue == null)
            {
                return string.Empty;
            }

            var fileName = "spatial";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                fileName = Path.ChangeExtension(fileName, ".exe");
            }

            var splitPath = pathValue.Split(Path.PathSeparator);

            if (Application.platform == RuntimePlatform.OSXEditor && !splitPath.Contains(UsrLocalBinDir))
            {
                splitPath = splitPath.Union(new[] { UsrLocalBinDir }).ToArray();
            }

            foreach (var path in splitPath)
            {
                var testPath = Path.Combine(path, fileName);
                if (File.Exists(testPath))
                {
                    return testPath;
                }
            }

            return string.Empty;
        }
    }
}

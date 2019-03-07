using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Improbable.Gdk.Tools.MiniJSON;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    /// <summary>
    ///     Catch-all class for common helpers and utilities.
    /// </summary>
    public static class Common
    {
        /// <summary>
        ///     The version of the CoreSdk the GDK is pinned to.
        ///     Modify the core-sdk.version file in this source file's directory to change the version.
        /// </summary>
        public static string CoreSdkVersion { get; }

        /// <summary>
        ///     The absolute path to the root folder of the SpatialOS project.
        /// </summary>
        public static readonly string
            SpatialProjectRootDir = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", ".."));

        /// <summary>
        ///     The absolute path to the `spatial` binary, or the empty string if it doesn't exist.
        /// </summary>
        public static string SpatialBinary => DiscoverLocation("spatial");

        public static string DotNetBinary => DiscoverLocation("dotnet");

        public const string ProductName = "SpatialOS for Unity";

        public const string PackagesDir = "Packages";
        public static readonly string ManifestPath = Path.Combine(PackagesDir, "manifest.json");

        private const string UsrLocalBinDir = "/usr/local/bin";
        private const string UsrLocalShareDir = "/usr/local/share";

        private static readonly string[] MacPaths = { UsrLocalBinDir, UsrLocalShareDir };

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

        internal static string GetThisPackagePath()
        {
            return GetPackagePath("com.improbable.gdk.tools");
        }

        /// <summary>
        ///     Finds the "file:" reference path from the package manifest.
        /// </summary>
        public static string GetPackagePath(string packageName)
        {
            var manifest = ParseDependencies(ManifestPath);

            if (!manifest.TryGetValue(packageName, out var path))
            {
                throw new Exception($"The project manifest must reference '{packageName}'.");
            }

            if (!path.StartsWith("file:"))
            {
                throw new Exception($"The '{packageName}' package must exist on disk.");
            }

            path = path.Replace("file:", string.Empty);

            if (Path.IsPathRooted(path))
            {
                // A "rooted path" is an absolute path, therefore it will point directly at the package.
                return path;
            }

            path = Path.GetFullPath(Path.Combine(PackagesDir, path));

            return path;
        }

        /// <summary>
        ///     Parses a json file and returns the dependencies found in it.
        /// </summary>
        /// <param name="filePath">Path to the json file to be parsed.</param>
        /// <returns>The dependencies in the given json file.</returns>
        internal static Dictionary<string, string> ParseDependencies(string filePath)
        {
            try
            {
                var package = Json.Deserialize(File.ReadAllText(filePath, Encoding.UTF8));
                if (!package.TryGetValue("dependencies", out var dependenciesJson))
                {
                    return new Dictionary<string, string>();
                }

                return ((Dictionary<string, object>) dependenciesJson).ToDictionary(kv => kv.Key,
                    kv => (string) kv.Value);
            }
            catch (Exception e)
            {
                var errorMessage = $"Failed to parse dependencies: {e.Message}";
                Debug.LogError(errorMessage);
                throw new ArgumentException(errorMessage);
            }
        }

        /// <summary>
        ///     Tries to find the full path to a binary in the system PATH.
        ///     On MacOS, also looks in `/usr/local/bin` because applications launched from the Finder
        ///     don't always have that in the PATH provided to them.
        /// </summary>
        /// <param name="binarybaseName">The base name of the binary to find (without an extension).</param>
        /// <returns></returns>
        private static string DiscoverLocation(string binarybaseName)
        {
            var pathValue = Environment.GetEnvironmentVariable("PATH");
            if (pathValue == null)
            {
                Debug.LogError("PATH has not been specified in the system environment.");
                return string.Empty;
            }

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                binarybaseName = Path.ChangeExtension(binarybaseName, ".exe");

                if (binarybaseName == null)
                {
                    return string.Empty;
                }
            }

            var splitPath = pathValue.Split(Path.PathSeparator);

            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                foreach (var macPath in MacPaths)
                {
                    if (!splitPath.Contains(macPath))
                    {
                        splitPath = splitPath.Union(new[] { macPath, Path.Combine(macPath, binarybaseName) }).ToArray();
                    }
                }
            }

            var location = splitPath.Select(p => Path.Combine(p, binarybaseName)).FirstOrDefault(File.Exists);
            if (location != null)
            {
                return location;
            }

            Debug.LogError($"Could not discover location for {binarybaseName}");
            return string.Empty;
        }

        /// <summary>
        /// Checks whether `dotnet` and `spatial` exist on the PATH.
        /// </summary>
        /// <returns></returns>
        public static bool CheckDependencies()
        {
            var hasDotnet = !string.IsNullOrEmpty(Common.DotNetBinary);
            var hasSpatial = !string.IsNullOrEmpty(Common.SpatialBinary);

            if (hasDotnet && hasSpatial)
            {
                return true;
            }

            var builder = new StringBuilder();

            builder.AppendLine(
                "The SpatialOS GDK for Unity requires 'dotnet' and 'spatial' on your PATH to run its tooling.");
            builder.AppendLine();

            if (!hasDotnet)
            {
                builder.AppendLine("Could not find 'dotnet' on your PATH.");
            }

            if (!hasSpatial)
            {
                builder.AppendLine("Could not find 'spatial' on your PATH.");
            }

            builder.AppendLine();
            builder.AppendLine("If these exist on your PATH, restart Unity and Unity Hub.");
            builder.AppendLine();
            builder.AppendLine("Otherwise, install them by following our setup guide:");
            builder.AppendLine("https://docs.improbable.io/unity/alpha/content/get-started/set-up");

            EditorApplication.delayCall += () =>
            {
                EditorUtility.DisplayDialog("GDK dependencies check failed", builder.ToString(), "OK");
            };

            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Improbable.Gdk.Tools.MiniJSON;
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
        ///     The absolute path to the `spatial` binary, or the empty string if it doesn't exist.
        /// </summary>
        public static string SpatialBinary => DiscoverLocation("spatial");

        public static string DotNetBinary => DiscoverLocation("dotnet");

        public const string ProductName = "SpatialOS for Unity";

        private const string PackagesDir = "Packages";
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

        /// <summary>
        ///     Finds the "file:" reference path from the package manifest.
        /// </summary>
        internal static string GetThisPackagePath()
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

            if (Path.IsPathRooted(path))
            {
                // A "rooted path" is an absolute path, therefore it will point directly at the package.
                return path;
            }

            path = Path.GetFullPath(Path.Combine(PackagesDir, path));

            return path;
        }

        internal static Dictionary<string, string> GetManifestDependencies()
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
    }
}

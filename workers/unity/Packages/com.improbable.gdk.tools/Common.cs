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
        public static string SpatialBinary => DiscoverSpatialLocation();


        private const string PackagesDir = "Packages";
        private const string UsrLocalBinDir = "/usr/local/bin";


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
            path = $"{PackagesDir}/{path}";

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

using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.PackageManager;
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

        private const string UsrLocalBinDir = "/usr/local/bin";
        private const string UsrLocalShareDir = "/usr/local/share";

        private static readonly string[] MacPaths = { UsrLocalBinDir, UsrLocalShareDir };

        internal static string GetThisPackagePath()
        {
            return GetPackagePath("com.improbable.gdk.tools");
        }

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
        ///     Finds the path for a given package referenced directly in the manifest.json,
        ///     or indirectly referenced as a package dependency.
        /// </summary>
        public static string GetPackagePath(string packageName)
        {
            // Get package info
            var request = Client.List(offlineMode: true);
            while (!request.IsCompleted)
            {
                // Wait for the request to complete
            }

            // Package directly referenced
            var package = request.Result.FirstOrDefault(info => info.name.Equals(packageName));
            if (package != null)
            {
                return package.resolvedPath;
            }

            // Package indirectly referenced (dependency)
            var cachedPackage = Directory
                .GetDirectories("Library/PackageCache")
                .Where(path => !string.IsNullOrEmpty(path))
                .FirstOrDefault(path => Path.GetFileName(path).StartsWith($"{packageName}@"));
            if (!string.IsNullOrEmpty(cachedPackage))
            {
                return Path.GetFullPath(cachedPackage);
            }

            // Unable to find given package
            throw new ArgumentException(
                $"Could not find '{packageName}', is it in your project's manifest?\n{request.Error?.message}");
        }

        /// <summary>
        ///     Tries to find the full path to a binary in the system PATH.
        ///     On MacOS, also looks in `/usr/local/bin` because applications launched from the Finder
        ///     don't always have that in the PATH provided to them.
        /// </summary>
        /// <param name="binaryBaseName">The base name of the binary to find (without an extension).</param>
        /// <returns></returns>
        private static string DiscoverLocation(string binaryBaseName)
        {
            var pathValue = Environment.GetEnvironmentVariable("PATH");
            if (pathValue == null)
            {
                Debug.LogError("PATH has not been specified in the system environment.");
                return string.Empty;
            }

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                binaryBaseName = Path.ChangeExtension(binaryBaseName, ".exe");

                if (binaryBaseName == null)
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
                        splitPath = splitPath.Union(new[] { macPath, Path.Combine(macPath, binaryBaseName) }).ToArray();
                    }
                }
            }

            var location = splitPath.Select(p => Path.Combine(p, binaryBaseName)).FirstOrDefault(File.Exists);
            if (location != null)
            {
                return location;
            }

            Debug.LogError($"Could not discover location for {binaryBaseName}");
            return string.Empty;
        }

        /// <summary>
        ///     Checks whether `dotnet` and `spatial` exist on the PATH.
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
            builder.AppendLine("https://docs.improbable.io/unity/alpha/machine-setup#2-install-the-gdk-dependencies");

            EditorApplication.delayCall += () =>
            {
                EditorUtility.DisplayDialog("GDK dependencies check failed", builder.ToString(), "OK");
            };

            return false;
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    public enum DownloadResult
    {
        Success,
        AlreadyInstalled,
        Error
    }

    public static class DownloadCoreSdk
    {
        private const int DownloadForcePriority = 50;

        private static readonly string InstallMarkerFile =
            Path.GetFullPath($"build/CoreSdk/{Common.CoreSdkVersion}.installed");

        private static readonly string ProjectPath =
            Path.GetFullPath(Path.Combine(Common.GetThisPackagePath(), ".DownloadCoreSdk/DownloadCoreSdk.csproj"));

        private const string DownloadForceMenuItem = "Improbable/Download CoreSdk (force)";

        [MenuItem(DownloadForceMenuItem, false, DownloadForcePriority)]
        private static void DownloadForceMenu()
        {
            Download();
            AssetDatabase.Refresh();
        }

        /// <summary>
        ///     Windows only: Ensures that the user can't try to force a download if any of Improbable's native plugins are loaded
        ///     by Unity.
        /// </summary>
        [MenuItem(DownloadForceMenuItem, true, DownloadForcePriority)]
        private static bool DownloadForceValidate()
        {
            var failedPlugins = new List<PluginImporter>();

            // Unity never unloads native plugins. Detect them here and give a more useful error message.
            foreach (var plugin in PluginImporter.GetAllImporters().Where(ShouldCheckPluginForLock))
            {
                var path = plugin.assetPath;
                try
                {
                    // Try to open the file to see if it's locked.
                    using (new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.None))
                    {
                    }
                }
                catch
                {
                    failedPlugins.Add(plugin);
                }
            }

            return !failedPlugins.Any();
        }

        private static bool ShouldCheckPluginForLock(PluginImporter p)
        {
            if (!p.assetPath.StartsWith("Assets/Plugins/Improbable"))
            {
                return false;
            }

            return p.isNativePlugin && File.Exists(p.assetPath);
        }

        /// <summary>
        ///     Downloads the Core Sdk only if it has not already been downloaded and unzipped.
        /// </summary>
        public static DownloadResult TryDownload()
        {
            if (File.Exists(InstallMarkerFile))
            {
                return DownloadResult.AlreadyInstalled;
            }

            return Download();
        }

        /// <summary>
        ///     Downloads and extracts the Core Sdk and associated libraries and tools.
        /// </summary>
        public static DownloadResult Download()
        {
            try
            {
                File.Delete(InstallMarkerFile);
            }
            catch
            {
            }

            int exitCode;
            try
            {
                EditorApplication.LockReloadAssemblies();

                using (new ShowProgressBarScope($"Installing SpatialOS libraries, version {Common.CoreSdkVersion}..."))
                {
                    exitCode = RedirectedProcess.Run("dotnet", "run", "-p", $"\"{ProjectPath}\"", "--",
                        Common.SpatialBinary, Common.CoreSdkVersion);
                    if (exitCode != 0)
                    {
                        Debug.LogError($"Failed to download SpatialOS Core Sdk version {Common.CoreSdkVersion}.");
                    }
                    else
                    {
                        File.WriteAllText(InstallMarkerFile, string.Empty);
                    }
                }
            }
            finally
            {
                EditorApplication.UnlockReloadAssemblies();
            }

            return exitCode == 0 ? DownloadResult.Success : DownloadResult.Error;
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    internal enum DownloadResult
    {
        Success,
        AlreadyInstalled,
        Error
    }

    internal static class DownloadCoreSdk
    {
        internal static readonly string ImprobablePluginsPath = Path.Combine("Assets", "Plugins", "Improbable");

        private const int DownloadForcePriority = 50;

        private static readonly string InstallMarkerFile =
            Path.GetFullPath($"build/CoreSdk/{Common.CoreSdkVersion}.installed");

        private static readonly string ProjectPath =
            Path.GetFullPath(Path.Combine(Common.GetThisPackagePath(), ".DownloadCoreSdk/DownloadCoreSdk.csproj"));

        private const string DownloadForceMenuItem = "SpatialOS/Download CoreSdk (force)";

        internal static bool IsImprobablePlugin(string assetPath)
        {
            return Path.GetFullPath(assetPath).Contains(ImprobablePluginsPath);
        }

        [MenuItem(DownloadForceMenuItem, false, DownloadForcePriority)]
        private static void DownloadForceMenu()
        {
            if (!CanDownload())
            {
                EditorUtility.DisplayDialog(Common.ProductName,
                    $"Files in the {Common.ProductName} libraries have been locked by Unity.\n\nPlease restart the Unity editor and try again.",
                    "Ok");
                return;
            }

            Download();
            AssetDatabase.Refresh();
        }

        private static void RemoveMarkerFile()
        {
            try
            {
                File.Delete(InstallMarkerFile);
            }
            catch
            {
                // Nothing to handle if this fails - no need to abort the process.
            }
        }

        /// <summary>
        ///     Windows only: Ensures that the user can't try to force a download if any of Improbable's native plugins are loaded
        ///     by Unity.
        /// </summary>
        private static bool CanDownload()
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
            if (!IsImprobablePlugin(p.assetPath))
            {
                return false;
            }

            return p.isNativePlugin && File.Exists(p.assetPath);
        }

        /// <summary>
        ///     Downloads the Core Sdk only if it has not already been downloaded and unzipped.
        /// </summary>
        internal static DownloadResult TryDownload()
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
        private static DownloadResult Download()
        {
            if (!Common.CheckDependencies())
            {
                return DownloadResult.Error;
            }

            RemoveMarkerFile();

            int exitCode;
            try
            {
                EditorApplication.LockReloadAssemblies();

                using (new ShowProgressBarScope($"Installing SpatialOS libraries, version {Common.CoreSdkVersion}..."))
                {
                    exitCode = RedirectedProcess.Command(Common.DotNetBinary).WithArgs(ConstructArguments()).Run();
                    if (exitCode != 0)
                    {
                        Debug.LogError($"Failed to download SpatialOS Core Sdk version {Common.CoreSdkVersion}. You can use SpatialOS -> Download CoreSDK (force) to retry this.");
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

        private static string[] ConstructArguments()
        {
            var toolsConfig = GdkToolsConfiguration.GetOrCreateInstance();

            var baseArgs = new List<string>
            {
                "run",
                "-p",
                $"\"{ProjectPath}\"",
                "--",
                $"\"{Common.SpatialBinary}\"",
                $"\"{Common.CoreSdkVersion}\"",
                $"\"{toolsConfig.SchemaStdLibDir}\""
            };

            return baseArgs.ToArray();
        }
    }
}

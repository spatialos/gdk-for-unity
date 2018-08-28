using System.IO;
using UnityEngine;
using UnityEditor;

namespace Improbable.Gdk.Tools
{
    public static class DownloadCoreSdk
    {
        private const int DownloadPriority = 50;
        private const int DownloadForcePriority = 51;

        [MenuItem("Improbable/Download CoreSdk", false, DownloadPriority)]
        public static void DownloadMenu()
        {
            Download();
            AssetDatabase.Refresh();
        }

        [MenuItem("Improbable/Download CoreSdk (force)", false, DownloadForcePriority)]
        public static void DownloadForceMenu()
        {
            var path = Path.Combine("build", "CoreSdk");
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            DownloadMenu();
        }

        public static void Download()
        {
            try
            {
                EditorApplication.LockReloadAssemblies();

                var projectPath = Path.GetFullPath(Path.Combine(Common.GetThisPackagePath(),
                    ".DownloadCoreSdk/DownloadCoreSdk.csproj"));

                var exitCode = Common.RunProcess("dotnet", "run", "-p", $"\"{projectPath}\"", "--",
                    Common.CoreSdkVersion);
                if (exitCode != 0)
                {
                    Debug.LogError($"Failed to download SpatialOS Core Sdk version {Common.CoreSdkVersion}.");
                }
            }
            finally
            {
                EditorApplication.UnlockReloadAssemblies();
            }
        }
    }
}

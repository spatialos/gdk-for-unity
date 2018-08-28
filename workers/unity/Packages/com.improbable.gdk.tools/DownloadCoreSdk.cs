using System.IO;
using UnityEngine;
using UnityEditor;

namespace Improbable.Gdk.Tools
{
    public static class DownloadCoreSdk
    {
        [MenuItem("Improbable/Download CoreSdk", false, 50)]
        public static void DownloadMenu()
        {
            Download();
            AssetDatabase.Refresh();
        }

        [MenuItem("Improbable/Download CoreSdk (force)", false, 51)]
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

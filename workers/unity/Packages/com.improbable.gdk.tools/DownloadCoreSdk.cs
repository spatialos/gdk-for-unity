using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Improbable.Gdk.Tools
{
    public static class DownloadCoreSdk
    {
        [MenuItem("Improbable/Download CoreSdk")]
        public static void DownloadMenu()
        {
            Download();
            AssetDatabase.Refresh();
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
                    Debug.LogError("Failed to download the SpatialOS Core Sdk.");
                }
            }
            finally
            {
                EditorApplication.UnlockReloadAssemblies();
            }
        }
    }
}

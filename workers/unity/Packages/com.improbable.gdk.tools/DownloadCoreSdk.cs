using System;
using System.IO;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    public static class DownloadCoreSdk
    {
        public static void Download()
        {
            try
            {
                var projectPath = Path.GetFullPath(Path.Combine(Common.GetThisPackagePath(),
                    ".DownloadCoreSdk/DownloadCoreSdk.csproj"));

                var exitCode = Common.RunProcess("dotnet", "run", "-p", $"\"{projectPath}\"", "--",
                    Common.CoreSdkVersion);
                if (exitCode != 0)
                {
                    Debug.LogError("Failed to download the SpatialOS Core Sdk.");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}

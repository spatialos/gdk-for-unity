using System;
using System.IO;
using UnityEditor;
using Debug = UnityEngine.Debug;

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

                Common.RunProcess("dotnet", "run", "-p", $"\"{projectPath}\"", "--", Common.CoreSdkVersion);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Tools
{
    [InitializeOnLoad]
    public static class DownloadCoreSdk
    {
        static DownloadCoreSdk()
        {
            try
            {
                var projectPath = Path.GetFullPath(Path.Combine(Application.dataPath,
                    "../Packages/com.improbable.gdk.tools/.DownloadCoreSdk/DownloadCoreSdk.csproj"));

                Common.RunProcess("dotnet", "run", "-p", $"\"{projectPath}\"", "--", Common.CoreSdkVersion);

                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}

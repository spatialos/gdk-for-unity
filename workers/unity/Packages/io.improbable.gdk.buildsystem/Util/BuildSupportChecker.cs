using System.IO;
using Improbable.Gdk.BuildSystem.Configuration;
using UnityEditor;
using UnityEngine;

public static class BuildSupportChecker
{
    public static bool CanBuildTarget(BuildTarget target)
    {
        var editorDirectory = Directory.GetParent(EditorApplication.applicationPath);

        string playbackEnginesDirectory;

        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
                playbackEnginesDirectory = Path.Combine(editorDirectory.FullName, "PlaybackEngines");
                break;
            case RuntimePlatform.WindowsEditor:
                playbackEnginesDirectory = Path.Combine(editorDirectory.FullName, "Data", "PlaybackEngines");
                break;
            default:
                return true;
        }

        if (target == BuildTarget.StandaloneOSX && Application.platform == RuntimePlatform.OSXEditor)
        {
            // OSXEditor will always have StandaloneOSX support
            return true;
        }

        if (WorkerBuildData.BuildTargetSupportDirectoryNames.TryGetValue(target,
            out var playbackEnginesDirectoryName))
        {
            return Directory.Exists(Path.Combine(playbackEnginesDirectory, playbackEnginesDirectoryName));
        }

        return true;
    }
}

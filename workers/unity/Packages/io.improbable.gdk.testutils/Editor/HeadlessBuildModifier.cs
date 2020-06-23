using Improbable.Gdk.TestUtils.Editor;
using UnityEditor;
using UnityEditor.TestTools;

[assembly: TestPlayerBuildModifier(typeof(HeadlessBuildModifier))]

namespace Improbable.Gdk.TestUtils.Editor
{
    public class HeadlessBuildModifier : ITestPlayerBuildModifier
    {
        public BuildPlayerOptions ModifyOptions(BuildPlayerOptions playerOptions)
        {
            if (playerOptions.target == BuildTarget.StandaloneWindows64 ||
                playerOptions.target == BuildTarget.StandaloneOSX ||
                playerOptions.target == BuildTarget.StandaloneLinux64)
            {
                playerOptions.options |= BuildOptions.EnableHeadlessMode;
            }

            return playerOptions;
        }
    }
}

using Improbable.Gdk.PlaymodeTests.Editor;
using UnityEditor;
using UnityEditor.TestTools;

[assembly: TestPlayerBuildModifier(typeof(HeadlessBuildModifier))]

namespace Improbable.Gdk.PlaymodeTests.Editor
{
    public class HeadlessBuildModifier : ITestPlayerBuildModifier
    {
        public BuildPlayerOptions ModifyOptions(BuildPlayerOptions playerOptions)
        {
            if (playerOptions.target == BuildTarget.StandaloneWindows64 ||
                playerOptions.target == BuildTarget.StandaloneOSX ||
                playerOptions.target == BuildTarget.StandaloneLinux64)
            {
                // As our CI agents do not have a display, we must ensure standalone test players are headless.
                // Standalone tests will normally have a separate window pop up, even if -nographics is set.
                // Test runs *will* fail without setting this.
                playerOptions.options |= BuildOptions.EnableHeadlessMode;
            }

            return playerOptions;
        }
    }
}

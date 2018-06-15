using System.IO;

namespace Improbable.Gdk.Legacy.BuildSystem.Configuration
{
    public static class BuildPaths
    {
        public static string BuildScratchDirectory => Path.GetFullPath(Path.Combine("build", "worker"));
    }
}

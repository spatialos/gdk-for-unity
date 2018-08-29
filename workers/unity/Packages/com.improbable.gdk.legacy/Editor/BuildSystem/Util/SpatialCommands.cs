using System.IO;
using Improbable.Gdk.Legacy.BuildSystem.Configuration;

namespace Improbable.Gdk.Legacy.BuildSystem.Util
{
    internal static class SpatialCommands
    {
        internal static void GenerateBuildConfiguration()
        {
         //   SpatialCommandRunner.RunSpatialCommand("build build-config", "generate build configuration");
        }
    
        internal static void Zip(string zipAbsolutePath, string basePath, PlayerCompression compression)
        {
            ZipThroughSpatial(zipAbsolutePath, basePath, compression);
        }

        private static void ZipThroughSpatial(string zipAbsolutePath, string basePath, PlayerCompression useCompression)
        {
            var zipFileFullPath = Path.GetFullPath(zipAbsolutePath);

            // SpatialCommandRunner.RunSpatialCommand(ZipArgs(basePath, zipFileFullPath, useCompression),
            //     string.Format("package the folder {0}", basePath));
        }

        private static string ZipArgs(string basePath, string zipFileFullPath,
            PlayerCompression useCompression)
        {
            var filePattern = "**";
            var subFolder = "";

            return
                $"file zip --output=\"{zipFileFullPath}\" --basePath=\"{Path.GetFullPath(basePath)}\" --relativePath=. \"{PathUtil.EnsureTrailingSlash(subFolder) + filePattern}\" --compression={useCompression == PlayerCompression.Enabled}";
        }
    }
}

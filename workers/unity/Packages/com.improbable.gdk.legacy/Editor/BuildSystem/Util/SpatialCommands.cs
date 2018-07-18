using System.IO;
using Improbable.Gdk.Legacy.BuildSystem.Configuration;

namespace Improbable.Gdk.Legacy.BuildSystem.Util
{
    internal static class SpatialCommands
    {
        #region Build Configuration

        internal static void GenerateBuildConfiguration()
        {
            SpatialCommandRunner.RunSpatialCommand("build build-config", "generate build configuration");
        }

        #endregion Build Configuration

        #region Zip

        internal static void Zip(string zipAbsolutePath, string basePath, PlayerCompression compression)
        {
            ZipThroughSpatial(zipAbsolutePath, basePath, compression);
        }

        private static void ZipThroughSpatial(string zipAbsolutePath, string basePath, PlayerCompression useCompression)
        {
            var zipFileFullPath = Path.GetFullPath(zipAbsolutePath);

            SpatialCommandRunner.RunSpatialCommand(ZipArgs(basePath, zipFileFullPath, useCompression),
                string.Format("package the folder {0}", basePath));
        }

        private static string ZipArgs(string basePath, string zipFileFullPath,
            PlayerCompression useCompression)
        {
            var filePattern = "**";
            var subFolder = "";

            return string.Format(
                "file zip --output=\"{0}\" --basePath=\"{1}\" --relativePath=. \"{2}\" --compression={3}",
                zipFileFullPath,
                Path.GetFullPath(basePath),
                PathUtil.EnsureTrailingSlash(subFolder) + filePattern,
                useCompression == PlayerCompression.Enabled);
        }

        #endregion Zip
    }
}

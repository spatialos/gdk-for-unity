using System.IO;

namespace Improbable.Gdk.BuildSystem.Util
{
    public static class UnityPathUtil
    {
        public static void EnsureDirectoryClean(string directory)
        {
            EnsureDirectoryRemoved(directory);
            PathUtil.EnsureDirectoryExists(directory);
        }

        public static void EnsureDirectoryRemoved(string directory)
        {
            if (Directory.Exists(directory))
            {
                var directoryInfo = new DirectoryInfo(directory);

                foreach (var file in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
                {
                    file.Attributes &= ~FileAttributes.ReadOnly;
                }

                Directory.Delete(directory, true);
                DeleteMetaFile(directory);
            }
        }

        public static void EnsureFileRemoved(string file)
        {
            if (File.Exists(file))
            {
                DeleteFile(file);
            }
        }

        public static void DeleteFile(string file)
        {
            File.Delete(file);
            DeleteMetaFile(file);
        }

        public static void DeleteMetaFile(string path)
        {
            var metaPath = path + ".meta";
            if (File.Exists(metaPath))
            {
                File.Delete(metaPath);
            }
        }
    }
}

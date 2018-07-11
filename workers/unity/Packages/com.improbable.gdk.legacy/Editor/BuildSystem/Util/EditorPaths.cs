namespace Improbable.Gdk.Legacy.BuildSystem.Util
{
    /// <summary>
    ///     Contains common directories related to building assets and players.
    /// </summary>
    /// <remarks>
    ///     All directories should be in Unity path format e.g. "Foo/Bar".
    /// </remarks>
    public static class EditorPaths
    {
        public static readonly string PluginDirectory = PathUtil.Combine("Assets", "Plugins").ToUnityPath();

        public static readonly string DataDirectory = PathUtil.Combine("..", "..", "build").ToUnityPath();

        public static readonly string AssetDatabaseDirectory =
            PathUtil.Combine(DataDirectory, "assembly").ToUnityPath();
    }
}

using System.IO;
using UnityEngine;

namespace Improbable.Gdk.BuildSystem
{
    /// <summary>
    ///     Contains common directories related to building assets and players.
    /// </summary>
    /// <remarks>
    ///     All directories should be in Unity path format e.g. "Foo/Bar".
    /// </remarks>
    public static class EditorPaths
    {
        /// <summary>
        ///     The path to the SpatialOS assembly directory where SpatialOS assembly artifacts are placed.
        /// </summary>
        public static readonly string SpatialAssemblyDirectory =
            Path.Combine(Application.dataPath, "..", "..", "..", "build", "assembly");

        public static readonly string PlayerBuildDirectory =
            Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), EditorPaths.SpatialAssemblyDirectory,
                "worker"));
    }
}

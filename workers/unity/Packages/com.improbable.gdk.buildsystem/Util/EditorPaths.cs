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
        ///     The path to the Unity project build directory that worker build artifacts are placed into.
        /// </summary>
        public static readonly string BuildScratchDirectory =
            Path.GetFullPath(Path.Combine(Application.dataPath, "..", "build", "worker"));

        /// <summary>
        ///     The path to the SpatialOS assembly directory where SpatialOS assembly artifacts are placed.
        /// </summary>
        public static readonly string SpatialAssemblyDirectory =
            Path.Combine(Application.dataPath, "..", "..", "..", "build", "assembly");
    }
}

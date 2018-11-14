using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Denotes that an object can be snapshotted.
    /// </summary>
    /// <typeparam name="T">The type of the snapshot.</typeparam>
    public interface ISnapshottable<T> where T : ISpatialComponentSnapshot
    {
        T ToComponentSnapshot(World world);
    }
}

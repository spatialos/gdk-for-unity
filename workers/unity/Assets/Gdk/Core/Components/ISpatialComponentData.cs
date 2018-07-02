using Unity.Mathematics;

namespace Improbable.Gdk.Core
{
    public interface ISpatialComponentData
    {
        BlittableBool DirtyBit { get; set; }
    }
}

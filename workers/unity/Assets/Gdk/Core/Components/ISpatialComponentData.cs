using Unity.Mathematics;

namespace Improbable.Gdk.Core
{
    public interface ISpatialComponentData
    {
        bool1 DirtyBit { get; set; }
    }
}

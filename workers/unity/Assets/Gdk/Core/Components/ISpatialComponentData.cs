using Unity.Entities;
using Unity.Mathematics;

namespace Improbable.Gdk.Core
{
    public interface ISpatialComponentData : IComponentData
    {
        bool1 DirtyBit { get; set; }
    }
}

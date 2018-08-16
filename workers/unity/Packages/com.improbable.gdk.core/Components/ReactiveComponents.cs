using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public struct ComponentAdded<T> : IComponentData where T : ISpatialComponentData
    {
    }

    public struct ComponentRemoved<T> : IComponentData where T : ISpatialComponentData
    {
    }
}

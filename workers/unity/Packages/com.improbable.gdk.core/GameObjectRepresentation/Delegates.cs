using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public static class GameObjectDelegates
    {
        public delegate void AuthorityChanged(Authority newAuthority);

        public delegate void ComponentUpdated<TComponent>(ISpatialComponentUpdate<TComponent> updateData)
            where TComponent : ISpatialComponentData, IComponentData;
    }
}

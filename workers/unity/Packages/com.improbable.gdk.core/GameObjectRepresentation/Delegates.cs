using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public static class GameObjectDelegates
    {
        public delegate void AuthorityChanged(Authority newAuthority);

        public delegate void ComponentUpdated<TSpatialComponentData>(ISpatialComponentUpdate<TSpatialComponentData> updateData)
            where TSpatialComponentData : ISpatialComponentData;
    }
}

using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    public static class GameObjectDelegates
    {
        public delegate void AuthorityChanged(Authority newAuthority);

        public delegate void ComponentUpdated<TComponentUpdate>(TComponentUpdate updateData)
            where TComponentUpdate : ISpatialComponentUpdate;
    }
}

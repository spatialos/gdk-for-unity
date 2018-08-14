using Improbable.Worker;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public static class GameObjectDelegates
    {
        public delegate void AuthorityChanged(Authority newAuthority);

        public delegate void ComponentUpdated<TComponentUpdate>(TComponentUpdate updateData)
            where TComponentUpdate : ISpatialComponentUpdate;
    }
}

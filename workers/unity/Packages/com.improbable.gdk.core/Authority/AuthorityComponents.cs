using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public struct Authoritative<T> : IComponentData
    {
    }

    public struct NotAuthoritative<T> : IComponentData
    {
    }

    public struct AuthorityLossImminent<T> : IComponentData
    {
        public BlittableBool AuthorityLossAcknowledged;
        public BlittableBool AuthorityLossAcknowledgmentSent;
    }
}

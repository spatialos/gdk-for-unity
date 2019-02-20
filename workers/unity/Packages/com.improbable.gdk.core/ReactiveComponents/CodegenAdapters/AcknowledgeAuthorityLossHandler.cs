using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    public abstract class AbstractAcknowledgeAuthorityLossHandler
    {
        public abstract EntityArchetypeQuery Query { get; }

        public abstract void AcknowledgeAuthorityLoss(ComponentGroup group, ComponentSystemBase system,
            Connection connection);
    }
}

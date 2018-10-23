using Improbable.Worker.Core;
using Unity.Entities;

namespace Improbable.Gdk.Core.CodegenAdapters
{
    public abstract class AbstractAcknowledgeAuthorityLossHandler
    {
        public abstract EntityArchetypeQuery Query { get; }

        public abstract void AcknowledgeAuthorityLoss(ComponentGroup group, ComponentSystemBase system,
            Connection connection);
    }
}

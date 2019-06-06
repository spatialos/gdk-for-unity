using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    public abstract class AbstractAcknowledgeAuthorityLossHandler
    {
        public abstract EntityQueryDesc Query { get; }

        public abstract void AcknowledgeAuthorityLoss(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
            ComponentUpdateSystem updateSystem);
    }
}

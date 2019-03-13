using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    public interface IReactiveComponentReplicationHandler
    {
        uint ComponentId { get; }
        EntityArchetypeQuery EventQuery { get; }
        EntityArchetypeQuery[] CommandQueries { get; }

        void SendEvents(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
            ComponentUpdateSystem componentUpdateSystem);

        void SendCommands(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system, CommandSystem commandSystem);
    }
}

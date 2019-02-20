using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    public interface IReactiveComponentReplicationHandler
    {
        uint ComponentId { get; }
        EntityArchetypeQuery EventQuery { get; }
        EntityArchetypeQuery[] CommandQueries { get; }

        void SendEvents(ComponentGroup replicationGroup, ComponentSystemBase system,
            ComponentUpdateSystem componentUpdateSystem);

        void SendCommands(ComponentGroup commandGroup, ComponentSystemBase system, CommandSystem commandSystem);
    }
}

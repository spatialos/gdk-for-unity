using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core.CodegenAdapters
{
    public abstract class ComponentReplicationHandler
    {
        public abstract uint ComponentId { get; }
        public abstract EntityArchetypeQuery ComponentUpdateQuery { get; }
        public abstract EntityArchetypeQuery[] CommandQueries { get; }

        public abstract void ExecuteReplication(ComponentGroup replicationGroup, ComponentSystemBase system,
            Connection connection);

        public abstract void SendCommands(ComponentGroup commandGroup, ComponentSystemBase system,
            Connection connection);

        protected EntityManager EntityManager;
        protected readonly CommandParameters ShortCircuitParameters;

        // Can remove when WRK-796 is fixed!
        protected readonly UpdateParameters UpdateParameters;

        protected ComponentReplicationHandler(EntityManager entityManager)
        {
            EntityManager = entityManager;
            ShortCircuitParameters = new CommandParameters { AllowShortCircuiting = true };
            UpdateParameters = new UpdateParameters
            {
                Loopback = ComponentUpdateLoopback.ShortCircuited
            };
        }
    }
}

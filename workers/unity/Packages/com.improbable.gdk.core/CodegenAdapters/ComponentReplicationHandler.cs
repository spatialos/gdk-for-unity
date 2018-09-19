using System.Collections.Generic;
using Improbable.Worker.Core;
using Unity.Entities;

namespace Improbable.Gdk.Core.CodegenAdapters
{
    public abstract class ComponentReplicationHandler
    {
        public abstract uint ComponentId { get; }
        public abstract ComponentType[] ReplicationComponentTypes { get; }

        public abstract void ExecuteReplication(ComponentGroup replicationGroup, Connection connection);
        public abstract void SendCommands(SpatialOSSendSystem sendSystem, Connection connection);

        protected EntityManager EntityManager;
        protected readonly CommandParameters ShortCircuitParameters;

        protected ComponentReplicationHandler(EntityManager entityManager)
        {
            EntityManager = entityManager;
            ShortCircuitParameters = new CommandParameters { AllowShortCircuiting = true };
        }
    }
}

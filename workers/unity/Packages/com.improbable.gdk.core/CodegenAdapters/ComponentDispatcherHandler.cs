using System;
using Improbable.Worker.Core;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.CodegenAdapters
{
    public abstract class ComponentDispatcherHandler
    {
        public abstract uint ComponentId { get; }
        public abstract void OnAddComponent(AddComponentOp op);
        public abstract void OnRemoveComponent(RemoveComponentOp op);
        public abstract void OnComponentUpdate(ComponentUpdateOp op);
        public abstract void OnAuthorityChange(AuthorityChangeOp op);

        public abstract void OnCommandRequest(CommandRequestOp op);
        public abstract void OnCommandResponse(CommandResponseOp op);

        public abstract void AddCommandComponents(Entity entity);

        public abstract void Dispose();

        protected ILogDispatcher LogDispatcher;
        protected World World;
        protected WorkerSystem Worker;

        protected const string ReceivedDuplicateComponentAdded =
            "Received ComponentAdded, but already received one for this entity.";

        protected const string ReceivedDuplicateComponentRemoved =
            "Received ComponentRemoved, but already received one for this entity.";

        protected const string EntityNotFound = "No entity found for entity specified in op.";
        protected const string CommandIndexNotFound = "Command index not found.";
        protected const string InvalidAuthorityChange = "Invalid authority state change received.";

        protected ComponentDispatcherHandler(WorkerSystem worker, World world)
        {
            LogDispatcher = worker.LogDispatcher;
            World = world;
            Worker = worker;
        }
    }
}

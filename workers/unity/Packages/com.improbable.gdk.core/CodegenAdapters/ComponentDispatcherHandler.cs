using Improbable.Worker.Core;
using Unity.Entities;

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

        protected MutableView MutableView;
        protected ILogDispatcher LogDispatcher;

        protected const string ReceivedDuplicateComponentAdded =
            "Received ComponentAdded, but already received one for this entity.";
        protected const string ReceivedDuplicateComponentRemoved =
            "Received ComponentRemoved, but already received one for this entity.";
        protected const string EntityNotFound = "No entity found for entity specified in op.";
        protected const string CommandIndexNotFound = "Command index not found.";
        protected const string InvalidAuthorityChange = "Invalid authority state change received.";

        protected ComponentDispatcherHandler(MutableView mutableView)
        {
            MutableView = mutableView;
            LogDispatcher = mutableView.LogDispatcher;
        }
    }
}

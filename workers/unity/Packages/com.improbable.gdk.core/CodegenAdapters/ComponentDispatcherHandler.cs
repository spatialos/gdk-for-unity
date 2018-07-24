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

        protected ComponentDispatcherHandler(MutableView mutableView)
        {
            MutableView = mutableView;
        }
    }
}

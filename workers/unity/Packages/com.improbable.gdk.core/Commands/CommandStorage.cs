using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public abstract class CommandStorage
    {
    }

    public struct CommandRequestStore<T>
    {
        public Entity Entity;
        public T Request;
        public object Context;
        public long RequestId;

        public CommandRequestStore(Entity entity, T request, object context, long requestId)
        {
            Entity = entity;
            Request = request;
            Context = context;
            RequestId = requestId;
        }
    }
}

using System;
using Unity.Entities;

namespace Improbable.Gdk.Core.Commands
{
    public abstract class CommandStorage
    {
        public abstract Type CommandType { get; }
    }

    public struct CommandRequestStore<T>
    {
        public Entity Entity;
        public T Request;
        public object Context;

        public CommandRequestStore(Entity entity, T request, object context)
        {
            Entity = entity;
            Request = request;
            Context = context;
        }
    }
}

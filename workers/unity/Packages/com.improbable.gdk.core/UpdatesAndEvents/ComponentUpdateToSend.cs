using Improbable.Worker;

namespace Improbable.Gdk.Core
{
    public struct ComponentUpdateToSend<T> where T : ISpatialComponentData
    {
        public T Update;
        public EntityId EntityId;

        public ComponentUpdateToSend(T update, EntityId entityId)
        {
            Update = update;
            EntityId = entityId;
        }
    }

    public struct ComponentEventToSend<T> where T : IEvent
    {
        public T Event;
        public EntityId EntityId;

        public ComponentEventToSend(T @event, EntityId entityId)
        {
            Event = @event;
            EntityId = entityId;
        }
    }

    public readonly struct ComponentUpdateReceived<T> where T : ISpatialComponentUpdate
    {
        public readonly T Update;
        public readonly ulong UpdateId;
        public readonly EntityId EntityId;

        public ComponentUpdateReceived(T update, EntityId entityId, ulong updateId)
        {
            Update = update;
            UpdateId = updateId;
            EntityId = entityId;
        }
    }

    public readonly struct ComponentEventReceived<T> where T : IEvent
    {
        public readonly T Event;
        public readonly ulong UpdateId;
        public readonly EntityId EntityId;

        public ComponentEventReceived(T @event, EntityId entityId, ulong updateId)
        {
            Event = @event;
            UpdateId = updateId;
            EntityId = entityId;
        }
    }
}

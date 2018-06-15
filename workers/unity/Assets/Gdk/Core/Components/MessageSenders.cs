using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface IMessageSender<T> : IComponentData
    {
        long EntityId { get; }
        uint InternalHandleToTranslation { get; }
    }

    public struct CommandRequestSender<T> : IMessageSender<T>
    {
        public long EntityId { get; }
        public uint InternalHandleToTranslation { get; }

        public CommandRequestSender(long entityId, uint handleToTranslation)
        {
            EntityId = entityId;
            InternalHandleToTranslation = handleToTranslation;
        }
    }

    public struct EventSender<T> : IMessageSender<T>
    {
        public long EntityId { get; }
        public uint InternalHandleToTranslation { get; }

        public EventSender(long entityId, uint handleToTranslation)
        {
            EntityId = entityId;
            InternalHandleToTranslation = handleToTranslation;
        }
    }
}

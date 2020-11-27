using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public readonly struct AddComponentRequestToSend
    {
        public readonly EntityId EntityId;
        public readonly ComponentData ComponentData;
        public readonly UpdateParameters? Parameters;

        public AddComponentRequestToSend(EntityId entityId, ComponentData componentData, UpdateParameters? parameters)
        {
            EntityId = entityId;
            ComponentData = componentData;
            Parameters = parameters;
        }
    }

    public readonly struct RemoveComponentRequestToSend
    {
        public readonly EntityId EntityId;
        public readonly uint ComponentId;
        public readonly UpdateParameters? Parameters;

        public RemoveComponentRequestToSend(EntityId entityId, uint componentId, UpdateParameters? parameters)
        {
            EntityId = entityId;
            ComponentId = componentId;
            Parameters = parameters;
        }
    }
}

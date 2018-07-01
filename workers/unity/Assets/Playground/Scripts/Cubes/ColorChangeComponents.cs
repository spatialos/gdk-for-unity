using Improbable.Gdk.Core;
using Unity.Entities;

namespace Playground
{
    /// <summary>
    /// Tag used to enable and disable sending color change events
    /// </summary>
    public struct ColorChangeComponent : IComponentData
    {
        public float TimeBetweenEvents;
    }

    /// <summary>
    /// Denotes that a color change event should happen this tick
    /// </summary>
    [RemoveAtEndOfTick]
    public struct ShouldSendColorChangeTemporary : IComponentData
    {
    }
}

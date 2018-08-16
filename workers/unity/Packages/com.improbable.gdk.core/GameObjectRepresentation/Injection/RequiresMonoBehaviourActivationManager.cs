using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Tag component for marking entities that require a MonoBehaviourActivationManager.
    /// </summary>
    [RemoveAtEndOfTick]
    public struct RequiresMonoBehaviourActivationManager : IComponentData
    {
    }
}

using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Tag component for marking entities that were just created and still require setup.
    ///     This component is automatically added to an entity upon its creation and automatically removed at the end of the
    ///     same frame.
    /// </summary>
    public struct NewlyCreatedEntity : IComponentData
    {
    }
}

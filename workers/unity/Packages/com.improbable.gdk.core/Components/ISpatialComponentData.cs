namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Denotes that an object is a SpatialOS component.
    /// </summary>
    public interface ISpatialComponentData
    {
        /// <summary>
        ///     The component ID of the SpatialOS component as defined in schema.
        /// </summary>
        uint ComponentId { get; }

        /// <summary>
        ///     A marker to determine whether the SpatialOS component has been changed since it was last replicated.
        /// </summary>
        BlittableBool DirtyBit { get; set; }
    }
}

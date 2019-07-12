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
    }
}

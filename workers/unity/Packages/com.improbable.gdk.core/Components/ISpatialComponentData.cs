namespace Improbable.Gdk.Core
{
    public interface ISpatialComponentData
    {
        uint ComponentId { get; }
        BlittableBool DirtyBit { get; set; }
    }
}

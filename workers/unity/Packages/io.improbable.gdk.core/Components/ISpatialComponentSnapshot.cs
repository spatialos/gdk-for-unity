using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public interface ISpatialComponentSnapshot
    {
        uint ComponentId { get; }
        ComponentData Serialize();
    }
}

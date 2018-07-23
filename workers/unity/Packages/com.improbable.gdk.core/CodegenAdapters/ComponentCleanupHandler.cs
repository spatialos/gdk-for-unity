using Unity.Entities;

namespace Improbable.Gdk.Core.CodegenAdapters
{
    public abstract class ComponentCleanupHandler
    {
        public abstract ComponentType[] CleanUpComponentTypes { get; }
    }
}

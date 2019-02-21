using System;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public interface IViewStorage
    {
        Type GetSnapshotType();

        // TODO: This should take an actual diff once the data is exposed.
        void ApplyDiff(ComponentUpdateSystem componentUpdateSystem);
    }

    public interface IViewComponentStorage<T> where T : struct, ISpatialComponentSnapshot
    {
        bool HasComponent(long entityId);
        T GetComponent(long entityId);

        Authority GetAuthority(long entityId);
    }
}

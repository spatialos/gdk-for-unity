using System;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public interface IViewStorage
    {
        Type GetSnapshotType();
        uint GetComponentId();

        bool HasComponent(long entityId);
        Authority GetAuthority(long entityId);

        void ApplyDiff(ViewDiff viewDiff);
    }

    public interface IViewComponentStorage<T> where T : struct, ISpatialComponentSnapshot
    {
        T GetComponent(long entityId);
    }
}

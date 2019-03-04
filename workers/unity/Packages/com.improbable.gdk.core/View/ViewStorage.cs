using System;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public interface IViewStorage
    {
        Type GetSnapshotType();
        Type GetUpdateType();
        uint GetComponentId();

        bool HasComponent(long entityId);
        Authority GetAuthority(long entityId);

        void ApplyDiff(ViewDiff viewDiff);
    }

    public interface IViewComponentStorage<T> where T : struct, ISpatialComponentSnapshot
    {
        T GetComponent(long entityId);
    }

    public interface IViewComponentUpdater<T> where T : struct, ISpatialComponentUpdate
    {
        void ApplyUpdate(long entityId, in T update);
    }
}

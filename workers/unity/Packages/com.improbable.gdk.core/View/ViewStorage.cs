using System;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public interface IViewStorage
    {
        Type GetSnapshotType();
        Type GetUpdateType();
    }

    public interface IViewComponentStorage<T> where T : struct, ISpatialComponentSnapshot
    {
        void AddComponent(long entityId, T component);
        void RemoveComponent(long entityId);

        bool HasComponent(long entityId);
        T GetComponent(long entityId);

        Authority GetAuthority(long entityId);
        void SetAuthority(long entityId, Authority authority);
    }

    public interface IViewUpdateHandler<U> where U : struct, ISpatialComponentUpdate
    {
        void ApplyUpdate(long entityId, U update);
    }
}

using System;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface IEcsViewManager
    {
        void Init(World world);
        void Clean(World world);

        uint GetComponentId();

        ComponentType[] GetInitialComponents();

        void ApplyDiff(ViewDiff diff);
    }
}

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    /// <summary>
    ///     ECS Component stores an ordered list of authority changes.
    /// </summary>
    /// <remarks>
    ///     This component is created during the <see cref="SpatialOSReceiveSystem"/> and populated with all the
    ///     authority changes received in a single update.
    ///     This component will be removed during the <see cref="CleanReactiveComponentsSystem"/>
    /// </remarks>
    /// <typeparam name="T">The SpatialOS component which had authority changes.</typeparam>
    public struct AuthorityChanges<T> : IComponentData where T : ISpatialComponentData
    {
        public uint Handle;

        public List<Authority> Changes
        {
            get => AuthorityChangesProvider.Get(Handle);
            set => AuthorityChangesProvider.Set(Handle, value);
        }
    }
}
#endif

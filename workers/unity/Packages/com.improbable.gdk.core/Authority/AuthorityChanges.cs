using System.Collections.Generic;
using Improbable.Worker.Core;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
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

using System;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Core
{
    public interface IComponentMetaclass
    {
        uint ComponentId { get; }
        string Name { get; }

        Type Data { get; }
        Type Authority { get; }
        Type Snapshot { get; }
        Type Update { get; }
        Type[] Events { get; }

        Type ReplicationSystem { get; }
        Type EventSerializer { get; }
        Type DiffDeserializer { get; }

        Type DiffStorage { get; }
        Type EcsViewManager { get; }
        Type DynamicInvokable { get; }

        ICommandMetaclass[] Commands { get; }
    }
}

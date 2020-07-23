using System;

namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandMetaclass
    {
        uint ComponentId { get; }
        uint CommandIndex { get; }
        string Name { get; }

        Type DiffDeserializer { get; }
        Type Serializer { get; }

        Type MetaDataStorage { get; }
        Type SendStorage { get; }
        Type DiffStorage { get; }

        Type Response { get; }
        Type ReceivedResponse { get; }
        Type Request { get; }
        Type ReceivedRequest { get; }
    }
}

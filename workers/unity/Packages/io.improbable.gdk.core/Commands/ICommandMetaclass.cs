using System;

namespace Improbable.Gdk.Core.Commands
{
    public interface ICommandMetaclass
    {
        uint CommandIndex { get; }
        string Name { get; }

        Type DiffDeserializer { get; }
        Type Serializer { get; }

        Type MetaDataStorage { get; }
        Type SendStorage { get; }
        Type DiffStorage { get; }
    }
}

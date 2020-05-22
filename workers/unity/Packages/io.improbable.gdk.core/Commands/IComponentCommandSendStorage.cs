using System;

namespace Improbable.Gdk.Core.Commands
{
    public interface IComponentCommandSendStorage : ICommandSendStorage
    {
        uint ComponentId { get; }
        uint CommandId { get; }

        Type RequestType { get; }
        Type ResponseType { get; }
    }
}

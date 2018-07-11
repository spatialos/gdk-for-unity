using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public enum CommandStatusCode
    {
        Invalid = 0,
        Success = 1,
        Timeout = 2,
        NotFound = 3,
        AuthorityLost = 4,
        PermissionDenied = 5,
        ApplicationError = 6,
        InternalError = 7,
    }

    public interface IOutgoingCommandRequest
    {
        long TargetEntityId { get; }

        long SenderEntityId { get; }
    }

    public interface IIncomingCommandRequest
    {
        List<string> CallerAttributeSet { get; }
    }

    public interface IOutgoingCommandResponse
    {
        uint RequestId { get; }
    }

    public interface IIncomingCommandResponse
    {
    }
}

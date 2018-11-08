using System;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker;

namespace Improbable.Gdk.GameObjectRepresentation
{
    public interface ICommandRequestSender
    {
        EntityId EntityId { get; }
        
        long SendCommand<TCommand, TRequest, TResponse>(
            CommandTypeInformation<TCommand, TRequest, TResponse> typeInformation, EntityId entityId, TRequest request,
            Action<TResponse> callback, uint? timeoutMillis, bool allowShortCircuiting)
            where TRequest : struct
            where TResponse : struct, IReceivedResponse;
    }

    public struct CommandTypeInformation<TCommand, TRequest, TResponse>
        where TRequest : struct
        where TResponse : struct, IReceivedResponse
    {
    }
}

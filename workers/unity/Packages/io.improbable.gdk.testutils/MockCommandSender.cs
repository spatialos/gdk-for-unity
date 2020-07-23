using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.TestUtils
{
    public class MockCommandSender : ICoreCommandSender
    {
        private readonly MockWorld world;

        private long nextRequestId = 1;

        private readonly Dictionary<Type, SortedSet<long>> requestIds = new Dictionary<Type, SortedSet<long>>();

        private readonly Dictionary<long, ICommandRequest> outboundRequests = new Dictionary<long, ICommandRequest>();

        internal MockCommandSender(MockWorld world)
        {
            this.world = world;
        }

        public IEnumerable<(CommandRequestId, TRequest)> GetOutboundCommandRequests<TRequest>() where TRequest : ICommandRequest
        {
            return requestIds[typeof(TRequest)].Select(id => (new CommandRequestId(id), (TRequest) outboundRequests[id]));
        }

        public CommandRequestId SendCommand<TRequest>(TRequest request, Entity sendingEntity = default) where TRequest : ICommandRequest
        {
            if (!requestIds.ContainsKey(typeof(TRequest)))
            {
                requestIds.Add(typeof(TRequest), new SortedSet<long>());
            }

            requestIds[typeof(TRequest)].Add(nextRequestId);
            outboundRequests.Add(nextRequestId, request);
            return new CommandRequestId(nextRequestId++);
        }

        public void GenerateResponses<TRequest, TResponse>(Func<CommandRequestId, TRequest, TResponse> creator)
            where TRequest : ICommandRequest
            where TResponse : struct, IReceivedCommandResponse
        {
            if (!requestIds.TryGetValue(typeof(TRequest), out var ids))
            {
                throw new ArgumentException($"Cannot generate {typeof(TResponse)} without first sending a {typeof(TRequest)}");
            }

            var commandClass = ComponentDatabase.GetRequestCommandMetaclass<TRequest>();
            if (typeof(TResponse) != commandClass.ReceivedResponse)
            {
                throw new ArgumentException($"Invalid response type {typeof(TResponse)}, expected {commandClass.ReceivedResponse}");
            }

            foreach (var id in ids)
            {
                var request = outboundRequests[id];
                world.Connection.AddCommandResponse(creator(new CommandRequestId(id), (TRequest) request), commandClass.ComponentId, commandClass.CommandIndex);
                outboundRequests.Remove(id);
            }

            ids.Clear();
        }

        public void GenerateResponse<TRequest, TResponse>(CommandRequestId id, Func<CommandRequestId, TRequest, TResponse> creator)
            where TRequest : ICommandRequest
            where TResponse : struct, IReceivedCommandResponse
        {
            if (!outboundRequests.TryGetValue(id.Raw, out var request))
            {
                throw new ArgumentException($"Could not find a request with request id {id}");
            }

            var commandClass = ComponentDatabase.GetRequestCommandMetaclass<TRequest>();
            if (typeof(TResponse) != commandClass.ReceivedResponse)
            {
                throw new ArgumentException($"Invalid response type {typeof(TResponse)}, expected {commandClass.ReceivedResponse}");
            }

            world.Connection.AddCommandResponse(creator(id, (TRequest) request), commandClass.ComponentId, commandClass.CommandIndex);
            requestIds[typeof(TRequest)].Remove(id.Raw);
            outboundRequests.Remove(id.Raw);
        }
    }
}

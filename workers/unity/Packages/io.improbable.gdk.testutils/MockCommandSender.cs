using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.TestUtils;
using Unity.Entities;

namespace Packages.io.improbable.gdk.testutils
{
    public class MockCommandSender : ICoreCommandSender
    {
        public long NextRequestId { get; private set; } = 1;

        private readonly Dictionary<Type, uint> componentIds = new Dictionary<Type, uint>();

        private Dictionary<long, ICommandRequest> requests = new Dictionary<long, ICommandRequest>();

        public void Setup<TRequest>(uint componentId)
        {
            componentIds.Add(typeof(TRequest), componentId);
        }

        public CommandRequestId SendCommand<T>(T request, Entity sendingEntity = default) where T : ICommandRequest
        {
            if (!componentIds.ContainsKey(typeof(T)))
            {
                throw new AggregateException($"Component ID of {typeof(T)} must be set up");
            }

            requests.Add(NextRequestId, request);
            return new CommandRequestId(NextRequestId++);
        }

        public void FlushResponses<TRequest, TResponse>(MockWorld world,
            Func<TRequest, CommandRequestId, TResponse> creator)
            where TRequest : ICommandRequest
            where TResponse : struct, IReceivedCommandResponse
        {
            var filteredRequests = requests.Where(pair => pair.Value.GetType() == typeof(TRequest)).ToArray();
            foreach (var request in filteredRequests)
            {
                world.Connection.AddCommandResponse(creator((TRequest) request.Value, new CommandRequestId(request.Key)), componentIds[typeof(TRequest)], (uint) request.Key);
            }

            requests = (Dictionary<long, ICommandRequest>) requests.Except(filteredRequests);
        }
    }
}

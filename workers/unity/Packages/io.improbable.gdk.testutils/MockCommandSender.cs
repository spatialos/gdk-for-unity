using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.TestUtils;
using Unity.Entities;

namespace Packages.io.improbable.gdk.testutils
{
    public class MockCommandSender : ICoreCommandSender
    {
        private readonly MockWorld world;
        public long NextRequestId { get; private set; } = 1;

        private readonly Dictionary<Type, uint> componentIds = new Dictionary<Type, uint>();

        private readonly Dictionary<Type, HashSet<long>> requestIds = new Dictionary<Type, HashSet<long>>();

        private readonly Dictionary<long, ICommandRequest> outboundRequests = new Dictionary<long, ICommandRequest>();

        internal MockCommandSender(MockWorld world)
        {
            this.world = world;
        }

        public void Setup<TRequest>(uint componentId)
        {
            componentIds.Add(typeof(TRequest), componentId);
        }

        public CommandRequestId SendCommand<TRequest>(TRequest request, Entity sendingEntity = default) where TRequest : ICommandRequest
        {
            if (!requestIds.ContainsKey(typeof(TRequest)))
            {
                requestIds.Add(typeof(TRequest), new HashSet<long>());
            }

            if (!componentIds.ContainsKey(typeof(TRequest)))
            {
                throw new AggregateException($"Could not retrieve Component ID of the {typeof(TRequest)} command");
            }

            requestIds[typeof(TRequest)].Add(NextRequestId);
            outboundRequests.Add(NextRequestId, request);
            return new CommandRequestId(NextRequestId++);
        }

        public void GenerateResponses<TRequest, TResponse>(Func<CommandRequestId, TRequest, TResponse> creator)
            where TRequest : ICommandRequest
            where TResponse : struct, IReceivedCommandResponse
        {
            var ids = requestIds[typeof(TRequest)];
            var componentId = componentIds[typeof(TRequest)];
            foreach (var id in ids)
            {
                var request = outboundRequests[id];
                world.Connection.AddCommandResponse(creator(new CommandRequestId(id), (TRequest) request), componentId, (uint) id);
                outboundRequests.Remove(id);
            }

            ids.Clear();
        }

        public void GenerateResponse<TRequest, TResponse>(CommandRequestId id, Func<CommandRequestId, TRequest, TResponse> creator)
            where TRequest : ICommandRequest
            where TResponse : struct, IReceivedCommandResponse
        {
            var componentId = componentIds[typeof(TRequest)];
            var request = outboundRequests[id.Raw];
            world.Connection.AddCommandResponse(creator(id, (TRequest) request), componentId, (uint) id.Raw);
            requestIds[typeof(TRequest)].Remove(id.Raw);
            outboundRequests.Remove(id.Raw);
        }
    }
}

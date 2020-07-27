using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core.NetworkStats;
using Improbable.Worker.CInterop;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    public class MockConnectionHandlerBuilder : IConnectionHandlerBuilder
    {
        public readonly MockConnectionHandler ConnectionHandler;

        public MockConnectionHandlerBuilder(bool enableSerialization = false)
        {
            ConnectionHandler = new MockConnectionHandler
            {
                EnableSerialization = enableSerialization
            };
        }

        public Task<IConnectionHandler> CreateAsync(CancellationToken? token = null)
        {
            return Task.FromResult((IConnectionHandler) ConnectionHandler);
        }

        public string WorkerType { get; }
    }

    public class MockConnectionHandler : IConnectionHandler
    {
        internal bool EnableSerialization;

        private readonly SerializedMessagesToSend serializedMessagesToSend = new SerializedMessagesToSend();
        private readonly CommandMetaData commandMetaData = new CommandMetaData();
        private readonly Dictionary<Type, SortedSet<long>> requestIds = new Dictionary<Type, SortedSet<long>>();
        private readonly Dictionary<long, ICommandRequest> outgoingRequests = new Dictionary<long, ICommandRequest>();
        internal readonly IOutgoingCommandHandler OutgoingCommandHandler;

        internal MockConnectionHandler()
        {
            OutgoingCommandHandler = new MockOutgoingCommandHandler(OnSendCommand);
        }

        private uint updateId;

        private ViewDiff CurrentDiff => diffs[currentDiffIndex];

        private readonly ViewDiff[] diffs = new[]
        {
            new ViewDiff(), new ViewDiff()
        };

        private int currentDiffIndex = 0;

        public void CreateEntity(long entityId, EntityTemplate template)
        {
            var handler = new CreateEntityTemplateDynamicHandler(template, entityId, CurrentDiff);
            Dynamic.ForEachComponent(handler);
        }

        public void ChangeAuthority(long entityId, uint componentId, Authority newAuthority)
        {
            CurrentDiff.SetAuthority(entityId, componentId, newAuthority);
        }

        public void UpdateComponent<T>(long entityId, uint componentId, T update) where T : ISpatialComponentUpdate
        {
            CurrentDiff.AddComponentUpdate(update, entityId, componentId, updateId++);
        }

        public void AddEvent<T>(long entityId, uint componentId, T ev) where T : IEvent
        {
            CurrentDiff.AddEvent(ev, entityId, componentId, updateId++);
        }

        public void RemoveEntity(long entityId)
        {
            CurrentDiff.RemoveEntity(entityId);
        }

        public void RemoveEntityAndComponents(long entityId, EntityTemplate template)
        {
            var handler = new RemoveEntityTemplateDynamicHandler(template, entityId, CurrentDiff);
            Dynamic.ForEachComponent(handler);

            RemoveEntity(entityId);
        }

        public void RemoveComponent(long entityId, uint componentId)
        {
            CurrentDiff.RemoveComponent(entityId, componentId);
        }

        public void AddComponent<T>(long entityId, uint componentId, T component)
            where T : ISpatialComponentUpdate
        {
            CurrentDiff.AddComponent(component, entityId, componentId);
        }

        public void UpdateComponentAndAddEvents<TUpdate, TEvent>(long entityId, uint componentId, TUpdate update,
            params TEvent[] events)
            where TUpdate : ISpatialComponentUpdate
            where TEvent : IEvent
        {
            var thisUpdateId = updateId++;

            CurrentDiff.AddComponentUpdate(update, entityId, componentId, thisUpdateId);
            foreach (var ev in events)
            {
                CurrentDiff.AddEvent(ev, entityId, componentId, thisUpdateId);
            }
        }

        private void OnSendCommand(long nextRequestId, Type requestType, ICommandRequest request)
        {
            if (!requestIds.ContainsKey(requestType))
            {
                requestIds.Add(requestType, new SortedSet<long>());
            }

            requestIds[requestType].Add(nextRequestId);
            outgoingRequests.Add(nextRequestId, request);
        }

        public IEnumerable<CommandRequestId> GetOutboundCommandRequests<TRequest>() where TRequest : ICommandRequest
        {
            if (!requestIds.TryGetValue(typeof(TRequest), out var ids))
            {
                ids = new SortedSet<long>();
                requestIds.Add(typeof(TRequest), ids);
            }

            return ids.Select(id => new CommandRequestId(id));
        }

        public void GenerateResponses<TRequest, TResponse>(Func<CommandRequestId, TRequest, TResponse> creator)
            where TRequest : ICommandRequest
            where TResponse : struct, IReceivedCommandResponse
        {
            var ids = GetOutboundCommandRequests<TRequest>();
            var commandClass = ComponentDatabase.GetCommandMetaclassFromRequest<TRequest>();
            if (typeof(TResponse) != commandClass.ReceivedResponse)
            {
                throw new ArgumentException($"Invalid response type {typeof(TResponse)}, expected {commandClass.ReceivedResponse}");
            }

            foreach (var id in ids)
            {
                var request = outgoingRequests[id.Raw];
                CurrentDiff.AddCommandResponse(creator(id, (TRequest) request), commandClass.ComponentId, commandClass.CommandIndex);
                outgoingRequests.Remove(id.Raw);
            }

            requestIds[typeof(TRequest)].Clear();
        }

        public void GenerateResponse<TRequest, TResponse>(CommandRequestId id, Func<CommandRequestId, TRequest, TResponse> creator)
            where TRequest : ICommandRequest
            where TResponse : struct, IReceivedCommandResponse
        {
            if (!outgoingRequests.TryGetValue(id.Raw, out var request))
            {
                throw new ArgumentException($"Could not find a request with request id {id}");
            }

            var commandClass = ComponentDatabase.GetCommandMetaclassFromRequest<TRequest>();
            if (typeof(TResponse) != commandClass.ReceivedResponse)
            {
                throw new ArgumentException($"Invalid response type {typeof(TResponse)}, expected {commandClass.ReceivedResponse}");
            }

            CurrentDiff.AddCommandResponse(creator(id, (TRequest) request), commandClass.ComponentId, commandClass.CommandIndex);
            requestIds[typeof(TRequest)].Remove(id.Raw);
            outgoingRequests.Remove(id.Raw);
        }

        public void AddCommandRequest<TRequest>(TRequest receivedRequest)
            where TRequest : struct, IReceivedCommandRequest
        {
            var commandClass = ComponentDatabase.GetCommandMetaclassFromReceivedRequest<TRequest>();
            CurrentDiff.AddCommandRequest(receivedRequest, commandClass.ComponentId, commandClass.CommandIndex);
        }

        #region IConnectionHandler implementation

        public string GetWorkerId()
        {
            return "TestWorker";
        }

        public List<string> GetWorkerAttributes()
        {
            return new List<string> { "attribute_the_first", "attribute_the_second" };
        }

        public void GetMessagesReceived(ref ViewDiff viewDiff)
        {
            var diffToReturn = CurrentDiff;

            currentDiffIndex = (currentDiffIndex + 1) % diffs.Length;
            var nextDiff = CurrentDiff;
            nextDiff.Clear();

            viewDiff = diffToReturn;
        }

        public MessagesToSend GetMessagesToSendContainer()
        {
            return new MessagesToSend();
        }

        public void PushMessagesToSend(MessagesToSend messages, NetFrameStats netFrameStats)
        {
            if (!EnableSerialization)
            {
                return;
            }

            serializedMessagesToSend.SerializeFrom(messages, commandMetaData);

            // Unable to actually send anything over a Connection, so just tidy up after serialization.

            serializedMessagesToSend.Clear();
            messages.Clear();
        }

        public bool IsConnected()
        {
            return true;
        }

        #endregion

        private class CreateEntityTemplateDynamicHandler : Dynamic.IHandler
        {
            private readonly EntityTemplate template;
            private readonly long entityId;
            private readonly ViewDiff viewDiff;

            public CreateEntityTemplateDynamicHandler(EntityTemplate template, long entityId, ViewDiff viewDiff)
            {
                this.template = template;
                this.viewDiff = viewDiff;
                this.entityId = entityId;

                viewDiff.AddEntity(this.entityId);
            }

            public void Accept<TUpdate, TSnapshot>(uint componentId, Dynamic.VTable<TUpdate, TSnapshot> vtable)
                where TUpdate : struct, ISpatialComponentUpdate
                where TSnapshot : struct, ISpatialComponentSnapshot
            {
                var maybeSnapshot = template.GetComponent<TSnapshot>();

                if (!maybeSnapshot.HasValue)
                {
                    return;
                }

                var snapshot = maybeSnapshot.Value;
                viewDiff.AddComponent(vtable.ConvertSnapshotToUpdate(snapshot), entityId, componentId);
            }
        }

        private class RemoveEntityTemplateDynamicHandler : Dynamic.IHandler
        {
            private readonly EntityTemplate template;
            private readonly long entityId;
            private readonly ViewDiff viewDiff;

            public RemoveEntityTemplateDynamicHandler(EntityTemplate template, long entityId, ViewDiff viewDiff)
            {
                this.template = template;
                this.entityId = entityId;
                this.viewDiff = viewDiff;
            }

            public void Accept<TUpdate, TSnapshot>(uint componentId, Dynamic.VTable<TUpdate, TSnapshot> vtable)
                where TUpdate : struct, ISpatialComponentUpdate
                where TSnapshot : struct, ISpatialComponentSnapshot
            {
                if (template.HasComponent<TSnapshot>())
                {
                    viewDiff.RemoveComponent(entityId, componentId);
                }
            }
        }

        private class MockOutgoingCommandHandler : IOutgoingCommandHandler
        {
            private long nextRequestId = 1;

            private readonly Action<long, Type, ICommandRequest> onSendCommand;

            internal MockOutgoingCommandHandler(Action<long, Type, ICommandRequest> onSendCommand)
            {
                this.onSendCommand = onSendCommand;
            }

            public CommandRequestId SendCommand<TRequest>(TRequest request, Entity sendingEntity = default) where TRequest : ICommandRequest
            {
                onSendCommand(nextRequestId, typeof(TRequest), request);
                return new CommandRequestId(nextRequestId++);
            }

            public void SendResponse<T>(T response) where T : ICommandResponse
            {
                // Do nothing
            }
        }

        public void Dispose()
        {
        }
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core.NetworkStats;
using Improbable.Worker.CInterop;

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

        public void AddCommandResponse<TResponse>(TResponse response, uint componentId, uint commandId)
            where TResponse : struct, IReceivedCommandResponse
        {
            CurrentDiff.AddCommandResponse(response, componentId, commandId);
        }

        public void AddCommandRequest<TRequest>(TRequest response, uint componentId, uint commandId)
            where TRequest : struct, IReceivedCommandRequest
        {
            CurrentDiff.AddCommandRequest(response, componentId, commandId);
        }

        // TODO: Commands

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

        public void Dispose()
        {
        }
    }
}

using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    public class CommandSystem : ComponentSystem
    {
        internal IOutgoingCommandHandler OutgoingHandler { get; set; }
        private WorkerSystem worker;

        public CommandRequestId SendCommand<T>(T request, Entity sendingEntity = default) where T : ICommandRequest
        {
            return OutgoingHandler.SendCommand(request, sendingEntity);
        }

        public void SendResponse<T>(T response) where T : ICommandResponse
        {
            OutgoingHandler.SendResponse(response);
        }

        public MessagesSpan<T> GetRequests<T>() where T : struct, IReceivedCommandRequest
        {
            var manager = (IDiffCommandRequestStorage<T>) worker.Diff.GetCommandDiffStorage(typeof(T));
            return manager.GetRequests();
        }

        public MessagesSpan<T> GetRequests<T>(EntityId targetEntityId) where T : struct, IReceivedCommandRequest
        {
            var manager = (IDiffCommandRequestStorage<T>) worker.Diff.GetCommandDiffStorage(typeof(T));
            return manager.GetRequests(targetEntityId);
        }

        public MessagesSpan<T> GetResponses<T>() where T : struct, IReceivedCommandResponse
        {
            var manager = (IDiffCommandResponseStorage<T>) worker.Diff.GetCommandDiffStorage(typeof(T));
            return manager.GetResponses();
        }

        public T? GetResponse<T>(CommandRequestId requestId) where T : struct, IReceivedCommandResponse
        {
            var manager = (IDiffCommandResponseStorage<T>) worker.Diff.GetCommandDiffStorage(typeof(T));
            return manager.GetResponse(requestId);
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            worker = World.GetExistingSystem<WorkerSystem>();
            OutgoingHandler = new OutgoingCommandHandler(worker);
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }

        private class OutgoingCommandHandler : IOutgoingCommandHandler
        {
            private long nextRequestId = 1;

            private WorkerSystem Worker { get; }

            public OutgoingCommandHandler(WorkerSystem worker)
            {
                Worker = worker;
            }

            public CommandRequestId SendCommand<T>(T request, Entity sendingEntity = default) where T : ICommandRequest
            {
                var requestId = new CommandRequestId(nextRequestId++);
                Worker.MessagesToSend.AddCommandRequest(request, sendingEntity, requestId);
                return requestId;
            }

            public void SendResponse<T>(T response) where T : ICommandResponse
            {
                Worker.MessagesToSend.AddCommandResponse(response);
            }
        }
    }
}

using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    public class CommandSystem : ComponentSystem
    {
        private WorkerSystem worker;

        private long nextRequestId = 1;

        public long SendCommand<T>(T request, Entity sendingEntity) where T : ICommandRequest
        {
            worker.MessagesToSend.AddCommandRequest(request, sendingEntity, nextRequestId);
            return nextRequestId++;
        }

        public long SendCommand<T>(T request) where T : ICommandRequest
        {
            worker.MessagesToSend.AddCommandRequest(request, Entity.Null, nextRequestId);
            return nextRequestId++;
        }

        public void SendResponse<T>(T response) where T : ICommandResponse
        {
            worker.MessagesToSend.AddCommandResponse(response);
        }

        public ReceivedMessagesSpan<T> GetRequests<T>() where T : struct, IReceivedCommandRequest
        {
            var manager = (IDiffCommandRequestStorage<T>) worker.Diff.GetCommandDiffStorage(typeof(T));
            return manager.GetRequests();
        }

        public ReceivedMessagesSpan<T> GetRequests<T>(EntityId targetEntityId) where T : struct, IReceivedCommandRequest
        {
            var manager = (IDiffCommandRequestStorage<T>) worker.Diff.GetCommandDiffStorage(typeof(T));
            return manager.GetRequests(targetEntityId);
        }

        public ReceivedMessagesSpan<T> GetResponses<T>() where T : struct, IReceivedCommandResponse
        {
            var manager = (IDiffCommandResponseStorage<T>) worker.Diff.GetCommandDiffStorage(typeof(T));
            return manager.GetResponses();
        }

        public ReceivedMessagesSpan<T> GetResponse<T>(long requestId) where T : struct, IReceivedCommandResponse
        {
            var manager = (IDiffCommandResponseStorage<T>) worker.Diff.GetCommandDiffStorage(typeof(T));
            return manager.GetResponse(requestId);
        }

        protected override void OnCreateManager()
        {
            worker = World.GetExistingManager<WorkerSystem>();

            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}

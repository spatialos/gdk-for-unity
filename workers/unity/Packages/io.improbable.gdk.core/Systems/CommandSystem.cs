using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface ICoreCommandSender
    {
        long NextRequestId { get; }
        CommandRequestId SendCommand<T>(T request, Entity sendingEntity = default) where T : ICommandRequest;
    }

    internal class CommandSender : ICoreCommandSender
    {
        public CommandSender(WorkerSystem worker)
        {
            Worker = worker;
        }

        public long NextRequestId { get; private set; } = 1;

        private WorkerSystem Worker { get; }

        public CommandRequestId SendCommand<T>(T request, Entity sendingEntity = default) where T : ICommandRequest
        {
            var requestId = new CommandRequestId(NextRequestId++);
            Worker.MessagesToSend.AddCommandRequest(request, sendingEntity, requestId);
            return requestId;
        }
    }

    [DisableAutoCreation]
    public class CommandSystem : ComponentSystem
    {
        private ICoreCommandSender sender;
        private WorkerSystem worker;

        public CommandRequestId SendCommand<T>(T request, Entity sendingEntity = default) where T : ICommandRequest
        {
            return sender.SendCommand(request, sendingEntity);
        }

        public void SendResponse<T>(T response) where T : ICommandResponse
        {
            worker.MessagesToSend.AddCommandResponse(response);
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
            sender = new CommandSender(worker);
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}

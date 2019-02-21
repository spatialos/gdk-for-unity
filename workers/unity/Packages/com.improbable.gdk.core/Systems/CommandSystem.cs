using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    [UpdateBefore(typeof(ComponentUpdateSystem))]
    public class CommandSystem : ComponentSystem
    {
        private WorkerSystem worker;

        private readonly List<ICommandManager> managers = new List<ICommandManager>();

        private readonly Dictionary<Type, int> requestTypeToIndex = new Dictionary<Type, int>();
        private readonly Dictionary<Type, int> receivedResponseTypeToIndex = new Dictionary<Type, int>();

        private long nextRequestId = 1;

        public long SendCommand<T>(T request, Entity sendingEntity) where T : ICommandRequest
        {
            if (requestTypeToIndex.TryGetValue(typeof(T), out var index))
            {
                return ((ICommandRequestSender<T>) managers[index]).SendCommand(request, sendingEntity);
            }

            worker.MessagesToSend.AddCommandRequest(request, sendingEntity, nextRequestId);
            return nextRequestId++;
        }

        public long SendCommand<T>(T request) where T : ICommandRequest
        {
            if (requestTypeToIndex.TryGetValue(typeof(T), out var index))
            {
                return ((ICommandRequestSender<T>) managers[index]).SendCommand(request, Entity.Null);
            }

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

        public ReceivedMessagesSpan<T> GetResponses<T>() where T : struct, IReceivedCommandResponse
        {
            if (receivedResponseTypeToIndex.TryGetValue(typeof(T), out var index))
            {
                return ((ICommandResponseReceiver<T>) managers[index]).GetResponsesReceived();
            }

            var manager = (IDiffCommandResponseStorage<T>) worker.Diff.GetCommandDiffStorage(typeof(T));
            return manager.GetResponses();
        }

        internal void ApplyDiff(ViewDiff diff)
        {
            foreach (var manager in managers)
            {
                manager.ApplyAndCleanDiff(diff);
            }
        }

        protected override void OnCreateManager()
        {
            worker = World.GetExistingManager<WorkerSystem>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(ICommandManager).IsAssignableFrom(type) || type.IsAbstract)
                    {
                        continue;
                    }

                    var instance = (ICommandManager) Activator.CreateInstance(type);
                    instance.Init(World);

                    switch (instance)
                    {
                        case IComponentCommandManager componentCommandManager:
                            break;
                        case IWorldCommandManager worldCommandManager:
                            requestTypeToIndex.Add(worldCommandManager.GetRequestType(), managers.Count);
                            receivedResponseTypeToIndex.Add(worldCommandManager.GetReceivedResponseType(), managers.Count);
                            break;
                    }

                    managers.Add(instance);
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (var manager in managers)
            {
                manager.SendAll();
            }
        }
    }
}

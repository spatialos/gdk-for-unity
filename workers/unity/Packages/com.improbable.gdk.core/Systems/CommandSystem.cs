using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class CommandSystem : ComponentSystem
    {
        private readonly List<ICommandManager> managers = new List<ICommandManager>();
        private readonly Dictionary<Type, int> requestTypeToIndex = new Dictionary<Type, int>();
        private readonly Dictionary<Type, int> receivedRequestTypeToIndex = new Dictionary<Type, int>();
        private readonly Dictionary<Type, int> responseTypeToIndex = new Dictionary<Type, int>();
        private readonly Dictionary<Type, int> receivedResponseTypeToIndex = new Dictionary<Type, int>();

        public long SendCommand<T>(T request, Entity sendingEntity) where T : ICommandRequest
        {
            if (!requestTypeToIndex.TryGetValue(typeof(T), out var index))
            {
                throw new ArgumentException("Type is not a valid command request");
            }

            return ((ICommandRequestSender<T>) managers[index]).SendCommand(request, sendingEntity);
        }

        public void SendResponse<T>(T response) where T : ICommandResponse
        {
            if (!responseTypeToIndex.TryGetValue(typeof(T), out var index))
            {
                throw new ArgumentException("Type is not a valid command request");
            }

            ((ICommandResponseSender<T>) managers[index]).SendResponse(response);
        }

        public List<T> GetRequests<T>() where T : IReceivedCommandRequest
        {
            if (!receivedRequestTypeToIndex.TryGetValue(typeof(T), out var index))
            {
                throw new ArgumentException("Type is not a valid received request");
            }

            return ((ICommandRequestReceiver<T>) managers[index]).GetRequestsReceived();
        }

        public List<T> GetResponses<T>() where T : IReceivedCommandResponse
        {
            if (!receivedResponseTypeToIndex.TryGetValue(typeof(T), out var index))
            {
                throw new ArgumentException("Type is not a valid received response");
            }

            return ((ICommandResponseReceiver<T>) managers[index]).GetResponsesReceived();
        }

        protected override void OnCreateManager()
        {
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
                            requestTypeToIndex.Add(componentCommandManager.GetRequestType(), managers.Count);
                            receivedRequestTypeToIndex.Add(componentCommandManager.GetReceivedRequestType(), managers.Count);
                            responseTypeToIndex.Add(componentCommandManager.GetResponseType(), managers.Count);
                            receivedResponseTypeToIndex.Add(componentCommandManager.GetReceivedResponseType(), managers.Count);
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

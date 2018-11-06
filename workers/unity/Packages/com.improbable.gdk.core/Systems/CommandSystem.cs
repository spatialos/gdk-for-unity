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

        public void SendCommand<T>(T request, Entity sendingEntity)
        {
            if (!requestTypeToIndex.TryGetValue(typeof(T), out var index))
            {
                throw new ArgumentException("Type is not a valid command request");
            }

            ((ICommandRequestSender<T>) managers[index]).SendCommand(request, sendingEntity);
        }

        public void SendResponse<T>(T response)
        {
            if (!responseTypeToIndex.TryGetValue(typeof(T), out var index))
            {
                throw new ArgumentException("Type is not a valid command request");
            }

            ((ICommandResponseSender<T>) managers[index]).SendResponse(response);
        }

        public List<T> GetResponses<T>()
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

                    requestTypeToIndex.Add(instance.GetRequestType(), managers.Count);
                    receivedRequestTypeToIndex.Add(instance.GetReceivedRequestType(), managers.Count);
                    responseTypeToIndex.Add(instance.GetResponseType(), managers.Count);
                    receivedResponseTypeToIndex.Add(instance.GetReceivedResponseType(), managers.Count);
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

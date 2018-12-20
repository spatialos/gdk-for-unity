using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public class CommandTaskManager<T> : ITaskManager where T : IReceivedCommandResponse
    {
        private readonly Dictionary<long, TaskCompletionSource<T>> registeredTasks =
            new Dictionary<long, TaskCompletionSource<T>>();

        private readonly CommandSystem commandSystem;

        public CommandTaskManager(World world)
        {
            commandSystem = world.GetExistingManager<CommandSystem>();
        }

        public void RegisterTask(long requestId, TaskCompletionSource<T> task)
        {
            if (registeredTasks.ContainsKey(requestId))
            {
                task.SetException(new ArgumentException("Request Id is already registered."));
                return;
            }
            registeredTasks.Add(requestId, task);
        }

        public void CompleteTasks()
        {
            var responses = commandSystem.GetResponses<T>();
            foreach (var response in responses)
            {
                var requestId = response.GetRequestId();
                if (registeredTasks.TryGetValue(requestId, out var task))
                {
                    task.SetResult(response);
                    registeredTasks.Remove(requestId);
                }
            }
        }
    }
}

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

        public Task<T> RegisterTask(long requestId)
        {
            TaskCompletionSource<T> taskSource = new TaskCompletionSource<T>();
            if (registeredTasks.ContainsKey(requestId))
            {
                taskSource.SetException(new ArgumentException("Request Id is already registered."));
            }
            else
            {
                registeredTasks.Add(requestId, taskSource);
            }

            return taskSource.Task;
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

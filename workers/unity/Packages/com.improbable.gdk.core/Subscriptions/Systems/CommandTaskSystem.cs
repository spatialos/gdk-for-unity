using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [DisableAutoCreation]
    public class CommandTaskSystem : ComponentSystem
    {
        private readonly Dictionary<Type, ITaskManager> taskManagers =
            new Dictionary<Type, ITaskManager>();

        public void RegisterTask<T>(long requestId, TaskCompletionSource<T> task)
            where T : IReceivedCommandResponse
        {
            if (!taskManagers.TryGetValue(typeof(T), out var manager))
            {
                manager = new CommandTaskManager<T>(World);
                taskManagers.Add(typeof(T), manager);
            }

            ((CommandTaskManager<T>) manager).RegisterTask(requestId, task);
        }

        internal void CompleteTasks()
        {
            foreach (var manager in taskManagers.Values)
            {
                manager.CompleteTasks();
            }
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}

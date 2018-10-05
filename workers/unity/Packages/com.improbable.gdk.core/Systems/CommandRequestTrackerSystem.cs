using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Tracks command requests sent from this worker that have not received a response yet.
    /// </summary>
    [DisableAutoCreation]
    public class CommandRequestTrackerSystem : ComponentSystem
    {
        private readonly Dictionary<Type, CommandStorage> commandStorageInstances =
            new Dictionary<Type, CommandStorage>();

        public TConcreteCommandStorage GetCommandStorageForType<TConcreteCommandStorage>()
            where TConcreteCommandStorage : CommandStorage
        {
            if (!commandStorageInstances.TryGetValue(typeof(TConcreteCommandStorage), out var storage))
            {
                throw new ArgumentException(
                    $"Could not find command storage of type {typeof(TConcreteCommandStorage).Name}");
            }

            return (TConcreteCommandStorage) storage;
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            Enabled = false;
            CreateConcreteCommandStorage();
        }

        // Will never tick - only need this to compile.
        protected override void OnUpdate()
        {
        }

        private void CreateConcreteCommandStorage()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var commandStorageTypes = assembly.GetTypes()
                    .Where(type => typeof(CommandStorage).IsAssignableFrom(type) && !type.IsAbstract);

                foreach (var commandStorageType in commandStorageTypes)
                {
                    var storage = (CommandStorage) Activator.CreateInstance(commandStorageType);
                    commandStorageInstances.Add(storage.GetType(), storage);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class CommandSenderSystem : ComponentSystem
    {
        private readonly Dictionary<Type, int> typeToIndex = new Dictionary<Type, int>();
        private readonly List<ICommandSender> senders = new List<ICommandSender>();

        public T GetCommandSender<T>() where T : ICommandSender
        {
            return (T) senders[typeToIndex[typeof(T)]];
        }

        protected override void OnCreateManager()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Find all components with the RemoveAtEndOfTick attribute
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(ICommandSender).IsAssignableFrom(type) || type.IsAbstract)
                    {
                        continue;
                    }

                    var instance = (ICommandSender) Activator.CreateInstance(type);
                    instance.Init(World);

                    typeToIndex.Add(type, senders.Count);
                    senders.Add(instance);
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (var sender in senders)
            {
                sender.SendAll();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    public class CommandComponentSystem : ComponentSystem
    {
        private readonly List<ICommandComponentManager> managers = new List<ICommandComponentManager>();

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(ICommandComponentManager).IsAssignableFrom(type) || type.IsAbstract)
                    {
                        continue;
                    }

                    var instance = (ICommandComponentManager) Activator.CreateInstance(type);
                    managers.Add(instance);
                }
            }
        }

        [Inject] private EntityManager entityManager;
        [Inject] private CommandSystem commandSystem;
        [Inject] private WorkerSystem workerSystem;

        protected override void OnUpdate()
        {
            foreach (var manager in managers)
            {
                manager.PopulateCommandComponents(commandSystem, entityManager, workerSystem, World);
            }
        }
    }
}

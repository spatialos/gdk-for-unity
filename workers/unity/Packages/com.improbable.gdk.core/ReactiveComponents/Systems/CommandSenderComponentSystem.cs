using System;
using System.Collections.Generic;
using Improbable.Gdk.ReactiveComponents;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    public class CommandSenderComponentSystem : ComponentSystem
    {
        private readonly List<ICommandSenderComponentManager> managers = new List<ICommandSenderComponentManager>();

        [Inject] private WorkerSystem workerSystem;
        [Inject] private EntitySystem entitySystem;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(ICommandSenderComponentManager).IsAssignableFrom(type) || type.IsAbstract)
                    {
                        continue;
                    }

                    var instance = (ICommandSenderComponentManager) Activator.CreateInstance(type);
                    managers.Add(instance);

                    // Add stuff to the worker entity
                    instance.AddComponents(workerSystem.WorkerEntity, EntityManager, World);
                }
            }
        }

        protected override void OnDestroyManager()
        {
            foreach (var manager in managers)
            {
                manager.Clean(World);
            }

            base.OnDestroyManager();
        }

        protected override void OnUpdate()
        {
            foreach (var entityId in entitySystem.GetEntitiesAdded())
            {
                foreach (var manager in managers)
                {
                    workerSystem.TryGetEntity(entityId, out var entity);
                    manager.AddComponents(entity, EntityManager, World);
                }
            }

            foreach (var entityId in entitySystem.GetEntitiesRemoved())
            {
                foreach (var manager in managers)
                {
                    manager.RemoveComponents(entityId, EntityManager, World);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateAfter(typeof(SpatialOSReceiveSystem))]
    public class CommandSenderComponentSystem : ComponentSystem
    {
        private readonly List<ICommandSenderComponentManager> managers = new List<ICommandSenderComponentManager>();

        private WorkerSystem workerSystem;
        private EntitySystem entitySystem;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            workerSystem = World.GetExistingManager<WorkerSystem>();
            entitySystem = World.GetExistingManager<EntitySystem>();

            foreach (var type in ReflectionUtility.GetNonAbstractTypes(typeof(ICommandSenderComponentManager)))
            {
                var instance = (ICommandSenderComponentManager) Activator.CreateInstance(type);
                managers.Add(instance);

                // Add stuff to the worker entity
                instance.AddComponents(workerSystem.WorkerEntity, EntityManager, World);
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

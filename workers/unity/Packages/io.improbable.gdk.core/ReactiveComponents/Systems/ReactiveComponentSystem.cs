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
    public class ReactiveComponentSystem : ComponentSystem
    {
        private readonly List<IReactiveComponentManager> managers = new List<IReactiveComponentManager>();

        private EntityManager entityManager;
        private ComponentUpdateSystem updateSystem;
        private WorkerSystem workerSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            entityManager = World.EntityManager;
            updateSystem = World.GetExistingSystem<ComponentUpdateSystem>();
            workerSystem = World.GetExistingSystem<WorkerSystem>();

            foreach (var type in ReflectionUtility.GetNonAbstractTypes(typeof(IReactiveComponentManager)))
            {
                var instance = (IReactiveComponentManager) Activator.CreateInstance(type);
                managers.Add(instance);
            }
        }

        protected override void OnDestroy()
        {
            foreach (var manager in managers)
            {
                manager.Clean(World);
            }

            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
            foreach (var manager in managers)
            {
                manager.PopulateReactiveComponents(updateSystem, entityManager, workerSystem, World);
            }
        }
    }
}

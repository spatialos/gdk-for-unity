using System;
using System.Collections.Generic;
using System.Reflection;
using Improbable.Gdk.Core.CodegenAdapters;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Removes GDK reactive components and components with attribute RemoveAtEndOfTick from all entities
    /// </summary>
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSCleanGroup))]
    public class CleanReactiveComponentsSystem : ComponentSystem
    {
        private MutableView view;

        private readonly List<Action> removeComponentActions = new List<Action>();

        // Here to prevent adding an action for the same type multiple times
        private readonly HashSet<Type> typesToRemove = new HashSet<Type>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            var worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;

            GenerateComponentGroups();
        }

        private void GenerateComponentGroups()
        {
            var addRemoveComponentActionMethod =
                typeof(CleanReactiveComponentsSystem).GetMethod("AddRemoveComponentAction",
                    BindingFlags.NonPublic | BindingFlags.Instance);


            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Find all components with the RemoveAtEndOfTick attribute
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttribute<RemoveAtEndOfTick>(false) == null)
                    {
                        continue;
                    }

                    if (!typeof(IComponentData).IsAssignableFrom(type)
                        && !typeof(ISharedComponentData).IsAssignableFrom(type))
                    {
                        continue;
                    }

                    if (typesToRemove.Contains(type))
                    {
                        continue;
                    }

                    typesToRemove.Add(type);
                    addRemoveComponentActionMethod.MakeGenericMethod(type).Invoke(this, null);
                }

                // Find all ComponentCleanupHandlers
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(ComponentCleanupHandler).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var componentCleanupHandler = (ComponentCleanupHandler) Activator.CreateInstance(type);
                        foreach (var componentType in componentCleanupHandler.CleanUpComponentTypes)
                        {
                            typesToRemove.Add(componentType.GetManagedType());
                            addRemoveComponentActionMethod.MakeGenericMethod(componentType.GetManagedType()).Invoke(this, null);
                        }
                    }
                }
            }
        }

        private void AddRemoveComponentAction<T>()
        {
            var componentGroup = GetComponentGroup(ComponentType.ReadOnly<T>());
            removeComponentActions.Add(() =>
            {
                if (componentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entityArray = componentGroup.GetEntityArray();
                for (var i = 0; i < entityArray.Length; ++i)
                {
                    PostUpdateCommands.RemoveComponent<T>(entityArray[i]);
                }
            });
        }

        protected override void OnUpdate()
        {
            foreach (var removeComponentAction in removeComponentActions)
            {
                removeComponentAction();
            }
        }
    }
}

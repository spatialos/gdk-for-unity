using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Lorem ipsum
    /// </summary>
    // [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSCleanGroup))]
    public class GameObjectDispatcherCallbacksSystem : ComponentSystem
    {
        private MutableView view;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            var worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;

            GenerateComponentGroups();
        }

        private void GenerateComponentGroups()
        {
            foreach (var monoBehaviourTranslation in view.MonoBehaviourTranslations)
            {
                monoBehaviourTranslation.ComponentAddedComponentGroup =
                    GetComponentGroup(monoBehaviourTranslation.ComponentAddedComponentTypes);
                monoBehaviourTranslation.ComponentRemovedComponentGroup =
                    GetComponentGroup(monoBehaviourTranslation.ComponentRemovedComponentTypes);
                monoBehaviourTranslation.AuthoritiesChangedComponentGroup =
                    GetComponentGroup(monoBehaviourTranslation.AuthoritiesChangedComponentTypes);
                if (monoBehaviourTranslation.ComponentsUpdatedComponentTypes.Length > 0)
                {
                    monoBehaviourTranslation.ComponentsUpdatedComponentGroup =
                        GetComponentGroup(monoBehaviourTranslation.ComponentsUpdatedComponentTypes);
                }

                monoBehaviourTranslation.EventsReceivedComponentGroups = new List<ComponentGroup>();
                foreach (var eventsReceivedComponentTypes in monoBehaviourTranslation.EventsReceivedComponentTypeArrays)
                {
                    monoBehaviourTranslation.EventsReceivedComponentGroups.Add(GetComponentGroup(eventsReceivedComponentTypes));
                }

                monoBehaviourTranslation.CommandRequestsComponentGroups = new List<ComponentGroup>();
                foreach (var commandRequestsComponentTypes in monoBehaviourTranslation.CommandRequestsComponentTypeArrays)
                {
                    monoBehaviourTranslation.CommandRequestsComponentGroups.Add(GetComponentGroup(commandRequestsComponentTypes));
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (var monoBehaviourTranslation in view.MonoBehaviourTranslations)
            {
                monoBehaviourTranslation.InvokeOnAddComponentCallbacks();
                monoBehaviourTranslation.InvokeOnRemoveComponentCallbacks();
                monoBehaviourTranslation.InvokeOnAuthorityChangeCallbacks();
                monoBehaviourTranslation.InvokeOnComponentUpdateCallbacks();
                monoBehaviourTranslation.InvokeOnEventCallbacks();
                monoBehaviourTranslation.InvokeOnCommandRequestCallbacks();
            }
        }
    }
}

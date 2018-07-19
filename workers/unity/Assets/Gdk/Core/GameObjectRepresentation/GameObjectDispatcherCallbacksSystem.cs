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
            foreach (var monoBehaviourTranslation in view.GameObjectTranslations)
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

                monoBehaviourTranslation.EventsReceivedComponentGroups = new ComponentGroup[monoBehaviourTranslation.EventsReceivedComponentTypeArrays.Length];
                for (var i = 0; i < monoBehaviourTranslation.EventsReceivedComponentTypeArrays.Length; i++)
                {
                    monoBehaviourTranslation.EventsReceivedComponentGroups[i] =
                        GetComponentGroup(monoBehaviourTranslation.EventsReceivedComponentTypeArrays[i]);
                }

                monoBehaviourTranslation.CommandRequestsComponentGroups = new ComponentGroup[monoBehaviourTranslation.CommandRequestsComponentTypeArrays.Length];
                for (var i = 0; i < monoBehaviourTranslation.CommandRequestsComponentTypeArrays.Length; i++)
                {
                    monoBehaviourTranslation.CommandRequestsComponentGroups[i] =
                        GetComponentGroup(monoBehaviourTranslation.CommandRequestsComponentTypeArrays[i]);
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (var monoBehaviourTranslation in view.GameObjectTranslations)
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

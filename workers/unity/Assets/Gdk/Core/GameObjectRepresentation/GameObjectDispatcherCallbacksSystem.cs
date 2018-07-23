using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Gathers incoming dispatcher ops and invokes callbacks on relevant GameObjects.
    /// </summary>
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
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
            foreach (var gameObjectTranslation in view.GameObjectTranslations)
            {
                gameObjectTranslation.ComponentAddedComponentGroup =
                    GetComponentGroup(gameObjectTranslation.ComponentAddedComponentTypes);
                gameObjectTranslation.ComponentRemovedComponentGroup =
                    GetComponentGroup(gameObjectTranslation.ComponentRemovedComponentTypes);
                gameObjectTranslation.AuthoritiesChangedComponentGroup =
                    GetComponentGroup(gameObjectTranslation.AuthoritiesChangedComponentTypes);
                if (gameObjectTranslation.ComponentsUpdatedComponentTypes.Length > 0)
                {
                    gameObjectTranslation.ComponentsUpdatedComponentGroup =
                        GetComponentGroup(gameObjectTranslation.ComponentsUpdatedComponentTypes);
                }

                gameObjectTranslation.EventsReceivedComponentGroups =
                    new ComponentGroup[gameObjectTranslation.EventsReceivedComponentTypeArrays.Length];
                for (var i = 0; i < gameObjectTranslation.EventsReceivedComponentTypeArrays.Length; i++)
                {
                    gameObjectTranslation.EventsReceivedComponentGroups[i] =
                        GetComponentGroup(gameObjectTranslation.EventsReceivedComponentTypeArrays[i]);
                }

                gameObjectTranslation.CommandRequestsComponentGroups =
                    new ComponentGroup[gameObjectTranslation.CommandRequestsComponentTypeArrays.Length];
                for (var i = 0; i < gameObjectTranslation.CommandRequestsComponentTypeArrays.Length; i++)
                {
                    gameObjectTranslation.CommandRequestsComponentGroups[i] =
                        GetComponentGroup(gameObjectTranslation.CommandRequestsComponentTypeArrays[i]);
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (var gameObjectTranslation in view.GameObjectTranslations)
            {
                gameObjectTranslation.InvokeOnAddComponentCallbacks();
                gameObjectTranslation.InvokeOnRemoveComponentCallbacks();
                gameObjectTranslation.InvokeOnAuthorityChangeCallbacks();
                gameObjectTranslation.InvokeOnComponentUpdateCallbacks();
                gameObjectTranslation.InvokeOnEventCallbacks();
                gameObjectTranslation.InvokeOnCommandRequestCallbacks();
            }
        }
    }
}

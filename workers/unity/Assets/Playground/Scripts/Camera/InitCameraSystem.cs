using Improbable.Gdk.Core;
using Improbable.Gdk.ReactiveComponents;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class InitCameraSystem : ComponentSystem
    {
        [Inject] private ComponentUpdateSystem componentUpdateSystem;

        private ComponentGroup inputGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            inputGroup = GetComponentGroup(
                ComponentType.ReadOnly<PlayerInput.ComponentAuthority>(),
                ComponentType.ReadOnly<SpatialEntityId>()
            );
            inputGroup.SetFilter(new PlayerInput.ComponentAuthority(true));
        }

        protected override void OnUpdate()
        {
            var entities = inputGroup.GetEntityArray();
            var spatialIdData = inputGroup.GetComponentDataArray<SpatialEntityId>();

            for (var i = 0; i < entities.Length; i++)
            {
                var authChanges = componentUpdateSystem.GetAuthorityChangesReceived(spatialIdData[i].EntityId, PlayerInput.ComponentId);
                if (authChanges.Count > 0)
                {
                    var entity = entities[i];
                    PostUpdateCommands.AddComponent(entity, CameraComponentDefaults.Input);
                    PostUpdateCommands.AddComponent(entity, CameraComponentDefaults.Transform);

                    Cursor.lockState = CursorLockMode.Locked;

                    // Disable system after first run.
                    Enabled = false;
                }
            }
        }

        protected override void OnDestroyManager()
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

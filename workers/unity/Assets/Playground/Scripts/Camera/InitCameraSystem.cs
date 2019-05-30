using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class InitCameraSystem : ComponentSystem
    {
        private ComponentUpdateSystem componentUpdateSystem;
        private ComponentGroup inputGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            componentUpdateSystem = World.GetExistingManager<ComponentUpdateSystem>();

            inputGroup = GetComponentGroup(
                ComponentType.ReadOnly<PlayerInput.ComponentAuthority>(),
                ComponentType.ReadOnly<SpatialEntityId>()
            );
            inputGroup.SetFilter(PlayerInput.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            Entities.With(inputGroup).ForEach((Entity entity, ref SpatialEntityId spatialEntityId) =>
            {
                var authChanges =
                    componentUpdateSystem.GetAuthorityChangesReceived(spatialEntityId.EntityId,
                        PlayerInput.ComponentId);
                if (authChanges.Count > 0)
                {
                    PostUpdateCommands.AddComponent(entity, CameraComponentDefaults.Input);
                    PostUpdateCommands.AddComponent(entity, CameraComponentDefaults.Transform);

                    Cursor.lockState = CursorLockMode.Locked;

                    // Disable system after first run.
                    Enabled = false;
                }
            });
        }

        protected override void OnDestroyManager()
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

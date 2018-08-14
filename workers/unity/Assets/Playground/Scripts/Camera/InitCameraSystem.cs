using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class InitCameraSystem : ComponentSystem
    {
        public struct Data
        {
#pragma warning disable 649
            public readonly int Length;
            public EntityArray Entites;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSPlayerInput>> PlayerInput;
            [ReadOnly] public ComponentArray<AuthoritiesChanged<SpatialOSPlayerInput>> PlayerInputAuthority;
#pragma warning restore 649
        }

#pragma warning disable 649
        [Inject] private Data data;
#pragma warning restore 649

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                PostUpdateCommands.AddComponent(data.Entites[i], CameraComponentDefaults.Input);
                PostUpdateCommands.AddComponent(data.Entites[i], CameraComponentDefaults.Transform);

                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}

using Improbable.Gdk.Core;
using Improbable.Gdk.ReactiveComponents;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class InitCameraSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public EntityArray Entites;
            [ReadOnly] public ComponentDataArray<Authoritative<PlayerInput.Component>> PlayerInput;
            [ReadOnly] public ComponentDataArray<AuthorityChanges<PlayerInput.Component>> PlayerInputAuthority;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var entity = data.Entites[i];
                PostUpdateCommands.AddComponent(entity, CameraComponentDefaults.Input);
                PostUpdateCommands.AddComponent(entity, CameraComponentDefaults.Transform);

                Cursor.lockState = CursorLockMode.Locked;

                // Disable system after first run.
                Enabled = false;
            }
        }

        protected override void OnDestroyManager()
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}

using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global

#endregion

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class LocalPlayerInputSync : ComponentSystem
    {
        public struct PlayerInputData
        {
            public readonly int Length;
            public ComponentDataArray<SpatialOSPlayerInput> PlayerInput;
            public ComponentDataArray<CameraTransform> CameraTransform;
            public ComponentDataArray<Authoritative<SpatialOSPlayerInput>> PlayerInputAuthority;
        }

        [Inject] private PlayerInputData playerInputData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < playerInputData.Length; i++)
            {
                var cameraTransform = playerInputData.CameraTransform[i];
                var forward = cameraTransform.Rotation * Vector3.up;
                var right = cameraTransform.Rotation * Vector3.right;
                var input = Input.GetAxisRaw("Horizontal") * right + Input.GetAxisRaw("Vertical") * forward;

                var newPlayerInput = new SpatialOSPlayerInput
                {
                    Horizontal = input.x,
                    Vertical = input.z,
                    Running = Input.GetKey(KeyCode.LeftShift)
                };

                playerInputData.PlayerInput[i] = newPlayerInput;
            }
        }
    }
}

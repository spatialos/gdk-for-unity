using System;
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
        private struct PlayerInputData
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
                var isShiftDown = Input.GetKey(KeyCode.LeftShift);

                var oldPlayerInput = playerInputData.PlayerInput[i];

                if (Math.Abs(oldPlayerInput.Horizontal - input.x) > 0.01f
                    || Math.Abs(oldPlayerInput.Vertical - input.z) > 0.01f
                    || oldPlayerInput.Running != isShiftDown)
                {
                    var newPlayerInput = new SpatialOSPlayerInput
                    {
                        Horizontal = input.x,
                        Vertical = input.z,
                        Running = isShiftDown
                    };
                    playerInputData.PlayerInput[i] = newPlayerInput;
                }
            }
        }
    }
}

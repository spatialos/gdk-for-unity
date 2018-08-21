using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
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
            [ReadOnly] public ComponentDataArray<LocalInput> LocalInput;
            public ComponentDataArray<SpatialOSPlayerInput> PlayerInput;
            [ReadOnly] public ComponentDataArray<CameraTransform> CameraTransform;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSPlayerInput>> PlayerInputAuthority;
        }

        [Inject] private PlayerInputData playerInputData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < playerInputData.Length; i++)
            {
                var localInput = playerInputData.LocalInput[i];
                var cameraTransform = playerInputData.CameraTransform[i];
                var forward = cameraTransform.Rotation * Vector3.up;
                var right = cameraTransform.Rotation * Vector3.right;
                var input = localInput.LeftStick.x * right + localInput.LeftStick.y * forward;
                var newPlayerInput = new SpatialOSPlayerInput
                {
                    Horizontal = input.x,
                    Vertical = input.z,
                    Running = localInput.Running
                };
                playerInputData.PlayerInput[i] = newPlayerInput;
            }
        }
    }
}

using System;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class LocalPlayerInputSync : ComponentSystem
    {
        private const float MinInputChange = 0.01f;

        private ComponentGroup inputGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            inputGroup = GetComponentGroup(
                ComponentType.Create<PlayerInput.Component>(),
                ComponentType.Create<CameraTransform>(),
                ComponentType.ReadOnly<PlayerInput.ComponentAuthority>()
            );
            inputGroup.SetFilter(PlayerInput.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            var cameraTransformData = inputGroup.GetComponentDataArray<CameraTransform>();
            var playerInputData = inputGroup.GetComponentDataArray<PlayerInput.Component>();

            for (var i = 0; i < cameraTransformData.Length; i++)
            {
                var cameraTransform = cameraTransformData[i];
                var forward = cameraTransform.Rotation * Vector3.up;
                var right = cameraTransform.Rotation * Vector3.right;
                var input = Input.GetAxisRaw("Horizontal") * right + Input.GetAxisRaw("Vertical") * forward;
                var isShiftDown = Input.GetKey(KeyCode.LeftShift);

                var oldPlayerInput = playerInputData[i];

                if (Math.Abs(oldPlayerInput.Horizontal - input.x) > MinInputChange
                    || Math.Abs(oldPlayerInput.Vertical - input.z) > MinInputChange
                    || oldPlayerInput.Running != isShiftDown)
                {
                    var newPlayerInput = new PlayerInput.Component
                    {
                        Horizontal = input.x,
                        Vertical = input.z,
                        Running = isShiftDown
                    };

                    playerInputData[i] = newPlayerInput;
                }
            }
        }
    }
}

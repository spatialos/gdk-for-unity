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

        private EntityQuery inputGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            inputGroup = GetEntityQuery(
                ComponentType.ReadWrite<PlayerInput.Component>(),
                ComponentType.ReadWrite<CameraTransform>(),
                ComponentType.ReadOnly<PlayerInput.ComponentAuthority>()
            );
            inputGroup.SetFilter(PlayerInput.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            Entities.With(inputGroup).ForEach(
                (ref CameraTransform cameraTransform, ref PlayerInput.Component playerInput) =>
                {
                    var forward = cameraTransform.Rotation * Vector3.up;
                    var right = cameraTransform.Rotation * Vector3.right;
                    var input = Input.GetAxisRaw("Horizontal") * right + Input.GetAxisRaw("Vertical") * forward;
                    var isShiftDown = Input.GetKey(KeyCode.LeftShift);

                    var oldPlayerInput = playerInput;

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

                        playerInput = newPlayerInput;
                    }
                });
        }
    }
}

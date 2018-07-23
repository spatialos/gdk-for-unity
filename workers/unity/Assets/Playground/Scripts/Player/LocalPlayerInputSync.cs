using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

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

        public VirtualJoystick VirtualJoystick;

        [Inject] private PlayerInputData playerInputData;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            VirtualJoystick = GameObject.FindGameObjectWithTag("GameController").GetComponent<VirtualJoystick>();
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < playerInputData.Length; i++)
            {
                var cameraTransform = playerInputData.CameraTransform[i];
                var forward = cameraTransform.Rotation * Vector3.up;
                var right = cameraTransform.Rotation * Vector3.right;
                var input = Vector3.zero;
                if (VirtualJoystick.InputDirection != Vector3.zero)
                {
                    input = VirtualJoystick.InputDirection.x * forward + VirtualJoystick.InputDirection.z * right;
                }

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

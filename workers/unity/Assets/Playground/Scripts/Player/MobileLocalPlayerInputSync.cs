using System;
using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class MobileLocalPlayerInputSync : ComponentSystem
    {
        private struct PlayerInputData
        {
            public readonly int Length;
            public ComponentDataArray<SpatialOSPlayerInput> PlayerInput;
            public ComponentDataArray<CameraTransform> CameraTransform;
            public ComponentDataArray<Authoritative<SpatialOSPlayerInput>> PlayerInputAuthority;
        }

        private struct LocalPlayerInputData
        {
            public readonly int Length;
            public ComponentDataArray<LocalInput> LocalPlayerInput;
        }

        [Inject] private PlayerInputData playerInputData;
        [Inject] private LocalPlayerInputData localPlayerInputData;

        private VirtualJoystick VirtualJoystick;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            try
            {
                var controllerJoystick = GameObject.FindGameObjectWithTag("GameController");
                VirtualJoystick = controllerJoystick.GetComponent<VirtualJoystick>();
            }
            catch (NullReferenceException)
            {
                WorkerRegistry.GetWorkerForWorld(World)
                    .View.LogDispatcher.HandleLog(LogType.Error,
                        new LogEvent("Could not find movement virtual stick. Movement is now disabled on mobile"));
                Enabled = false;
            }
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < playerInputData.Length; i++)
            {
                var cameraTransform = playerInputData.CameraTransform[i];
                var forward = cameraTransform.Rotation * Vector3.up;
                var right = cameraTransform.Rotation * Vector3.right;
                var input = VirtualJoystick.InputDirection.x * right + VirtualJoystick.InputDirection.y * forward;
                var newPlayerInput = new SpatialOSPlayerInput
                {
                    Horizontal = input.x,
                    Vertical = input.z,
                    //TODO: Enable running on mobile somehow?
                    Running = Input.GetKey(KeyCode.LeftShift)
                };
                playerInputData.PlayerInput[i] = newPlayerInput;
            }

            for (var i = 0; i < localPlayerInputData.Length; i++)
            {
                var newLocalPlayerInput = new LocalInput
                {
                    //TODO: Make this detect touches on non-virtual sticks only
                    ShootSmall = Input.GetMouseButtonDown(0),
                    ShootLarge = Input.GetMouseButtonDown(1)
                };
                localPlayerInputData.LocalPlayerInput[i] = newLocalPlayerInput;
            }
        }
    }
}

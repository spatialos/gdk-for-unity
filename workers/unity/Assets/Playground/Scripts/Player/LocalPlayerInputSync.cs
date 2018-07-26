using System;
using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

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
            try
            {
                GameObject controllerJoystick = GameObject.FindGameObjectWithTag("GameController");
                VirtualJoystick = controllerJoystick.GetComponent<VirtualJoystick>();
#if !(UNITY_ANDROID || UNITY_IOS)
                controllerJoystick.SetActive(false);
#endif
            }
            catch (NullReferenceException)
            {
                WorkerRegistry.GetWorkerForWorld(World)
                    .View.LogDispatcher.HandleLog(LogType.Error,
                        new LogEvent("Could not find movement virtual stick. Movement is now disabled on mobile"));
#if (UNITY_ANDROID || UNITY_IOS)
                Enabled = false;
#endif
            }
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < playerInputData.Length; i++)
            {
                var cameraTransform = playerInputData.CameraTransform[i];
                var forward = cameraTransform.Rotation * Vector3.up;
                var right = cameraTransform.Rotation * Vector3.right;
#if UNITY_ANDROID || UNITY_IOS
                var input = VirtualJoystick.InputDirection.x * right + VirtualJoystick.InputDirection.y * forward;
#else
                var input = Input.GetAxisRaw("Horizontal") * right + Input.GetAxisRaw("Vertical") * forward;
#endif
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

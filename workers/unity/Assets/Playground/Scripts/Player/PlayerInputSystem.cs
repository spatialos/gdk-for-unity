using System;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public class PlayerInputSystem : ComponentSystem
    {
        private struct InputData
        {
            public readonly int Length;
            public ComponentDataArray<LocalInput> PlayerInput;
        }

        [Inject] private InputData inputData;

        private static VirtualJoystick MovementJoystick;
        private static VirtualJoystick CameraJoystick;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            try
            {
                var controllerJoystick = GameObject.FindGameObjectWithTag("MovementJoystick");
                MovementJoystick = controllerJoystick.GetComponent<VirtualJoystick>();
                MovementJoystick.gameObject.SetActive(false);
            }
            catch (NullReferenceException)
            {
                WorkerRegistry.GetWorkerForWorld(World)
                    .View.LogDispatcher.HandleLog(LogType.Error,
                        new LogEvent("Could not find movement virtual stick. Movement is now disabled on mobile"));
            }

            try
            {
                var controllerJoystick = GameObject.FindGameObjectWithTag("CameraJoystick");
                CameraJoystick = controllerJoystick.GetComponent<VirtualJoystick>();
                CameraJoystick.gameObject.SetActive(false);
            }
            catch (NullReferenceException)
            {
                WorkerRegistry.GetWorkerForWorld(World).View.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent("Could not find virtual camera joystick. Camera movement is now disabled on mobile"));
            }
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < inputData.Length; i++)
            {
                var input = inputData.PlayerInput[i];
                input.Horizontal = Input.GetAxis("Horizontal");
                input.Vertical = Input.GetAxis("Vertical");
                input.RightStickHorizontal = Input.GetAxis("Mouse X");
                input.RightStickVertical = Input.GetAxis("Mouse Y");
                input.CameraDistance = Input.GetAxis("Mouse ScrollWheel");
                input.Running = Input.GetKey(KeyCode.LeftShift);
                input.ShootSmall = Input.GetMouseButtonDown(0);
                input.ShootLarge = Input.GetMouseButtonDown(1);
                inputData.PlayerInput[i] = input;
            }
        }
    }
}

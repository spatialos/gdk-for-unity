using System;
using System.Linq;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Playground
{
    public class MobilePlayerInputSystem : ComponentSystem
    {
        private struct InputData
        {
            public readonly int Length;
            public ComponentDataArray<LocalInput> PlayerInput;
        }

        [Inject] private InputData inputData;

        private static GameObject controllerJoystick;
        private static GameObject cameraControllerJoystick;
        private static VirtualJoystick MovementJoystick;
        private static VirtualJoystick CameraJoystick;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            try
            {
                controllerJoystick = GameObject.FindGameObjectWithTag("MovementJoystick");
                MovementJoystick = controllerJoystick.GetComponent<VirtualJoystick>();
            }
            catch (NullReferenceException)
            {
                WorkerRegistry.GetWorkerForWorld(World)
                    .View.LogDispatcher.HandleLog(LogType.Error,
                        new LogEvent("Could not find movement virtual stick. Movement is now disabled on mobile"));
            }

            try
            {
                cameraControllerJoystick = GameObject.FindGameObjectWithTag("CameraJoystick");
                CameraJoystick = controllerJoystick.GetComponent<VirtualJoystick>();
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
                input.Horizontal = MovementJoystick.InputDirection.x;
                input.Vertical = MovementJoystick.InputDirection.y;
                input.RightStickHorizontal = CameraJoystick.InputDirection.x;
                input.RightStickVertical = CameraJoystick.InputDirection.y;
                // input.CameraDistance = Input.GetAxis("Mouse ScrollWheel");
                // input.Running = Input.GetKey(KeyCode.LeftShift);
                input.ShootSmall = isSmallShot();
                input.ShootLarge = isLargeShot();
                inputData.PlayerInput[i] = input;
            }
        }

        private bool isSmallShot()
        {
            if (Input.touches.Length <= 0)
            {
                return false;
            }

            var touchNum = Input.touches.Count(touch =>
                !(controllerJoystick.GetComponent<Image>().rectTransform.rect.Contains(touch.position) &&
                    cameraControllerJoystick.GetComponent<Image>().rectTransform.rect.Contains(touch.position)));

            return touchNum == 1;
        }

        private bool isLargeShot()
        {
            if (Input.touches.Length <= 0)
            {
                return false;
            }

            var touchNum = Input.touches.Count(touch =>
                !(controllerJoystick.GetComponent<Image>().rectTransform.rect.Contains(touch.position) &&
                    cameraControllerJoystick.GetComponent<Image>().rectTransform.rect.Contains(touch.position)));

            return touchNum > 1;
        }
    }
}

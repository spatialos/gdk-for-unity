using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(LocalPlayerInputSync))]
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
        private static VirtualJoystick movementJoystick;
        private static VirtualJoystick cameraJoystick;
        private static HashSet<int> oldTouchSet;
        private static HashSet<int> newTouchSet;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            try
            {
                controllerJoystick = GameObject.FindGameObjectWithTag("MovementJoystick");
                movementJoystick = controllerJoystick.GetComponent<VirtualJoystick>();
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
                cameraJoystick = cameraControllerJoystick.GetComponent<VirtualJoystick>();
            }
            catch (NullReferenceException)
            {
                WorkerRegistry.GetWorkerForWorld(World).View.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent("Could not find virtual camera joystick. Camera movement is now disabled on mobile"));
            }

            oldTouchSet = new HashSet<int>();
            newTouchSet = new HashSet<int>();
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < inputData.Length; i++)
            {
                var input = inputData.PlayerInput[i];
                var touches = GetNonJoystickTouches();
                input.Horizontal = movementJoystick.InputDirection.x;
                input.Vertical = movementJoystick.InputDirection.y;
                input.RightStickHorizontal = cameraJoystick.InputDirection.x;
                input.RightStickVertical = cameraJoystick.InputDirection.y;
                // input.CameraDistance = Input.GetAxis("Mouse ScrollWheel");
                // input.Running = Input.GetKey(KeyCode.LeftShift);
                input.ShootSmall = touches == 1;
                input.ShootLarge = touches >= 2;
                inputData.PlayerInput[i] = input;
            }
        }


        private static int GetNonJoystickTouches()
        {
            var count = 0;
            newTouchSet.Clear();
            foreach (var touch in Input.touches)
            {
                if (oldTouchSet.Contains(touch.fingerId))
                {
                    newTouchSet.Add(touch.fingerId);
                }
            }

            var temp = newTouchSet;
            newTouchSet = oldTouchSet;
            oldTouchSet = temp;

            foreach (var touch in Input.touches)
            {
                if (!(RectTransformUtility.RectangleContainsScreenPoint(
                        controllerJoystick.GetComponent<Image>().rectTransform, touch.position, null) ||
                    RectTransformUtility.RectangleContainsScreenPoint(
                        cameraControllerJoystick.GetComponent<Image>().rectTransform, touch.position, null)) && !oldTouchSet.Contains(touch.fingerId))
                {
                    count++;
                }

                oldTouchSet.Add(touch.fingerId);
            }

            return count;
        }
    }
}

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

        private static VirtualJoystick movementJoystick;
        private static VirtualJoystick cameraJoystick;
        private static RectTransform movementJoystickBoundaries;
        private static RectTransform cameraJoystickBoundaries;
        private static HashSet<int> oldTouchSet = new HashSet<int>();
        private static HashSet<int> newTouchSet = new HashSet<int>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            try
            {
                var controllerJoystick = GameObject.FindGameObjectWithTag("MovementJoystick");
                movementJoystick = controllerJoystick.GetComponent<VirtualJoystick>();
                movementJoystickBoundaries = controllerJoystick.GetComponent<Image>().rectTransform;
            }
            catch (NullReferenceException)
            {
                Worker.GetWorkerFromWorld(World)
                    .LogDispatcher.HandleLog(LogType.Error,
                        new LogEvent("Could not find movement virtual stick. Movement is now disabled on mobile"));
            }

            try
            {
                var controllerJoystick = GameObject.FindGameObjectWithTag("CameraJoystick");
                cameraJoystick = controllerJoystick.GetComponent<VirtualJoystick>();
                cameraJoystickBoundaries = controllerJoystick.GetComponent<Image>().rectTransform;
            }
            catch (NullReferenceException)
            {
                Worker.GetWorkerFromWorld(World).LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent("Could not find virtual camera joystick. Camera movement is now disabled on mobile"));
            }
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < inputData.Length; i++)
            {
                var input = inputData.PlayerInput[i];
                var touches = GetNonJoystickTouches();
                input.LeftStick = movementJoystick.InputDirection;
                input.RightStick = cameraJoystick.InputDirection;
                // Camera zoom and running are not implemented for mobile
                input.ShootSmall = touches == 1;
                input.ShootLarge = touches >= 2;
                inputData.PlayerInput[i] = input;
            }
        }


        private static int GetNonJoystickTouches()
        {
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

            var nonJoystickTouches = 0;
            foreach (var touch in Input.touches)
            {
                if (!oldTouchSet.Contains(touch.fingerId) && !(RectTransformUtility.RectangleContainsScreenPoint(
                        movementJoystickBoundaries, touch.position,
                        null) ||
                    RectTransformUtility.RectangleContainsScreenPoint(cameraJoystickBoundaries, touch.position,
                        null)))
                {
                    nonJoystickTouches++;
                }

                oldTouchSet.Add(touch.fingerId);
            }

            return nonJoystickTouches;
        }
    }
}

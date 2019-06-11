using Unity.Entities;
using UnityEngine;

namespace Playground
{
    /// <summary>
    ///     Reads player input from MouseX, MouseY, ScrollWheel and uses them to
    ///     control a 3rd-person camera Orbiting around their center.
    /// </summary>
    /// <remarks>
    ///     MouseX: controls y-axis orbit.
    ///     MouseY: controls x-axis orbit.
    ///     ScrollWheel: controls the orbit radius, moving the camera closer to or
    ///     farther from the player.
    /// </remarks>
    public class FollowCameraSystem : ComponentSystem
    {
        // The min/max distance from camera to the player.
        private const float MinCameraDistance = 2.0f;
        private const float MaxCameraDistance = 10.0f;
        private const float ZoomScale = 10.0f;

        // The min/max vertical angles for the camera (so you can't clip through the floor).
        private const float MinYAngle = 10.0f;
        private const float MaxYAngle = 80.0f;

        // Origin offset to make camera orbit character's head rather than their feet.
        private static readonly Vector3 TargetOffset = new Vector3(0, 1, 0);

        private EntityQuery group;

        protected override void OnCreate()
        {
            base.OnCreate();

            group = GetEntityQuery(
                ComponentType.ReadWrite<CameraInput>(),
                ComponentType.ReadWrite<CameraTransform>(),
                ComponentType.ReadOnly<Rigidbody>()
            );
        }

        protected override void OnUpdate()
        {
            Entities.With(group).ForEach((Entity entity, Rigidbody rigidbody, ref CameraInput cameraInput,
                ref CameraTransform cameraTransform) =>
            {
                cameraInput = UpdateCameraInput(cameraInput);
                cameraTransform = UpdateCameraTransform(cameraInput, rigidbody.position);

                UpdateCamera(cameraTransform);
            });
        }

        private static CameraInput UpdateCameraInput(CameraInput input)
        {
            var x = input.X + Input.GetAxis("Mouse X");
            var y = input.Y - Input.GetAxis("Mouse Y");
            var distance = input.Distance + Input.GetAxis("Mouse ScrollWheel") * ZoomScale;

            x %= 360;
            y = Mathf.Clamp(y, MinYAngle, MaxYAngle);
            distance = Mathf.Clamp(distance, MinCameraDistance, MaxCameraDistance);

            return new CameraInput
            {
                X = x,
                Y = y,
                Distance = distance
            };
        }

        private static CameraTransform UpdateCameraTransform(CameraInput input, Vector3 targetPosition)
        {
            var dir = new Vector3(0, 0, -input.Distance);
            var orbitRotation = Quaternion.Euler(input.Y, input.X, 0);
            var position = targetPosition + TargetOffset + orbitRotation * dir;
            var rotation = Quaternion.LookRotation(targetPosition + TargetOffset - position);

            return new CameraTransform
            {
                Position = position,
                Rotation = rotation
            };
        }

        private static void UpdateCamera(CameraTransform transform)
        {
            var cameraTransform = Camera.main.transform;
            cameraTransform.position = transform.Position;
            cameraTransform.rotation = transform.Rotation;
        }
    }
}

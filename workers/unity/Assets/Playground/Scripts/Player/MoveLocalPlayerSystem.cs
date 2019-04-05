using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateAfter(typeof(LocalPlayerInputSync))]
    internal class MoveLocalPlayerSystem : ComponentSystem
    {
        public struct Speed : IComponentData
        {
            public float CurrentSpeed;
            public float SpeedSmoothVelocity;
        }

        private ComponentGroup newPlayerGroup;
        private ComponentGroup playerInputGroup;

        private const float WalkSpeed = 2.0f;
        private const float RunSpeed = 6.0f;
        private const float MaxSpeed = 8.0f;

        private const float TurnSmoothTime = 0.2f;
        private float turnSmoothVelocity;

        private const float SpeedSmoothTime = 0.1f;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            newPlayerGroup = GetComponentGroup(
                ComponentType.ReadOnly<PlayerInput.Component>(),
                ComponentType.ReadOnly<PlayerInput.ComponentAuthority>(),
                ComponentType.Subtractive<Speed>()
            );
            newPlayerGroup.SetFilter(PlayerInput.ComponentAuthority.Authoritative);

            playerInputGroup = GetComponentGroup(
                ComponentType.Create<Rigidbody>(),
                ComponentType.Create<Speed>(),
                ComponentType.ReadOnly<PlayerInput.Component>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            playerInputGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            var newSpeedEntities = newPlayerGroup.GetEntityArray();

            for (var i = 0; i < newSpeedEntities.Length; i++)
            {
                var speed = new Speed
                {
                    CurrentSpeed = 0f,
                    SpeedSmoothVelocity = 0f
                };

                PostUpdateCommands.AddComponent(newSpeedEntities[i], speed);
            }

            var entities = playerInputGroup.GetEntityArray();
            var playerInputData = playerInputGroup.GetComponentDataArray<PlayerInput.Component>();
            var speedData = playerInputGroup.GetComponentDataArray<Speed>();

            for (var i = 0; i < playerInputData.Length; i++)
            {
                var rigidbody = EntityManager.GetComponentObject<Rigidbody>(entities[i]);
                var playerInput = playerInputData[i];

                var inputDir = new Vector2(playerInput.Horizontal, playerInput.Vertical).normalized;
                if (inputDir != Vector2.zero)
                {
                    var targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
                    rigidbody.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
                        rigidbody.transform.eulerAngles.y, targetRotation,
                        ref turnSmoothVelocity, TurnSmoothTime);
                }

                var targetSpeed = (playerInput.Running ? RunSpeed : WalkSpeed) * inputDir.magnitude;

                var speed = speedData[i];
                var currentSpeed = speed.CurrentSpeed;
                var speedSmoothVelocity = speed.SpeedSmoothVelocity;

                currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, SpeedSmoothTime,
                    MaxSpeed, Time.deltaTime);
                var newSpeed = new Speed
                {
                    CurrentSpeed = currentSpeed,
                    SpeedSmoothVelocity = speedSmoothVelocity
                };

                speedData[i] = newSpeed;

                // This needs to be used instead of add force because this is running in update.
                // It would be better to store this in another component and have something else use it on fixed update.
                rigidbody.velocity = rigidbody.transform.forward * currentSpeed;
            }
        }
    }
}

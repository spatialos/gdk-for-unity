using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;
using Unity.Collections;
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

        private EntityQuery newPlayerGroup;
        private EntityQuery playerInputGroup;

        private const float WalkSpeed = 2.0f;
        private const float RunSpeed = 6.0f;
        private const float MaxSpeed = 8.0f;

        private const float TurnSmoothTime = 0.2f;
        private float turnSmoothVelocity;

        private const float SpeedSmoothTime = 0.1f;

        protected override void OnCreate()
        {
            base.OnCreate();

            newPlayerGroup = GetEntityQuery(
                ComponentType.ReadOnly<PlayerInput.Component>(),
                ComponentType.ReadOnly<PlayerInput.ComponentAuthority>(),
                ComponentType.Exclude<Speed>()
            );
            newPlayerGroup.SetFilter(PlayerInput.ComponentAuthority.Authoritative);

            playerInputGroup = GetEntityQuery(
                ComponentType.ReadWrite<Rigidbody>(),
                ComponentType.ReadWrite<Speed>(),
                ComponentType.ReadOnly<PlayerInput.Component>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            playerInputGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            using (var newSpeedEntities = newPlayerGroup.ToEntityArray(Allocator.TempJob))
            {
                foreach (var entity in newSpeedEntities)
                {
                    var speed = new Speed
                    {
                        CurrentSpeed = 0f,
                        SpeedSmoothVelocity = 0f
                    };

                    PostUpdateCommands.AddComponent(entity, speed);
                }
            }

            Entities.With(playerInputGroup).ForEach(
                (Entity entity, ref PlayerInput.Component playerInput, ref Speed speed) =>
                {
                    var rigidbody = EntityManager.GetComponentObject<Rigidbody>(entity);

                    var inputDir = new Vector2(playerInput.Horizontal, playerInput.Vertical).normalized;
                    if (inputDir != Vector2.zero)
                    {
                        var targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
                        rigidbody.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
                            rigidbody.transform.eulerAngles.y, targetRotation,
                            ref turnSmoothVelocity, TurnSmoothTime);
                    }

                    var targetSpeed = (playerInput.Running ? RunSpeed : WalkSpeed) * inputDir.magnitude;

                    var currentSpeed = speed.CurrentSpeed;
                    var speedSmoothVelocity = speed.SpeedSmoothVelocity;

                    currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, SpeedSmoothTime,
                        MaxSpeed, Time.deltaTime);
                    speed = new Speed
                    {
                        CurrentSpeed = currentSpeed,
                        SpeedSmoothVelocity = speedSmoothVelocity
                    };

                    // This needs to be used instead of add force because this is running in update.
                    // It would be better to store this in another component and have something else use it on fixed update.
                    rigidbody.velocity = rigidbody.transform.forward * currentSpeed;
                });
        }
    }
}

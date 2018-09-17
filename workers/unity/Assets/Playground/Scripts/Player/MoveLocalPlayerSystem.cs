using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

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

        private struct NewPlayerData
        {
            public readonly int Length;
            public EntityArray Entity;
            [ReadOnly] public ComponentDataArray<PlayerInput.Component> PlayerInput;
            [ReadOnly] public ComponentDataArray<Authoritative<TransformInternal.Component>> TransformAuthority;
            public SubtractiveComponent<Speed> NoSpeed;
        }

        private struct PlayerInputData
        {
            public readonly int Length;
            public ComponentArray<Rigidbody> Rigidbody;
            public ComponentDataArray<Speed> SpeedData;
            [ReadOnly] public ComponentDataArray<PlayerInput.Component> PlayerInput;
            [ReadOnly] public ComponentDataArray<Authoritative<TransformInternal.Component>> TransformAuthority;
        }

        [Inject] private NewPlayerData newPlayerData;
        [Inject] private PlayerInputData playerInputData;

        private const float WalkSpeed = 2.0f;
        private const float RunSpeed = 6.0f;
        private const float MaxSpeed = 8.0f;

        private const float TurnSmoothTime = 0.2f;
        private float turnSmoothVelocity;

        private const float SpeedSmoothTime = 0.1f;

        protected override void OnUpdate()
        {
            for (var i = 0; i < newPlayerData.Length; i++)
            {
                var entity = newPlayerData.Entity[i];
                var speed = new Speed
                {
                    CurrentSpeed = 0f,
                    SpeedSmoothVelocity = 0f
                };

                PostUpdateCommands.AddComponent(entity, speed);
            }

            for (var i = 0; i < playerInputData.Length; i++)
            {
                var rigidbody = playerInputData.Rigidbody[i];
                var playerInput = playerInputData.PlayerInput[i];

                var input = new Vector2(playerInput.Horizontal, playerInput.Vertical);
                var inputDir = input.normalized;

                if (inputDir != Vector2.zero)
                {
                    var targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
                    rigidbody.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
                        rigidbody.transform.eulerAngles.y, targetRotation,
                        ref turnSmoothVelocity, TurnSmoothTime);
                }

                var targetSpeed = (playerInput.Running ? RunSpeed : WalkSpeed) * inputDir.magnitude;
                var speed = playerInputData.SpeedData[i];
                var currentSpeed = speed.CurrentSpeed;
                var speedSmoothVelocity = speed.SpeedSmoothVelocity;

                currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, SpeedSmoothTime,
                    MaxSpeed, Time.deltaTime);
                playerInputData.SpeedData[i] = new Speed
                {
                    CurrentSpeed = currentSpeed,
                    SpeedSmoothVelocity = speedSmoothVelocity
                };

                // This needs to be used instead of add force because this is running in update.
                // It would be better to store this in another component and have something else use it on fixed update.
                rigidbody.velocity = rigidbody.transform.forward * currentSpeed;
            }
        }
    }
}

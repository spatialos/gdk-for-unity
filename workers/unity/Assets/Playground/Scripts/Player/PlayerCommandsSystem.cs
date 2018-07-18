using System;
using Generated.Improbable;
using Generated.Playground;
using Improbable.Gdk.Core;
using Playground.Scripts.UI;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class PlayerCommandsSystem : ComponentSystem
    {
        private enum PlayerCommand
        {
            None,
            LaunchSmall,
            LaunchLarge
        };

        private const float LargeEnergy = 50.0f;
        private const float SmallEnergy = 10.0f;

        private MutableView view;

        private struct PlayerData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntity;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSPlayerInput>> PlayerInputAuthority;
            [ReadOnly] public ComponentDataArray<CommandRequestSender<SpatialOSLauncher>> Sender;
        }

        [Inject] private PlayerData playerData;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            view = WorkerRegistry.GetWorkerForWorld(World).View;
        }

        protected override void OnUpdate()
        {
            if (playerData.Length == 0)
            {
                return;
            }

            if (playerData.Length > 1)
            {
                throw new ArgumentOutOfRangeException("playerData",
                    $"Expected at most 1 playerData, got: {playerData.Length}");
            }

            PlayerCommand command;
            if (Input.GetMouseButtonDown(0))
            {
                command = PlayerCommand.LaunchSmall;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                command = PlayerCommand.LaunchLarge;
            }
            else
            {
                return;
            }

            var ray = Camera.main.ScreenPointToRay(UIComponent.Main.Reticle.transform.position);
            RaycastHit info;
            if (!Physics.Raycast(ray, out info) || info.rigidbody == null)
            {
                return;
            }

            var rigidBody = info.rigidbody;
            var sender = playerData.Sender[0];
            var playerId = playerData.SpatialEntity[0].EntityId;

            var component = rigidBody.gameObject.GetComponent<SpatialOSComponent>();
            if (component != null && view.HasComponent(component.Entity, typeof(SpatialOSLaunchable)))
            {
                var impactPoint = new Vector3f { X = info.point.x, Y = info.point.y, Z = info.point.z };
                var launchDirection = new Vector3f { X = ray.direction.x, Y = ray.direction.y, Z = ray.direction.z };

                sender.SendLaunchEntityRequest(playerId, new Generated.Playground.LaunchCommandRequest
                {
                    EntityToLaunch = component.SpatialEntityId,
                    ImpactPoint = impactPoint,
                    LaunchDirection = launchDirection,
                    LaunchEnergy = command == PlayerCommand.LaunchLarge ? LargeEnergy : SmallEnergy
                });
            }
        }
    }
}

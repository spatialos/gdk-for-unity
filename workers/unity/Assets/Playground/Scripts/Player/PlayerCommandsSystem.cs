using System;
using Improbable;
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
            // ReSharper disable once UnusedMember.Local
            None,
            LaunchSmall,
            LaunchLarge
        }

        private const float LargeEnergy = 50.0f;
        private const float SmallEnergy = 10.0f;

        private CommandSystem commandSystem;
        private EntityQuery launchGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            commandSystem = World.GetExistingSystem<CommandSystem>();
            launchGroup = GetEntityQuery(
                ComponentType.ReadOnly<SpatialEntityId>(),
                ComponentType.ReadOnly<PlayerInput.ComponentAuthority>()
            );
            launchGroup.SetFilter(PlayerInput.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            using (var entities = launchGroup.ToEntityArray(Allocator.TempJob))
            using (var spatialIdData = launchGroup.ToComponentDataArray<SpatialEntityId>(Allocator.TempJob))
            {
                if (spatialIdData.Length > 1)
                {
                    throw new InvalidOperationException(
                        $"Expected at most 1 playerData but got {spatialIdData.Length}");
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
                if (!Physics.Raycast(ray, out var info) || info.rigidbody == null)
                {
                    return;
                }

                var rigidBody = info.rigidbody;
                var playerId = spatialIdData[0].EntityId;

                var component = rigidBody.gameObject.GetComponent<LaunchableBehaviour>();

                if (component == null || !EntityManager.HasComponent(component.Entity, typeof(Launchable.Component)))
                {
                    return;
                }

                var impactPoint = Vector3f.FromUnityVector(info.point);
                var launchDirection = Vector3f.FromUnityVector(ray.direction);

                var request = new Launcher.LaunchEntity.Request(playerId,
                    new LaunchCommandRequest(component.EntityId, impactPoint, launchDirection,
                        command == PlayerCommand.LaunchLarge ? LargeEnergy : SmallEnergy,
                        playerId
                    ));

                commandSystem.SendCommand(request, entities[0]);
            }
        }
    }
}

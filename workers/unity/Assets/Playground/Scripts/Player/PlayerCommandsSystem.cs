using System;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.ReactiveComponents;
using Playground.Scripts.UI;
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


        private struct PlayerData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntity;
            [ReadOnly] public ComponentDataArray<Authoritative<PlayerInput.Component>> PlayerInputAuthority;
            public ComponentDataArray<Launcher.CommandSenders.LaunchEntity> Sender;
        }

        [Inject] private PlayerData playerData;

        protected override void OnUpdate()
        {
            if (playerData.Length == 0)
            {
                return;
            }

            if (playerData.Length > 1)
            {
                throw new InvalidOperationException($"Expected at most 1 playerData but got {playerData.Length}");
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
            var sender = playerData.Sender[0];
            var playerId = playerData.SpatialEntity[0].EntityId;

            var component = rigidBody.gameObject.GetComponent<LaunchableBehaviour>();

            if (component == null || !EntityManager.HasComponent(component.Entity, typeof(Launchable.Component)))
            {
                return;
            }

            var impactPoint = Vector3f.FromUnityVector(info.point);
            var launchDirection = Vector3f.FromUnityVector(ray.direction);

            sender.RequestsToSend.Add(new Launcher.LaunchEntity.Request(playerId,
                new LaunchCommandRequest(component.EntityId, impactPoint, launchDirection,
                    command == PlayerCommand.LaunchLarge ? LargeEnergy : SmallEnergy,
                    playerId
                )));

            playerData.Sender[0] = sender;
        }
    }
}

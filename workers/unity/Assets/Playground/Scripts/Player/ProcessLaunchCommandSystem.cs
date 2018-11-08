using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
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
    public class ProcessLaunchCommandSystem : ComponentSystem
    {
        private const float RechargeTime = 2.0f;

        private struct LaunchCommandData
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentDataArray<Launcher.Component> Launcher;
            public ComponentDataArray<Launchable.CommandSenders.LaunchMe> Senders;
            [ReadOnly] public ComponentDataArray<Launcher.CommandRequests.LaunchEntity> Requests;
        }

        private struct LaunchableData
        {
            public readonly int Length;
            public ComponentDataArray<Launchable.Component> Launchable;
            public ComponentArray<Rigidbody> Rigidbody;
            public ComponentDataArray<Launcher.CommandSenders.IncreaseScore> Sender;
            [ReadOnly] public ComponentDataArray<Launchable.CommandRequests.LaunchMe> Requests;
        }

        [Inject] private CommandSystem commandSender;
        [Inject] private LaunchCommandData launchCommandData;
        [Inject] private LaunchableData launchableData;

        protected override void OnUpdate()
        {
            // Handle Launch Commands from players. Only allow if they have energy etc.
            for (var i = 0; i < launchCommandData.Length; i++)
            {
                var launcher = launchCommandData.Launcher[i];
                var entity = launchCommandData.Entity[i];

                if (launcher.RechargeTimeLeft > 0)
                {
                    return;
                }

                var requests = launchCommandData.Requests[i].Requests;
                var energyLeft = launcher.EnergyLeft;
                var j = 0;
                while (energyLeft > 0f && j < requests.Count)
                {
                    var info = requests[j].Payload;
                    var energy = math.min(info.LaunchEnergy, energyLeft);
                    var request = new Launchable.LaunchMe.Request(info.EntityToLaunch,
                        new LaunchMeCommandRequest(info.ImpactPoint, info.LaunchDirection,
                            energy, info.Player));
                    commandSender.SendCommand(request, entity);
                    energyLeft -= energy;
                    j++;
                }

                if (energyLeft <= 0.01f)
                {
                    launcher.EnergyLeft = 0.0f;
                    launcher.RechargeTimeLeft = RechargeTime;
                    PostUpdateCommands.AddComponent(launchCommandData.Entity[i], new Recharging());
                }
                else
                {
                    launcher.EnergyLeft = energyLeft;
                }

                launchCommandData.Launcher[i] = launcher;
            }

            // Handle Launch Me Commands by applying force to rigidbodies proportional to launch energy.
            // Also add launch energy to launcher component.
            for (var i = 0; i < launchableData.Length; i++)
            {
                var rigidbody = launchableData.Rigidbody[i];
                var launchable = launchableData.Launchable[i];
                var sender = launchableData.Sender[i];

                foreach (var request in launchableData.Requests[i].Requests)
                {
                    var info = request.Payload;
                    rigidbody.AddForceAtPosition(
                        new Vector3(info.LaunchDirection.X, info.LaunchDirection.Y, info.LaunchDirection.Z) *
                        info.LaunchEnergy * 100.0f,
                        new Vector3(info.ImpactPoint.X, info.ImpactPoint.Y, info.ImpactPoint.Z)
                    );
                    launchable.MostRecentLauncher = info.Player;

                    sender.RequestsToSend.Add(new Launcher.IncreaseScore.Request(
                        launchable.MostRecentLauncher,
                        new ScoreIncreaseRequest(1.0f)));
                }

                launchableData.Sender[i] = sender;
                launchableData.Launchable[i] = launchable;
            }
        }
    }
}

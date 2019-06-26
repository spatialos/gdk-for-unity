using Improbable.Gdk.Core;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class ProcessLaunchCommandSystem : ComponentSystem
    {
        private const float RechargeTime = 2.0f;

        private CommandSystem commandSystem;
        private WorkerSystem workerSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            commandSystem = World.GetExistingSystem<CommandSystem>();
            workerSystem = World.GetExistingSystem<WorkerSystem>();
        }

        protected override void OnUpdate()
        {
            HandleLaunchEntityRequests();
            HandleLaunchMeRequests();
        }

        private void HandleLaunchEntityRequests()
        {
            var requests = commandSystem.GetRequests<Launcher.LaunchEntity.ReceivedRequest>();
            var launcherComponents = GetComponentDataFromEntity<Launcher.Component>();

            // Handle Launch Commands from players. Only allow if they have energy etc.
            for (var i = 0; i < requests.Count; i++)
            {
                ref readonly var request = ref requests[i];
                if (!workerSystem.TryGetEntity(request.EntityId, out var entity))
                {
                    continue;
                }

                var launcher = launcherComponents[entity];

                if (launcher.RechargeTimeLeft <= 0)
                {
                    var energyLeft = launcher.EnergyLeft;
                    var info = request.Payload;
                    var energy = math.min(info.LaunchEnergy, energyLeft);
                    var launchMeRequest = new Launchable.LaunchMe.Request(info.EntityToLaunch,
                        new LaunchMeCommandRequest(
                            info.ImpactPoint,
                            info.LaunchDirection,
                            energy,
                            info.Player)
                    );

                    commandSystem.SendCommand(launchMeRequest, entity);

                    energyLeft -= energy;
                    if (energyLeft <= 0.01f)
                    {
                        launcher.EnergyLeft = 0.0f;
                        launcher.RechargeTimeLeft = RechargeTime;
                        PostUpdateCommands.AddComponent(entity, new Recharging());
                    }
                    else
                    {
                        launcher.EnergyLeft = energyLeft;
                    }

                    launcherComponents[entity] = launcher;
                }

                commandSystem.SendResponse(new Launcher.LaunchEntity.Response(request.RequestId, new LaunchCommandResponse()));
            }
        }

        private void HandleLaunchMeRequests()
        {
            // Handle Launch Me Commands by applying force to rigidbodies proportional to launch energy.
            // Also add launch energy to launcher component.
            var requests = commandSystem.GetRequests<Launchable.LaunchMe.ReceivedRequest>();
            var launchableComponents = GetComponentDataFromEntity<Launchable.Component>();

            for (var i = 0; i < requests.Count; i++)
            {
                ref readonly var request = ref requests[i];
                if (!workerSystem.TryGetEntity(request.EntityId, out var entity))
                {
                    continue;
                }

                var launchable = launchableComponents[entity];
                var rigidbody = EntityManager.GetComponentObject<Rigidbody>(entity);

                var info = request.Payload;

                rigidbody.AddForceAtPosition(
                    info.LaunchEnergy * 100.0f * new Vector3(info.LaunchDirection.X, info.LaunchDirection.Y, info.LaunchDirection.Z),
                    new Vector3(info.ImpactPoint.X, info.ImpactPoint.Y, info.ImpactPoint.Z)
                );

                launchable.MostRecentLauncher = info.Player;
                launchableComponents[entity] = launchable;

                commandSystem.SendCommand(
                    new Launcher.IncreaseScore.Request(
                        launchable.MostRecentLauncher,
                        new ScoreIncreaseRequest(1.0f)
                    ),
                    entity
                );
            }
        }
    }
}

using Improbable.Gdk.Core;
using Unity.Entities;

namespace Playground
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class ProcessScoresSystem : ComponentSystem
    {
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
            var requests = commandSystem.GetRequests<Launcher.IncreaseScore.ReceivedRequest>();
            var scoreComponents = GetComponentDataFromEntity<Score.Component>();

            for (var i = 0; i < requests.Count; i++)
            {
                var request = requests[i];
                if (!workerSystem.TryGetEntity(request.EntityId, out var entity))
                {
                    continue;
                }

                var component = scoreComponents[entity];
                component.Score += request.Payload.Amount;
                scoreComponents[entity] = component;
            }
        }
    }
}

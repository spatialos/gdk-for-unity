using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Playground
{
    [RemoveAtEndOfTick]
    public struct CollisionComponent : IComponentData
    {
        public Entity ownEntity;
        public Entity otherEntity;
    }

    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class CollisionProcessSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public EntityArray Entities;
            public ComponentDataArray<SpatialOSLaunchable> Launchable;
            [ReadOnly] public ComponentDataArray<CollisionComponent> Collision;
            [ReadOnly] public ComponentDataArray<CommandRequestSender<SpatialOSLauncher>> Sender;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSLaunchable>> DenotesAuthority;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                // Handle all the different possible outcomes of the collision.
                // This requires looking at their most recent launchers.
                var collision = data.Collision[i];
                var first = data.Collision[i].ownEntity;
                var second = data.Collision[i].otherEntity;
                var first_launchable = EntityManager.GetComponentData<SpatialOSLaunchable>(first);
                var second_launchable = EntityManager.GetComponentData<SpatialOSLaunchable>(second);
                var first_launcher = first_launchable.MostRecentLauncher;
                var second_launcher = second_launchable.MostRecentLauncher;
                if (first_launcher == second_launcher)
                {
                    if (first_launcher != 0)
                    {
                        data.Sender[i].SendIncreaseScoreRequest(first_launcher,
                            new Generated.Playground.ScoreIncreaseRequest()
                            {
                                Amount = 1,
                            });
                    }
                }
                else if (second_launcher != 0)
                {
                    var launchable = data.Launchable[i];
                    if (first_launcher == 0)
                    {
                        launchable.MostRecentLauncher = second_launcher;
                        data.Sender[i].SendIncreaseScoreRequest(second_launcher,
                            new Generated.Playground.ScoreIncreaseRequest()
                            {
                                Amount = 1,
                            });
                    }
                    else
                    {
                        launchable.MostRecentLauncher = 0;
                    }

                    PostUpdateCommands.SetComponent(data.Entities[i], launchable);
                }
            }
        }
    }
}

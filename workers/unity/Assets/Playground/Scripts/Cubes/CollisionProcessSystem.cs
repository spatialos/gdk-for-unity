using Generated.Playground;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Playground
{
    [RemoveAtEndOfTick]
    public struct CollisionComponent : IComponentData
    {
        public Entity OwnEntity;
        public Entity OtherEntity;
    }

    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class CollisionProcessSystem : ComponentSystem
    {
        private struct Data
        {
#pragma warning disable 649
            public readonly int Length;
            public EntityArray Entities;
            public ComponentDataArray<SpatialOSLaunchable> Launchable;
            [ReadOnly] public ComponentDataArray<CollisionComponent> Collision;
            [ReadOnly] public ComponentDataArray<CommandRequestSender<SpatialOSLauncher>> Sender;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSLaunchable>> DenotesAuthority;
#pragma warning restore 649
        }

#pragma warning disable 649
        [Inject] private Data data;
#pragma warning restore 649

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                // Handle all the different possible outcomes of the collision.
                // This requires looking at their most recent launchers.
                var collision = data.Collision[i];
                var first = collision.OwnEntity;
                var second = collision.OtherEntity;
                var firstLaunchable = EntityManager.GetComponentData<SpatialOSLaunchable>(first);
                var secondLaunchable = EntityManager.GetComponentData<SpatialOSLaunchable>(second);
                var firstLauncher = firstLaunchable.MostRecentLauncher;
                var secondLauncher = secondLaunchable.MostRecentLauncher;
                if (firstLauncher == secondLauncher)
                {
                    if (firstLauncher != 0)
                    {
                        data.Sender[i].SendIncreaseScoreRequest(firstLauncher,
                            new Generated.Playground.ScoreIncreaseRequest
                            {
                                Amount = 1
                            });
                    }
                }
                else if (secondLauncher != 0)
                {
                    var launchable = data.Launchable[i];
                    if (firstLauncher == 0)
                    {
                        launchable.MostRecentLauncher = secondLauncher;
                        data.Sender[i].SendIncreaseScoreRequest(secondLauncher,
                            new Generated.Playground.ScoreIncreaseRequest
                            {
                                Amount = 1
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

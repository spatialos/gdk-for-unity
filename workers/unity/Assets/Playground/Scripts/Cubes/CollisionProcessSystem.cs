using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Collections;
using Unity.Entities;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global

#endregion

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
            public readonly int Length;
            public EntityArray Entities;
            public ComponentDataArray<SpatialOSLaunchable> Launchable;
            [ReadOnly] public ComponentDataArray<CollisionComponent> Collision;
            [ReadOnly] public ComponentDataArray<Launcher.CommandSenders.IncreaseScore> Sender;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSLaunchable>> DenotesAuthority;
        }

        [Inject] private Data data;

        private static EntityId EmptyEntityId = new EntityId(0);

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
                    if (firstLauncher != EmptyEntityId)
                    {
                        data.Sender[i].RequestsToSend.Add(new Launcher.IncreaseScore.Request(
                            firstLauncher, new Generated.Playground.ScoreIncreaseRequest { Amount = 1 }));
                    }
                }
                else if (secondLauncher != EmptyEntityId)
                {
                    var launchable = data.Launchable[i];
                    if (firstLauncher == EmptyEntityId)
                    {
                        launchable.MostRecentLauncher = secondLauncher;
                        data.Sender[i].RequestsToSend.Add(new Launcher.IncreaseScore.Request(secondLauncher,
                            new ScoreIncreaseRequest { Amount = 1 }));
                    }
                    else
                    {
                        launchable.MostRecentLauncher = EmptyEntityId;
                    }

                    PostUpdateCommands.SetComponent(data.Entities[i], launchable);
                }
            }
        }
    }
}

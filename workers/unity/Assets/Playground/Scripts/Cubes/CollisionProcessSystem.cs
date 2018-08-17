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

        public CollisionComponent(Entity ownEntity, Entity otherEntity)
        {
            OwnEntity = ownEntity;
            OtherEntity = otherEntity;
        }
    }

    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class CollisionProcessSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<SpatialOSLaunchable> Launchable; // Gets updated through PostUpdate
            [ReadOnly] public ComponentDataArray<CollisionComponent> Collision;
            [ReadOnly] public ComponentDataArray<Launcher.CommandSenders.IncreaseScore> Sender;
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
                var sender = data.Sender[i];

                var firstLaunchable = EntityManager.GetComponentData<SpatialOSLaunchable>(collision.OwnEntity);
                var secondLaunchable = EntityManager.GetComponentData<SpatialOSLaunchable>(collision.OtherEntity);
                var firstLauncher = firstLaunchable.MostRecentLauncher;
                var secondLauncher = secondLaunchable.MostRecentLauncher;

                if (firstLauncher == secondLauncher)
                {
                    if (firstLauncher.IsValid())
                    {
                        sender.RequestsToSend.Add(new Launcher.IncreaseScore.Request(
                            firstLauncher, new ScoreIncreaseRequest { Amount = 1 }));
                        data.Sender[i] = sender;
                    }
                }
                else if (secondLauncher.IsValid())
                {
                    var launchable = data.Launchable[i];
                    if (!firstLauncher.IsValid())
                    {
                        sender.RequestsToSend.Add(new Launcher.IncreaseScore.Request(secondLauncher,
                            new ScoreIncreaseRequest { Amount = 1 }));
                        data.Sender[i] = sender;
                        
                        launchable.MostRecentLauncher = secondLauncher;
                    }
                    else
                    {
                        launchable.MostRecentLauncher = new EntityId(0);
                    }

                    PostUpdateCommands.SetComponent(data.Entities[i], launchable);
                }
            }
        }
    }
}

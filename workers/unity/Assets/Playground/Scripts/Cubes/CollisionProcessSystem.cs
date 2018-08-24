using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

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
        public Entity OtherEntity;

        public CollisionComponent(Entity otherEntity)
        {
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

            // Gets updated through PostUpdate
            [ReadOnly] public ComponentDataArray<SpatialOSLaunchable> Launchable;
            [ReadOnly] public ComponentDataArray<CollisionComponent> Collision;
            public ComponentDataArray<Launcher.CommandSenders.IncreaseScore> Sender;
            [ReadOnly] public ComponentDataArray<Authoritative<SpatialOSLaunchable>> DenotesAuthority;
        }

        [Inject] private Data data;

        private static readonly EntityId InvalidEntityId = new EntityId(0);

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                // Handle all the different possible outcomes of the collision.
                // This requires looking at their most recent launchers.
                var launchable = data.Launchable[i];
                var collision = data.Collision[i];
                var sender = data.Sender[i];

                var otherLaunchable = EntityManager.GetComponentData<SpatialOSLaunchable>(collision.OtherEntity);
                var ourOwner = launchable.MostRecentLauncher;
                var otherOwner = otherLaunchable.MostRecentLauncher;

                if (ourOwner == otherOwner)
                {
                    if (ourOwner.IsValid())
                    {
                        sender.RequestsToSend.Add(new Launcher.IncreaseScore.Request(
                            ourOwner, new ScoreIncreaseRequest { Amount = 1 }));
                        data.Sender[i] = sender;
                    }
                }
                else if (otherOwner.IsValid())
                {
                    if (!ourOwner.IsValid())
                    {
                        sender.RequestsToSend.Add(new Launcher.IncreaseScore.Request(otherOwner,
                            new ScoreIncreaseRequest { Amount = 1 }));
                        data.Sender[i] = sender;

                        launchable.MostRecentLauncher = otherOwner;
                    }
                    else
                    {
                        launchable.MostRecentLauncher = InvalidEntityId;
                    }

                    PostUpdateCommands.SetComponent(data.Entities[i], launchable);
                }
            }
        }
    }
}

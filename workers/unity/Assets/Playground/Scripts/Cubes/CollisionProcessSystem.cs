using Improbable.Gdk.Core;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Playground
{
    [RemoveAtEndOfTick]
    public struct CollisionComponent : IComponentData
    {
        public Entity OtherEntity;
    }

    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    internal class CollisionProcessSystem : ComponentSystem
    {
        private static readonly EntityId InvalidEntityId = new EntityId(0);

        private ComponentGroup collisionGroup;
        private CommandSystem commandSystem;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            commandSystem = World.GetExistingManager<CommandSystem>();

            collisionGroup = GetComponentGroup(
                ComponentType.ReadOnly<Launchable.Component>(),
                ComponentType.ReadOnly<Launchable.ComponentAuthority>(),
                ComponentType.ReadOnly<CollisionComponent>()
            );
            collisionGroup.SetFilter(Launchable.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            var entities = collisionGroup.GetEntityArray();
            var launchableData = collisionGroup.GetComponentDataArray<Launchable.Component>();
            var collisionData = collisionGroup.GetComponentDataArray<CollisionComponent>();
            var launchableForEntity = GetComponentDataFromEntity<Launchable.Component>(true);

            for (var i = 0; i < entities.Length; i++)
            {
                // Handle all the different possible outcomes of the collision.
                // This requires looking at their most recent launchers.
                var launchable = launchableData[i];
                var collision = collisionData[i];

                var otherLaunchable = launchableForEntity[collision.OtherEntity];
                var ourOwner = launchable.MostRecentLauncher;
                var otherOwner = otherLaunchable.MostRecentLauncher;

                if (ourOwner == otherOwner)
                {
                    if (ourOwner.IsValid())
                    {
                        var request = new Launcher.IncreaseScore.Request(
                            ourOwner, new ScoreIncreaseRequest(1));

                        commandSystem.SendCommand(request, entities[i]);
                    }
                }
                else if (otherOwner.IsValid())
                {
                    if (!ourOwner.IsValid())
                    {
                        var request = new Launcher.IncreaseScore.Request(otherOwner,
                            new ScoreIncreaseRequest(1));

                        commandSystem.SendCommand(request, entities[i]);

                        launchable.MostRecentLauncher = otherOwner;
                    }
                    else
                    {
                        launchable.MostRecentLauncher = InvalidEntityId;
                    }

                    PostUpdateCommands.SetComponent(entities[i], launchable);
                }
            }
        }
    }
}

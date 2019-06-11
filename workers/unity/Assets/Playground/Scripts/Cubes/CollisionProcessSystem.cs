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

        private EntityQuery collisionGroup;
        private CommandSystem commandSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            commandSystem = World.GetExistingSystem<CommandSystem>();

            collisionGroup = GetEntityQuery(
                ComponentType.ReadOnly<Launchable.Component>(),
                ComponentType.ReadOnly<Launchable.ComponentAuthority>(),
                ComponentType.ReadOnly<CollisionComponent>()
            );
            collisionGroup.SetFilter(Launchable.ComponentAuthority.Authoritative);
        }

        protected override void OnUpdate()
        {
            var launchableForEntity = GetComponentDataFromEntity<Launchable.Component>(true);

            Entities.With(collisionGroup).ForEach(
                (Entity entity, ref Launchable.Component launchable, ref CollisionComponent collision) =>
                {
                    // Handle all the different possible outcomes of the collision.
                    // This requires looking at their most recent launchers.
                    var otherLaunchable = launchableForEntity[collision.OtherEntity];
                    var ourOwner = launchable.MostRecentLauncher;
                    var otherOwner = otherLaunchable.MostRecentLauncher;

                    if (ourOwner == otherOwner)
                    {
                        if (ourOwner.IsValid())
                        {
                            var request = new Launcher.IncreaseScore.Request(
                                ourOwner, new ScoreIncreaseRequest(1));

                            commandSystem.SendCommand(request, entity);
                        }
                    }
                    else if (otherOwner.IsValid())
                    {
                        if (!ourOwner.IsValid())
                        {
                            var request = new Launcher.IncreaseScore.Request(otherOwner,
                                new ScoreIncreaseRequest(1));

                            commandSystem.SendCommand(request, entity);

                            launchable.MostRecentLauncher = otherOwner;
                        }
                        else
                        {
                            launchable.MostRecentLauncher = InvalidEntityId;
                        }

                        PostUpdateCommands.SetComponent(entity, launchable);
                    }
                });
        }
    }
}

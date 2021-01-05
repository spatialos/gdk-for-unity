using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.QueryBasedInterest;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Generated;
using UnityEngine;
using Worker = Improbable.Restricted.Worker;

namespace Playground
{
    public static class EntityTemplates
    {
        private const int CheckoutRadius = 25;

        public static EntityTemplate CreatePlayerEntityTemplate(EntityId entityId, EntityId clientWorkerEntityId, byte[] playerCreationArguments)
        {
            var template = BaseTemplate();

            template.AddComponent(new Position.Snapshot());
            template.AddComponent(new Metadata.Snapshot("Character"));
            template.AddComponent(new PlayerInput.Snapshot());
            template.AddComponent(new Launcher.Snapshot(100, 0));
            template.AddComponent(new Score.Snapshot());
            template.AddComponent(new CubeSpawner.Snapshot(new List<EntityId>()));
            template.AddTransformSynchronizationComponents();
            template.AddPlayerLifecycleComponents(clientWorkerEntityId);

            var clientSelfInterest = InterestQuery.Query(Constraint.EntityId(entityId)).FilterResults(new[]
            {
                Position.ComponentId, Metadata.ComponentId, TransformInternal.ComponentId, CubeSpawner.ComponentId,
                Score.ComponentId, Launcher.ComponentId
            });

            var clientRangeInterest = InterestQuery.Query(Constraint.RelativeCylinder(radius: CheckoutRadius))
                .FilterResults(new[]
                {
                    Position.ComponentId, Metadata.ComponentId, TransformInternal.ComponentId, Collisions.ComponentId,
                    SpinnerColor.ComponentId, SpinnerRotation.ComponentId, CubeColor.ComponentId, Score.ComponentId,
                    Launchable.ComponentId
                });

            var interest = InterestTemplate
                .Create()
                .AddQueries(ComponentSets.PlayerClientSet, clientSelfInterest, clientRangeInterest);
            template.AddComponent(interest.ToSnapshot());


            return template;
        }

        public static EntityTemplate CreateCubeEntityTemplate(Vector3 location)
        {
            var template = BaseTemplate();

            template.AddComponent(new Position.Snapshot(location.ToCoordinates()));
            template.AddComponent(new Metadata.Snapshot("Cube"));
            template.AddComponent(new Persistence.Snapshot());
            template.AddComponent(new CubeColor.Snapshot());
            template.AddComponent(new CubeTargetVelocity.Snapshot(new Vector3f(-2.0f, 0, 0)));
            template.AddComponent(new Launchable.Snapshot());
            template.AddTransformSynchronizationComponents(Quaternion.identity, location);

            return template;
        }

        public static EntityTemplate CreateSpinnerEntityTemplate(Coordinates coords)
        {
            var transform = TransformUtils.CreateTransformSnapshot(coords.ToUnityVector(), Quaternion.identity);

            var template = BaseTemplate();
            template.AddComponent(new Position.Snapshot(coords));
            template.AddComponent(new Metadata.Snapshot("Spinner"));
            template.AddComponent(transform);
            template.AddComponent(new Persistence.Snapshot());
            template.AddComponent(new Collisions.Snapshot());
            template.AddComponent(new SpinnerColor.Snapshot(Color.BLUE));
            template.AddComponent(new SpinnerRotation.Snapshot());

            return template;
        }

        public static EntityTemplate CreatePlayerSpawnerEntityTemplate(Coordinates playerSpawnerLocation)
        {
            var template = BaseTemplate();
            template.AddComponent(new Position.Snapshot(playerSpawnerLocation));
            template.AddComponent(new Metadata.Snapshot("PlayerCreator"));
            template.AddComponent(new Persistence.Snapshot());
            template.AddComponent(new PlayerCreator.Snapshot());

            return template;
        }

        public static EntityTemplate CreateLoadBalancingPartition()
        {
            var template = BaseTemplate();
            template.AddComponent(new Position.Snapshot());
            template.AddComponent(new Metadata.Snapshot("LB Partition"));

            var query = InterestQuery.Query(Constraint.Component<Position.Component>());
            var interest = InterestTemplate.Create().AddQueries(ComponentSets.AuthorityDelegationSet, query);
            template.AddComponent(interest.ToSnapshot());
            return template;
        }

        private static EntityTemplate BaseTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new AuthorityDelegation.Snapshot(new Dictionary<uint, long>
            {
                { ComponentSets.AuthorityDelegationSet.ComponentSetId, 1 }
            }));
            return template;
        }
    }
}

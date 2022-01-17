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

        public static readonly EntityId PlayerCreatorEntityId = new EntityId(1);
        public static readonly EntityId LoadBalancerPartitionEntityId = new EntityId(2);

        public static EntityTemplate CreatePlayerEntityTemplate(EntityId entityId, EntityId clientWorkerEntityId, byte[] playerCreationArguments)
        {
            var template = new EntityTemplate();

            template.AddComponent(new Position.Snapshot());
            template.AddComponent(new Metadata.Snapshot("Character"));
            template.AddComponent(new PlayerInput.Snapshot());
            template.AddComponent(new Launcher.Snapshot(100, 0));
            template.AddComponent(new Score.Snapshot());
            template.AddComponent(new CubeSpawner.Snapshot(new List<EntityId>()));
            template.AddTransformSynchronizationComponents();
            template.AddPlayerLifecycleComponents(clientWorkerEntityId);

            var clientSelfInterest = InterestQuery.Query(Constraint.Self()).FilterResults(new[]
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

            var serverSelfInterest = InterestQuery.Query(Constraint.Self()).FilterResults(new[]
            {
                Position.ComponentId, Metadata.ComponentId, TransformInternal.ComponentId, Score.ComponentId
            });

            var serverRangeInterest = InterestQuery.Query(Constraint.RelativeCylinder(radius: CheckoutRadius))
                .FilterResults(new[]
                {
                    Position.ComponentId, Metadata.ComponentId, TransformInternal.ComponentId, Collisions.ComponentId,
                    SpinnerColor.ComponentId, SpinnerRotation.ComponentId, Score.ComponentId
                });

            var interest = InterestTemplate
                .Create()
                .AddQueries(ComponentSets.PlayerClientSet, clientSelfInterest, clientRangeInterest)
                .AddQueries(ComponentSets.PlayerServerSet, serverSelfInterest, serverRangeInterest);

            template.AddComponent(interest.ToSnapshot());

            AddComponentSets(template, ComponentSets.PlayerServerSet);
            AddClientComponentSets(template, clientWorkerEntityId, ComponentSets.PlayerClientSet);

            return template;
        }

        public static EntityTemplate CreateCubeEntityTemplate(Vector3 location)
        {
            var template = new EntityTemplate();

            template.AddComponent(new Position.Snapshot(location.ToCoordinates()));
            template.AddComponent(new Metadata.Snapshot("Cube"));
            template.AddComponent(new Persistence.Snapshot());
            template.AddComponent(new CubeColor.Snapshot());
            template.AddComponent(new CubeTargetVelocity.Snapshot(new Vector3f(-2.0f, 0, 0)));
            template.AddComponent(new Launchable.Snapshot());
            template.AddTransformSynchronizationComponents(Quaternion.identity, location);

            var query = InterestQuery.Query(Constraint.RelativeCylinder(radius: CheckoutRadius)).FilterResults(new[]
            {
                Position.ComponentId, Metadata.ComponentId, TransformInternal.ComponentId
            });

            var interest = InterestTemplate.Create().AddQueries(ComponentSets.CubeServerSet, query);
            template.AddComponent(interest.ToSnapshot());

            AddComponentSets(template, ComponentSets.CubeServerSet);

            return template;
        }

        public static EntityTemplate CreateSpinnerEntityTemplate(Coordinates coords)
        {
            var transform = TransformUtils.CreateTransformSnapshot(coords.ToUnityVector(), Quaternion.identity);

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(coords));
            template.AddComponent(new Metadata.Snapshot("Spinner"));
            template.AddComponent(transform);
            template.AddComponent(new Persistence.Snapshot());
            template.AddComponent(new Collisions.Snapshot());
            template.AddComponent(new SpinnerColor.Snapshot(Color.BLUE));
            template.AddComponent(new SpinnerRotation.Snapshot());

            var query = InterestQuery.Query(Constraint.RelativeCylinder(radius: CheckoutRadius)).FilterResults(new[]
            {
                Position.ComponentId, Metadata.ComponentId, TransformInternal.ComponentId
            });

            var interest = InterestTemplate.Create().AddQueries(ComponentSets.SpinnerServerSet, query);
            template.AddComponent(interest.ToSnapshot());

            AddComponentSets(template, ComponentSets.SpinnerServerSet);

            return template;
        }

        public static EntityTemplate CreatePlayerSpawnerEntityTemplate(Coordinates playerSpawnerLocation)
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(playerSpawnerLocation));
            template.AddComponent(new Metadata.Snapshot("PlayerCreator"));
            template.AddComponent(new Persistence.Snapshot());
            template.AddComponent(new PlayerCreator.Snapshot());

            AddComponentSets(template, ComponentSets.PlayerCreatorServerSet);

            return template;
        }

        public static EntityTemplate CreateLoadBalancingPartition()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            template.AddComponent(new Metadata.Snapshot("LB Partition"));

            var entityQuery = InterestQuery.Query(Constraint.Component<Position.Component>()).FilterResults(new[]
            {
                Position.ComponentId, AuthorityDelegation.ComponentId
            });
            var workerQuery = InterestQuery.Query(Constraint.Component<Improbable.Restricted.Worker.Component>()).FilterResults(new[]
            {
                Improbable.Restricted.Worker.ComponentId
            });
            var interest = InterestTemplate.Create().AddQueries(ComponentSets.AuthorityDelegationSet, entityQuery, workerQuery);
            template.AddComponent(interest.ToSnapshot());

            template.AddComponent(new AuthorityDelegation.Snapshot(new Dictionary<uint, long>
            {
                { ComponentSets.AuthorityDelegationSet.ComponentSetId, LoadBalancerPartitionEntityId.Id }
            }));

            return template;
        }

        private static void AddComponentSets(EntityTemplate template, params ComponentSet[] componentSets)
        {
            var authorityDelegation = GetOrCreateAuthorityDelegationSnapshot(template);

            foreach (var componentSet in componentSets)
            {
                // Add component sets, assign to invalid partition id
                authorityDelegation.Delegations.Add(componentSet.ComponentSetId, 0);
            }
        }

        private static void AddClientComponentSets(EntityTemplate template, EntityId ClientWorkerId, params ComponentSet[] componentSets)
        {
            var authorityDelegation = GetOrCreateAuthorityDelegationSnapshot(template);

            foreach (var componentSet in componentSets)
            {
                // Add component sets, assign to client
                authorityDelegation.Delegations.Add(componentSet.ComponentSetId, ClientWorkerId.Id);
            }
        }

        private static AuthorityDelegation.Snapshot GetOrCreateAuthorityDelegationSnapshot(EntityTemplate template)
        {
            if (!template.TryGetComponent<AuthorityDelegation.Snapshot>(out var authorityDelegation))
            {
                var delegations = new Dictionary<uint, long>
                {
                    // Default component set for Authority delegations, pre assigned to the load balancer
                    { ComponentSets.AuthorityDelegationSet.ComponentSetId, LoadBalancerPartitionEntityId.Id }
                };

                authorityDelegation = new AuthorityDelegation.Snapshot(delegations);
                template.AddComponent(authorityDelegation);
            }

            return authorityDelegation;
        }
    }
}

using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.QueryBasedInterest;
using Improbable.Gdk.TransformSynchronization;
using UnityEngine;

namespace Playground
{
    public static class EntityTemplates
    {
        private const int CheckoutRadius = 25;

        public static EntityTemplate CreatePlayerEntityTemplate(EntityId entityId, string clientWorkerId, byte[] playerCreationArguments)
        {
            var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(clientWorkerId);

            var template = new EntityTemplate();

            template.AddComponent(new Position.Snapshot(), clientAttribute);
            template.AddComponent(new Metadata.Snapshot("Character"), WorkerUtils.UnityGameLogic);
            template.AddComponent(new PlayerInput.Snapshot(), clientAttribute);
            template.AddComponent(new Launcher.Snapshot(100, 0), WorkerUtils.UnityGameLogic);
            template.AddComponent(new Score.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new CubeSpawner.Snapshot(new List<EntityId>()), WorkerUtils.UnityGameLogic);

            TransformSynchronizationHelper.AddTransformSynchronizationComponents(template, clientAttribute);
            PlayerLifecycleHelper.AddPlayerLifecycleComponents(template, clientWorkerId, WorkerUtils.UnityGameLogic);

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

            var serverSelfInterest = InterestQuery.Query(Constraint.EntityId(entityId)).FilterResults(new[]
            {
                Position.ComponentId, Metadata.ComponentId, TransformInternal.ComponentId, Score.ComponentId
            });

            var serverRangeInterest = InterestQuery.Query(Constraint.RelativeCylinder(radius: CheckoutRadius))
                .FilterResults(new[]
                {
                    Position.ComponentId, Metadata.ComponentId, TransformInternal.ComponentId, Collisions.ComponentId,
                    SpinnerColor.ComponentId, SpinnerRotation.ComponentId, Score.ComponentId
                });

            var interest = InterestTemplate.Create()
                .AddQueries<Position.Component>(clientSelfInterest, clientRangeInterest)
                .AddQueries<Metadata.Component>(serverSelfInterest, serverRangeInterest);
            template.AddComponent(interest.ToSnapshot());

            template.SetReadAccess(WorkerUtils.MobileClient, WorkerUtils.UnityClient, WorkerUtils.UnityGameLogic);

            return template;
        }

        public static EntityTemplate CreateCubeEntityTemplate(Vector3 location)
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(location.ToCoordinates()), WorkerUtils.UnityGameLogic);
            template.AddComponent(new Metadata.Snapshot("Cube"), WorkerUtils.UnityGameLogic);
            template.AddComponent(new Persistence.Snapshot());
            template.AddComponent(new CubeColor.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new CubeTargetVelocity.Snapshot(new Vector3f(-2.0f, 0, 0)),
                WorkerUtils.UnityGameLogic);
            template.AddComponent(new Launchable.Snapshot(), WorkerUtils.UnityGameLogic);

            TransformSynchronizationHelper.AddTransformSynchronizationComponents(template, WorkerUtils.UnityGameLogic, Quaternion.identity, location);

            var query = InterestQuery.Query(Constraint.RelativeCylinder(radius: CheckoutRadius)).FilterResults(new[]
            {
                Position.ComponentId, Metadata.ComponentId, TransformInternal.ComponentId
            });

            var interest = InterestTemplate.Create()
                .AddQueries<Position.Component>(query);
            template.AddComponent(interest.ToSnapshot());

            template.SetReadAccess(WorkerUtils.MobileClient, WorkerUtils.UnityClient, WorkerUtils.UnityGameLogic);

            return template;
        }

        public static EntityTemplate CreateSpinnerEntityTemplate(Coordinates coords)
        {
            var transform = TransformUtils.CreateTransformSnapshot(coords.ToUnityVector(), Quaternion.identity);

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(coords), WorkerUtils.UnityGameLogic);
            template.AddComponent(new Metadata.Snapshot("Spinner"), WorkerUtils.UnityGameLogic);
            template.AddComponent(transform, WorkerUtils.UnityGameLogic);
            template.AddComponent(new Persistence.Snapshot());
            template.AddComponent(new Collisions.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new SpinnerColor.Snapshot(Color.BLUE), WorkerUtils.UnityGameLogic);
            template.AddComponent(new SpinnerRotation.Snapshot(), WorkerUtils.UnityGameLogic);

            var query = InterestQuery.Query(Constraint.RelativeCylinder(radius: CheckoutRadius)).FilterResults(new[]
            {
                Position.ComponentId, Metadata.ComponentId, TransformInternal.ComponentId
            });

            var interest = InterestTemplate.Create()
                .AddQueries<Position.Component>(query);
            template.AddComponent(interest.ToSnapshot());

            template.SetReadAccess(WorkerUtils.MobileClient, WorkerUtils.UnityClient, WorkerUtils.UnityGameLogic);

            return template;
        }

        public static EntityTemplate CreatePlayerSpawnerEntityTemplate(Coordinates playerSpawnerLocation)
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(playerSpawnerLocation));
            template.AddComponent(new Metadata.Snapshot("PlayerCreator"));
            template.AddComponent(new Persistence.Snapshot());
            template.AddComponent(new PlayerCreator.Snapshot(), WorkerUtils.UnityGameLogic);

            return template;
        }
    }
}

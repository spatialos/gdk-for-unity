using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Improbable.Gdk.Core.Representation.Types;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.GameObjectCreation.EditmodeTests
{
    public class GameObjectCreationTests : MockBase
    {
        private long entityId = 100;

        private const string WorkerType = "TestWorkerType";

        private EntityGameObjectLinker linker;

        protected override MockWorld.Options GetOptions()
        {
            return new MockWorld.Options
            {
                AdditionalSystems = world =>
                {
                    var testGameObjectCreator = new TestGameObjectCreator(WorkerType);
                    var dummyDatabase = ScriptableObject.CreateInstance<EntityRepresentationMapping>();
                    dummyDatabase.EntityRepresentationResolvers = new List<IEntityRepresentationResolver>
                    {
                        new SimpleEntityResolver("TestObject", new GameObject())
                    };

                    GameObjectCreationHelper.EnableStandardGameObjectCreation(world, testGameObjectCreator, dummyDatabase);
                }
            };
        }

        [SetUp]
        public new void Setup()
        {
            base.Setup();

            var goInitSystem = World.GetSystem<GameObjectInitializationSystem>();
            linker = goInitSystem.Linker;
        }

        [TearDown]
        public new void TearDown()
        {
            World.Dispose();
        }

        [Test]
        public void Create_GameObject_after_required_components_arrive_in_same_frame()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(entityId, new EntityTemplate());

                    world.Connection.AddComponent(entityId, Position.ComponentId,
                        new Position.Update { Coords = Coordinates.Zero });

                    world.Connection.AddComponent(entityId, Metadata.ComponentId,
                        new Metadata.Update { EntityType = "TestObject" });

                    var gameObjectExists = linker.EntityIdToGameObjects
                        .TryGetValue(new EntityId(entityId), out var gameObjects);

                    Assert.IsFalse(gameObjectExists);
                })
                .Step(world =>
                {
                    var gameObjectExists = linker.EntityIdToGameObjects
                        .TryGetValue(new EntityId(entityId), out var gameObjects);

                    Assert.IsTrue(gameObjectExists);
                });
        }

        [Test]
        public void Create_GameObject_after_required_components_arrive_in_multiple_frames()
        {
            World
                .Step(world =>
                {
                    world.Connection.CreateEntity(entityId, new EntityTemplate());
                })
                .Step(world =>
                {
                    world.Connection.AddComponent(entityId, Position.ComponentId,
                        new Position.Update { Coords = Coordinates.Zero });

                    var gameObjectExists = linker.EntityIdToGameObjects
                        .TryGetValue(new EntityId(entityId), out var gameObjects);

                    Assert.IsFalse(gameObjectExists);
                })
                .Step(world =>
                {
                    world.Connection.AddComponent(entityId, Metadata.ComponentId,
                        new Metadata.Update { EntityType = "TestObject" });

                    var gameObjectExists = linker.EntityIdToGameObjects
                        .TryGetValue(new EntityId(entityId), out var gameObjects);

                    Assert.IsFalse(gameObjectExists);
                })
                .Step(world =>
                {
                    var gameObjectExists = linker.EntityIdToGameObjects
                        .TryGetValue(new EntityId(entityId), out var gameObjects);

                    Assert.IsTrue(gameObjectExists);
                });
        }

        private class TestGameObjectCreator : IEntityGameObjectCreator
        {
            private readonly string workerType;

            private readonly Dictionary<EntityId, GameObject> entityIdToGameObject = new Dictionary<EntityId, GameObject>();

            public TestGameObjectCreator(string workerType)
            {
                this.workerType = workerType;
            }

            public void PopulateEntityTypeExpectations(EntityTypeExpectations entityTypeExpectations)
            {
                entityTypeExpectations.RegisterDefault(new[]
                {
                    typeof(Metadata.Component), typeof(Position.Component)
                });
            }

            public void OnEntityCreated(SpatialOSEntityInfo entityInfo, GameObject prefab, EntityManager entityManager, EntityGameObjectLinker linker)
            {
                var gameObject = new GameObject();
                gameObject.transform.position = Vector3.one;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.name = $"TestObject(SpatialOS: {entityInfo.SpatialOSEntityId}, Worker: {workerType})";

                entityIdToGameObject.Add(entityInfo.SpatialOSEntityId, gameObject);
                linker.LinkGameObjectToSpatialOSEntity(entityInfo.SpatialOSEntityId, gameObject);
            }

            public void OnEntityRemoved(EntityId entityId)
            {
                if (!entityIdToGameObject.TryGetValue(entityId, out var go))
                {
                    return;
                }

                Object.DestroyImmediate(go);
                entityIdToGameObject.Remove(entityId);
            }
        }
    }
}

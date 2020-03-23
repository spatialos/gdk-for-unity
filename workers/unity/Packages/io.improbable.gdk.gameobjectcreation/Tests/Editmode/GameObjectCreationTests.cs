using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
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
                AdditionalSystems = new (Type systemType, object[] constructorArgs)[]
                {
                    (typeof(GameObjectInitializationSystem),
                        new object[] { new TestGameObjectCreator(WorkerType), null })
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
        public void Create_gameobject_after_required_components_arrive_in_same_frame()
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
        public void Create_gameobject_after_required_components_arrive_in_multiple_frames()
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

            public ComponentType[] MinimumComponentTypes { get; } =
            {
                ComponentType.ReadOnly<Position.Component>(),
                ComponentType.ReadOnly<Metadata.Component>()
            };

            public TestGameObjectCreator(string workerType)
            {
                this.workerType = workerType;
            }

            public void OnEntityCreated(SpatialOSEntity entity, EntityGameObjectLinker linker)
            {
                var gameObject = new GameObject();
                gameObject.transform.position = Vector3.one;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.name = $"TestObject(SpatialOS: {entity.SpatialOSEntityId}, Worker: {workerType})";

                entityIdToGameObject.Add(entity.SpatialOSEntityId, gameObject);
                linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, gameObject);
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

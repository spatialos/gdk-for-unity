using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Utility
{
    [TestFixture]
    public class EntitySnapshotTests
    {
        private List<Action> cleanUps = new List<Action>();

        [TearDown]
        public void TearDown()
        {
            foreach (var cleanup in cleanUps)
            {
                try
                {
                    cleanup();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to cleanup test with error: {e}");
                }
            }
        }

        [Test]
        public void Can_create_from_entity_template()
        {
            // Note this tests the internal EntitySnapshot(Entity entity) constructor too!
            var entityTemplate = new EntityTemplate();
            entityTemplate.AddComponent(new Position.Snapshot(new Coordinates(10, 10, 10)), "worker");
            entityTemplate.AddComponent(new PlayerHeartbeatClient.Snapshot(), "test");

            var snapshot = entityTemplate.GetEntitySnapshot();

            Assert.IsTrue(snapshot.TryGetComponentSnapshot<Position.Snapshot>(out var position));
            Assert.IsTrue(snapshot.TryGetComponentSnapshot<PlayerHeartbeatClient.Snapshot>(out var playerHeartbeat));
            Assert.AreEqual(position.Coords.X, 10, Double.Epsilon);
        }

        [Test]
        public void Can_create_from_schema_object()
        {
            var data = new ComponentData(new SchemaComponentData(0)); // Easiest way to get a valid `SchemaObject`.
            var schemaObject = data.SchemaData.Value.GetFields();

            cleanUps.Add(() =>
            {
                data.SchemaData.Value.Destroy();
            });

            var position = new Position.Snapshot(new Coordinates(10, 10, 10));
            Position.Serialization.SerializeSnapshot(position, schemaObject.AddObject(Position.ComponentId));
            var playerHeartbeatClient = new PlayerHeartbeatClient.Snapshot();
            PlayerHeartbeatClient.Serialization.SerializeSnapshot(playerHeartbeatClient, schemaObject.AddObject(PlayerHeartbeatClient.ComponentId));

            var snapshot = new EntitySnapshot(schemaObject);

            Assert.IsTrue(snapshot.TryGetComponentSnapshot<Position.Snapshot>(out var outPosition));
            Assert.IsTrue(snapshot.TryGetComponentSnapshot<PlayerHeartbeatClient.Snapshot>(out var playerHeartbeat));
            Assert.AreEqual(outPosition.Coords.X, 10, Double.Epsilon);
        }

        [Test]
        public void AddComponent_overrides_preexisting_components()
        {
            var entityTemplate = new EntityTemplate();
            entityTemplate.AddComponent(new Position.Snapshot(new Coordinates(10, 10, 10)), "worker");
            entityTemplate.AddComponent(new PlayerHeartbeatClient.Snapshot(), "test");

            var snapshot = entityTemplate.GetEntitySnapshot();

            snapshot.AddComponentSnapshot(new Position.Snapshot(new Coordinates(100, 100, 100)));
            Assert.IsTrue(snapshot.TryGetComponentSnapshot<Position.Snapshot>(out var outPosition));
            Assert.AreEqual(outPosition.Coords.X, 100, Double.Epsilon);
        }
    }
}

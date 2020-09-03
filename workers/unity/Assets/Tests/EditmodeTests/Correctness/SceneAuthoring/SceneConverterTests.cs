using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.SceneAuthoring;
using Improbable.Gdk.Core.SceneAuthoring.AuthoringComponents;
using Improbable.Gdk.Core.SceneAuthoring.Editor;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.SceneAuthoring
{
    [TestFixture]
    public class SceneConverterTests
    {
        [Test]
        public void GameObjects_with_duplicate_entity_ids_throws_exception()
        {
            var entityId = new EntityId(1);
            var gameObjects = new[]
            {
                CreateGameObject(desiredEntityId: entityId), CreateGameObject(desiredEntityId: entityId)
            };
            Assert.Throws<InvalidOperationException>(() => SceneConverter.Convert(gameObjects));
        }

        [Test]
        public void GameObjects_with_multiple_converters_are_rejected()
        {
            var gameObject = CreateGameObject();
            gameObject.AddComponent<CustomConverter>();

            Assert.Throws<InvalidOperationException>(() => SceneConverter.Convert(new[] { gameObject }));
        }

        [Test]
        public void Child_gameobjects_are_not_considered_if_includeChildren_is_false()
        {
            var gameobject = CreateGameObject();
            var child = CreateGameObject();
            child.transform.SetParent(gameobject.transform);

            var snapshot = SceneConverter.Convert(new[] { gameobject });
            Assert.AreEqual(1, snapshot.Count);
        }

        [Test]
        public void Child_gameobjects_are_considered_if_includeChildren_is_true()
        {
            var gameobject = CreateGameObject();
            var child = CreateGameObject();
            var grandChild = CreateGameObject();

            child.transform.SetParent(gameobject.transform);
            grandChild.transform.SetParent(child.transform);

            var snapshot = SceneConverter.Convert(new[] { gameobject }, includeChildren: true);
            Assert.AreEqual(3, snapshot.Count);
        }

        [Test]
        public void GameObjects_with_specific_entity_id_are_always_put_there()
        {
            var firstPosition = new Vector3(1, 0, 0);
            var secondPosition = new Vector3(0, 1, 0);
            var restPosition = new Vector3(0, 0, 1);

            var firstEntityId = new EntityId(1);
            var secondEntityId = new EntityId(2);

            var gameObjects = new[]
            {
                CreateGameObject(position: restPosition),
                CreateGameObject(desiredEntityId: firstEntityId, position: firstPosition),
                CreateGameObject(position: restPosition),
                CreateGameObject(desiredEntityId: secondEntityId, position: secondPosition),
                CreateGameObject(position: restPosition)
            };

            var snapshot = SceneConverter.Convert(gameObjects);

            var firstEntity = snapshot[firstEntityId];
            var firstEntityPosition = GetPosition(firstEntity).Coords.ToUnityVector();
            Assert.AreEqual(firstPosition, firstEntityPosition);

            var secondEntity = snapshot[secondEntityId];
            var secondEntityPosition = GetPosition(secondEntity).Coords.ToUnityVector();
            Assert.AreEqual(secondPosition, secondEntityPosition);
        }

        private static GameObject CreateGameObject(EntityId desiredEntityId = default, Vector3 position = default)
        {
            var go = new GameObject();
            go.transform.position = position;

            go.AddComponent<PositionFromGameObjectAuthoringComponent>();
            var conversion = go.AddComponent<ConvertToSingleEntity>();

            if (desiredEntityId.IsValid())
            {
                conversion.UseSpecificEntityId = true;
                conversion.DesiredEntityId = desiredEntityId.Id;
            }

            return go;
        }

        private static Position.Snapshot GetPosition(Entity entity)
        {
            var schemaObject = entity.Get(Position.ComponentId).Value.SchemaData.Value.GetFields();
            return Position.Serialization.DeserializeSnapshot(schemaObject);
        }

        private class CustomConverter : MonoBehaviour, IConvertGameObjectToSpatialOsEntity
        {
            public List<ConvertedEntity> Convert()
            {
                throw new NotImplementedException();
            }
        }
    }
}

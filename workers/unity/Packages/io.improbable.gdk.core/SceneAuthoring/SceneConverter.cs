using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Improbable.Gdk.Core.SceneAuthoring.Editor
{
    public static class SceneConverter
    {
        public static Snapshot Convert(Scene scene, bool includeChildren = false)
        {
            return Convert(scene.GetRootGameObjects(), includeChildren);
        }

        public static Snapshot Convert(IEnumerable<GameObject> gameObjects, bool includeChildren = false)
        {
            var (entities, entitiesWithIds) = gameObjects
                .SelectMany(gameObject => CollectGameObjects(gameObject, includeChildren))
                .SelectMany(GetEntities)
                .Partition();

            var snapshot = new Snapshot();

            foreach (var kvp in entitiesWithIds)
            {
                snapshot.AddEntity(kvp.Key, kvp.Value);
            }

            var nextId = 1;

            foreach (var entity in entities)
            {
                while (entitiesWithIds.ContainsKey(new EntityId(nextId)))
                {
                    nextId++;
                }

                snapshot.AddEntity(new EntityId(nextId), entity);
                nextId++;
            }

            return snapshot;
        }

        private static IEnumerable<GameObject> CollectGameObjects(GameObject root, bool includeChildren)
        {
            yield return root;

            if (!includeChildren)
            {
                yield break;
            }

            foreach (Transform childTransform in root.transform)
            {
                foreach (var child in CollectGameObjects(childTransform.gameObject, true))
                {
                    yield return child;
                }
            }
        }

        private static IEnumerable<ConvertedEntity> GetEntities(GameObject gameObject)
        {
            var converters = gameObject.GetComponents<IConvertGameObjectToSpatialOsEntity>();

            switch (converters.Length)
            {
                case 0:
                    return Enumerable.Empty<ConvertedEntity>();
                case 1:
                    return converters[0].Convert();
                default:
                    var componentNames = string.Join(", ", converters.Select(c => c.GetType().Name));
                    throw new InvalidOperationException($"GameObject {gameObject} has more than one component that implements {nameof(IConvertGameObjectToSpatialOsEntity)}: '{componentNames}'");
            }
        }

        private static (List<EntityTemplate>, Dictionary<EntityId, EntityTemplate>) Partition(
            this IEnumerable<ConvertedEntity> convertedEntities)
        {
            var entities = new List<EntityTemplate>();
            var entitiesWithIds = new Dictionary<EntityId, EntityTemplate>();

            foreach (var convertedEntity in convertedEntities)
            {
                if (!convertedEntity.EntityId.HasValue)
                {
                    entities.Add(convertedEntity.Template);
                    continue;
                }

                var entityId = convertedEntity.EntityId.Value;

                if (entitiesWithIds.ContainsKey(entityId))
                {
                    throw new InvalidOperationException($"More than one entity is specified with EntityId {entityId}");
                }

                entitiesWithIds[entityId] = convertedEntity.Template;
            }

            return (entities, entitiesWithIds);
        }
    }
}

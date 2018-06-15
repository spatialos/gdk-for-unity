using System;
using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Legacy;
using Improbable.PlayerLifecycle;
using Improbable.Transform;
using Improbable.Worker;
using UnityEngine;
using Quaternion = Improbable.Transform.Quaternion;
using Transform = Improbable.Transform.Transform;

namespace Playground.Editor.SnapshotGenerator
{
    internal static class SnapshotGenerator
    {
        public struct Arguments
        {
            public int NumberEntities;
            public float WorldLength;
            public string OutputPath;
        }

        private const string UnityGameLogicType = "UnityGameLogic";
        private const string UnityClientType = "UnityClient";

        private static readonly WorkerRequirementSet AllWorkersSet = new WorkerRequirementSet(
            new Improbable.Collections.List<WorkerAttributeSet>
            {
                new WorkerAttributeSet(new Improbable.Collections.List<string> { UnityGameLogicType }),
                new WorkerAttributeSet(new Improbable.Collections.List<string> { UnityClientType })
            });

        private static readonly WorkerRequirementSet WorkerSet = new WorkerRequirementSet(
            new Improbable.Collections.List<WorkerAttributeSet>
            {
                new WorkerAttributeSet(new Improbable.Collections.List<string> { UnityGameLogicType })
            });

        private static readonly WorkerRequirementSet ClientSet = new WorkerRequirementSet(
            new Improbable.Collections.List<WorkerAttributeSet>
            {
                new WorkerAttributeSet(new Improbable.Collections.List<string> { UnityClientType })
            });

        private const string EntityName = "Cube";

        public static void Generate(Arguments arguments)
        {
            Debug.Log("Generating snapshot.");
            var entities = CreateEntities(arguments.WorldLength, arguments.NumberEntities);
            WriteSnapshot(entities, arguments.OutputPath);
        }

        private static Dictionary<EntityId, Entity> CreateEntities(float worldLength, int numberEntities)
        {
            var entities = new Dictionary<EntityId, Entity>();

            var spawner = EntityBuilder.Begin()
                .AddPositionComponent(new Coordinates(0, 0, 0), WorkerSet)
                .AddComponent(new Metadata.Data("PlayerCreator"), WorkerSet)
                .SetPersistence(true)
                .SetReadAcl(AllWorkersSet)
                .AddComponent(new PlayerCreator.Data(), WorkerSet)
                .Build();
            entities[new EntityId(entities.Count + 1)] = spawner;

            var gridLength = (int) Math.Ceiling(Math.Sqrt(numberEntities));

            if (gridLength % 2 == 1)
            {
                gridLength += 1; // To make sure nothing is in (0, 0)
            }

            numberEntities += 1; //One extra entity because of the PlayerCreator entity

            for (var i = -gridLength + 1; i <= gridLength - 1; i += 2)
            {
                for (var j = -gridLength + 1; j <= gridLength - 1; j += 2)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    if (entities.Count == numberEntities)
                    {
                        return entities;
                    }

                    var coords = new Coordinates(i, 0, j);
                    var transform = new Transform.Data(new Location(i, 1, j), new Quaternion(1, 0, 0, 0), 0);
                    var cubeColor = new CubeColor.Data();
                    var prefab = new Prefab.Data("Cube");
                    var launchable = new Launchable.Data();
                    var archetypeComponent = new ArchetypeComponent.Data("Cube");

                    var entity = EntityBuilder.Begin()
                        .AddPositionComponent(coords, WorkerSet)
                        .AddComponent(new Metadata.Data(EntityName), WorkerSet)
                        .SetPersistence(true)
                        .SetReadAcl(AllWorkersSet)
                        .AddComponent(transform, WorkerSet)
                        .AddComponent(cubeColor, WorkerSet)
                        .AddComponent(prefab, WorkerSet)
                        .AddComponent(archetypeComponent, WorkerSet)
                        .AddComponent(launchable, WorkerSet)
                        .Build();

                    entities[new EntityId(entities.Count + 1)] = entity;
                }
            }

            return entities;
        }

        private static void WriteSnapshot(Dictionary<EntityId, Entity> entities, string snapshotName)
        {
            Debug.Log("Writing snapshot to: " + snapshotName);
            var outputStream = new SnapshotOutputStream(snapshotName);

            foreach (var entry in entities)
            {
                var error = outputStream.WriteEntity(entry.Key, entry.Value);
                if (error.HasValue)
                {
                    Debug.Log(error.Value);
                }
            }

            outputStream.Dispose();
        }
    }
}

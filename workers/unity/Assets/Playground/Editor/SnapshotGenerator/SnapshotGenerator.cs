using System;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Improbable.Transform;
using UnityEngine;
using Quaternion = Improbable.Transform.Quaternion;

namespace Playground.Editor.SnapshotGenerator
{
    internal static class SnapshotGenerator
    {
        public struct Arguments
        {
            public int NumberEntities;
            public string OutputPath;
        }

        public static void Generate(Arguments arguments)
        {
            Debug.Log("Generating snapshot.");
            var snapshot = CreateSnapshot(arguments.NumberEntities);

            Debug.Log($"Writing snapshot to: {arguments.OutputPath}");
            snapshot.WriteToFile(arguments.OutputPath);
        }

        private static Snapshot CreateSnapshot(int cubeCount)
        {
            var snapshot = new Snapshot();

            AddPlayerSpawner(snapshot);
            AddCubeGrid(snapshot, cubeCount);
            CreateSpinner(snapshot, new Coordinates { X = 5.5, Y = 0.5f, Z = 0.0 });
            CreateSpinner(snapshot, new Coordinates { X = -5.5, Y = 0.5f, Z = 0.0 });

            return snapshot;
        }

        private static void AddPlayerSpawner(Snapshot snapshot)
        {
            var playerCreator = PlayerCreator.Component.CreateSchemaComponentData();
            var spawner = EntityBuilder.Begin()
                .AddPosition(0, 0, 0, WorkerUtils.UnityGameLogic)
                .AddMetadata("PlayerCreator", WorkerUtils.UnityGameLogic)
                .SetPersistence(true)
                .SetReadAcl(WorkerUtils.AllWorkerAttributes)
                .AddComponent(playerCreator, WorkerUtils.UnityGameLogic)
                .Build();
            snapshot.AddEntity(spawner);
        }

        private static void AddCubeGrid(Snapshot snapshot, int cubeCount)
        {
            // Calculate grid size
            var gridLength = (int) Math.Ceiling(Math.Sqrt(cubeCount));
            if (gridLength % 2 == 1) // To make sure nothing is in (0, 0)
            {
                gridLength += 1;
            }

            var cubesToSpawn = cubeCount;

            for (var x = -gridLength + 1; x <= gridLength - 1; x += 2)
            {
                for (var z = -gridLength + 1; z <= gridLength - 1; z += 2)
                {
                    // Leave the centre empty
                    if (x == 0 && z == 0)
                    {
                        continue;
                    }

                    // Exit when we've hit our cube limit
                    if (cubesToSpawn-- <= 0)
                    {
                        return;
                    }

                    var entityTemplate = CubeTemplate.CreateCubeEntityTemplate(new Coordinates(x, 1, z));
                    snapshot.AddEntity(entityTemplate);
                }
            }
        }

        private static void CreateSpinner(Snapshot snapshot, Coordinates coords)
        {
            const string entityType = "Spinner";

            var transform = TransformInternal.Component.CreateSchemaComponentData(
                new Location { X = (float) coords.X, Y = (float) coords.Y, Z = (float) coords.Z },
                new Quaternion { W = 1, X = 0, Y = 0, Z = 0 },
                new Velocity(0.0f, 0.0f, 0.0f),
                0,
                0.0f
            );

            var collisions = Collisions.Component.CreateSchemaComponentData();
            var color = SpinnerColor.Component.CreateSchemaComponentData(Color.BLUE);
            var spinnerRotation = SpinnerRotation.Component.CreateSchemaComponentData();

            var entityTemplate = EntityBuilder.Begin()
                .AddPosition(coords.X, coords.Y, coords.Z, WorkerUtils.UnityGameLogic)
                .AddMetadata(entityType, WorkerUtils.UnityGameLogic)
                .SetPersistence(true)
                .SetReadAcl(WorkerUtils.AllWorkerAttributes)
                .AddComponent(collisions, WorkerUtils.UnityGameLogic)
                .AddComponent(transform, WorkerUtils.UnityGameLogic)
                .AddComponent(color, WorkerUtils.UnityGameLogic)
                .AddComponent(spinnerRotation, WorkerUtils.UnityGameLogic)
                .Build();

            snapshot.AddEntity(entityTemplate);
        }
    }
}

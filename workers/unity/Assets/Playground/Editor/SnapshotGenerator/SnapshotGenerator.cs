using System;
using System.Collections.Generic;
using Generated.Improbable;
using Generated.Improbable.PlayerLifecycle;
using Generated.Improbable.Transform;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Worker;
using UnityEngine;
using Color = Generated.Playground.Color;
using Quaternion = Generated.Improbable.Transform.Quaternion;
using Transform = UnityEngine.Transform;

namespace Playground.Editor.SnapshotGenerator
{
    internal static class SnapshotGenerator
    {
        public struct Arguments
        {
            public int NumberEntities;
            public string OutputPath;
        }

        private static readonly List<string> UnityWorkers =
            new List<string> { WorkerUtils.UnityGameLogic, WorkerUtils.UnityClient};

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
            CreateSpinner(snapshot, new Coordinates { X = 0, Y = 0.5f, Z = 5.5 });
            CreateSpinner(snapshot, new Coordinates { X = 0, Y = 0.5f, Z = -5.5 });

            return snapshot;
        }

        private static void AddPlayerSpawner(Snapshot snapshot)
        {
            var playerCreator = PlayerCreator.Component.CreateSchemaComponentData();
            var spawner = EntityBuilder.Begin()
                .AddPosition(0, 0, 0, WorkerUtils.UnityGameLogic)
                .AddMetadata("PlayerCreator", WorkerUtils.UnityGameLogic)
                .SetPersistence(true)
                .SetReadAcl(UnityWorkers)
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
            const string entityType = "Cube";

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

                    var transform = Generated.Improbable.Transform.Transform.Component.CreateSchemaComponentData(
                        new Location { X = x, Y = 1, Z = z },
                        new Quaternion { W = 1, X = 0, Y = 0, Z = 0 },
                        0
                    );

                    var cubeColor = CubeColor.Component.CreateSchemaComponentData();
                    var cubeTargetVelocity = CubeTargetVelocity.Component.CreateSchemaComponentData(new Vector3f { X = -2.0f });
                    var prefab = Prefab.Component.CreateSchemaComponentData(entityType);
                    var launchable = Launchable.Component.CreateSchemaComponentData(new EntityId(0));
                    var archetypeComponent = ArchetypeComponent.Component.CreateSchemaComponentData(entityType);

                    var entity = EntityBuilder.Begin()
                        .AddPosition(x, 0, z, WorkerUtils.UnityGameLogic)
                        .AddMetadata(entityType, WorkerUtils.UnityGameLogic)
                        .SetPersistence(true)
                        .SetReadAcl(UnityWorkers)
                        .AddComponent(transform, WorkerUtils.UnityGameLogic)
                        .AddComponent(cubeColor, WorkerUtils.UnityGameLogic)
                        .AddComponent(cubeTargetVelocity, WorkerUtils.UnityGameLogic)
                        .AddComponent(prefab, WorkerUtils.UnityGameLogic)
                        .AddComponent(archetypeComponent, WorkerUtils.UnityGameLogic)
                        .AddComponent(launchable, WorkerUtils.UnityGameLogic)
                        .Build();

                    snapshot.AddEntity(entity);
                }
            }
        }

        private static void CreateSpinner(Snapshot snapshot, Coordinates coords)
        {
            const string entityType = "Spinner";

            var transform = Generated.Improbable.Transform.Transform.Component.CreateSchemaComponentData(
                new Location { X = (float) coords.X, Y = (float) coords.Y, Z = (float) coords.Z },
                new Quaternion { W = 1, X = 0, Y = 0, Z = 0 },
                0
            );

            var prefab = Prefab.Component.CreateSchemaComponentData(entityType);
            var collisions = Collisions.Component.CreateSchemaComponentData();
            var archetype = ArchetypeComponent.Component.CreateSchemaComponentData(entityType);
            var color = SpinnerColor.Component.CreateSchemaComponentData(Color.BLUE);
            var spinnerRotation = SpinnerRotation.Component.CreateSchemaComponentData();

            var entity = EntityBuilder.Begin()
                .AddPosition(coords.X, coords.Y, coords.Z, WorkerUtils.UnityGameLogic)
                .AddMetadata(entityType, WorkerUtils.UnityGameLogic)
                .SetPersistence(true)
                .SetReadAcl(UnityWorkers)
                .AddComponent(collisions, WorkerUtils.UnityGameLogic)
                .AddComponent(transform, WorkerUtils.UnityGameLogic)
                .AddComponent(prefab, WorkerUtils.UnityGameLogic)
                .AddComponent(archetype, WorkerUtils.UnityGameLogic)
                .AddComponent(color, WorkerUtils.UnityGameLogic)
                .AddComponent(spinnerRotation, WorkerUtils.UnityGameLogic)
                .Build();

            snapshot.AddEntity(entity);
        }
    }
}

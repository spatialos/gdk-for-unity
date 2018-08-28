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
            new List<string> { SystemConfig.UnityGameLogic, SystemConfig.UnityClient };

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
            var playerCreator = SpatialOSPlayerCreator.CreateSchemaComponentData();
            var spawner = EntityBuilder.Begin()
                .AddPosition(0, 0, 0, SystemConfig.UnityGameLogic)
                .AddMetadata("PlayerCreator", SystemConfig.UnityGameLogic)
                .SetPersistence(true)
                .SetReadAcl(UnityWorkers)
                .AddComponent(playerCreator, SystemConfig.UnityGameLogic)
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

                    var transform = SpatialOSTransform.CreateSchemaComponentData(
                        new Location { X = x, Y = 1, Z = z },
                        new Quaternion { W = 1, X = 0, Y = 0, Z = 0 },
                        0
                    );

                    var cubeColor = SpatialOSCubeColor.CreateSchemaComponentData();
                    var cubeTargetVelocity = SpatialOSCubeTargetVelocity.CreateSchemaComponentData(new Vector3f { X = -2.0f });
                    var prefab = SpatialOSPrefab.CreateSchemaComponentData(entityType);
                    var launchable = SpatialOSLaunchable.CreateSchemaComponentData(new EntityId(0));
                    var archetypeComponent = SpatialOSArchetypeComponent.CreateSchemaComponentData(entityType);

                    var entity = EntityBuilder.Begin()
                        .AddPosition(x, 0, z, SystemConfig.UnityGameLogic)
                        .AddMetadata(entityType, SystemConfig.UnityGameLogic)
                        .SetPersistence(true)
                        .SetReadAcl(UnityWorkers)
                        .AddComponent(transform, SystemConfig.UnityGameLogic)
                        .AddComponent(cubeColor, SystemConfig.UnityGameLogic)
                        .AddComponent(cubeTargetVelocity, SystemConfig.UnityGameLogic)
                        .AddComponent(prefab, SystemConfig.UnityGameLogic)
                        .AddComponent(archetypeComponent, SystemConfig.UnityGameLogic)
                        .AddComponent(launchable, SystemConfig.UnityGameLogic)
                        .Build();

                    snapshot.AddEntity(entity);
                }
            }
        }

        private static void CreateSpinner(Snapshot snapshot, Coordinates coords)
        {
            const string entityType = "Spinner";

            var transform = SpatialOSTransform.CreateSchemaComponentData(
                new Location { X = (float) coords.X, Y = (float) coords.Y, Z = (float) coords.Z },
                new Quaternion { W = 1, X = 0, Y = 0, Z = 0 },
                0
            );

            var prefab = SpatialOSPrefab.CreateSchemaComponentData(entityType);
            var collisions = SpatialOSCollisions.CreateSchemaComponentData();
            var archetypeComponent = SpatialOSArchetypeComponent.CreateSchemaComponentData(entityType);
            var color = SpatialOSSpinnerColor.CreateSchemaComponentData(Color.BLUE);

            var entity = EntityBuilder.Begin()
                .AddPosition(coords.X, coords.Y, coords.Z, SystemConfig.UnityGameLogic)
                .AddMetadata(entityType, SystemConfig.UnityGameLogic)
                .SetPersistence(true)
                .SetReadAcl(UnityWorkers)
                .AddComponent(collisions, SystemConfig.UnityGameLogic)
                .AddComponent(transform, SystemConfig.UnityGameLogic)
                .AddComponent(prefab, SystemConfig.UnityGameLogic)
                .AddComponent(archetypeComponent, SystemConfig.UnityGameLogic)
                .AddComponent(color, SystemConfig.UnityGameLogic)
                .Build();

            snapshot.AddEntity(entity);
        }
    }
}

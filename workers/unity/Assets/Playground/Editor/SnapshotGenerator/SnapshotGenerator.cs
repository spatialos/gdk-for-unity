using System;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using UnityEngine;

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

            AddPlayerSpawner(snapshot, new Coordinates(200, 0, 200));
            AddPlayerSpawner(snapshot, new Coordinates(200, 0, -200));
            AddPlayerSpawner(snapshot, new Coordinates(-200, 0, -200));
            AddPlayerSpawner(snapshot, new Coordinates(-200, 0, 200));

            AddCubeGrid(snapshot, cubeCount);
            CreateSpinner(snapshot, new Coordinates { X = 5.5, Y = 0.5f, Z = 0.0 });
            CreateSpinner(snapshot, new Coordinates { X = -5.5, Y = 0.5f, Z = 0.0 });

            return snapshot;
        }

        private static void AddPlayerSpawner(Snapshot snapshot, Coordinates playerSpawnerLocation)
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(playerSpawnerLocation), WorkerUtils.UnityGameLogic);
            template.AddComponent(new Metadata.Snapshot { EntityType = "PlayerCreator" }, WorkerUtils.UnityGameLogic);
            template.AddComponent(new Persistence.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new PlayerCreator.Snapshot(), WorkerUtils.UnityGameLogic);

            template.SetReadAccess(WorkerUtils.UnityGameLogic, WorkerUtils.UnityClient, WorkerUtils.MobileClient);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, WorkerUtils.UnityGameLogic);

            snapshot.AddEntity(template);
        }

        private static void AddCubeGrid(Snapshot snapshot, int cubeCount)
        {
            var cubeTemplate = CubeTemplate.CreateCubeEntityTemplate();

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

                    var location = new Vector3(x, 1, z);
                    var positionSnapshot = new Position.Snapshot(location.ToCoordinates());
                    var transformSnapshot = TransformUtils.CreateTransformSnapshot(location, Quaternion.identity);

                    cubeTemplate.SetComponent(positionSnapshot);
                    cubeTemplate.SetComponent(transformSnapshot);
                    snapshot.AddEntity(cubeTemplate);
                }
            }
        }

        private static void CreateSpinner(Snapshot snapshot, Coordinates coords)
        {
            const string entityType = "Spinner";

            var transform = TransformUtils.CreateTransformSnapshot(coords, Quaternion.identity);

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(coords), WorkerUtils.UnityGameLogic);
            template.AddComponent(new Metadata.Snapshot(entityType), WorkerUtils.UnityGameLogic);
            template.AddComponent(transform, WorkerUtils.UnityGameLogic);
            template.AddComponent(new Persistence.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new Collisions.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new SpinnerColor.Snapshot(Color.BLUE), WorkerUtils.UnityGameLogic);
            template.AddComponent(new SpinnerRotation.Snapshot(), WorkerUtils.UnityGameLogic);

            template.SetReadAccess(WorkerUtils.UnityGameLogic, WorkerUtils.UnityClient, WorkerUtils.MobileClient);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, WorkerUtils.UnityGameLogic);

            snapshot.AddEntity(template);
        }
    }
}

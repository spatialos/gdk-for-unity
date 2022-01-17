using System;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.QueryBasedInterest;
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
            using (var snapshot = CreateSnapshot(arguments.NumberEntities))
            {
                Debug.Log($"Writing snapshot to: {arguments.OutputPath}");
                snapshot.WriteToFile(arguments.OutputPath);
            }
        }

        private static Snapshot CreateSnapshot(int cubeCount)
        {
            var snapshot = new Snapshot();

            snapshot.AddEntity(EntityTemplates.LoadBalancerPartitionEntityId, EntityTemplates.CreateLoadBalancingPartition());
            snapshot.AddEntity(EntityTemplates.PlayerCreatorEntityId, EntityTemplates.CreatePlayerSpawnerEntityTemplate(new Coordinates()));

            AddCubeGrid(snapshot, cubeCount);

            CreateSpinner(snapshot, new Coordinates { X = 5.5, Y = 0.5f, Z = 0.0 });
            CreateSpinner(snapshot, new Coordinates { X = -5.5, Y = 0.5f, Z = 0.0 });

            return snapshot;
        }

        private static void CreateSpinner(Snapshot snapshot, Coordinates coords)
        {
            var template = EntityTemplates.CreateSpinnerEntityTemplate(coords);
            snapshot.AddEntity(template);
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

                    var location = new Vector3(x, 1, z);
                    var cubeTemplate = EntityTemplates.CreateCubeEntityTemplate(location);
                    snapshot.AddEntity(cubeTemplate);
                }
            }
        }
    }
}

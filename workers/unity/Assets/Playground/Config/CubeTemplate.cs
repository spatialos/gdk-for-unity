using Generated.Improbable;
using Generated.Improbable.Transform;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Improbable.Worker.Core;

namespace Playground
{
    public static class CubeTemplate
    {
        public static Entity CreateCubeEntityTemplate(Coordinates coords)
        {
            const string entityType = "Cube";

            var transform = Transform.Component.CreateSchemaComponentData(
                new Location { X = (float) coords.X, Y = (float) coords.Y, Z = (float) coords.Z },
                new Quaternion { W = 1, X = 0, Y = 0, Z = 0 },
                0
            );

            var cubeColor = CubeColor.Component.CreateSchemaComponentData();
            var cubeTargetVelocity = CubeTargetVelocity.Component.CreateSchemaComponentData(new Vector3f { X = -2.0f });
            var prefab = Prefab.Component.CreateSchemaComponentData(entityType);
            var launchable = Launchable.Component.CreateSchemaComponentData(new EntityId(0));
            var archetypeComponent = ArchetypeComponent.Component.CreateSchemaComponentData(entityType);

            var entity = EntityBuilder.Begin()
                .AddPosition(coords.X, 0, coords.Z, WorkerUtils.UnityGameLogic)
                .AddMetadata(entityType, WorkerUtils.UnityGameLogic)
                .SetPersistence(true)
                .SetReadAcl(TemplateUtils.AllWorkerAttributes)
                .AddComponent(transform, WorkerUtils.UnityGameLogic)
                .AddComponent(cubeColor, WorkerUtils.UnityGameLogic)
                .AddComponent(cubeTargetVelocity, WorkerUtils.UnityGameLogic)
                .AddComponent(prefab, WorkerUtils.UnityGameLogic)
                .AddComponent(archetypeComponent, WorkerUtils.UnityGameLogic)
                .AddComponent(launchable, WorkerUtils.UnityGameLogic)
                .Build();

            return entity;
        }
    }
}

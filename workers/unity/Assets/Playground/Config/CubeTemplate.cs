using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Transform;
using Improbable.Worker;
using Improbable.Worker.Core;

namespace Playground
{
    public static class CubeTemplate
    {
        public static Entity CreateCubeEntityTemplate(Coordinates coords)
        {
            const string entityType = "Cube";

            var cubeColor = CubeColor.Component.CreateSchemaComponentData();
            var cubeTargetVelocity = CubeTargetVelocity.Component.CreateSchemaComponentData(new Vector3f { X = -2.0f });
            var launchable = Launchable.Component.CreateSchemaComponentData(new EntityId(0));

            var entityBuilder = EntityBuilder.Begin()
                .AddPosition(coords.X, coords.Y, coords.Z, WorkerUtils.UnityGameLogic)
                .AddMetadata(entityType, WorkerUtils.UnityGameLogic)
                .SetPersistence(true)
                .SetReadAcl(WorkerUtils.AllWorkerAttributes)
                .AddComponent(cubeColor, WorkerUtils.UnityGameLogic)
                .AddComponent(cubeTargetVelocity, WorkerUtils.UnityGameLogic)
                .AddComponent(launchable, WorkerUtils.UnityGameLogic);

            TransformSynchronizationHelper.AddComponents(ref entityBuilder, WorkerUtils.UnityGameLogic,
                location: new Location((float) coords.X, (float) coords.Y, (float) coords.Z));

            return entityBuilder.Build();
        }
    }
}

using Improbable;
using Improbable.Gdk.Core;
using Improbable.Transform;
using Improbable.Worker;
using Improbable.Worker.Core;

namespace Playground
{
    public static class CubeTemplate
    {
        public static Entity CreateCubeEntityTemplate(Coordinates coords)
        {
            var transform = TransformInternal.Component.CreateSchemaComponentData(
                new Location((float) coords.X, (float) coords.Y, (float) coords.Z),
                new Quaternion(1.0f, 0.0f, 0.0f, 0.0f),
                new Velocity(0.0f, 0.0f, 0.0f),
                0,
                0.0f
            );

            var cubeColor = CubeColor.Component.CreateSchemaComponentData();
            var cubeTargetVelocity = CubeTargetVelocity.Component.CreateSchemaComponentData(new Vector3f { X = -2.0f });
            var launchable = Launchable.Component.CreateSchemaComponentData(new EntityId(0));

            var entity = EntityBuilder.Begin()
                .AddPosition(coords.X, coords.Y, coords.Z, WorkerUtils.UnityGameLogic)
                .AddMetadata("Cube", WorkerUtils.UnityGameLogic)
                .SetPersistence(true)
                .SetReadAcl(WorkerUtils.AllWorkerAttributes)
                .AddComponent(transform, WorkerUtils.UnityGameLogic)
                .AddComponent(cubeColor, WorkerUtils.UnityGameLogic)
                .AddComponent(cubeTargetVelocity, WorkerUtils.UnityGameLogic)
                .AddComponent(launchable, WorkerUtils.UnityGameLogic)
                .Build();

            return entity;
        }
    }
}

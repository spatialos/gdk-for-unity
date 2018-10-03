using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Worker;
using Improbable.Worker.Core;

namespace Playground
{
    public static class CubeTemplate
    {
        public static EntityTemplate CreateCubeEntityTemplate(Coordinates coords)
        {
            var cubeColor = CubeColor.Component.CreateSchemaComponentData();
            var cubeTargetVelocity = CubeTargetVelocity.Component.CreateSchemaComponentData(new Vector3f { X = -2.0f });
            var launchable = Launchable.Component.CreateSchemaComponentData(new EntityId(0));

            var entityBuilder = EntityBuilder.Begin()
                .AddPosition(coords.X, coords.Y, coords.Z, WorkerUtils.UnityGameLogic)
                .AddMetadata("Cube", WorkerUtils.UnityGameLogic)
                .SetPersistence(true)
                .SetReadAcl(WorkerUtils.AllWorkerAttributes)
                .AddComponent(cubeColor, WorkerUtils.UnityGameLogic)
                .AddComponent(cubeTargetVelocity, WorkerUtils.UnityGameLogic)
                .AddComponent(launchable, WorkerUtils.UnityGameLogic)
                .AddTransformSynchronizationComponents(WorkerUtils.UnityGameLogic,
                    location: coords.NarrowToUnityVector());

            return entityBuilder.Build();
        }
    }
}

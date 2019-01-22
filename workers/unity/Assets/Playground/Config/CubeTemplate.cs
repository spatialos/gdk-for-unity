using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;

namespace Playground
{
    public static class CubeTemplate
    {
        public static EntityTemplate CreateCubeEntityTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new Metadata.Snapshot { EntityType = "Cube" }, WorkerUtils.UnityGameLogic);
            template.AddComponent(new Persistence.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new CubeColor.Snapshot(), WorkerUtils.UnityGameLogic);
            template.AddComponent(new CubeTargetVelocity.Snapshot { TargetVelocity = new Vector3f(-2.0f, 0, 0) },
                WorkerUtils.UnityGameLogic);
            template.AddComponent(new Launchable.Snapshot(), WorkerUtils.UnityGameLogic);
            TransformSynchronizationHelper.AddTransformSynchronizationComponents(template, WorkerUtils.UnityGameLogic);

            template.SetReadAccess(WorkerUtils.UnityGameLogic, WorkerUtils.UnityClient, WorkerUtils.iOSClient, WorkerUtils.AndroidClient);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, WorkerUtils.UnityGameLogic);

            return template;
        }
    }
}

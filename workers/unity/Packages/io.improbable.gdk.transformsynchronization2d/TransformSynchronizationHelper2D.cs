using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformSynchronizationHelper2D
    {
        public static void Enable2DMode(World world)
        {
            var syncImpl = new Rigidbody2DTransformSync();

            var defaultApplyLatestTransformSystem = world.GetOrCreateSystem<DefaultApplyLatestTransformSystem>();
            defaultApplyLatestTransformSystem.RegisterTransformSyncType(syncImpl);

            var defaultUpdateLatestTransformSystem = world.GetOrCreateSystem<DefaultUpdateLatestTransformSystem>();
            defaultUpdateLatestTransformSystem.RegisterTransformSyncType(syncImpl);

            var authorityGainedSystem = world.GetOrCreateSystem<ResetForAuthorityGainedSystem>();
            authorityGainedSystem.RegisterTransformSyncType(syncImpl);

            var kinematicSystem = world.GetOrCreateSystem<SetKinematicFromAuthoritySystem>();
            kinematicSystem.RegisterTransformSyncType(syncImpl);
        }
    }
}

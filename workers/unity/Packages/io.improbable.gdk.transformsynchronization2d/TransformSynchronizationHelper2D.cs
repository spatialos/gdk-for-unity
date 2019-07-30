using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformSynchronizationHelper2D
    {
        public static void Enable2DMode(World world)
        {
            var syncImpl = new Rigidbody2DTransformSync();

            var defaultApplyLatestTransformSystem = world.GetOrCreateSystem<DefaultApplyLatestTransformSystem>();
            defaultApplyLatestTransformSystem.RegisterType(syncImpl);

            var defaultUpdateLatestTransformSystem = world.GetOrCreateSystem<DefaultUpdateLatestTransformSystem>();
            defaultUpdateLatestTransformSystem.RegisterType(syncImpl);

            var authorityGainedSystem = world.GetOrCreateSystem<ResetForAuthorityGainedSystem>();
            authorityGainedSystem.RegisterType(syncImpl);

            var kinematicSystem = world.GetOrCreateSystem<SetKinematicFromAuthoritySystem>();
            kinematicSystem.RegisterType(syncImpl);
        }
    }
}

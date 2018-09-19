using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformSynchronizationEntityBuilderHelper
    {
        public static void AddComponents(EntityBuilder entityBuilder, string writeAccess,
            Location location = new Location(),
            Quaternion quaternion = default(Quaternion),
            Velocity velocity = new Velocity(),
            uint physicsTick = 0,
            float ticksPerSecond = 0)
        {
            var transform = TransformInternal.Component.CreateSchemaComponentData(
                location,
                quaternion,
                velocity,
                physicsTick,
                ticksPerSecond
            );
            entityBuilder.AddComponent(transform, writeAccess);
        }
    }
}

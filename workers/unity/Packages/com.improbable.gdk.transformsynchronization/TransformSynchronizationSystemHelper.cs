using System;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformSynchronizationSystemHelper
    {
        public static readonly Type[] ClientSystems =
        {
            typeof(TickSystem),
            typeof(LocalTransformSyncSystem),
            typeof(InterpolateTransformSystem),
            typeof(ApplyTransformUpdatesSystem),
            typeof(TransformSendSystem),
            typeof(PositionSendSystem)
        };

        public static readonly Type[] ServerSystems =
        {
            typeof(TickSystem),
            typeof(LocalTransformSyncSystem),
            typeof(InterpolateTransformSystem),
            typeof(ApplyTransformUpdatesSystem),
            typeof(TransformSendSystem),
            typeof(PositionSendSystem)
        };
    }
}

using System;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class CoreSystemHelper
    {
        public static readonly Type[] Systems =
        {
            typeof(SpatialOSReceiveSystem),
            typeof(SpatialOSSendSystem),
            typeof(CleanReactiveComponentsSystem),
            typeof(WorldCommandsCleanSystem),
            typeof(WorldCommandsSendSystem),
            typeof(CommandRequestTrackerSystem),
        };
    }
}

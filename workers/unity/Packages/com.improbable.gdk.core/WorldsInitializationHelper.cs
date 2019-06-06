using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public static class WorldsInitializationHelper
    {
        public static void DomainUnloadShutdown()
        {
            World.DisposeAllWorlds();
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(null);
        }
    }
}

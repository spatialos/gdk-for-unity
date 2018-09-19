using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;

namespace Improbable.Gdk.PlayerLifecycle
{
    public class PlayerLifecycleEntityBuilderHelper
    {
        public static void AddComponents(EntityBuilder entityBuilder, string clientAccess, string serverAccess)
        {
            var clientHeartbeat = PlayerHeartbeatClient.Component.CreateSchemaComponentData();
            var serverHeartbeat = PlayerHeartbeatServer.Component.CreateSchemaComponentData();
            entityBuilder.AddComponent(clientHeartbeat, clientAccess);
            entityBuilder.AddComponent(serverHeartbeat, serverAccess);
        }
    }
}

using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;

namespace Improbable.Gdk.PlayerLifecycle
{
    public class PlayerLifecycleEntityBuilderHelper
    {
        public static EntityBuilder AddComponents(EntityBuilder entityBuilder, string clientAccess, string serverAccess)
        {
            var clientHeartbeat = PlayerHeartbeatClient.Component.CreateSchemaComponentData();
            var serverHeartbeat = PlayerHeartbeatServer.Component.CreateSchemaComponentData();
            return entityBuilder
                .AddComponent(clientHeartbeat, clientAccess)
                .AddComponent(serverHeartbeat, serverAccess);
        }
    }
}

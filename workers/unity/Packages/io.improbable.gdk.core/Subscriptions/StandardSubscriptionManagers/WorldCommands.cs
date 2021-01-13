using System;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    [AutoRegisterSubscriptionManager]
    public class WorldCommandSenderSubscriptionManager : CommandSenderSubscriptionManagerBase<WorldCommandSender>
    {
        public WorldCommandSenderSubscriptionManager(World world) : base(world)
        {
        }

        protected override WorldCommandSender CreateSender(Entity entity, World world)
        {
            return new WorldCommandSender(entity, world);
        }
    }

    public class WorldCommandSender : CommandSender
    {
        public WorldCommandSender(Entity entity, World world) : base(entity, world)
        {
        }

        public void SendCreateEntityCommand(WorldCommands.CreateEntity.Request request,
            Action<WorldCommands.CreateEntity.ReceivedResponse> callback = null)
        {
            SendCommand(request, callback);
        }

        public void SendDeleteEntityCommand(WorldCommands.DeleteEntity.Request request,
            Action<WorldCommands.DeleteEntity.ReceivedResponse> callback = null)
        {
            SendCommand(request, callback);
        }

        public void SendReserveEntityIdsCommand(WorldCommands.ReserveEntityIds.Request request,
            Action<WorldCommands.ReserveEntityIds.ReceivedResponse> callback = null)
        {
            SendCommand(request, callback);
        }

        public void SendEntityQueryCommand(WorldCommands.EntityQuery.Request request,
            Action<WorldCommands.EntityQuery.ReceivedResponse> callback = null)
        {
            SendCommand(request, callback);
        }
    }
}

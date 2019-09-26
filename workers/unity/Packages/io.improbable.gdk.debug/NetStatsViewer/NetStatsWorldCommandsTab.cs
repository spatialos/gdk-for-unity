using System.Collections.Generic;
using Improbable.Gdk.Core.NetworkStats;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.NetStats
{
    internal class NetStatsWorldCommandsTab : NetStatsTab
    {
        public new class UxmlFactory : UxmlFactory<NetStatsWorldCommandsTab>
        {
        }

        protected override void PopulateElementInfo()
        {
            ElementInfo = new List<(string, MessageTypeUnion)>()
            {
                ("CreateEntity.Request", MessageTypeUnion.WorldCommandRequest(WorldCommand.CreateEntity)),
                ("CreateEntity.Response", MessageTypeUnion.WorldCommandResponse(WorldCommand.CreateEntity)),
                ("DeleteEntity.Request", MessageTypeUnion.WorldCommandRequest(WorldCommand.DeleteEntity)),
                ("DeleteEntity.Response", MessageTypeUnion.WorldCommandResponse(WorldCommand.DeleteEntity)),
                ("EntityQuery.Request", MessageTypeUnion.WorldCommandRequest(WorldCommand.EntityQuery)),
                ("EntityQuery.Response", MessageTypeUnion.WorldCommandResponse(WorldCommand.EntityQuery)),
                ("ReserveEntityIds.Request", MessageTypeUnion.WorldCommandRequest(WorldCommand.ReserveEntityIds)),
                ("ReserveEntityIds.Response", MessageTypeUnion.WorldCommandResponse(WorldCommand.ReserveEntityIds)),
            };
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.NetworkStats;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.NetStats
{
    internal class NetStatsCommandsTab : NetStatsTab
    {
        public new class UxmlFactory : UxmlFactory<NetStatsCommandsTab>
        {
        }

        protected override void PopulateElementInfo()
        {
            ElementInfo = ComponentDatabase.Metaclasses
                .SelectMany(pair => pair.Value.Commands
                    .SelectMany(metaclass => new List<(string, MessageTypeUnion)>()
                    {
                        ($"{metaclass.Name}.Request",
                            MessageTypeUnion.CommandRequest(pair.Key, metaclass.CommandIndex)),
                        ($"{metaclass.Name}.Response",
                            MessageTypeUnion.CommandResponse(pair.Key, metaclass.CommandIndex))
                    })
                )
                .ToList();
        }
    }
}

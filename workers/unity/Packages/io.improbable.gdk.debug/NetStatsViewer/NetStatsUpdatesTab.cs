using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.NetworkStats;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.NetStats
{
    internal class NetStatsUpdatesTab : NetStatsTab
    {
        public new class UxmlFactory : UxmlFactory<NetStatsUpdatesTab>
        {
        }

        protected override void PopulateElementInfo()
        {
            ElementInfo = ComponentDatabase.Metaclasses
                .Select(pair => (pair.Value.Name, MessageTypeUnion.Update(pair.Key)))
                .ToList();
        }
    }
}

using Improbable.Gdk.Core.NetworkStats;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.NetStats
{
    internal abstract class NetStatsTab : VisualElement
    {
        protected NetworkStatisticsSystem netStatSystem;

        public abstract void InitializeTab();
        public abstract void Update();

        internal void SetSystem(NetworkStatisticsSystem system)
        {
            netStatSystem = system;
        }
    }
}

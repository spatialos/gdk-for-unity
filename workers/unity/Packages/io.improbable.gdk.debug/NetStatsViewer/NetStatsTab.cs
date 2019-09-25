using Improbable.Gdk.Core.NetworkStats;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.NetStats
{
    internal abstract class NetStatsTab : VisualElement
    {
        protected NetworkStatisticsSystem netStatSystem;

        public abstract void InitializeTab();
        protected abstract void UpdateElement(int index, VisualElement element);

        public void Update()
        {
            var scrollView = this.Q<ScrollView>("container");
            var count = scrollView.childCount;
            var viewportBound = scrollView.contentViewport.localBound;

            // Update all visible items in the list
            for (var i = 0; i < count; i++)
            {
                var element = scrollView[i];
                if (viewportBound.Overlaps(element.localBound))
                {
                    UpdateElement(i, element);
                }
            }
        }

        internal void SetSystem(NetworkStatisticsSystem system)
        {
            netStatSystem = system;
        }
    }
}

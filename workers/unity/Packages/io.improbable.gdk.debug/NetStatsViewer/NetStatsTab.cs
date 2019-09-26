using System.Collections.Generic;
using Improbable.Gdk.Core.NetworkStats;
using UnityEditor;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.NetStats
{
    internal abstract class NetStatsTab : VisualElement
    {
        private const string UpdateRowUxmlPath =
            "Packages/io.improbable.gdk.debug/NetStatsViewer/Templates/UpdateRow.uxml";

        private NetworkStatisticsSystem netStatSystem;
        protected List<(string, MessageTypeUnion)> ElementInfo;

        protected abstract void PopulateElementInfo();

        public void InitializeTab()
        {
            PopulateElementInfo();

            // Load update row
            var itemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UpdateRowUxmlPath);

            // Clone row for each component, fill list
            var scrollView = this.Q<ScrollView>("container");
            for (var i = 0; i < ElementInfo.Count; i++)
            {
                var element = itemTemplate.CloneTree();
                UpdateElement(i, element);
                scrollView.Add(element);
            }
        }

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

        // TODO: Temporary placeholder to allow sorting
        private (string name, MessageTypeUnion messageType) GetInfoForIndex(int index)
        {
            return ElementInfo[index];
        }

        private void UpdateElement(int index, VisualElement element)
        {
            // Set component name
            var (elementName, messageType) = GetInfoForIndex(index);
            var nameElement = element.Q<Label>("name");
            nameElement.text = elementName;
            nameElement.tooltip = elementName;

            if (netStatSystem?.World == null || !netStatSystem.World.IsCreated)
            {
                return;
            }

            // Incoming stats
            var (dataIn, time) =
                netStatSystem.GetSummary(messageType, 60, Direction.Incoming);

            if (time == 0)
            {
                time = 1;
            }

            element.Q<Label>("opsIn").text = (dataIn.Count / time).ToString("F1");
            element.Q<Label>("sizeIn").text = (dataIn.Size / 1024f / time).ToString("F3");

            // Outgoing stats
            var (dataOut, _) =
                netStatSystem.GetSummary(messageType, 60, Direction.Outgoing);

            element.Q<Label>("opsOut").text = (dataOut.Count / time).ToString("F1");
            element.Q<Label>("sizeOut").text = (dataOut.Size / 1024f / time).ToString("F3");
        }
    }
}

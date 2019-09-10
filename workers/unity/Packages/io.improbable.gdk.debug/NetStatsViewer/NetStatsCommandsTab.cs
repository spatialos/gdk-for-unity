using System.Collections.Generic;
using Improbable.Gdk.Core.NetworkStats;
using UnityEditor;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.NetStats
{
    internal class NetStatsCommandsTab : VisualElement
    {
        private const string UpdateRowUxmlPath = "Packages/io.improbable.gdk.debug/NetStatsViewer/Templates/UpdateRow.uxml";

        private NetworkStatisticsSystem netStatSystem;

        private List<(string Name, uint ComponentId)> spatialComponents;
        private Dictionary<int, VisualElement> listElements;

        public new class UxmlFactory : UxmlFactory<NetStatsCommandsTab>
        {
        }

        internal void InitializeTab(List<(string Name, uint ComponentId)> spatialComponents)
        {
            this.spatialComponents = spatialComponents;

            // Load update row
            var itemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UpdateRowUxmlPath);

            // Clone row for each component, fill list
            var scrollView = this.Q<ScrollView>("updatesContainer");
            for (var i = 0; i < spatialComponents.Count; i++)
            {
                var element = itemTemplate.CloneTree();
                UpdateElement(i, element);
                scrollView.Add(element);
            }
        }

        internal void SetSystem(NetworkStatisticsSystem system)
        {
            netStatSystem = system;
        }

        internal void Update()
        {
            var scrollView = this.Q<ScrollView>("updatesContainer");
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

        // TODO: Temporary placeholder to allow sorting
        private (string Name, uint ComponentId) GetComponentInfoForIndex(int index)
        {
            return spatialComponents[index];
        }

        private void UpdateElement(int index, VisualElement element)
        {
            // Set component name
            var (componentName, componentId) = GetComponentInfoForIndex(index);
            element.Q<Label>("name").text = componentName;

            if (netStatSystem?.World == null || !netStatSystem.World.IsCreated)
            {
                return;
            }

            // Incoming stats
            var (dataIn, time) =
                netStatSystem.GetSummary(MessageTypeUnion.Update(componentId), 60, Direction.Incoming);

            if (time == 0)
            {
                time = 1;
            }

            element.Q<Label>("opsIn").text = (dataIn.Count / time).ToString("F1");
            element.Q<Label>("sizeIn").text = (dataIn.Size / 1024f / time).ToString("F3");

            // Outgoing stats
            var (dataOut, _) =
                netStatSystem.GetSummary(MessageTypeUnion.Update(componentId), 60, Direction.Outgoing);

            element.Q<Label>("opsOut").text = (dataOut.Count / time).ToString("F1");
            element.Q<Label>("sizeOut").text = (dataOut.Size / 1024f / time).ToString("F3");
        }
    }
}

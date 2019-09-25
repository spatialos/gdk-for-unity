using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.NetworkStats;
using UnityEditor;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.NetStats
{
    internal class NetStatsUpdatesTab : NetStatsTab
    {
        private const string UpdateRowUxmlPath =
            "Packages/io.improbable.gdk.debug/NetStatsViewer/Templates/UpdateRow.uxml";

        private List<IComponentMetaclass> spatialComponents;

        public new class UxmlFactory : UxmlFactory<NetStatsUpdatesTab>
        {
        }

        public override void InitializeTab()
        {
            spatialComponents = ComponentDatabase.Metaclasses.Select(pair => pair.Value).ToList();

            // Load update row
            var itemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UpdateRowUxmlPath);

            // Clone row for each component, fill list
            var scrollView = this.Q<ScrollView>("updatesContainer");
            for (var i = 0; i < ComponentDatabase.Metaclasses.Count; i++)
            {
                var element = itemTemplate.CloneTree();
                UpdateElement(i, element);
                scrollView.Add(element);
            }
        }

        public override void Update()
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
        private IComponentMetaclass GetComponentInfoForIndex(int index)
        {
            return spatialComponents[index];
        }

        private void UpdateElement(int index, VisualElement element)
        {
            // Set component name
            var componentMetaclass = GetComponentInfoForIndex(index);
            var componentId = componentMetaclass.ComponentId;
            var nameElement = element.Q<Label>("name");
            nameElement.text = componentMetaclass.Name;
            nameElement.tooltip = componentMetaclass.Name;

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

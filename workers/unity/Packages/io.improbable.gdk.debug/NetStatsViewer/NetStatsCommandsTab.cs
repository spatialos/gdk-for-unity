using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.NetworkStats;
using UnityEditor;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.NetStats
{
    internal class NetStatsCommandsTab : NetStatsTab
    {
        private const string UpdateRowUxmlPath =
            "Packages/io.improbable.gdk.debug/NetStatsViewer/Templates/UpdateRow.uxml";

        private List<(string, MessageTypeUnion)> elementInfo;

        public new class UxmlFactory : UxmlFactory<NetStatsCommandsTab>
        {
        }

        public override void InitializeTab()
        {
            elementInfo = ComponentDatabase.Metaclasses
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

            // Load update row
            var itemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UpdateRowUxmlPath);

            // Clone row for each component, fill list
            var scrollView = this.Q<ScrollView>("container");
            for (var i = 0; i < elementInfo.Count; i++)
            {
                var element = itemTemplate.CloneTree();
                UpdateElement(i, element);
                scrollView.Add(element);
            }
        }

        public override void Update()
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

        // TODO: Temporary placeholder to allow sorting
        private (string name, MessageTypeUnion messageType) GetCommandInfoForIndex(int index)
        {
            return elementInfo[index];
        }

        private void UpdateElement(int index, VisualElement element)
        {
            // Set component name
            var infoForIndex = GetCommandInfoForIndex(index);
            var nameElement = element.Q<Label>("name");
            nameElement.text = infoForIndex.name;
            nameElement.tooltip = infoForIndex.name;

            if (netStatSystem?.World == null || !netStatSystem.World.IsCreated)
            {
                return;
            }

            // Incoming stats
            var (dataIn, time) =
                netStatSystem.GetSummary(infoForIndex.messageType, 60, Direction.Incoming);

            if (time == 0)
            {
                time = 1;
            }

            element.Q<Label>("opsIn").text = (dataIn.Count / time).ToString("F1");
            element.Q<Label>("sizeIn").text = (dataIn.Size / 1024f / time).ToString("F3");

            // Outgoing stats
            var (dataOut, _) =
                netStatSystem.GetSummary(infoForIndex.messageType, 60, Direction.Outgoing);

            element.Q<Label>("opsOut").text = (dataOut.Count / time).ToString("F1");
            element.Q<Label>("sizeOut").text = (dataOut.Size / 1024f / time).ToString("F3");
        }
    }
}

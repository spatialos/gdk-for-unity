using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;
using UnityEditor;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug
{
    public class NetStatViewer : EditorWindow
    {
        private const string WindowUxmlPath = "Packages/io.improbable.gdk.debug/Templates/NetStatsWindow.uxml";
        private const string ItemUxmlPath = "Packages/io.improbable.gdk.debug/Templates/Item.uxml";

        private World selectedWorld;
        private NetworkStatisticsSystem netStatSystem;
        private List<(string Name, uint ComponentId)> spatialComponents;

        private static bool visible;

        [MenuItem("SpatialOS/Network Analyzer &n", false, 52)]
        public static void ShowWindow()
        {
            var inspectorWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            var window = GetWindow<NetStatViewer>(inspectorWindowType);

            if (visible)
            {
                window.Close();
                window = GetWindow<NetStatViewer>(inspectorWindowType);
            }

            window.titleContent.text = "Network Analyzer";
            window.titleContent.tooltip = "Tooltip";
            window.Show();
        }

        public void OnDestroy()
        {
            visible = false;
        }

        private void OnEnable()
        {
            // Hack
            selectedWorld = World.AllWorlds.FirstOrDefault(w => w.GetExistingSystem<NetworkStatisticsSystem>() != null);
            if (selectedWorld == null)
            {
                return;
            }

            netStatSystem = selectedWorld.GetExistingSystem<NetworkStatisticsSystem>();

            // Generate list of types
            spatialComponents = ComponentDatabase.ComponentsToIds
                .Select(pair => (pair.Key.DeclaringType.Name, pair.Value))
                .ToList();

            SetupUI();

            visible = true;
        }

        private void SetupUI()
        {
            var windowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WindowUxmlPath);
            windowTemplate.CloneTree(rootVisualElement);

            var itemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ItemUxmlPath);

            var listView = rootVisualElement.Q<ListView>("DataContainer");
            listView.selectionType = SelectionType.None;
            listView.makeItem = () => itemTemplate.CloneTree();
            listView.bindItem = BindItem;
            listView.itemsSource = spatialComponents;
        }

        private (string Name, uint ComponentId) GetComponentInfoForIndex(int index)
        {
            return spatialComponents[index];
        }

        private void BindItem(VisualElement element, int index)
        {
            var componentInfo = GetComponentInfoForIndex(index);
            var componentName = componentInfo.Name;
            var (dataIn, time) =
                netStatSystem.GetSummary(MessageTypeUnion.Update(componentInfo.ComponentId), 60, Direction.Incoming);

            if (time == 0)
            {
                time = 1;
            }

            element.Q<Label>("name").text = componentName;
            element.Q<Label>("opsIn").text = (dataIn.Count / time).ToString("F1");
            element.Q<Label>("sizeIn").text = (dataIn.Size / 1024f / time).ToString("F3");

            var (dataOut, _) =
                netStatSystem.GetSummary(MessageTypeUnion.Update(componentInfo.ComponentId), 60, Direction.Outgoing);

            element.Q<Label>("opsOut").text = (dataOut.Count / time).ToString("F1");
            element.Q<Label>("sizeOut").text = (dataOut.Size / 1024f / time).ToString("F3");
        }

        private void OnInspectorUpdate()
        {
            if (netStatSystem?.World == null || !netStatSystem.World.IsCreated)
            {
                return;
            }

            var listView = rootVisualElement.Q<ListView>("DataContainer");
            listView.Refresh();
        }
    }
}

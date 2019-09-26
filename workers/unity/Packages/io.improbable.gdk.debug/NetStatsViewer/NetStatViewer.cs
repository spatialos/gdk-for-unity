using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug
{
    internal class NetStatViewer : EditorWindow
    {
        private const string WindowUxmlPath = "Packages/io.improbable.gdk.debug/NetStatsViewer/Templates/NetStatsWindow.uxml";
        private const string UpdateRowUxmlPath = "Packages/io.improbable.gdk.debug/NetStatsViewer/Templates/UpdateRow.uxml";

        private World selectedWorld;
        private NetworkStatisticsSystem netStatSystem;
        private List<(string Name, uint ComponentId)> spatialComponents;
        private Dictionary<int, VisualElement> listElements;

        [MenuItem("SpatialOS/Window/Network Analyzer", false)]
        public static void ShowWindow()
        {
            var inspectorWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            var window = GetWindow<NetStatViewer>(inspectorWindowType);
            window.titleContent.text = "Network Analyzer";
            window.Show();
        }

        private void OnEnable()
        {
            // Generate list of types
            spatialComponents = ComponentDatabase.Metaclasses
                .Select(pair => (pair.Value.Name, pair.Key))
                .ToList();

            // Load UI
            SetupUI();
            SetupWorldSelection();
        }

        // Update UI at 10 FPS
        private void OnInspectorUpdate()
        {
            SetupWorldSelection();
            RefreshUpdates();
        }

        private void SetupUI()
        {
            // Load main window
            var windowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WindowUxmlPath);
            windowTemplate.CloneTree(rootVisualElement);

            // Load update row
            var itemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UpdateRowUxmlPath);

            // Clone row for each component, fill list
            var scrollView = rootVisualElement.Q<ScrollView>("updatesContainer");
            for (var i = 0; i < spatialComponents.Count; i++)
            {
                var element = itemTemplate.CloneTree();
                UpdateElement(i, element);
                scrollView.Add(element);
            }
        }

        private void SetupWorldSelection()
        {
            // Find spatial worlds
            var spatialWorlds = World.AllWorlds
                .Where(w => w.GetExistingSystem<NetworkStatisticsSystem>() != null)
                .ToList();

            // Fill menu items
            var worldMenu = rootVisualElement.Q<ToolbarMenu>("worldSelector").menu;
            for (var i = 0; i < spatialWorlds.Count; i++)
            {
                var spatialWorld = spatialWorlds[i];
                worldMenu.InsertAction(i, spatialWorld.Name,
                    action => SelectWorld((World) action.userData),
                    action => selectedWorld == (World) action.userData
                        ? DropdownMenuAction.Status.Checked
                        : DropdownMenuAction.Status.Normal,
                    spatialWorld);
            }

            // Trim excess items
            var menuSize = worldMenu.MenuItems().Count;
            for (var i = spatialWorlds.Count; i < menuSize; i++)
            {
                worldMenu.RemoveItemAt(spatialWorlds.Count);
            }

            // Update selected item if needed
            if (selectedWorld == null || !selectedWorld.IsCreated)
            {
                SelectWorld(spatialWorlds.FirstOrDefault());
            }
        }

        private void SelectWorld(World world)
        {
            selectedWorld = world;
            rootVisualElement.Q<ToolbarMenu>("worldSelector").text =
                selectedWorld?.Name ?? "No SpatialOS worlds active";
            netStatSystem = selectedWorld?.GetExistingSystem<NetworkStatisticsSystem>();
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

        private void RefreshUpdates()
        {
            var scrollView = rootVisualElement.Q<ScrollView>("updatesContainer");
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
    }
}

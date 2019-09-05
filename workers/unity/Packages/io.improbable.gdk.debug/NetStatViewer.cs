using System;
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
    public class NetStatViewer : EditorWindow
    {
        private const string WindowUxmlPath = "Packages/io.improbable.gdk.debug/Templates/NetStatsWindow.uxml";
        private const string ItemUxmlPath = "Packages/io.improbable.gdk.debug/Templates/Item.uxml";

        private World selectedWorld;
        private NetworkStatisticsSystem netStatSystem;
        private List<(string Name, uint ComponentId)> spatialComponents;
        private Dictionary<int, VisualElement> listElements;

        [MenuItem("SpatialOS/Network Analyzer &n", false, 52)]
        public static void ShowWindow()
        {
            var inspectorWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            var window = GetWindow<NetStatViewer>(inspectorWindowType);
            window.titleContent.text = "Network Analyzer";
            window.titleContent.tooltip = "Tooltip";
            window.Show();
        }

        private void OnEnable()
        {
            // Generate list of types
            spatialComponents = ComponentDatabase.ComponentsToIds
                .Select(pair => (pair.Key.DeclaringType.Name, pair.Value))
                .ToList();

            // Load UI
            SetupUI();
            SetupWorldSelection();
        }

        private void SetupWorldSelection()
        {
            // Find spatial worlds
            var spatialWorlds = World.AllWorlds.Where(w => w.GetExistingSystem<NetworkStatisticsSystem>() != null)
                .ToList();

            // Fill menu items
            var worldMenu = rootVisualElement.Q<ToolbarMenu>("worldSelector").menu;
            for (var i = 0; i < spatialWorlds.Count; i++)
            {
                var spatialWorld = spatialWorlds[i];
                worldMenu.InsertAction(i, spatialWorld.Name, action =>
                    {
                        SelectWorld((World) action.userData);
                    },
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

            if (selectedWorld == null || !selectedWorld.IsCreated)
            {
                SelectWorld(spatialWorlds.FirstOrDefault());
            }
        }

        private void SelectWorld(World world)
        {
            selectedWorld = world;
            rootVisualElement.Q<ToolbarMenu>("worldSelector").text = selectedWorld?.Name ?? "No SpatialOS worlds active";
            netStatSystem = selectedWorld?.GetExistingSystem<NetworkStatisticsSystem>();
        }

        private void SetupUI()
        {
            var windowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WindowUxmlPath);
            windowTemplate.CloneTree(rootVisualElement);

            var itemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ItemUxmlPath);

            var scrollView = rootVisualElement.Q<ScrollView>("updatesContainer");
            for (var i = 0; i < spatialComponents.Count; i++)
            {
                var element = itemTemplate.CloneTree();
                UpdateElement(i, element);
                scrollView.Add(element);
            }
        }

        private (string Name, uint ComponentId) GetComponentInfoForIndex(int index)
        {
            return spatialComponents[index];
        }

        private void UpdateElement(int index, VisualElement element)
        {
            // Set component name
            var componentInfo = GetComponentInfoForIndex(index);
            element.Q<Label>("name").text = componentInfo.Name;

            if (netStatSystem?.World == null || !netStatSystem.World.IsCreated)
            {
                return;
            }

            // Incoming stats
            var (dataIn, time) =
                netStatSystem.GetSummary(MessageTypeUnion.Update(componentInfo.ComponentId), 60, Direction.Incoming);

            if (time == 0)
            {
                time = 1;
            }

            element.Q<Label>("opsIn").text = (dataIn.Count / time).ToString("F1");
            element.Q<Label>("sizeIn").text = (dataIn.Size / 1024f / time).ToString("F3");

            // Outgoing stats
            var (dataOut, _) =
                netStatSystem.GetSummary(MessageTypeUnion.Update(componentInfo.ComponentId), 60, Direction.Outgoing);

            element.Q<Label>("opsOut").text = (dataOut.Count / time).ToString("F1");
            element.Q<Label>("sizeOut").text = (dataOut.Size / 1024f / time).ToString("F3");
        }

        private void RefreshUpdates()
        {
            var scrollView = rootVisualElement.Q<ScrollView>("updatesContainer");
            var count = scrollView.childCount;

            for (int i = 0; i < count; i++)
            {
                var element = scrollView[i];
                if (element.visible)
                {
                    UpdateElement(i, element);
                }
                else
                {
                    UnityEngine.Debug.Log("YEET");
                }
            }
        }

        private void OnInspectorUpdate()
        {
            SetupWorldSelection();
            RefreshUpdates();
        }
    }
}

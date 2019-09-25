using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.NetStats
{
    internal class NetStatViewer : EditorWindow
    {
        private enum Tab
        {
            Updates,
            Commands,
            WorldCommands
        }

        private const string WindowUxmlPath =
            "Packages/io.improbable.gdk.debug/NetStatsViewer/Templates/NetStatsWindow.uxml";

        private World selectedWorld;
        private Dictionary<Tab, NetStatsTab> tabs;
        private Tab selectedTab = Tab.Updates;

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
            // Load UI
            SetupUI();
            SetupWorldSelection();

            SelectTab(selectedTab);
        }

        // Update UI at 10 FPS
        private void OnInspectorUpdate()
        {
            SetupWorldSelection();

            foreach (var pair in tabs)
            {
                var tab = pair.Value;
                if (tab.visible)
                {
                    tab.Update();
                }
            }
        }

        private void SetupUI()
        {
            // Load main window
            var windowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WindowUxmlPath);
            windowTemplate.CloneTree(rootVisualElement);

            // Initialize tabs
            var updatesTab = rootVisualElement.Q<NetStatsUpdatesTab>();
            updatesTab.InitializeTab();

            var commandsTab = rootVisualElement.Q<NetStatsCommandsTab>();
            commandsTab.InitializeTab();

            var worldCommandsTab = rootVisualElement.Q<NetStatsWorldCommandsTab>();
            worldCommandsTab.InitializeTab();

            // Setup tab buttons
            tabs = new Dictionary<Tab, NetStatsTab>
            {
                { Tab.Updates, updatesTab },
                { Tab.Commands, commandsTab },
                { Tab.WorldCommands, worldCommandsTab }
            };

            rootVisualElement.Q<ToolbarButton>("updateSelector").clickable.clicked += () => SelectTab(Tab.Updates);
            rootVisualElement.Q<ToolbarButton>("commandSelector").clickable.clicked += () => SelectTab(Tab.Commands);
            rootVisualElement.Q<ToolbarButton>("worldCommandSelector").clickable.clicked += () => SelectTab(Tab.WorldCommands);
        }

        private void SelectTab(Tab tabType)
        {
            foreach (var pair in tabs)
            {
                pair.Value.AddToClassList("tab-hidden");
                pair.Value.visible = false;
            }

            tabs[tabType].RemoveFromClassList("tab-hidden");
            tabs[tabType].visible = true;
            selectedTab = tabType;
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
            var netStatSystem = selectedWorld?.GetExistingSystem<NetworkStatisticsSystem>();

            foreach (var pair in tabs)
            {
                pair.Value.SetSystem(netStatSystem);
            }
        }
    }
}

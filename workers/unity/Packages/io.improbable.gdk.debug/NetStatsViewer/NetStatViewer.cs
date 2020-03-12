using System.Collections.Generic;
using Improbable.Gdk.Core.Editor.UIElements;
using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.NetStats
{
    internal class NetStatViewer : EditorWindow
    {
        private enum TabType
        {
            Updates,
            Commands,
            WorldCommands
        }

        private const string WindowUxmlPath =
            "Packages/io.improbable.gdk.debug/NetStatsViewer/Templates/NetStatsWindow.uxml";

        private WorldSelector worldSelector;
        private Dictionary<TabType, NetStatsTab> tabs;
        private TabType selectedTabType = TabType.Updates;

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
            worldSelector.UpdateWorldSelection();

            SelectTab(selectedTabType);
        }

        // Update UI at 10 FPS
        private void OnInspectorUpdate()
        {
            worldSelector.UpdateWorldSelection();

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

            worldSelector = rootVisualElement.Q<WorldSelector>();
            worldSelector.OnWorldChanged += OnWorldChanged;

            // Initialize tabs
            var updatesTab = rootVisualElement.Q<NetStatsUpdatesTab>();
            updatesTab.InitializeTab();

            var commandsTab = rootVisualElement.Q<NetStatsCommandsTab>();
            commandsTab.InitializeTab();

            var worldCommandsTab = rootVisualElement.Q<NetStatsWorldCommandsTab>();
            worldCommandsTab.InitializeTab();

            // Setup tab buttons
            tabs = new Dictionary<TabType, NetStatsTab>
            {
                { TabType.Updates, updatesTab },
                { TabType.Commands, commandsTab },
                { TabType.WorldCommands, worldCommandsTab }
            };

            rootVisualElement.Q<ToolbarButton>("updateSelector").clickable.clicked += () => SelectTab(TabType.Updates);
            rootVisualElement.Q<ToolbarButton>("commandSelector").clickable.clicked += () => SelectTab(TabType.Commands);
            rootVisualElement.Q<ToolbarButton>("worldCommandSelector").clickable.clicked +=
                () => SelectTab(TabType.WorldCommands);
        }

        private void SelectTab(TabType tabTypeType)
        {
            foreach (var pair in tabs)
            {
                pair.Value.AddToClassList("tab-hidden");
                pair.Value.visible = false;
            }

            tabs[tabTypeType].RemoveFromClassList("tab-hidden");
            tabs[tabTypeType].visible = true;
            selectedTabType = tabTypeType;
        }

        private void OnWorldChanged(World world)
        {
            var netStatSystem = world?.GetExistingSystem<NetworkStatisticsSystem>();

            foreach (var pair in tabs)
            {
                pair.Value.SetSystem(netStatSystem);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug
{
    public class NetStatViewer : EditorWindow
    {
        private const string WindowUxmlPath = "Packages/io.improbable.gdk.debug/Templates/NetStatsWindow.uxml";
        private const string ItemUxmlPath = "Packages/io.improbable.gdk.debug/Templates/Item.uxml";

        [MenuItem("SpatialOS/Network Analyzer", false, 52)]
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
            var windowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WindowUxmlPath);
            windowTemplate.CloneTree(rootVisualElement);

            var itemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ItemUxmlPath);
            var testList = new List<int>();
            for (var i = 0; i < 100; i++)
            {
                testList.Add(i);
            }

            Func<VisualElement> makeItem = () => itemTemplate.CloneTree();

            var listView = rootVisualElement.Q<ListView>("DataContainer");
            listView.selectionType = SelectionType.None;
            listView.makeItem = makeItem;
            listView.bindItem = BindItem;
            listView.itemsSource = testList;
        }

        private void BindItem(VisualElement element, int index)
        {
            element.Q<Label>("name").text = $"Component {index.ToString()}";
        }
    }
}

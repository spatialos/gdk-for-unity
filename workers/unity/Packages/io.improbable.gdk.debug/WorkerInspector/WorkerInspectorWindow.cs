using Improbable.Gdk.Core.Editor.UIElements;
using Unity.Entities;
using UnityEditor;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector
{
    internal class WorkerInspectorWindow : EditorWindow
    {
        private WorldSelector worldSelector;
        private EntityList entityList;
        private EntityDetail entityDetail;
        private WorkerDetail workerDetail;

        [MenuItem("SpatialOS/Window/Worker Inspector", false)]
        public static void ShowWindow()
        {
            var windowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            var window = GetWindow<WorkerInspectorWindow>("Worker Inspector", windowType);
            window.Show();
        }

        private void OnEnable()
        {
            SetupUI();
            worldSelector.UpdateWorldSelection();
        }

        private void OnInspectorUpdate()
        {
            worldSelector.UpdateWorldSelection();
            entityList.Update();
            entityDetail.Update();
            workerDetail.Update();
        }

        private void SetupUI()
        {
            const string windowUxmlPath = "Packages/io.improbable.gdk.debug/WorkerInspector/Templates/WorkerInspectorWindow.uxml";
            const string windowUssPath = "Packages/io.improbable.gdk.debug/WorkerInspector/Templates/WorkerInspectorWindow.uss";

            var windowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(windowUxmlPath);
            windowTemplate.CloneTree(rootVisualElement);

            var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(windowUssPath);
            rootVisualElement.styleSheets.Add(stylesheet);

            worldSelector = rootVisualElement.Q<WorldSelector>();
            worldSelector.OnWorldChanged += OnWorldChanged;

            entityList = rootVisualElement.Q<EntityList>();
            entityList.OnEntitySelected += OnEntitySelected;

            entityDetail = rootVisualElement.Q<EntityDetail>();

            workerDetail = rootVisualElement.Q<WorkerDetail>();
        }

        private void OnWorldChanged(World world)
        {
            entityList.SetWorld(world);
            entityDetail.SetWorld(world);
            workerDetail.SetWorld(world);
        }

        private void OnEntitySelected(EntityData entityData)
        {
            entityDetail.SetSelectedEntity(entityData);
        }
    }
}

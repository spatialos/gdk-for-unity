using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEditor;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector
{
    internal class WorkerDetail : VisualElement
    {
        private const string FlagKeyName = "key";
        private const string FlagValueName = "value";

        private readonly TextField workerType;
        private readonly TextField workerId;
        private readonly ListView workerFlags;

        private readonly List<KeyValuePair<string, string>> workerFlagData = new List<KeyValuePair<string, string>>();

        private WorkerSystem workerSystem;

        public WorkerDetail()
        {
            const string uxmlPath = "Packages/io.improbable.gdk.debug/WorkerInspector/Templates/WorkerDetail.uxml";
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            template.CloneTree(this);

            workerType = this.Q<TextField>("worker-type");
            workerType.SetEnabled(false);

            workerId = this.Q<TextField>("worker-id");
            workerId.SetEnabled(false);

            workerFlags = this.Q<ListView>("worker-flags-list");
            workerFlags.makeItem = MakeItem;
            workerFlags.bindItem = BindElement;
            workerFlags.itemsSource = workerFlagData;
            workerFlags.SetEnabled(false);
        }

        public void SetWorld(World world)
        {
            if (world == null)
            {
                workerType.value = "";
                workerId.value = "";
                workerFlagData.Clear();
                workerFlags.Refresh();
                workerSystem = null;
                return;
            }

            workerSystem = world.GetExistingSystem<WorkerSystem>();

            workerType.value = workerSystem.WorkerType;
            workerId.value = workerSystem.WorkerId;
            Update();
        }

        public void Update()
        {
            if (workerSystem == null)
            {
                return;
            }

            workerFlagData.Clear();

            foreach (var pair in workerSystem.WorkerFlags)
            {
                workerFlagData.Add(pair);
            }

            workerFlags.Refresh();
        }

        private static VisualElement MakeItem()
        {
            var container = new VisualElement();
            container.AddToClassList("flag-element-container");

            var key = new Label { name = FlagKeyName, };

            key.AddToClassList("flag-element");

            var value = new Label { name = FlagValueName };

            value.AddToClassList("flag-element");

            container.Add(key);
            container.Add(value);

            return container;
        }

        private void BindElement(VisualElement element, int index)
        {
            var key = element.Q<Label>(FlagKeyName);
            var value = element.Q<Label>(FlagValueName);
            var kvp = workerFlagData[index];

            key.text = kvp.Key;
            value.text = kvp.Value;
        }

        public new class UxmlFactory : UxmlFactory<WorkerDetail>
        {
        }
    }
}

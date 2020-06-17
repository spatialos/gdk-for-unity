using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public abstract class SchemaTypeVisualElement<T> : VisualElement
    {
        public string Label
        {
            get => labelElement.text;
            set => labelElement.text = value;
        }

        protected readonly VisualElement Container;
        private readonly Label labelElement;

        protected SchemaTypeVisualElement(string label)
        {
            AddToClassList("user-defined-type-container");

            labelElement = new Label(label);
            Add(labelElement);

            Container = new VisualElement();
            Container.AddToClassList("user-defined-type-container-data");
            Add(Container);
        }

        public abstract void Update(T data);
    }
}

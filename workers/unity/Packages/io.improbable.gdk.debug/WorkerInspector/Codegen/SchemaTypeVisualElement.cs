using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public abstract class SchemaTypeVisualElement<T> : VisualElement
    {
        protected readonly VisualElement Container;

        protected SchemaTypeVisualElement(string label)
        {
            AddToClassList("user-defined-type-container");

            Add(new Label(label));

            Container = new VisualElement();
            Container.AddToClassList("user-defined-type-container-data");
            Add(Container);
        }

        public abstract void Update(T data);
    }
}

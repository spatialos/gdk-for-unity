using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public abstract class SchemaTypeVisualElement<T> : VisualElement, INotifyValueChanged<T>
    {
        public string Label
        {
            get => labelElement.text;
            set => labelElement.text = value;
        }

        public T value
        {
            get => mValue;
            set
            {
                if (EqualityComparer<T>.Default.Equals(mValue, value))
                {
                    return;
                }

                if (panel == null)
                {
                    SetValueWithoutNotify(value);
                    return;
                }

                using (var pooled = ChangeEvent<T>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }

                SetValueWithoutNotify(value);
            }
        }

        protected readonly VisualElement Container;
        private readonly Label labelElement;
        private T mValue;

        protected SchemaTypeVisualElement(string label)
        {
            AddToClassList("user-defined-type-container");

            labelElement = new Label(label);
            Add(labelElement);

            Container = new VisualElement();
            Container.AddToClassList("user-defined-type-container-data");
            Add(Container);
        }

        protected override void ExecuteDefaultActionAtTarget(EventBase evt)
        {
            base.ExecuteDefaultActionAtTarget(evt);
            if (evt is HideCollectionEvent hideEvent)
            {
                hideEvent.PropagateToChildren(Container);
            }
        }

        protected abstract void Update(T data);

        public void SetValueWithoutNotify(T newValue)
        {
            mValue = newValue;
            Update(newValue);
        }
    }
}

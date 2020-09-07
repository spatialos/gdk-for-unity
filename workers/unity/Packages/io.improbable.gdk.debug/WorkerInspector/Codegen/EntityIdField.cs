using System.Collections.Generic;
using Improbable.Gdk.Core;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public class EntityIdField : VisualElement, INotifyValueChanged<EntityId>
    {
        public string label
        {
            get => inner.label;
            set => inner.label = value;
        }

        public EntityId value
        {
            get => new EntityId(inner.value);
            set
            {
                if (EqualityComparer<EntityId>.Default.Equals(mValue, value))
                {
                    return;
                }

                if (panel == null)
                {
                    SetValueWithoutNotify(value);
                    return;
                }

                using (var pooled = ChangeEvent<EntityId>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }

                SetValueWithoutNotify(value);
            }
        }

        private readonly LongField inner;
        private EntityId mValue;

        public EntityIdField(string label)
        {
            inner = new LongField(label);
            inner.RegisterValueChangedCallback(OnInnerChange);
            Add(inner);
        }

        public void SetValueWithoutNotify(EntityId newValue)
        {
            inner.SetValueWithoutNotify(newValue.Id);
            mValue = newValue;
        }

        private void OnInnerChange(ChangeEvent<long> change)
        {
            value = new EntityId(change.newValue);
        }
    }
}

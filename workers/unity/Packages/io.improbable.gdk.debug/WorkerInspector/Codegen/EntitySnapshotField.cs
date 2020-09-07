using Improbable.Gdk.Core;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    // TODO: Figure out what to do with this one..
    public class EntitySnapshotField : VisualElement, INotifyValueChanged<EntitySnapshot>
    {
        public string label
        {
            get => inner.label;
            set => inner.label = value;
        }

        public EntitySnapshot value
        {
            get => EntitySnapshot.Empty();
            set
            {
                /*
                if (EqualityComparer<EntitySnapshot>.Default.Equals(mValue, value))
                {
                    return;
                }
                */

                if (panel == null)
                {
                    SetValueWithoutNotify(value);
                    return;
                }

                using (var pooled = ChangeEvent<EntitySnapshot>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }

                SetValueWithoutNotify(value);
            }
        }

        private readonly TextField inner;
        private EntitySnapshot mValue;

        public EntitySnapshotField(string label)
        {
            inner = new TextField(label);
            inner.RegisterValueChangedCallback(OnInnerChange);
            Add(inner);
        }

        public void SetValueWithoutNotify(EntitySnapshot newValue)
        {
            inner.SetValueWithoutNotify(newValue.ToString());
            mValue = newValue;
        }

        private void OnInnerChange(ChangeEvent<string> change)
        {
            value = EntitySnapshot.Empty();
        }
    }
}

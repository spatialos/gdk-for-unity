using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public class BytesField : VisualElement, INotifyValueChanged<byte[]>
    {
        public string label
        {
            get => inner.label;
            set => inner.label = value;
        }

        public byte[] value
        {
            get => mValue;
            set
            {
                if (EqualityComparer<byte[]>.Default.Equals(mValue, value))
                {
                    return;
                }

                if (panel == null)
                {
                    SetValueWithoutNotify(value);
                    return;
                }

                using (var pooled = ChangeEvent<byte[]>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }

                SetValueWithoutNotify(value);
            }
        }

        private readonly TextField inner;
        private byte[] mValue;

        public BytesField(string label)
        {
            inner = new TextField(label);
            inner.RegisterValueChangedCallback(OnInnerChange);
            Add(inner);
        }

        public void SetValueWithoutNotify(byte[] newValue)
        {
            inner.SetValueWithoutNotify(System.Text.Encoding.Default.GetString(newValue));
            mValue = newValue;
        }

        private void OnInnerChange(ChangeEvent<string> change)
        {
            value = System.Text.Encoding.Default.GetBytes(change.newValue);
        }
    }
}

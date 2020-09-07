using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public class TypedEnumField<T> : EnumField, INotifyValueChanged<T> where T : struct, IConvertible
    {
        private readonly Func<T, Enum> toEnumClass;
        private T mValue;

        public TypedEnumField(string label, Func<T, Enum> toEnumClass) : base(label)
        {
            this.toEnumClass = toEnumClass;
            ((INotifyValueChanged<Enum>) this).RegisterValueChangedCallback(OnEnumChange);
        }

        public void SetValueWithoutNotify(T newValue)
        {
            mValue = newValue;
            base.SetValueWithoutNotify(toEnumClass(newValue));
        }

        public new T value
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

                using (var pooled = ChangeEvent<Option<T>>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }

                SetValueWithoutNotify(value);
            }
        }

        private void OnEnumChange(ChangeEvent<Enum> changeEvent)
        {
            value = (T) Enum.Parse(typeof(T), changeEvent.newValue.ToString());
        }
    }
}

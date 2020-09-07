using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public class PaginatedMapView<TKeyElement, TKeyData, TValueElement, TValueData> : VisualElement, INotifyValueChanged<Dictionary<TKeyData, TValueData>>
        where TKeyElement : VisualElement, INotifyValueChanged<TKeyData>
        where TValueElement : VisualElement, INotifyValueChanged<TValueData>
    {
        private readonly PaginatedListView<KeyValuePairElement, KeyValuePair<TKeyData, TValueData>> list;
        private readonly List<KeyValuePair<TKeyData, TValueData>> listData = new List<KeyValuePair<TKeyData, TValueData>>();
        private readonly Comparer<TKeyData> comparer = Comparer<TKeyData>.Default;
        private readonly VisualElementConcealer concealer;

        public Dictionary<TKeyData, TValueData> value
        {
            get => mValue;
            set
            {
                // TODO: Will this always be true due to reference equality?
                if (EqualityComparer<Dictionary<TKeyData, TValueData>>.Default.Equals(mValue, value))
                {
                    return;
                }

                if (panel == null)
                {
                    SetValueWithoutNotify(value);
                    return;
                }

                using (var pooled = ChangeEvent<Dictionary<TKeyData, TValueData>>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }

                SetValueWithoutNotify(value);
            }
        }

        private Dictionary<TKeyData, TValueData> mValue;

        public PaginatedMapView(string label, Func<TKeyElement> makeKey, Func<TValueElement> makeValue)
        {
            list = new PaginatedListView<KeyValuePairElement, KeyValuePair<TKeyData, TValueData>>(label,
                () => new KeyValuePairElement(makeKey(), makeValue()),
                (index, kvp, element) => element.SetValueWithoutNotify(kvp));
            list.RegisterValueChangedCallback(OnInnerChange);

            Add(list);
            concealer = new VisualElementConcealer(this);
        }

        public void SetValueWithoutNotify(Dictionary<TKeyData, TValueData> newValue)
        {
            mValue = newValue;
            Update();
        }

        protected override void ExecuteDefaultActionAtTarget(EventBase evt)
        {
            base.ExecuteDefaultActionAtTarget(evt);
            if (evt is HideCollectionEvent hideEvent)
            {
                concealer.HandleSettingChange(hideEvent);
                hideEvent.PropagateToTarget(list);
            }
        }

        private void Update()
        {
            listData.Clear();
            listData.AddRange(mValue);
            listData.Sort((first, second) => comparer.Compare(first.Key, second.Key));

            list.SetValueWithoutNotify(listData);
            concealer.SetVisibility(listData.Count == 0);
        }

        private void OnInnerChange(ChangeEvent<List<KeyValuePair<TKeyData, TValueData>>> change)
        {
            value = change.newValue.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private class KeyValuePairElement : VisualElement, INotifyValueChanged<KeyValuePair<TKeyData, TValueData>>
        {
            private readonly TKeyElement keyElement;
            private readonly TValueElement valueElement;

            public KeyValuePair<TKeyData, TValueData> value
            {
                get => mValue;
                set
                {
                    if (EqualityComparer<KeyValuePair<TKeyData, TValueData>>.Default.Equals(mValue, value))
                    {
                        return;
                    }

                    if (panel == null)
                    {
                        SetValueWithoutNotify(value);
                        return;
                    }

                    using (var pooled = ChangeEvent<KeyValuePair<TKeyData, TValueData>>.GetPooled(mValue, value))
                    {
                        pooled.target = this;
                        SendEvent(pooled);
                    }

                    SetValueWithoutNotify(value);
                }
            }

            private KeyValuePair<TKeyData, TValueData> mValue;

            public KeyValuePairElement(TKeyElement keyElement, TValueElement valueElement)
            {
                this.keyElement = keyElement;
                this.keyElement.RegisterValueChangedCallback(OnKeyChange);
                this.valueElement = valueElement;
                this.valueElement.RegisterValueChangedCallback(OnValueChange);

                Add(keyElement);
                Add(valueElement);

                AddToClassList("map-view__item");
            }

            public void SetValueWithoutNotify(KeyValuePair<TKeyData, TValueData> newValue)
            {
                mValue = newValue;
                Update();
            }

            protected override void ExecuteDefaultActionAtTarget(EventBase evt)
            {
                base.ExecuteDefaultActionAtTarget(evt);
                if (evt is HideCollectionEvent hideEvent)
                {
                    hideEvent.PropagateToTarget(keyElement);
                    hideEvent.PropagateToTarget(valueElement);
                }
            }

            private void Update()
            {
                keyElement.SetValueWithoutNotify(mValue.Key);
                valueElement.SetValueWithoutNotify(mValue.Value);
            }

            private void OnKeyChange(ChangeEvent<TKeyData> change)
            {
                value = new KeyValuePair<TKeyData, TValueData>(change.newValue, value.Value);
            }

            private void OnValueChange(ChangeEvent<TValueData> change)
            {
                value = new KeyValuePair<TKeyData, TValueData>(value.Key, change.newValue);
            }
        }
    }
}

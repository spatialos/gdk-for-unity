using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public class PaginatedMapView<TKeyElement, TKeyData, TValueElement, TValueData> : VisualElement, INotifyValueChanged<Dictionary<TKeyData, TValueData>>
        where TKeyElement : VisualElement
        where TValueElement : VisualElement
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
                if (EqualityComparer<Dictionary<TKeyData, TValueData>>.Default.Equals(mValue, value))
                {
                    return;
                }

                SetValueWithoutNotify(value);

                if (panel == null)
                {
                    return;
                }

                using (var pooled = ChangeEvent<Dictionary<TKeyData, TValueData>>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }
            }
        }

        private Dictionary<TKeyData, TValueData> mValue;

        public PaginatedMapView(string label, Func<TKeyElement> makeKey, Action<TKeyData, TKeyElement> bindKey,
            Func<TValueElement> makeValue, Action<TValueData, TValueElement> bindValue)
        {
            list = new PaginatedListView<KeyValuePairElement, KeyValuePair<TKeyData, TValueData>>(label,
                () => new KeyValuePairElement(makeKey(), makeValue(), bindKey, bindValue),
                (index, kvp, element) => element.SetValueWithoutNotify(kvp));

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

        private class KeyValuePairElement : VisualElement, INotifyValueChanged<KeyValuePair<TKeyData, TValueData>>
        {
            private readonly TKeyElement keyElement;
            private readonly TValueElement valueElement;

            private readonly Action<TKeyData, TKeyElement> bindKey;
            private readonly Action<TValueData, TValueElement> bindValue;

            public KeyValuePair<TKeyData, TValueData> value
            {
                get => mValue;
                set
                {
                    if (EqualityComparer<KeyValuePair<TKeyData, TValueData>>.Default.Equals(mValue, value))
                    {
                        return;
                    }

                    SetValueWithoutNotify(value);

                    if (panel == null)
                    {
                        return;
                    }

                    using (var pooled = ChangeEvent<KeyValuePair<TKeyData, TValueData>>.GetPooled(mValue, value))
                    {
                        pooled.target = this;
                        SendEvent(pooled);
                    }
                }
            }

            private KeyValuePair<TKeyData, TValueData> mValue;

            public KeyValuePairElement(TKeyElement keyElement, TValueElement valueElement,
                Action<TKeyData, TKeyElement> bindKey, Action<TValueData, TValueElement> bindValue)
            {
                this.keyElement = keyElement;
                this.valueElement = valueElement;
                this.bindKey = bindKey;
                this.bindValue = bindValue;

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
                bindKey(mValue.Key, keyElement);
                bindValue(mValue.Value, valueElement);
            }
        }
    }
}

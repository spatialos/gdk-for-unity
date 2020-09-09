using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public class PaginatedMapView<TKeyElement, TKeyData, TValueElement, TValueData> : VisualElement
        where TKeyElement : VisualElement
        where TValueElement : VisualElement
    {
        private readonly PaginatedListView<KeyValuePairElement, KeyValuePair<TKeyData, TValueData>> list;
        private readonly List<KeyValuePair<TKeyData, TValueData>> listData = new List<KeyValuePair<TKeyData, TValueData>>();
        private readonly Comparer<TKeyData> comparer = Comparer<TKeyData>.Default;
        private readonly VisualElementConcealer concealer;

        public PaginatedMapView(string label, Func<uint, TKeyElement> makeKey, Action<TKeyData, TKeyElement> bindKey,
            Func<uint, TValueElement> makeValue, Action<TValueData, TValueElement> bindValue, uint nest)
        {
            list = new PaginatedListView<KeyValuePairElement, KeyValuePair<TKeyData, TValueData>>(label,
                i => new KeyValuePairElement(makeKey(i), makeValue(i), bindKey, bindValue),
                (index, kvp, element) => element.Update(kvp), nest);

            Add(list);
            concealer = new VisualElementConcealer(this);
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

        public void Update(Dictionary<TKeyData, TValueData> data)
        {
            listData.Clear();
            listData.AddRange(data);
            listData.Sort((first, second) => comparer.Compare(first.Key, second.Key));

            list.Update(listData);
            concealer.SetVisibility(listData.Count == 0);
        }

        private class KeyValuePairElement : VisualElement
        {
            private readonly TKeyElement keyElement;
            private readonly TValueElement valueElement;

            private readonly Action<TKeyData, TKeyElement> bindKey;
            private readonly Action<TValueData, TValueElement> bindValue;

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

            protected override void ExecuteDefaultActionAtTarget(EventBase evt)
            {
                base.ExecuteDefaultActionAtTarget(evt);
                if (evt is HideCollectionEvent hideEvent)
                {
                    hideEvent.PropagateToTarget(keyElement);
                    hideEvent.PropagateToTarget(valueElement);
                }
            }

            public void Update(KeyValuePair<TKeyData, TValueData> keyValuePair)
            {
                bindKey(keyValuePair.Key, keyElement);
                bindValue(keyValuePair.Value, valueElement);
            }
        }
    }
}

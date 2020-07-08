using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public class PaginatedMapView<TKeyElement, TKeyData, TValueElement, TValueData> : VisualElement, IConcealable<Dictionary<TKeyData, TValueData>>
        where TKeyElement : VisualElement
        where TValueElement : VisualElement
    {
        private readonly PaginatedListView<KeyValuePairElement, KeyValuePair<TKeyData, TValueData>> list;
        private readonly List<KeyValuePair<TKeyData, TValueData>> listData = new List<KeyValuePair<TKeyData, TValueData>>();
        private readonly Comparer<TKeyData> comparer = Comparer<TKeyData>.Default;

        public PaginatedMapView(string label, Func<TKeyElement> makeKey, Action<TKeyData, TKeyElement> bindKey,
            Func<TValueElement> makeValue, Action<TValueData, TValueElement> bindValue)
        {
            list = new PaginatedListView<KeyValuePairElement, KeyValuePair<TKeyData, TValueData>>(label,
                () => new KeyValuePairElement(makeKey(), makeValue(), bindKey, bindValue),
                (index, kvp, element) => element.Update(kvp));

            Add(list);
        }

        public void SetVisibility(Dictionary<TKeyData, TValueData> data, bool hideIfEmpty)
        {
            if (data.Count == 0 && hideIfEmpty)
            {
                AddToClassList("hidden");
            }
            else
            {
                RemoveFromClassList("hidden");
            }

            Update(data);
            foreach (var i in Enumerable.Range(0, list.childCount))
            {
                if (list.ElementAt(i) is KeyValuePairElement element)
                {
                    element.SetVisibility(listData[i], hideIfEmpty);
                }
            }
        }

        public void Update(Dictionary<TKeyData, TValueData> data)
        {
            listData.Clear();
            listData.AddRange(data);
            listData.Sort((first, second) => comparer.Compare(first.Key, second.Key));

            list.Update(listData);
        }

        private class KeyValuePairElement : VisualElement, IConcealable<KeyValuePair<TKeyData, TValueData>>
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

            public void SetVisibility(KeyValuePair<TKeyData, TValueData> keyValuePair, bool hideIfEmpty)
            {
                if (keyElement is IConcealable<TKeyData> key)
                {
                    key.SetVisibility(keyValuePair.Key, hideIfEmpty);
                }

                if (valueElement is IConcealable<TValueData> value)
                {
                    value.SetVisibility(keyValuePair.Value, hideIfEmpty);
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

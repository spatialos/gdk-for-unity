using System;
using Improbable.Gdk.Core;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public sealed class OptionVisualElement<TElement, TData> : OptionalVisualElementBase<TElement, TData>
        where TElement : VisualElement
    {
        public OptionVisualElement(string label, TElement innerElement, Action<TElement, TData> applyData) : base(label, innerElement, applyData)
        {
        }

        public void Update(Option<TData> data)
        {
            if (data.HasValue)
            {
                UpdateWithData(data.Value);
            }
            else
            {
                UpdateWithoutData();
            }

            SetVisibility(!data.HasValue && HideIfEmpty);
        }
    }

    public sealed class NullableVisualElement<TElement, TData> : OptionalVisualElementBase<TElement, TData>
        where TData : struct
        where TElement : VisualElement
    {
        public NullableVisualElement(string label, TElement innerElement, Action<TElement, TData> applyData) : base(label, innerElement, applyData)
        {
        }

        public void Update(TData? data)
        {
            if (data.HasValue)
            {
                UpdateWithData(data.Value);
            }
            else
            {
                UpdateWithoutData();
            }

            SetVisibility(!data.HasValue && HideIfEmpty);
        }
    }

    public class OptionalVisualElementBase<TElement, TData> : ConcealableElement where TElement : VisualElement
    {
        protected sealed override VisualElement Container { get; }
        private readonly Label isEmptyLabel;
        private readonly Action<TElement, TData> applyData;
        private readonly TElement innerElement;

        protected OptionalVisualElementBase(string label, TElement innerElement, Action<TElement, TData> applyData)
        {
            AddToClassList("user-defined-type-container");
            Add(new Label(label));

            Container = new VisualElement();
            Container.AddToClassList("user-defined-type-container-data");
            Add(Container);

            isEmptyLabel = new Label("Option is empty.");
            isEmptyLabel.AddToClassList("label-empty-option");

            this.innerElement = innerElement;
            this.applyData = applyData;
        }

        protected void UpdateWithData(TData data)
        {
            RemoveIfPresent(isEmptyLabel);
            Container.Add(innerElement);

            applyData(innerElement, data);
        }

        protected void UpdateWithoutData()
        {
            RemoveIfPresent(innerElement);
            Container.Add(isEmptyLabel);
        }

        private void RemoveIfPresent(VisualElement element)
        {
            if (Container.Contains(element))
            {
                Container.Remove(element);
            }
        }
    }
}

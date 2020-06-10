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
        }
    }

    public class OptionalVisualElementBase<TElement, TData> : VisualElement where TElement : VisualElement
    {
        private readonly VisualElement container;
        private readonly TElement innerElement;
        private readonly Label isEmptyLabel;
        private readonly Action<TElement, TData> applyData;

        protected OptionalVisualElementBase(string label, TElement innerElement, Action<TElement, TData> applyData)
        {
            AddToClassList("user-defined-type-container");
            Add(new Label(label));

            container = new VisualElement();
            container.AddToClassList("user-defined-type-container-data");
            Add(container);

            isEmptyLabel = new Label("Option is empty.");
            isEmptyLabel.AddToClassList("label-empty-option");

            this.innerElement = innerElement;
            this.applyData = applyData;
        }

        protected void UpdateWithData(TData data)
        {
            RemoveIfPresent(isEmptyLabel);
            container.Add(innerElement);

            applyData(innerElement, data);
        }

        protected void UpdateWithoutData()
        {
            RemoveIfPresent(innerElement);
            container.Add(isEmptyLabel);
        }

        private void RemoveIfPresent(VisualElement element)
        {
            if (container.Contains(element))
            {
                container.Remove(element);
            }
        }
    }
}

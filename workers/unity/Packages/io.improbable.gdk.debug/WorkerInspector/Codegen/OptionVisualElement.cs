using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public sealed class OptionVisualElement<TElement, TData> : OptionalVisualElementBase<TElement, TData>, INotifyValueChanged<Option<TData>>
        where TElement : VisualElement
    {
        public OptionVisualElement(string label, TElement innerElement, Action<TElement, TData> applyData) : base(label, innerElement, applyData)
        {
        }

        private void Update(Option<TData> data)
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

        public void SetValueWithoutNotify(Option<TData> newValue)
        {
            mValue = newValue;
            Update(newValue);
        }

        public Option<TData> value
        {
            get => mValue;
            set
            {
                if (EqualityComparer<Option<TData>>.Default.Equals(mValue, value))
                {
                    return;
                }

                SetValueWithoutNotify(value);

                if (panel == null)
                {
                    return;
                }

                using (var pooled = ChangeEvent<Option<TData>>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }
            }
        }

        private Option<TData> mValue;
    }

    public sealed class NullableVisualElement<TElement, TData> : OptionalVisualElementBase<TElement, TData>, INotifyValueChanged<TData?>
        where TData : struct
        where TElement : VisualElement
    {
        public NullableVisualElement(string label, TElement innerElement, Action<TElement, TData> applyData) : base(label, innerElement, applyData)
        {
        }

        private void Update(TData? data)
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

        public void SetValueWithoutNotify(TData? newValue)
        {
            mValue = newValue;
            Update(newValue);
        }

        public TData? value
        {
            get => mValue;
            set
            {
                if (EqualityComparer<TData?>.Default.Equals(mValue, value))
                {
                    return;
                }

                SetValueWithoutNotify(value);

                if (panel == null)
                {
                    return;
                }

                using (var pooled = ChangeEvent<TData?>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }
            }
        }

        private TData? mValue;
    }

    public class OptionalVisualElementBase<TElement, TData> : VisualElement where TElement : VisualElement
    {
        private readonly VisualElement container;
        private readonly TElement innerElement;
        private readonly Label isEmptyLabel;
        private readonly Action<TElement, TData> applyData;
        private readonly VisualElementConcealer concealer;

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
            concealer = new VisualElementConcealer(this);
        }

        protected override void ExecuteDefaultActionAtTarget(EventBase evt)
        {
            base.ExecuteDefaultActionAtTarget(evt);
            if (evt is HideCollectionEvent hideEvent)
            {
                concealer.HandleSettingChange(hideEvent);
                hideEvent.PropagateToTarget(innerElement);
            }
        }

        protected void UpdateWithData(TData data)
        {
            RemoveIfPresent(isEmptyLabel);
            container.Add(innerElement);

            applyData(innerElement, data);
            concealer.SetVisibility(false);
        }

        protected void UpdateWithoutData()
        {
            RemoveIfPresent(innerElement);
            container.Add(isEmptyLabel);
            concealer.SetVisibility(true);
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

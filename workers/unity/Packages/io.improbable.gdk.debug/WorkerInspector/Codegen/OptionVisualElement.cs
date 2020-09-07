using System.Collections.Generic;
using Improbable.Gdk.Core;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public sealed class OptionVisualElement<TElement, TData> : OptionalVisualElementBase<TElement, TData>, INotifyValueChanged<Option<TData>>
        where TElement : VisualElement, INotifyValueChanged<TData>
    {
        public Option<TData> value
        {
            get => mValue;
            set
            {
                if (EqualityComparer<Option<TData>>.Default.Equals(mValue, value))
                {
                    return;
                }

                if (panel == null)
                {
                    SetValueWithoutNotify(value);
                    return;
                }

                using (var pooled = ChangeEvent<Option<TData>>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }

                SetValueWithoutNotify(value);
            }
        }

        private Option<TData> mValue;

        public OptionVisualElement(string label, TElement innerElement) : base(label, innerElement)
        {
        }

        public void SetValueWithoutNotify(Option<TData> newValue)
        {
            mValue = newValue;
            Update(newValue);
        }

        protected override void OnInnerChange(ChangeEvent<TData> changeEvent)
        {
            value = changeEvent.newValue;
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
    }

    public sealed class NullableVisualElement<TElement, TData> : OptionalVisualElementBase<TElement, TData>, INotifyValueChanged<TData?>
        where TData : struct
        where TElement : VisualElement, INotifyValueChanged<TData>
    {
        public TData? value
        {
            get => mValue;
            set
            {
                if (EqualityComparer<TData?>.Default.Equals(mValue, value))
                {
                    return;
                }

                if (panel == null)
                {
                    SetValueWithoutNotify(value);
                    return;
                }

                using (var pooled = ChangeEvent<TData?>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }

                SetValueWithoutNotify(value);
            }
        }

        private TData? mValue;

        public NullableVisualElement(string label, TElement innerElement) : base(label, innerElement)
        {
        }

        public void SetValueWithoutNotify(TData? newValue)
        {
            mValue = newValue;
            Update(newValue);
        }

        protected override void OnInnerChange(ChangeEvent<TData> changeEvent)
        {
            value = changeEvent.newValue;
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
    }

    public abstract class OptionalVisualElementBase<TElement, TData> : VisualElement
        where TElement : VisualElement, INotifyValueChanged<TData>
    {
        private readonly VisualElement container;
        private readonly TElement innerElement;
        private readonly Label isEmptyLabel;
        private readonly VisualElementConcealer concealer;

        protected OptionalVisualElementBase(string label, TElement innerElement)
        {
            AddToClassList("user-defined-type-container");
            Add(new Label(label));

            container = new VisualElement();
            container.AddToClassList("user-defined-type-container-data");
            Add(container);

            isEmptyLabel = new Label("Option is empty.");
            isEmptyLabel.AddToClassList("label-empty-option");

            this.innerElement = innerElement;
            innerElement.RegisterValueChangedCallback(OnInnerChange);

            concealer = new VisualElementConcealer(this);
        }

        protected abstract void OnInnerChange(ChangeEvent<TData> changeEvent);

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

            innerElement.SetValueWithoutNotify(data);
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

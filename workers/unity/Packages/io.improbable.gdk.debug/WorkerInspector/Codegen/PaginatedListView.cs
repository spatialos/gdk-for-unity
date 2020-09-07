using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public class PaginatedListView<TElement, TData> : VisualElement, INotifyValueChanged<List<TData>>
        where TElement : VisualElement, INotifyValueChanged<TData>
    {
        private const string UxmlPath =
            "Packages/io.improbable.gdk.debug/WorkerInspector/Templates/PaginatedListView.uxml";

        public List<TData> value
        {
            get => mValue;
            set
            {
                // TODO: Will this always be true due to reference equality?
                if (EqualityComparer<List<TData>>.Default.Equals(mValue, value))
                {
                    return;
                }

                SetValueWithoutNotify(value);

                if (panel == null)
                {
                    return;
                }

                // NOTE: Due to copy-by-reference, the previous value and the new value will always be the same.
                using (var pooled = ChangeEvent<List<TData>>.GetPooled(mValue, value))
                {
                    pooled.target = this;
                    SendEvent(pooled);
                }
            }
        }

        private List<TData> mValue;

        // TODO: Can I get rid of this and use `INotifyValueChanged<TData>.SetValueWithoutNotify` instead?
        private readonly Action<int, TData, TElement> bindElement;
        private readonly VisualElement container;
        private readonly ElementPool<TElement> elementPool;
        private readonly Dictionary<TElement, int> elementToIndex = new Dictionary<TElement, int>();
        private readonly int elementsPerPage;
        private readonly VisualElementConcealer concealer;

        private readonly VisualElement controlsContainer;
        private readonly Button forwardButton;
        private readonly Button backButton;
        private readonly Label pageCounter;

        private int currentPage = 0;
        private int numPages = 0;

        public PaginatedListView(string label, Func<TElement> makeElement, Action<int, TData, TElement> bindElement, int elementsPerPage = 5)
        {
            this.bindElement = bindElement;
            this.elementsPerPage = elementsPerPage;

            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
            template.CloneTree(this);

            this.Q<Label>(name: "list-name").text = label;
            container = this.Q<VisualElement>(className: "user-defined-type-container-data");

            controlsContainer = this.Q<VisualElement>(className: "paginated-list-controls");
            pageCounter = this.Q<Label>(name: "page-counter");

            backButton = this.Q<Button>(name: "back-button");
            backButton.clickable.clicked += () => ChangePageCount(-1);

            forwardButton = this.Q<Button>(name: "forward-button");
            forwardButton.clickable.clicked += () => ChangePageCount(1);

            elementPool = new ElementPool<TElement>(makeElement);

            concealer = new VisualElementConcealer(this);
        }

        protected override void ExecuteDefaultActionAtTarget(EventBase evt)
        {
            base.ExecuteDefaultActionAtTarget(evt);
            if (evt is HideCollectionEvent hideEvent)
            {
                concealer.HandleSettingChange(hideEvent);
                hideEvent.PropagateToChildren(container);
            }
        }

        private void Update()
        {
            if (mValue.Count == 0)
            {
                controlsContainer.AddToClassList("hidden");
                concealer.SetVisibility(true);
            }
            else
            {
                controlsContainer.RemoveFromClassList("hidden");
                concealer.SetVisibility(false);
            }

            CalculatePages();
            RefreshView();
        }

        internal void ChangePageCount(int diff)
        {
            currentPage += diff;
            currentPage = Mathf.Clamp(currentPage, 0, numPages - 1);
            CalculatePages();
            RefreshView();
        }

        private void CalculatePages()
        {
            numPages = Mathf.CeilToInt((float) mValue.Count / elementsPerPage);
            numPages = Mathf.Clamp(numPages, 1, numPages);
            currentPage = Mathf.Clamp(currentPage, 0, numPages - 1);

            pageCounter.text = $"{currentPage + 1}/{numPages}";
        }

        private void RefreshView()
        {
            // Calculate slice of list to be rendered.
            var firstIndex = currentPage * elementsPerPage;
            var length = Math.Min(elementsPerPage, mValue.Count - firstIndex);

            // If the child count is the same, don't adjust it.
            // If the child count is less, add the requisite number.
            // If the child count is more, pop elements off the end.

            var diff = container.childCount - length;

            if (diff > 0)
            {
                for (var i = 0; i < diff; i++)
                {
                    var element = (TElement) container.ElementAt(container.childCount - 1);
                    container.RemoveAt(container.childCount - 1);
                    elementPool.Return(element);
                }
            }
            else if (diff < 0)
            {
                for (var i = diff; i < 0; i++)
                {
                    var element = elementPool.GetOrCreate();
                    element.RegisterValueChangedCallback(change => OnElementChange(element, change));
                    container.Add(element);
                }
            }

            // At this point, container.Children() has the same length as the slice.
            var elementIndex = firstIndex;
            foreach (var child in container.Children())
            {
                var element = (TElement) child;
                elementToIndex[element] = elementIndex;
                bindElement(elementIndex, mValue[elementIndex], element);
                elementIndex++;
            }

            backButton.SetEnabled(currentPage != 0);
            forwardButton.SetEnabled(currentPage != numPages - 1);
        }

        public void SetValueWithoutNotify(List<TData> newValue)
        {
            mValue = newValue;
            Update();
        }

        private void OnElementChange(TElement element, ChangeEvent<TData> change)
        {
            var index = elementToIndex[element];
            value[index] = change.newValue;

            // Trigger change event.
            value = value;
        }
    }

    internal class ElementPool<TElement> where TElement : VisualElement
    {
        private readonly Stack<TElement> pool = new Stack<TElement>();
        private readonly Func<TElement> makeElement;

        public ElementPool(Func<TElement> makeElement)
        {
            this.makeElement = makeElement;
        }

        public TElement GetOrCreate()
        {
            return pool.Count == 0 ? makeElement() : pool.Pop();
        }

        public void Return(TElement element)
        {
            pool.Push(element);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public class PaginatedListView<TElement, TData> : VisualElement
        where TElement : VisualElement
    {
        private const string UxmlPath =
            "Packages/io.improbable.gdk.debug/WorkerInspector/Templates/PaginatedListView.uxml";

        private List<TData> data;

        private readonly Action<int, TData, TElement> bindElement;
        private readonly VisualElement container;
        private readonly ElementPool<TElement> elementPool;
        private readonly int elementsPerPage;
        private readonly VisualElementConcealer concealer;

        private readonly VisualElement controlsContainer;
        private readonly Button forwardButton;
        private readonly Button backButton;
        private readonly Label pageCounter;
        private readonly uint nestingLevel;

        private int currentPage = 0;
        private int numPages = 0;

        public PaginatedListView(string label, Func<uint, TElement> makeElement, Action<int, TData, TElement> bindElement, uint nest = 0, int elementsPerPage = 5)
        {
            nestingLevel = nest;
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

            elementPool = new ElementPool<TElement>(makeElement, nest);

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

        public void Update(List<TData> newData)
        {
            data = newData;

            if (data.Count == 0)
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
            numPages = Mathf.CeilToInt((float) data.Count / elementsPerPage);
            numPages = Mathf.Clamp(numPages, 1, numPages);
            currentPage = Mathf.Clamp(currentPage, 0, numPages - 1);

            pageCounter.text = $"{currentPage + 1}/{numPages}";
        }

        private void RefreshView()
        {
            // Calculate slice of list to be rendered.
            var firstIndex = currentPage * elementsPerPage;
            var length = Math.Min(elementsPerPage, data.Count - firstIndex);

            // If the child count is the same, don't adjust it.
            // If the child count is less, add the requisite number.
            // If the child count is more, pop elements off the end.

            var diff = container.childCount - length;

            if (diff > 0)
            {
                for (var i = 0; i < diff; i++)
                {
                    var element = container.ElementAt(container.childCount - 1);
                    container.RemoveAt(container.childCount - 1);
                    elementPool.Return((TElement) element);
                }
            }
            else if (diff < 0)
            {
                for (var i = diff; i < 0; i++)
                {
                    container.Add(elementPool.GetOrCreate());
                }
            }

            // At this point, container.Children() has the same length as the slice.
            var elementIndex = firstIndex;
            foreach (var child in container.Children())
            {
                bindElement(elementIndex, data[elementIndex], (TElement) child);
                elementIndex++;
            }

            backButton.SetEnabled(currentPage != 0);
            forwardButton.SetEnabled(currentPage != numPages - 1);
        }
    }

    internal class ElementPool<TElement> where TElement : VisualElement
    {
        private readonly Stack<TElement> pool = new Stack<TElement>();
        private readonly Func<uint, TElement> makeElement;
        private readonly uint nestingLevel;

        public ElementPool(Func<uint, TElement> makeElement, uint nest = 0)
        {
            nestingLevel = nest;
            this.makeElement = makeElement;
        }

        public TElement GetOrCreate()
        {
            return pool.Count == 0 ? makeElement(nestingLevel + 1) : pool.Pop();
        }

        public void Return(TElement element)
        {
            pool.Push(element);
        }
    }
}

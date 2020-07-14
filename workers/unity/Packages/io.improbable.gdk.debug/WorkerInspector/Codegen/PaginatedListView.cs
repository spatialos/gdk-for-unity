using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public class PaginatedListView<TElement, TData> : ConcealableElement
        where TElement : VisualElement
    {
        private const string UxmlPath =
            "Packages/io.improbable.gdk.debug/WorkerInspector/Templates/PaginatedListView.uxml";

        private List<TData> data;

        protected sealed override VisualElement Container { get; }
        private readonly Action<int, TData, TElement> bindElement;
        private readonly ElementPool<TElement> elementPool;
        private readonly int elementsPerPage;

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
            Container = this.Q<VisualElement>(className: "user-defined-type-container-data");

            controlsContainer = this.Q<VisualElement>(className: "paginated-list-controls");
            pageCounter = this.Q<Label>(name: "page-counter");

            backButton = this.Q<Button>(name: "back-button");
            backButton.clickable.clicked += () => ChangePageCount(-1);

            forwardButton = this.Q<Button>(name: "forward-button");
            forwardButton.clickable.clicked += () => ChangePageCount(1);

            elementPool = new ElementPool<TElement>(makeElement);
        }

        public void Update(List<TData> newData)
        {
            data = newData;

            if (data.Count == 0)
            {
                controlsContainer.AddToClassList("hidden");
            }
            else
            {
                controlsContainer.RemoveFromClassList("hidden");
            }

            CalculatePages();
            RefreshView();
            SetVisibility(data.Count == 0 && HideIfEmpty);
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

            var diff = Container.childCount - length;

            if (diff > 0)
            {
                for (var i = 0; i < diff; i++)
                {
                    var element = Container.ElementAt(Container.childCount - 1);
                    Container.RemoveAt(Container.childCount - 1);
                    elementPool.Return((TElement) element);
                }
            }
            else if (diff < 0)
            {
                for (var i = diff; i < 0; i++)
                {
                    Container.Add(elementPool.GetOrCreate());
                }
            }

            // At this point, container.Children() has the same length as the slice.
            var elementIndex = firstIndex;
            foreach (var child in Container.Children())
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

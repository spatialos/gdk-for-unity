using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen.EditmodeTests
{
    [TestFixture]
    public class PaginatedListViewTests
    {
        [Test]
        public void Only_up_to_elementsPerPage_are_rendered_at_once()
        {
            var data = GetDummyData(10);
            var observer = new PaginatedListViewObserver(5);

            observer.UpdateData(data);

            var elements = observer.VisibleElements.ToList();

            Assert.AreEqual(5, elements.Count);
        }

        [Test]
        public void Correct_list_slice_is_rendered_in_order()
        {
            var data = GetDummyData(10);
            var observer = new PaginatedListViewObserver(5);

            observer.UpdateData(data);

            var elements = observer.VisibleElements.ToList();

            for (var i = 0; i < elements.Count; i++)
            {
                Assert.AreEqual(i, ((PaginatedListViewObserver.DummyElement) elements[i]).Index);
            }
        }

        [Test]
        public void Forward_button_is_disabled_when_no_more_pages()
        {
            var data = GetDummyData(5);
            var observer = new PaginatedListViewObserver(5);
            observer.UpdateData(data);
            Assert.IsFalse(observer.IsPageForwardButtonEnabled);
        }

        [Test]
        public void Back_button_is_disabled_when_no_more_pages()
        {
            var data = GetDummyData(5);
            var observer = new PaginatedListViewObserver(5);
            observer.UpdateData(data);
            Assert.IsFalse(observer.IsPageBackButtonEnabled);
        }

        [Test]
        public void Forward_button_is_enabled_if_more_pages()
        {
            var data = GetDummyData(10);
            var observer = new PaginatedListViewObserver(5);
            observer.UpdateData(data);
            Assert.IsTrue(observer.IsPageForwardButtonEnabled);
        }

        [Test]
        public void Back_button_is_enabled_if_more_pages()
        {
            var data = GetDummyData(10);
            var observer = new PaginatedListViewObserver(5);
            observer.UpdateData(data);
            observer.TryPageForward();
            Assert.IsTrue(observer.IsPageBackButtonEnabled);
        }

        [Test]
        public void Paging_forward_updates_rendered_data()
        {
            var data = GetDummyData(15);
            var observer = new PaginatedListViewObserver(5);

            observer.UpdateData(data);
            observer.TryPageForward();

            var elements = observer.VisibleElements.ToList();

            for (var i = 0; i < elements.Count; i++)
            {
                var expectedIndex = i + 5; // We expect to be on the second page.
                Assert.AreEqual(expectedIndex, ((PaginatedListViewObserver.DummyElement) elements[i]).Index);
            }
        }

        [Test]
        public void Paging_backwards_updates_rendered_data()
        {
            var data = GetDummyData(15);
            var observer = new PaginatedListViewObserver(5);

            observer.UpdateData(data);
            observer.TryPageForward();
            observer.TryPageBackwards();

            var elements = observer.VisibleElements.ToList();

            for (var i = 0; i < elements.Count; i++)
            {
                Assert.AreEqual(i, ((PaginatedListViewObserver.DummyElement) elements[i]).Index);
            }
        }

        [Test]
        public void Sub_slice_of_list_can_be_rendered()
        {
            var data = GetDummyData(8);
            var observer = new PaginatedListViewObserver(5);

            observer.UpdateData(data);
            observer.TryPageForward();

            var elements = observer.VisibleElements.ToList();

            Assert.AreEqual(3, elements.Count);

            for (var i = 0; i < elements.Count; i++)
            {
                var expectedIndex = i + 5; // We expect to be on the second page.
                Assert.AreEqual(expectedIndex, ((PaginatedListViewObserver.DummyElement) elements[i]).Index);
            }
        }

        private class PaginatedListViewObserver
        {
            public bool IsPageForwardButtonEnabled => listView.Q<Button>(name: "forward-button").enabledSelf;
            public bool IsPageBackButtonEnabled => listView.Q<Button>(name: "back-button").enabledSelf;

            public IEnumerable<VisualElement> VisibleElements =>
                listView
                    .Q<VisualElement>(className: "user-defined-type-container-data")
                    .Children();

            private readonly PaginatedListView<DummyElement, int> listView;

            public PaginatedListViewObserver(int elementsPerPage)
            {
                listView = new PaginatedListView<DummyElement, int>("My List",
                    () => new DummyElement(),
                    (index, _, element) =>
                    {
                        element.Index = index;
                    }, elementsPerPage);
            }

            public void TryPageForward()
            {
                listView.ChangePageCount(1);
            }

            public void TryPageBackwards()
            {
                listView.ChangePageCount(-1);
            }

            public void UpdateData(List<int> data)
            {
                listView.SetValueWithoutNotify(data);
            }

            public class DummyElement : VisualElement
            {
                public int Index;
            }
        }

        private static List<int> GetDummyData(int count)
        {
            return Enumerable.Repeat(0, count).ToList();
        }
    }
}

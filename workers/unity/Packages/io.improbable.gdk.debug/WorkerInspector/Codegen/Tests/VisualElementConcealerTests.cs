using NUnit.Framework;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen.EditmodeTests
{
    [TestFixture]
    public class VisualElementConcealerTests
    {
        [TestCase(false)]
        [TestCase(true)]
        public void HideCollectionEvent_sets_HideIfEmpty(bool value)
        {
            var vis = new VisualElement();
            var stub = new VisualElementConcealer(vis);
            stub.HandleSettingChange(new HideCollectionEvent().WithValue(value).WithTarget(vis));
            Assert.AreEqual(value, stub.HideIfEmpty);
        }

        [Test]
        public void SetVisibility_adds_hidden_when_HideIfEmpty_and_IsEmptyAreTrue()
        {
            var vis = new VisualElement();
            var stub = new VisualElementConcealer(vis);
            stub.HandleSettingChange(new HideCollectionEvent().WithValue(true).WithTarget(vis));
            stub.SetVisibility(true);
            Assert.IsTrue(vis.ClassListContains("hidden"));
        }

        [Test]
        public void SetVisibility_removes_hidden_when_HideIfEmptyIsFalse_and_IsEmptyIsTrue()
        {
            var vis = new VisualElement();
            vis.AddToClassList("hidden");
            var stub = new VisualElementConcealer(vis);
            stub.HandleSettingChange(new HideCollectionEvent().WithValue(false).WithTarget(vis));
            stub.SetVisibility(true);
            Assert.IsFalse(vis.ClassListContains("hidden"));
        }

        [Test]
        public void SetVisibility_removes_hidden_when_HideIfEmptyIsTrue_and_IsEmptyIsFalse()
        {
            var vis = new VisualElement();
            vis.AddToClassList("hidden");
            var stub = new VisualElementConcealer(vis);
            stub.HandleSettingChange(new HideCollectionEvent().WithValue(true).WithTarget(vis));
            stub.SetVisibility(false);
            Assert.IsFalse(vis.ClassListContains("hidden"));
        }

        [Test]
        public void SetVisibility_removes_hidden_when_HideIfEmpty_and_IsEmptyAreFalse()
        {
            var vis = new VisualElement();
            vis.AddToClassList("hidden");
            var stub = new VisualElementConcealer(vis);
            stub.HandleSettingChange(new HideCollectionEvent().WithValue(false).WithTarget(vis));
            stub.SetVisibility(false);
            Assert.IsFalse(vis.ClassListContains("hidden"));
        }
    }
}

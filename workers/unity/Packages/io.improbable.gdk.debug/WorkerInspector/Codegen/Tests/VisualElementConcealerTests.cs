using NUnit.Framework;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen.EditmodeTests
{
    [TestFixture]
    public class VisualElementConcealerTests
    {
        [TestCase(false)]
        [TestCase(true)]
        public void HideCollectionEventSetsHideProperty(bool value)
        {
            var vis = new VisualElement();
            var stub = new VisualElementConcealer(vis);
            stub.HandleSettingChange(new HideCollectionEvent().WithValue(value).WithTarget(vis));
            Assert.AreEqual(value, stub.HideIfEmpty);
        }

        public void ConcealerHidesElementIfHideSettingAndIsHiddenIsTrue()
        {
            var vis = new VisualElement();
            var stub = new VisualElementConcealer(vis);
            stub.HandleSettingChange(new HideCollectionEvent().WithValue(true).WithTarget(vis));
            stub.SetVisibility(true);
            Assert.True(vis.ClassListContains("hidden"));
        }

        public void ConcealerUnhidesElementIfHideSettingIsFalseAndIsHiddenIsTrue()
        {
            var vis = new VisualElement();
            vis.AddToClassList("hidden");
            var stub = new VisualElementConcealer(vis);
            stub.HandleSettingChange(new HideCollectionEvent().WithValue(false).WithTarget(vis));
            stub.SetVisibility(true);
            Assert.False(vis.ClassListContains("hidden"));
        }

        public void ConcealerUnhidesElementIfHideSettingIsTrueAndIsHiddenIsFalse()
        {
            var vis = new VisualElement();
            vis.AddToClassList("hidden");
            var stub = new VisualElementConcealer(vis);
            stub.HandleSettingChange(new HideCollectionEvent().WithValue(true).WithTarget(vis));
            stub.SetVisibility(false);
            Assert.False(vis.ClassListContains("hidden"));
        }

        public void ConcealerUnhidesElementIfHideSettingAndIsHiddenIsFalse()
        {
            var vis = new VisualElement();
            vis.AddToClassList("hidden");
            var stub = new VisualElementConcealer(vis);
            stub.HandleSettingChange(new HideCollectionEvent().WithValue(false).WithTarget(vis));
            stub.SetVisibility(false);
            Assert.False(vis.ClassListContains("hidden"));
        }
    }
}

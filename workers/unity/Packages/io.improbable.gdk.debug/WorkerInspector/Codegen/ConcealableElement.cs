using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public abstract class ConcealableElement : VisualElement
    {
        protected abstract VisualElement Container { get; }

        protected override void ExecuteDefaultActionAtTarget(EventBase eventBase)
        {
            base.ExecuteDefaultActionAtTarget(eventBase);
            if (!(eventBase is HideCollectionEvent hideEvent))
            {
                return;
            }

            HideIfEmpty = hideEvent.HideIfEmpty;
            hideEvent.PropagateToChildren(Container);
        }

        protected bool HideIfEmpty { get; private set; }

        protected void SetVisibility(bool isHidden)
        {
            if (isHidden)
            {
                AddToClassList("hidden");
            }
            else
            {
                RemoveFromClassList("hidden");
            }
        }
    }
}

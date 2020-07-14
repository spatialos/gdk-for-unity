using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public class HideCollectionEvent : EventBase<HideCollectionEvent>
    {
        public bool HideIfEmpty { get; private set; }

        private HideCollectionEvent WithValue(bool value)
        {
            HideIfEmpty = value;
            return this;
        }

        private HideCollectionEvent WithTarget(IEventHandler handler)
        {
            target = handler;
            return this;
        }

        public void PropagateToTarget(IEventHandler handler)
        {
            SendToTarget(HideIfEmpty, handler);
        }

        public static void SendToTarget(bool value, IEventHandler target)
        {
            using (var evt = GetPooled().WithValue(value).WithTarget(target))
            {
                target.SendEvent(evt);
            }
        }

        public void PropagateToChildren(VisualElement parent)
        {
            if (parent == null)
            {
                return;
            }

            foreach (var child in parent.Children())
            {
                PropagateToTarget(child);
            }
        }
    }
}

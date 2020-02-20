// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using Improbable.Gdk.Core;

namespace Improbable.DependentSchema
{
    public partial class DependentDataComponent
    {
        public class DiffComponentStorage : DiffComponentStorage<Update>
            , IDiffEventStorage<FooEvent.Event>
        {
            private MessageList<ComponentEventReceived<FooEvent.Event>> fooEventEventStorage =
                new MessageList<ComponentEventReceived<FooEvent.Event>>();

            private readonly EventComparer<FooEvent.Event> fooEventComparer =
                new EventComparer<FooEvent.Event>();

            public override Type[] GetEventTypes()
            {
                return new Type[]
                {
                    typeof(FooEvent.Event)
                };
            }

            public override void Clear()
            {
                base.Clear();

                fooEventEventStorage.Clear();
            }

            protected override void ClearEventStorage(long entityId)
            {
                fooEventEventStorage.RemoveAll(change => change.EntityId.Id == entityId);
            }

            MessagesSpan<ComponentEventReceived<FooEvent.Event>> IDiffEventStorage<FooEvent.Event>.GetEvents(EntityId entityId)
            {
                var (firstIndex, count) = fooEventEventStorage.GetEntityRange(entityId);
                return fooEventEventStorage.Slice(firstIndex, count);
            }

            MessagesSpan<ComponentEventReceived<FooEvent.Event>> IDiffEventStorage<FooEvent.Event>.GetEvents()
            {
                return fooEventEventStorage.Slice();
            }

            void IDiffEventStorage<FooEvent.Event>.AddEvent(ComponentEventReceived<FooEvent.Event> ev)
            {
                fooEventEventStorage.InsertSorted(ev, fooEventComparer);
            }
        }
    }
}

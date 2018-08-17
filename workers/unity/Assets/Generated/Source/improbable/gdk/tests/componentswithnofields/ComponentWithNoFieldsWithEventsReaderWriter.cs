
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithEvents
    {
        [ComponentId(1004)]
        internal class ReaderWriterCreator : IReaderWriterCreator
        {
            public IReaderWriterInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [ReaderInterface]
        [ComponentId(1004)]
        public interface Reader : IReader<SpatialOSComponentWithNoFieldsWithEvents, SpatialOSComponentWithNoFieldsWithEvents.Update>
        {
            event Action<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> OnEvt;
        }

        [WriterInterface]
        [ComponentId(1004)]
        public interface Writer : IWriter<SpatialOSComponentWithNoFieldsWithEvents, SpatialOSComponentWithNoFieldsWithEvents.Update>
        {
            void SendEvt( global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload);
        }

        internal class ReaderWriterImpl :
            ReaderWriterBase<SpatialOSComponentWithNoFieldsWithEvents, SpatialOSComponentWithNoFieldsWithEvents.Update>, Reader, Writer
        {
            public ReaderWriterImpl(Entity entity,EntityManager entityManager,ILogDispatcher logDispatcher)
                : base(entity, entityManager, logDispatcher)
            {
            }

            protected override void TriggerFieldCallbacks(SpatialOSComponentWithNoFieldsWithEvents.Update update)
            {
            }
            protected override void ApplyUpdate(SpatialOSComponentWithNoFieldsWithEvents.Update update, ref SpatialOSComponentWithNoFieldsWithEvents data)
            {
            }

            private readonly List<Action<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>> evtDelegates = new System.Collections.Generic.List<System.Action<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>>();

            public event Action<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> OnEvt
            {
                add => evtDelegates.Add(value);
                remove => evtDelegates.Remove(value);
            }

            public void OnEvtEvent(global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload)
            {
                DispatchEventWithErrorHandling(payload, evtDelegates);
            }

            public void SendEvt(global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload)
            {
                var sender = EntityManager.GetComponentData<EventSender.Evt>(Entity);
                sender.Events.Add(payload);
            }
        }
    }
}

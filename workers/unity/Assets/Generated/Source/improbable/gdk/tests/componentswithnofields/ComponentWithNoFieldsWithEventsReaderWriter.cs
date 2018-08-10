
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Gdk.Core.MonoBehaviours;
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
        public interface Reader : IReader<SpatialOSComponentWithNoFieldsWithEvents>
        {
            event Action<EvtEvent> OnEvt;
        }

        [WriterInterface]
        [ComponentId(1004)]
        public interface Writer : IWriter<SpatialOSComponentWithNoFieldsWithEvents>
        {
        }

        internal class ReaderWriterImpl : ReaderWriterBase<SpatialOSComponentWithNoFieldsWithEvents>, Reader, Writer
        {
            public ReaderWriterImpl(Entity entity,EntityManager entityManager,ILogDispatcher logDispatcher)
                : base(entity, entityManager, logDispatcher)
            {
            }

            private readonly List<Action<EvtEvent>> evtDelegates = new System.Collections.Generic.List<System.Action<EvtEvent>>();

            public event Action<EvtEvent> OnEvt
            {
                add => evtDelegates.Add(value);
                remove => evtDelegates.Remove(value);
            }

            public void OnEvtEvent(EvtEvent payload)
            {
                DispatchEventWithErrorHandling(payload, evtDelegates);
            }
        }
    }
}

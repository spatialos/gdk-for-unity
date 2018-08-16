
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
        [InjectableId(InjectableType.ReaderWriter, 1004)]
        internal class ReaderWriterCreator : IReaderWriterCreator
        {
            public IInjectable CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [InjectableId(InjectableType.ReaderWriter, 1004)]
        [InjectionCondition(InjectionCondition.RequireComponentToRead)]
        public interface Reader : IReader<SpatialOSComponentWithNoFieldsWithEvents, SpatialOSComponentWithNoFieldsWithEvents.Update>
        {
            event Action<EvtEvent> OnEvt;
        }

        [InjectableId(InjectableType.ReaderWriter, 1004)]
        [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
        public interface Writer : IWriter<SpatialOSComponentWithNoFieldsWithEvents, SpatialOSComponentWithNoFieldsWithEvents.Update>
        {
            void SendEvt( global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload);
        }

        internal class ReaderWriterImpl :
            BlittableReaderWriterBase<SpatialOSComponentWithNoFieldsWithEvents, SpatialOSComponentWithNoFieldsWithEvents.Update>, Reader, Writer
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

            public void SendEvt( global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload)
            {
                var sender = EntityManager.GetComponentData<EventSender<SpatialOSComponentWithNoFieldsWithEvents>>(Entity);
                sender.SendEvtEvent(payload);
            }
        }
    }
}

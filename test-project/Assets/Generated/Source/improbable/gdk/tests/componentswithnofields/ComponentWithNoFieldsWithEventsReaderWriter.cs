
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithEvents
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 1004)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 1004)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Component, Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Update>
            {
                event Action<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> OnEvt;
            }

            [InjectableId(InjectableType.ReaderWriter, 1004)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : IWriter<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Component, Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Update>
            {
                void SendEvt( global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload);
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Component, Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                protected override void TriggerFieldCallbacks(Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Update update)
                {
                }

                protected override void ApplyUpdate(Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Update update, ref Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithEvents.Component data)
                {
                }

                private readonly List<Action<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>> EvtDelegates = new List<Action<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>>();

                public event Action<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> OnEvt
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        EvtDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        EvtDelegates.Remove(value);
                    }
                }

                public void OnEvtEvent(global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(payload, EvtDelegates, logDispatcher);
                }

                public void SendEvt(global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty payload)
                {
                    if (!VerifyNotDisposed())
                    {
                        return;
                    }

                    var sender = EntityManager.GetComponentData<EventSender.Evt>(Entity);
                    sender.Events.Add(payload);
                }
            }
        }
    }
}

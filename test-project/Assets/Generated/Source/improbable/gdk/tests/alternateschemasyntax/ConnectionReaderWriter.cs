
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Tests.AlternateSchemaSyntax
{
    public partial class Connection
    {
        public partial class Requirable
        {
            [InjectableId(InjectableType.ReaderWriter, 1105)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 1105)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component, Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>
            {
                event Action<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> OnMyEvent;
            }

            [InjectableId(InjectableType.ReaderWriter, 1105)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : Reader, IWriter<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component, Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>
            {
                void SendMyEvent( global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType payload);
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component, Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                protected override void TriggerFieldCallbacks(Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update update)
                {
                }

                protected override void ApplyUpdate(Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update update, ref Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component data)
                {
                }

                private readonly List<Action<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>> MyEventDelegates = new List<Action<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>>();

                public event Action<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> OnMyEvent
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        MyEventDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        MyEventDelegates.Remove(value);
                    }
                }

                public void OnMyEventEvent(global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType payload)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(payload, MyEventDelegates, logDispatcher);
                }

                public void SendMyEvent(global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType payload)
                {
                    if (!VerifyNotDisposed())
                    {
                        return;
                    }

                    var sender = EntityManager.GetComponentData<EventSender.MyEvent>(Entity);
                    sender.Events.Add(payload);
                }
            }
        }
    }
}


// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax
{
    public partial class Connection
    {
        public partial class Requirables
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
            public interface Reader : IReader<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component, Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>
            {
                event Action<global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> OnMyEvent;
            }

            [InjectableId(InjectableType.ReaderWriter, 1105)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : IWriter<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component, Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>
            {
                void SendMyEvent( global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType payload);
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component, Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                protected override void TriggerFieldCallbacks(Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update update)
                {
                }

                protected override void ApplyUpdate(Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update update, ref Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component data)
                {
                }

                private readonly List<Action<global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>> MyEventDelegates = new List<Action<global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>>();

                public event Action<global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> OnMyEvent
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

                public void OnMyEventEvent(global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType payload)
                {
                    GameObjectDelegates.DispatchWithErrorHandling(payload, MyEventDelegates, logDispatcher);
                }

                public void SendMyEvent(global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType payload)
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

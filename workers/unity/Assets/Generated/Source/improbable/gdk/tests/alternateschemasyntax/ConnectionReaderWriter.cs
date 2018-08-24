
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
            public interface Reader : IReader<SpatialOSConnection, SpatialOSConnection.Update>
            {
                event Action<global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> OnMyEvent;
            }

            [InjectableId(InjectableType.ReaderWriter, 1105)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : IWriter<SpatialOSConnection, SpatialOSConnection.Update>
            {
                void SendMyEvent( global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType payload);
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<SpatialOSConnection, SpatialOSConnection.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity,EntityManager entityManager,ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                protected override void TriggerFieldCallbacks(SpatialOSConnection.Update update)
                {
                }
                protected override void ApplyUpdate(SpatialOSConnection.Update update, ref SpatialOSConnection data)
                {
                }

                private readonly List<Action<global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>> myEventDelegates = new System.Collections.Generic.List<System.Action<global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>>();

                public event Action<global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> OnMyEvent
                {
                    add => myEventDelegates.Add(value);
                    remove => myEventDelegates.Remove(value);
                }

                public void OnMyEventEvent(global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType payload)
                {
                    DispatchEventWithErrorHandling(payload, myEventDelegates);
                }

                public void SendMyEvent(global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType payload)
                {
                    var sender = EntityManager.GetComponentData<EventSender.MyEvent>(Entity);
                    sender.Events.Add(payload);
                }
            }
        }
    }
}

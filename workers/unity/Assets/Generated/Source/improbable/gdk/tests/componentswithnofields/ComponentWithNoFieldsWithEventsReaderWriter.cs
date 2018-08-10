
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

            // TODO move into readerwriterbase
            private void DispatchEventWithErrorHandling<T>(T payload, System.Collections.Generic.IEnumerable<System.Action<T>> callbacks)
            {
                foreach (var callback in callbacks)
                {
                    try
                    {
                        callback(payload);
                    }
                    catch (System.Exception e)
                    {
                        // Log the exception but do not rethrow it, as other delegates should still get called
                        // TODO logDispatcher.HandleLog(LogType.Exception, new LogEvent().WithException(e));
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }

            private readonly System.Collections.Generic.List<System.Action<EvtEvent>> evtDelegates = new System.Collections.Generic.List<System.Action<EvtEvent>>();

            public event System.Action<EvtEvent> OnEvt
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

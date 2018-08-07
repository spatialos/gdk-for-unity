
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

namespace Generated.Improbable.Gdk.Tests
{
    public partial class NestedComponent
    {
        [ComponentId(20152)]
        internal class ReaderWriterCreator : IReaderWriterCreator
        {
            public IReaderWriterInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [ReaderInterface]
        [ComponentId(20152)]
        public interface Reader : IReader<SpatialOSNestedComponent, SpatialOSNestedComponent.Update>
        {
            event Action<global::Generated.Improbable.Gdk.Tests.TypeName> NestedTypeUpdated;
        }

        [WriterInterface]
        [ComponentId(20152)]
        public interface Writer : IWriter<SpatialOSNestedComponent, SpatialOSNestedComponent.Update>
        {
        }

        internal class ReaderWriterImpl :
            BlittableReaderWriterBase<SpatialOSNestedComponent, SpatialOSNestedComponent.Update>, Reader, Writer
        {
            public ReaderWriterImpl(Entity entity,EntityManager entityManager,ILogDispatcher logDispatcher)
                : base(entity, entityManager, logDispatcher)
            {
            }

            private readonly List<Action<global::Generated.Improbable.Gdk.Tests.TypeName>> nestedTypeDelegates = new List<Action<global::Generated.Improbable.Gdk.Tests.TypeName>>();

            public event Action<global::Generated.Improbable.Gdk.Tests.TypeName> NestedTypeUpdated
            {
                add => nestedTypeDelegates.Add(value);
                remove => nestedTypeDelegates.Remove(value);
            }

            protected override void TriggerFieldCallbacks(SpatialOSNestedComponent.Update update)
            {
                DispatchWithErrorHandling(update.NestedType, nestedTypeDelegates);
            }
            protected override void ApplyUpdate(SpatialOSNestedComponent.Update update, ref SpatialOSNestedComponent data)
            {
                if (update.NestedType.HasValue)
                {
                    data.NestedType = update.NestedType.Value;
                }
            }

            // TODO move into readerwriterbase
            private void DispatchEventWithErrorHandling<T>(T payload, IEnumerable<Action<T>> callbacks)
            {
                foreach (var callback in callbacks)
                {
                    try
                    {
                        callback(payload);
                    }
                    catch (Exception e)
                    {
                        // Log the exception but do not rethrow it, as other delegates should still get called
                        // TODO logDispatcher.HandleLog(LogType.Exception, new LogEvent().WithException(e));
                        Debug.LogException(e);
                    }
                }
            }
        }
    }
}

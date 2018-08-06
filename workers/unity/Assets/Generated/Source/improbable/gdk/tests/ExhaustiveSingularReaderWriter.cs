
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.MonoBehaviours;
using Improbable.Worker;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable.Gdk.Tests
{
    public partial class ExhaustiveSingular
    {
        [ComponentId(197715)]
        internal class ReaderWriterCreator : IReaderWriterCreator
        {
            public IReaderWriterInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [ReaderInterface]
        [ComponentId(197715)]
        public interface Reader : IReader<SpatialOSExhaustiveSingular, SpatialOSExhaustiveSingular.Update>
        {
        }

        [WriterInterface]
        [ComponentId(197715)]
        public interface Writer 
        {
        }

        internal class ReaderWriterImpl : IReaderWriterInternal, Reader, Writer
        {
            public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
            }
            
            public Authority Authority => throw new System.NotImplementedException();

            public SpatialOSExhaustiveSingular Data => throw new System.NotImplementedException();

            public event GameObjectDelegates.AuthorityChanged AuthorityChanged
            {
                add => throw new System.NotImplementedException();
                remove => throw new System.NotImplementedException();
            }

            public void OnAuthorityChange(global::Improbable.Worker.Authority auth)
            {
                throw new System.NotImplementedException();
            }

            public void OnComponentUpdate(SpatialOSExhaustiveSingular.Update update)
            {
                throw new System.NotImplementedException();
            }

            public event GameObjectDelegates.ComponentUpdated<SpatialOSExhaustiveSingular.Update> ComponentUpdated
            {
                add => throw new System.NotImplementedException();
                remove => throw new System.NotImplementedException();
            }
        }
    }
}

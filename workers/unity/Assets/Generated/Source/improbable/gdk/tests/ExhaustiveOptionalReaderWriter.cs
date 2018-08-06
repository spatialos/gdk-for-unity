
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
    public partial class ExhaustiveOptional
    {
        [ComponentId(197716)]
        internal class ReaderWriterCreator : IReaderWriterCreator
        {
            public IReaderWriterInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [ReaderInterface]
        [ComponentId(197716)]
        public interface Reader : IReader<SpatialOSExhaustiveOptional, SpatialOSExhaustiveOptional.Update>
        {
        }

        [WriterInterface]
        [ComponentId(197716)]
        public interface Writer 
        {
        }

        internal class ReaderWriterImpl : IReaderWriterInternal, Reader, Writer
        {
            public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
            }
            
            public Authority Authority => throw new System.NotImplementedException();

            public SpatialOSExhaustiveOptional Data => throw new System.NotImplementedException();

            public event GameObjectDelegates.AuthorityChanged AuthorityChanged
            {
                add => throw new System.NotImplementedException();
                remove => throw new System.NotImplementedException();
            }

            public void OnAuthorityChange(global::Improbable.Worker.Authority auth)
            {
                throw new System.NotImplementedException();
            }

            public void OnComponentUpdate(SpatialOSExhaustiveOptional.Update update)
            {
                throw new System.NotImplementedException();
            }

            public event GameObjectDelegates.ComponentUpdated<SpatialOSExhaustiveOptional.Update> ComponentUpdated
            {
                add => throw new System.NotImplementedException();
                remove => throw new System.NotImplementedException();
            }
        }
    }
}

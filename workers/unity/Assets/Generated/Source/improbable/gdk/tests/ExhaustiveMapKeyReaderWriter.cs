
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
    public partial class ExhaustiveMapKey
    {
        [ComponentId(197719)]
        internal class ReaderWriterCreator : IReaderWriterCreator
        {
            public IReaderWriterInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [ReaderInterface]
        [ComponentId(197719)]
        public interface Reader : IReader<SpatialOSExhaustiveMapKey, SpatialOSExhaustiveMapKey.Update>
        {
        }

        [WriterInterface]
        [ComponentId(197719)]
        public interface Writer 
        {
        }

        internal class ReaderWriterImpl : IReaderWriterInternal, Reader, Writer
        {
            public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
            }
            
            public Authority Authority => throw new System.NotImplementedException();

            public SpatialOSExhaustiveMapKey Data => throw new System.NotImplementedException();

            public event GameObjectDelegates.AuthorityChanged AuthorityChanged
            {
                add => throw new System.NotImplementedException();
                remove => throw new System.NotImplementedException();
            }

            public event GameObjectDelegates.ComponentUpdated<SpatialOSExhaustiveMapKey.Update> ComponentUpdated
            {
                add => throw new System.NotImplementedException();
                remove => throw new System.NotImplementedException();
            }
        }
    }
}

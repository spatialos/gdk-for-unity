
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
    public partial class ExhaustiveBlittableSingular
    {
        [ComponentId(197720)]
        internal class ReaderWriterCreator : IReaderWriterCreator
        {
            public IReaderInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [ReaderInterface]
        [ComponentId(197720)]
        public interface Reader
        {
        }

        public class ReaderWriterImpl : Reader, IReaderInternal
        {
            public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
            }

            // IReaderInternal methods
            void IReaderInternal.OnAuthorityChange(Authority authority)
            {
                throw new System.NotImplementedException();
            }

            void IReaderInternal.OnComponentUpdate()
            {
                throw new System.NotImplementedException();
            }
            
            void IReaderInternal.OnEvent(int eventIndex)
            {
                throw new System.NotImplementedException();
            }

            void IReaderInternal.OnCommandRequest(int commandIndex)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}

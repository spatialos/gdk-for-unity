
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.MonoBehaviours;
using Improbable.Worker;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        [ComponentId(1001)]
        internal class ReaderWriterCreator : IReaderWriterCreator
        {
            public IReaderWriterInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
                return new ReaderWriterImpl(entity, entityManager, logDispatcher);
            }
        }

        [ReaderInterface]
        [ComponentId(1001)]
        public interface Reader : IReader<SpatialOSBlittableComponent, SpatialOSBlittableComponent.Update>
        {
        }

        [WriterInterface]
        [ComponentId(1001)]
        public interface Writer 
        {
        }

        internal class ReaderWriterImpl : IReaderWriterInternal, Reader, Writer
        {
            public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
            {
            }
            
            public Authority Authority => throw new System.NotImplementedException();

            public SpatialOSBlittableComponent Data => throw new System.NotImplementedException();

            public event GameObjectDelegates.AuthorityChanged AuthorityChanged
            {
                add => throw new System.NotImplementedException();
                remove => throw new System.NotImplementedException();
            }

            public void OnAuthorityChange(global::Improbable.Worker.Authority auth)
            {
                throw new System.NotImplementedException();
            }

            public void OnComponentUpdate(SpatialOSBlittableComponent.Update update)
            {
                throw new System.NotImplementedException();
            }

            public event GameObjectDelegates.ComponentUpdated<SpatialOSBlittableComponent.Update> ComponentUpdated
            {
                add => throw new System.NotImplementedException();
                remove => throw new System.NotImplementedException();
            }

            public void OnFirstEventEvent(FirstEventEvent payload)
            {
                throw new System.NotImplementedException();
            }

            public void OnSecondEventEvent(SecondEventEvent payload)
            {
                throw new System.NotImplementedException();
            }

            public void OnFirstCommandCommandRequest(FirstCommand.Request request)
            {
                throw new System.NotImplementedException();
            }

            public void OnSecondCommandCommandRequest(SecondCommand.Request request)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}

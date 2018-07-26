using Improbable.Worker;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public abstract class WriterBase<TSpatialComponentData, TComponentUpdate>
        : IWriter<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        private readonly IReader<TSpatialComponentData, TComponentUpdate> reader;
        protected readonly Entity Entity;
        protected readonly EntityManager EntityManager;

        protected WriterBase(Entity entity, EntityManager entityManager,
            IReader<TSpatialComponentData, TComponentUpdate> reader)
        {
            Entity = entity;
            EntityManager = entityManager;
            this.reader = reader;
        }

        public void Send(TComponentUpdate update)
        {
            var newData = ApplyUpdateToFields(update);

            SetDataInEntity(newData);
        }

        protected abstract void SetDataInEntity(TSpatialComponentData newData);

        protected abstract TSpatialComponentData ApplyUpdateToFields(TComponentUpdate update);

        public void SendAuthorityLossImminentAcknowledgement()
        {
            throw new System.NotImplementedException();
        }

        public abstract void CopyAndSend(TComponentUpdate update);

        public Authority Authority => reader.Authority;
        public TSpatialComponentData Data => reader.Data;

        public event GameObjectDelegates.AuthorityChanged AuthorityChanged
        {
            add => reader.AuthorityChanged += value;
            remove => reader.AuthorityChanged -= value;
        }

        public event GameObjectDelegates.ComponentUpdated<TComponentUpdate> ComponentUpdated
        {
            add => reader.ComponentUpdated += value;
            remove => reader.ComponentUpdated -= value;
        }
    }
}

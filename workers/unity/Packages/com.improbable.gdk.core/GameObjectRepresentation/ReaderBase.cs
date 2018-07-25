using System.Collections.Generic;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public abstract class ReaderBase<TSpatialComponentData, TComponentUpdate>
        : IReader<TSpatialComponentData>,
            IReaderInternal
        where TSpatialComponentData : ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TSpatialComponentData>
    {
        protected readonly Unity.Entities.Entity Entity;
        protected readonly EntityManager EntityManager;

        private readonly List<GameObjectDelegates.AuthorityChanged> authorityChangedDelegates
            = new List<GameObjectDelegates.AuthorityChanged>();

        private readonly List<GameObjectDelegates.ComponentUpdated<TSpatialComponentData>> componentUpdateDelegates
            = new List<GameObjectDelegates.ComponentUpdated<TSpatialComponentData>>();

        protected ReaderBase(Unity.Entities.Entity entity, EntityManager entityManager)
        {
            this.Entity = entity;
            EntityManager = entityManager;
        }

        public Authority Authority
        {
            get
            {
                if (EntityManager.HasComponent<Authoritative<TSpatialComponentData>>(Entity))
                {
                    return Authority.Authoritative;
                }

                if (EntityManager.HasComponent<AuthorityLossImminent<TSpatialComponentData>>(Entity))
                {
                    return Authority.AuthorityLossImminent;
                }

                // TODO reviewers: should this throw an error instead?
                return Authority.NotAuthoritative;
            }
        }

        public abstract TSpatialComponentData Data { get; }

        public event GameObjectDelegates.AuthorityChanged AuthorityChanged
        {
            add => authorityChangedDelegates.Add(value);
            remove => authorityChangedDelegates.Remove(value);
        }

        public event GameObjectDelegates.ComponentUpdated<TSpatialComponentData> ComponentUpdated
        {
            add => componentUpdateDelegates.Add(value);
            remove => componentUpdateDelegates.Remove(value);
        }

        void IReaderInternal.OnAuthorityChange(Authority authority)
        {
            foreach (var authorityChangedDelegate in authorityChangedDelegates)
            {
                // TODO catch errors here?
                authorityChangedDelegate(authority);
            }
        }

        void IReaderInternal.OnComponentUpdate()
        {
            foreach (var update in EntityManager.GetComponentObject<ComponentsUpdated<TComponentUpdate>>(Entity).Buffer)
            {
                foreach (var componentUpdateDelegate in componentUpdateDelegates)
                {
                    // TODO catch errors here?
                    componentUpdateDelegate(update);
                }
            }
        }

        void IReaderInternal.OnEvent(int eventIndex)
        {
            // TODO
        }

        void IReaderInternal.OnCommandRequest(int commandIndex)
        {
            // TODO
        }
    }
}

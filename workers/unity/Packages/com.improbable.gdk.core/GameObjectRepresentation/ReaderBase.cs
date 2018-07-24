using System.Collections.Generic;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    public abstract class ReaderBase<TComponent, TComponentUpdate>
        : IReader<TComponent>,
            IReaderInternal
        where TComponent : ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TComponent>
    {
        protected readonly Unity.Entities.Entity Entity;
        protected readonly EntityManager EntityManager;

        private readonly List<GameObjectDelegates.AuthorityChanged> authorityChangedDelegates
            = new List<GameObjectDelegates.AuthorityChanged>();

        private readonly List<GameObjectDelegates.ComponentUpdated<TComponent>> componentUpdateDelegates
            = new List<GameObjectDelegates.ComponentUpdated<TComponent>>();

        protected ReaderBase(Unity.Entities.Entity entity, EntityManager entityManager)
        {
            this.Entity = entity;
            EntityManager = entityManager;
        }

        public Authority Authority
        {
            get
            {
                if (EntityManager.HasComponent<Authoritative<TComponent>>(Entity))
                {
                    return Authority.Authoritative;
                }

                if (EntityManager.HasComponent<AuthorityLossImminent<TComponent>>(Entity))
                {
                    return Authority.AuthorityLossImminent;
                }

                // TODO reviewers: should this throw an error instead?
                return Authority.NotAuthoritative;
            }
        }

        public abstract TComponent Data { get; }

        public event GameObjectDelegates.AuthorityChanged AuthorityChanged
        {
            add => authorityChangedDelegates.Add(value);
            remove => authorityChangedDelegates.Remove(value);
        }

        public event GameObjectDelegates.ComponentUpdated<TComponent> ComponentUpdated
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

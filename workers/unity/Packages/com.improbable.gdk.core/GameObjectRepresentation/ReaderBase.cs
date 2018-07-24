using System.Collections.Generic;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    public abstract class ReaderBase<TComponent, TComponentUpdate>
        : IReader<TComponent>,
            IReaderInternal
        where TComponent : ISpatialComponentData, IComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TComponent>
    {
        private readonly Unity.Entities.Entity entity;
        private readonly EntityManager manager;

        private readonly List<GameObjectDelegates.AuthorityChanged> authorityChangedDelegates
            = new List<GameObjectDelegates.AuthorityChanged>();

        private readonly List<GameObjectDelegates.ComponentUpdated<TComponent>> componentUpdateDelegates
            = new List<GameObjectDelegates.ComponentUpdated<TComponent>>();

        protected ReaderBase(Unity.Entities.Entity entity, EntityManager manager)
        {
            this.entity = entity;
            this.manager = manager;
        }

        public Authority Authority
        {
            get
            {
                if (manager.HasComponent<Authoritative<TComponent>>(entity))
                {
                    return Authority.Authoritative;
                }

                if (manager.HasComponent<AuthorityLossImminent<TComponent>>(entity))
                {
                    return Authority.AuthorityLossImminent;
                }

                // TODO reviewers: should this throw an error instead?
                return Authority.NotAuthoritative;
            }
        }

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
            foreach (var update in manager.GetComponentObject<ComponentsUpdated<TComponentUpdate>>(entity).Buffer)
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

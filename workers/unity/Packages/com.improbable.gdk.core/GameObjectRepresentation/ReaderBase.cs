using System.Collections.Generic;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    public abstract class ReaderBase<TComponent, TComponentUpdate>
        : IReader<TComponent, TComponentUpdate>, IReaderInternal
        where TComponent : ISpatialComponentData, IComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TComponent>
    {
        private readonly Unity.Entities.Entity entity;
        private readonly EntityManager manager;

        private readonly List<AuthorityChangedDelegate> authorityChangedDelegates
            = new List<AuthorityChangedDelegate>();

        private readonly List<ComponentUpdateDelegate<TComponentUpdate>> componentUpdateDelegates
            = new List<ComponentUpdateDelegate<TComponentUpdate>>();

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

                return Authority.NotAuthoritative;
            }
        }

        public event AuthorityChangedDelegate AuthorityChanged
        {
            add => authorityChangedDelegates.Add(value);
            remove => authorityChangedDelegates.Remove(value);
        }

        public event ComponentUpdateDelegate<TComponentUpdate> ComponentUpdated
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

        void IReaderInternal.OnComponentUpdate(object update)
        {
            // TODO catch invalid cast here?
            // Better to catch it higher up though
            TComponentUpdate componentUpdate = (TComponentUpdate) update;
            foreach (var componentUpdateDelegate in componentUpdateDelegates)
            {
                // TODO catch errors here?
                componentUpdateDelegate(componentUpdate);
            }
        }

        void IReaderInternal.OnEvent(int eventIndex, object payload)
        {
            // TODO
        }

        void IReaderInternal.OnCommandRequest(int commandIndex, object commandRequest)
        {
            // TODO
        }
    }
}
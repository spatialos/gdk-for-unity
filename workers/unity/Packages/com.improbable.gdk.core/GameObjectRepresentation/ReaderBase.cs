using System.Collections.Generic;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    public abstract class ReaderBase<TComponent, TComponentUpdate>
        : IReader<TComponent, TComponentUpdate>,
            IReaderInternal<TComponent, TComponentUpdate>
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

                // TODO reviewers: should this throw an error instead? If no comments, I'll assume no.
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

        void IReaderInternal<TComponent, TComponentUpdate>.OnAuthorityChange(Authority authority)
        {
            foreach (var authorityChangedDelegate in authorityChangedDelegates)
            {
                // TODO catch errors here?
                authorityChangedDelegate(authority);
            }
        }

        void IReaderInternal<TComponent, TComponentUpdate>.OnComponentUpdate(TComponentUpdate update)
        {
            foreach (var componentUpdateDelegate in componentUpdateDelegates)
            {
                // TODO catch errors here?
                componentUpdateDelegate(update);
            }
        }

        void IReaderInternal<TComponent, TComponentUpdate>.OnEvent<TEvent>(int eventIndex, TEvent payload)
        {
            // TODO
        }

        void IReaderInternal<TComponent, TComponentUpdate>.OnCommandRequest<TCommandRequest>(int commandIndex, TCommandRequest commandRequest)
        {
            // TODO
        }
    }
}

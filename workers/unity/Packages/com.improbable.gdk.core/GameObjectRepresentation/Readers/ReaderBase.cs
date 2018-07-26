using System;
using System.Collections.Generic;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    public abstract class ReaderBase<TSpatialComponentData, TComponentUpdate>
        : IReader<TSpatialComponentData>,
            IReaderInternal
        where TSpatialComponentData : ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TSpatialComponentData>
    {
        protected readonly Entity Entity;
        protected readonly EntityManager EntityManager;

        protected ReaderBase(Entity entity, EntityManager entityManager)
        {
            Entity = entity;
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

                if (EntityManager.HasComponent<NotAuthoritative<TSpatialComponentData>>(Entity))
                {
                    return Authority.NotAuthoritative;
                }

                throw new Exception("No authority component found for this entity.");
            }
        }

        public abstract TSpatialComponentData Data { get; }

        private readonly List<GameObjectDelegates.AuthorityChanged> authorityChangedDelegates
            = new List<GameObjectDelegates.AuthorityChanged>();

        public event GameObjectDelegates.AuthorityChanged AuthorityChanged
        {
            add => authorityChangedDelegates.Add(value);
            remove => authorityChangedDelegates.Remove(value);
        }

        private readonly List<GameObjectDelegates.ComponentUpdated<TSpatialComponentData>> componentUpdateDelegates
            = new List<GameObjectDelegates.ComponentUpdated<TSpatialComponentData>>();

        public event GameObjectDelegates.ComponentUpdated<TSpatialComponentData> ComponentUpdated
        {
            add => componentUpdateDelegates.Add(value);
            remove => componentUpdateDelegates.Remove(value);
        }

        void IReaderInternal.OnAuthorityChange(Authority authority)
        {
            foreach (var authorityChangedDelegate in authorityChangedDelegates)
            {
                try
                {
                    authorityChangedDelegate(authority);
                }
                catch (Exception e)
                {
                    // Log the exception but do not rethrow it, as other delegates should still get called
                    Debug.LogException(e);
                }
            }
        }

        void IReaderInternal.OnComponentUpdate()
        {
            foreach (var update in EntityManager.GetComponentObject<ComponentsUpdated<TComponentUpdate>>(Entity).Buffer)
            {
                foreach (var componentUpdateDelegate in componentUpdateDelegates)
                {
                    try
                    {
                        componentUpdateDelegate(update);
                    }
                    catch (Exception e)
                    {
                        // Log the exception but do not rethrow it, as other delegates should still get called
                        Debug.LogException(e);
                    }
                }

                HandleFieldUpdates(update);
            }
        }

        /// <summary>
        ///     Reader implementations will override this if their components have fields.
        /// </summary>
        /// <param name="update"></param>
        protected virtual void HandleFieldUpdates(TComponentUpdate update)
        {
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

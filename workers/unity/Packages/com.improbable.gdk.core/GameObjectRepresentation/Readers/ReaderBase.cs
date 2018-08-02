using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    internal abstract class ReaderBase<TSpatialComponentData, TComponentUpdate>
        : IReader<TSpatialComponentData, TComponentUpdate>,
            IReaderInternal
        where TSpatialComponentData : ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        protected readonly Entity Entity;
        protected readonly EntityManager EntityManager;

        private readonly ILogDispatcher logDispatcher;

        protected ReaderBase(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
        {
            Entity = entity;
            EntityManager = entityManager;
            this.logDispatcher = logDispatcher;
        }

        public Authority Authority
        {
            get
            {
                if (EntityManager.HasComponent<AuthorityLossImminent<TSpatialComponentData>>(Entity))
                {
                    return Authority.AuthorityLossImminent;
                }

                if (EntityManager.HasComponent<Authoritative<TSpatialComponentData>>(Entity))
                {
                    return Authority.Authoritative;
                }

                if (EntityManager.HasComponent<NotAuthoritative<TSpatialComponentData>>(Entity))
                {
                    return Authority.NotAuthoritative;
                }

                throw new ReaderAuthorityGetFailedException("No authority component found for this entity.");
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

        private readonly List<GameObjectDelegates.ComponentUpdated<TComponentUpdate>> componentUpdateDelegates
            = new List<GameObjectDelegates.ComponentUpdated<TComponentUpdate>>();

        public event GameObjectDelegates.ComponentUpdated<TComponentUpdate> ComponentUpdated
        {
            add => componentUpdateDelegates.Add(value);
            remove => componentUpdateDelegates.Remove(value);
        }

        /// <summary>
        /// Helper method to dispatch property updates to callbacks while forwarding exceptions to the log dispatcher.
        /// </summary>
        /// <param name="payload">The value for the property update.</param>
        /// <param name="callbacks">The property update handlers.</param>
        /// <typeparam name="T">The property update type.</typeparam>
        protected void DispatchWithErrorHandling<T>(Option<T> payload, IEnumerable<Action<T>> callbacks)
        {
            if (!payload.HasValue)
            {
                return;
            }

            foreach (var callback in callbacks)
            {
                try
                {
                    callback(payload.Value);
                }
                catch (Exception e)
                {
                    // Log the exception but do not rethrow it, as other delegates should still get called
                    logDispatcher.HandleLog(LogType.Exception, new LogEvent().WithException(e));
                }
            }
        }

        public void OnAuthorityChange(Authority authority)
        {
            foreach (var callback in authorityChangedDelegates)
            {
                try
                {
                    callback(authority);
                }
                catch (Exception e)
                {
                    // Log the exception but do not rethrow it, as other delegates should still get called
                    logDispatcher.HandleLog(LogType.Exception, new LogEvent().WithException(e));
                }
            }
        }

        public void OnComponentUpdate()
        {
            foreach (var update in EntityManager.GetComponentObject<ComponentsUpdated<TComponentUpdate>>(Entity).Buffer)
            {
                foreach (var callback in componentUpdateDelegates)
                {
                    try
                    {
                        callback(update);
                    }
                    catch (Exception e)
                    {
                        // Log the exception but do not rethrow it, as other delegates should still get called
                        logDispatcher.HandleLog(LogType.Exception, new LogEvent().WithException(e));
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

        public void OnEvent(int eventIndex)
        {
            // TODO
        }

        public void OnCommandRequest(int commandIndex)
        {
            // TODO
        }
    }
}

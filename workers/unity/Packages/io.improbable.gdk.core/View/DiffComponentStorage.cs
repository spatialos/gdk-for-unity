using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public abstract class DiffComponentStorage<TUpdate> : IDiffUpdateStorage<TUpdate>, IDiffComponentAddedStorage<TUpdate>, IDiffAuthorityStorage
        where TUpdate : ISpatialComponentUpdate
    {
        private readonly HashSet<EntityId> entitiesUpdated = new HashSet<EntityId>();
        private readonly uint componentId;

        private readonly List<EntityId> componentsAdded = new List<EntityId>();
        private readonly List<EntityId> componentsRemoved = new List<EntityId>();

        private readonly AuthorityComparer authorityComparer = new AuthorityComparer();
        private readonly UpdateComparer<TUpdate> updateComparer = new UpdateComparer<TUpdate>();

        // Used to represent a state machine of authority changes. Valid state changes are:
        // authority lost -> authority lost temporarily
        // authority lost temporarily -> authority lost
        // authority gained -> authority gained
        // Creating the authority lost temporarily set is the aim as it signifies authority epoch changes
        private readonly HashSet<EntityId> authorityLost = new HashSet<EntityId>();
        private readonly HashSet<EntityId> authorityGained = new HashSet<EntityId>();
        private readonly HashSet<EntityId> authorityLostTemporary = new HashSet<EntityId>();

        private readonly MessageList<ComponentUpdateReceived<TUpdate>> updateStorage =
            new MessageList<ComponentUpdateReceived<TUpdate>>();

        private readonly MessageList<AuthorityChangeReceived> authorityChanges =
            new MessageList<AuthorityChangeReceived>();

        public abstract Type[] GetEventTypes();

        public Type GetUpdateType() => typeof(TUpdate);

        public virtual void Clear()
        {
            entitiesUpdated.Clear();
            updateStorage.Clear();
            authorityChanges.Clear();
            componentsAdded.Clear();
            componentsRemoved.Clear();
        }

        public void RemoveEntityComponent(long entityId)
        {
            var id = new EntityId(entityId);

            // Adding a component always updates it, so this will catch the case where the component was just added
            if (entitiesUpdated.Remove(id))
            {
                updateStorage.RemoveAll(update => update.EntityId.Id == entityId);
                authorityChanges.RemoveAll(change => change.EntityId.Id == entityId);

                //playerCollidedEventStorage.RemoveAll(change => change.EntityId.Id == entityId);
                ClearEventStorage(entityId);
            }

            if (!componentsAdded.Remove(id))
            {
                componentsRemoved.Add(id);
            }
        }

        protected abstract void ClearEventStorage(long entityId);


        public void AddEntityComponent(long entityId, TUpdate component)
        {
            var id = new EntityId(entityId);
            if (!componentsRemoved.Remove(id))
            {
                componentsAdded.Add(id);
            }

            AddUpdate(new ComponentUpdateReceived<TUpdate>(component, id, 0));
        }

        public void AddUpdate(ComponentUpdateReceived<TUpdate> update)
        {
            entitiesUpdated.Add(update.EntityId);
            updateStorage.InsertSorted(update, updateComparer);
        }

        public void AddAuthorityChange(AuthorityChangeReceived authorityChange)
        {
            if (authorityChange.Authority == Authority.NotAuthoritative)
            {
                if (authorityLostTemporary.Remove(authorityChange.EntityId) || !authorityGained.Contains(authorityChange.EntityId))
                {
                    authorityLost.Add(authorityChange.EntityId);
                }
            }
            else if (authorityChange.Authority == Authority.Authoritative)
            {
                if (authorityLost.Remove(authorityChange.EntityId))
                {
                    authorityLostTemporary.Add(authorityChange.EntityId);
                }
                else
                {
                    authorityGained.Add(authorityChange.EntityId);
                }
            }

            authorityChanges.InsertSorted(authorityChange, authorityComparer);
        }

        public List<EntityId> GetComponentsAdded()
        {
            return componentsAdded;
        }

        public List<EntityId> GetComponentsRemoved()
        {
            return componentsRemoved;
        }

        public MessagesSpan<ComponentUpdateReceived<TUpdate>> GetUpdates()
        {
            return updateStorage.Slice();
        }

        public MessagesSpan<ComponentUpdateReceived<TUpdate>> GetUpdates(EntityId entityId)
        {
            var (firstIndex, count) = updateStorage.GetEntityRange(entityId);
            return updateStorage.Slice(firstIndex, count);
        }

        public MessagesSpan<AuthorityChangeReceived> GetAuthorityChanges()
        {
            return authorityChanges.Slice();
        }

        public MessagesSpan<AuthorityChangeReceived> GetAuthorityChanges(EntityId entityId)
        {
            var (firstIndex, count) = authorityChanges.GetEntityRange(entityId);
            return authorityChanges.Slice(firstIndex, count);
        }
    }
}

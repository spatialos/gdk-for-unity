using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public abstract class DiffComponentStorage<TUpdate> : IDiffUpdateStorage<TUpdate>, IDiffComponentAddedStorage<TUpdate>, IDiffAuthorityStorage
        where TUpdate : ISpatialComponentUpdate
    {
        protected readonly HashSet<EntityId> EntitiesUpdated = new HashSet<EntityId>();
        private readonly uint componentId;

        private readonly HashSet<EntityId> componentsAdded = new HashSet<EntityId>();
        private readonly HashSet<EntityId> componentsRemoved = new HashSet<EntityId>();

        private readonly MessageList<ComponentUpdateReceived<TUpdate>> updateStorage =
            new MessageList<ComponentUpdateReceived<TUpdate>>(new UpdateComparer<TUpdate>());

        private readonly MessageList<AuthorityChangeReceived> authorityChanges =
            new MessageList<AuthorityChangeReceived>(new AuthorityComparer());

        public abstract Type[] GetEventTypes();

        public Type GetUpdateType() => typeof(TUpdate);

        public bool Dirty { get; protected set; }

        public virtual void Clear()
        {
            EntitiesUpdated.Clear();
            updateStorage.Clear();
            authorityChanges.Clear();
            componentsAdded.Clear();
            componentsRemoved.Clear();
            Dirty = false;
        }

        public void RemoveEntityComponent(long entityId)
        {
            var id = new EntityId(entityId);

            // Adding a component always updates it, so this will catch the case where the component was just added
            if (EntitiesUpdated.Remove(id))
            {
                updateStorage.RemoveAll(update => update.EntityId.Id == entityId);
                ClearEventStorage(entityId);
            }

            if (!componentsAdded.Remove(id))
            {
                componentsRemoved.Add(id);
            }

            Dirty = true;
        }

        protected abstract void ClearEventStorage(long entityId);

        public void AddEntityComponent(long entityId, TUpdate component, uint updateId)
        {
            var id = new EntityId(entityId);
            if (!componentsRemoved.Remove(id))
            {
                componentsAdded.Add(id);
            }

            // This marks dirty
            AddUpdate(new ComponentUpdateReceived<TUpdate>(component, id, updateId));
        }

        public void AddUpdate(ComponentUpdateReceived<TUpdate> update)
        {
            EntitiesUpdated.Add(update.EntityId);
            updateStorage.Add(update);
            Dirty = true;
        }

        public void AddAuthorityChange(AuthorityChangeReceived authorityChange)
        {
            authorityChanges.Add(authorityChange);
            Dirty = true;
        }

        public HashSet<EntityId> GetComponentsAdded()
        {
            return componentsAdded;
        }

        public HashSet<EntityId> GetComponentsRemoved()
        {
            return componentsRemoved;
        }

        public MessagesSpan<ComponentUpdateReceived<TUpdate>> GetUpdates()
        {
            updateStorage.Sort();
            return updateStorage.Slice();
        }

        public MessagesSpan<ComponentUpdateReceived<TUpdate>> GetUpdates(EntityId entityId)
        {
            return updateStorage.GetEntityRange(entityId);
        }

        public MessagesSpan<AuthorityChangeReceived> GetAuthorityChanges()
        {
            authorityChanges.Sort();
            return authorityChanges.Slice();
        }

        public MessagesSpan<AuthorityChangeReceived> GetAuthorityChanges(EntityId entityId)
        {
            return authorityChanges.GetEntityRange(entityId);
        }
    }
}

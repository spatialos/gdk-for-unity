// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Tests
{
    public partial class ExhaustiveRepeated
    {
        public class DiffComponentStorage : IDiffUpdateStorage<Update>, IDiffComponentAddedStorage<Update>, IDiffAuthorityStorage
        {
            private readonly HashSet<EntityId> entitiesUpdated = new HashSet<EntityId>();

            private List<EntityId> componentsAdded = new List<EntityId>();
            private List<EntityId> componentsRemoved = new List<EntityId>();

            private readonly AuthorityComparer authorityComparer = new AuthorityComparer();
            private readonly UpdateComparer<Update> updateComparer = new UpdateComparer<Update>();

            // Used to represent a state machine of authority changes. Valid state changes are:
            // authority lost -> authority lost temporarily
            // authority lost temporarily -> authority lost
            // authority gained -> authority gained
            // Creating the authority lost temporarily set is the aim as it signifies authority epoch changes
            private readonly HashSet<EntityId> authorityLost = new HashSet<EntityId>();
            private readonly HashSet<EntityId> authorityGained = new HashSet<EntityId>();
            private readonly HashSet<EntityId> authorityLostTemporary = new HashSet<EntityId>();

            private MessageList<ComponentUpdateReceived<Update>> updateStorage =
                new MessageList<ComponentUpdateReceived<Update>>();

            private MessageList<AuthorityChangeReceived> authorityChanges =
                new MessageList<AuthorityChangeReceived>();

            public Type[] GetEventTypes()
            {
                return new Type[]
                {
                };
            }

            public Type GetUpdateType()
            {
                return typeof(Update);
            }

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public void Clear()
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
                }

                if (!componentsAdded.Remove(id))
                {
                    componentsRemoved.Add(id);
                }
            }

            public void AddEntityComponent(long entityId, Update component)
            {
                var id = new EntityId(entityId);
                if (!componentsRemoved.Remove(id))
                {
                    componentsAdded.Add(id);
                }

                AddUpdate(new ComponentUpdateReceived<Update>(component, id, 0));
            }

            public void AddUpdate(ComponentUpdateReceived<Update> update)
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

            public ReceivedMessagesSpan<ComponentUpdateReceived<Update>> GetUpdates()
            {
                return new ReceivedMessagesSpan<ComponentUpdateReceived<Update>>(updateStorage);
            }

            public ReceivedMessagesSpan<ComponentUpdateReceived<Update>> GetUpdates(EntityId entityId)
            {
                var range = updateStorage.GetEntityRange(entityId);
                return new ReceivedMessagesSpan<ComponentUpdateReceived<Update>>(updateStorage, range.FirstIndex,
                    range.Count);
            }

            public ReceivedMessagesSpan<AuthorityChangeReceived> GetAuthorityChanges()
            {
                return new ReceivedMessagesSpan<AuthorityChangeReceived>(authorityChanges);
            }

            public ReceivedMessagesSpan<AuthorityChangeReceived> GetAuthorityChanges(EntityId entityId)
            {
                var range = authorityChanges.GetEntityRange(entityId);
                return new ReceivedMessagesSpan<AuthorityChangeReceived>(authorityChanges, range.FirstIndex, range.Count);
            }
        }
    }
}


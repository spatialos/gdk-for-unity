// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class DiffComponentStorage : IDiffUpdateStorage<Update>, IDiffComponentAddedStorage<Update>, IDiffAuthorityStorage
            , IDiffEventStorage<FirstEvent.Event>
            , IDiffEventStorage<SecondEvent.Event>
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

            private MessageList<ComponentEventReceived<FirstEvent.Event>> firstEventEventStorage =
                new MessageList<ComponentEventReceived<FirstEvent.Event>>();

            private readonly EventComparer<FirstEvent.Event> firstEventComparer =
                new EventComparer<FirstEvent.Event>();

            private MessageList<ComponentEventReceived<SecondEvent.Event>> secondEventEventStorage =
                new MessageList<ComponentEventReceived<SecondEvent.Event>>();

            private readonly EventComparer<SecondEvent.Event> secondEventComparer =
                new EventComparer<SecondEvent.Event>();

            public Type[] GetEventTypes()
            {
                return new Type[]
                {
                    typeof(FirstEvent.Event),
                    typeof(SecondEvent.Event),
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

                firstEventEventStorage.Clear();

                secondEventEventStorage.Clear();
            }

            public void RemoveEntityComponent(long entityId)
            {
                var id = new EntityId(entityId);

                // Adding a component always updates it, so this will catch the case where the component was just added
                if (entitiesUpdated.Remove(id))
                {
                    updateStorage.RemoveAll(update => update.EntityId.Id == entityId);
                    authorityChanges.RemoveAll(change => change.EntityId.Id == entityId);
                    firstEventEventStorage.RemoveAll(change => change.EntityId.Id == entityId);
                    secondEventEventStorage.RemoveAll(change => change.EntityId.Id == entityId);
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

            ReceivedMessagesSpan<ComponentEventReceived<FirstEvent.Event>> IDiffEventStorage<FirstEvent.Event>.GetEvents(EntityId entityId)
            {
                var range = firstEventEventStorage.GetEntityRange(entityId);
                return new ReceivedMessagesSpan<ComponentEventReceived<FirstEvent.Event>>(
                    firstEventEventStorage, range.FirstIndex, range.Count);
            }

            ReceivedMessagesSpan<ComponentEventReceived<FirstEvent.Event>> IDiffEventStorage<FirstEvent.Event>.GetEvents()
            {
                return new ReceivedMessagesSpan<ComponentEventReceived<FirstEvent.Event>>(firstEventEventStorage);
            }

            void IDiffEventStorage<FirstEvent.Event>.AddEvent(ComponentEventReceived<FirstEvent.Event> ev)
            {
                firstEventEventStorage.InsertSorted(ev, firstEventComparer);
            }

            ReceivedMessagesSpan<ComponentEventReceived<SecondEvent.Event>> IDiffEventStorage<SecondEvent.Event>.GetEvents(EntityId entityId)
            {
                var range = secondEventEventStorage.GetEntityRange(entityId);
                return new ReceivedMessagesSpan<ComponentEventReceived<SecondEvent.Event>>(
                    secondEventEventStorage, range.FirstIndex, range.Count);
            }

            ReceivedMessagesSpan<ComponentEventReceived<SecondEvent.Event>> IDiffEventStorage<SecondEvent.Event>.GetEvents()
            {
                return new ReceivedMessagesSpan<ComponentEventReceived<SecondEvent.Event>>(secondEventEventStorage);
            }

            void IDiffEventStorage<SecondEvent.Event>.AddEvent(ComponentEventReceived<SecondEvent.Event> ev)
            {
                secondEventEventStorage.InsertSorted(ev, secondEventComparer);
            }
        }
    }
}


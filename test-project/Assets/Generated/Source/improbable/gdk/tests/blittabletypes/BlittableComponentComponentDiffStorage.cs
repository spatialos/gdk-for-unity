// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public class DiffComponentStorage : IDiffUpdateStorage<Update>, IDiffComponentAddedStorage<Update>, IDiffAuthorityStorage
            , IDiffEventStorage<FirstEvent.Event>
            , IDiffEventStorage<SecondEvent.Event>
        {
            private readonly HashSet<EntityId> entitiesUpdated = new HashSet<EntityId>();

            private List<EntityId> componentsAdded = new List<EntityId>();
            private List<EntityId> componentsRemoved = new List<EntityId>();

            private bool authoritySorted;
            private bool updatesSorted;

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

            // todo consider putting this in the list type
            private bool firstEventSorted;

            private readonly EventComparer<FirstEvent.Event> firstEventComparer =
                new EventComparer<FirstEvent.Event>();

            private MessageList<ComponentEventReceived<SecondEvent.Event>> secondEventEventStorage =
                new MessageList<ComponentEventReceived<SecondEvent.Event>>();

            // todo consider putting this in the list type
            private bool secondEventSorted;

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

                authoritySorted = false;
                updatesSorted = false;

                firstEventEventStorage.Clear();
                firstEventSorted = false;

                secondEventEventStorage.Clear();
                secondEventSorted = false;
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
                updateStorage.Add(update);
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

                authorityChanges.Add(authorityChange);
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
                // todo consider if this needs to be sorted
                // possible offer two functions
                // probably just sort it
                if (!updatesSorted)
                {
                    updatesSorted = true;
                    updateStorage.Sort(updateComparer);
                }

                return new ReceivedMessagesSpan<ComponentUpdateReceived<Update>>(updateStorage);
            }

            public ReceivedMessagesSpan<ComponentUpdateReceived<Update>> GetUpdates(EntityId entityId)
            {
                if (!updatesSorted)
                {
                    updatesSorted = true;
                    updateStorage.Sort(updateComparer);
                }

                var range = updateStorage.GetEntityRange(entityId);
                return new ReceivedMessagesSpan<ComponentUpdateReceived<Update>>(updateStorage, range.FirstIndex,
                    range.Count);
            }

            public ReceivedMessagesSpan<AuthorityChangeReceived> GetAuthorityChanges()
            {
                if (!authoritySorted)
                {
                    authoritySorted = true;
                    authorityChanges.Sort(authorityComparer);
                }

                return new ReceivedMessagesSpan<AuthorityChangeReceived>(authorityChanges);
            }

            public ReceivedMessagesSpan<AuthorityChangeReceived> GetAuthorityChanges(EntityId entityId)
            {
                if (!authoritySorted)
                {
                    authoritySorted = true;
                    authorityChanges.Sort(authorityComparer);
                }

                var range = authorityChanges.GetEntityRange(entityId);
                return new ReceivedMessagesSpan<AuthorityChangeReceived>(authorityChanges, range.FirstIndex, range.Count);
            }

            ReceivedMessagesSpan<ComponentEventReceived<FirstEvent.Event>> IDiffEventStorage<FirstEvent.Event>.GetEvents(EntityId entityId)
            {
                if (!firstEventSorted)
                {
                    firstEventEventStorage.Sort(firstEventComparer);
                    firstEventSorted = true;
                }

                var range = firstEventEventStorage.GetEntityRange(entityId);
                return new ReceivedMessagesSpan<ComponentEventReceived<FirstEvent.Event>>(
                    firstEventEventStorage, range.FirstIndex, range.Count);
            }

            ReceivedMessagesSpan<ComponentEventReceived<FirstEvent.Event>> IDiffEventStorage<FirstEvent.Event>.GetEvents()
            {
                if (!firstEventSorted)
                {
                    firstEventEventStorage.Sort(firstEventComparer);
                    firstEventSorted = true;
                }

                return new ReceivedMessagesSpan<ComponentEventReceived<FirstEvent.Event>>(firstEventEventStorage);
            }

            void IDiffEventStorage<FirstEvent.Event>.AddEvent(ComponentEventReceived<FirstEvent.Event> ev)
            {
                firstEventEventStorage.Add(ev);
            }

            ReceivedMessagesSpan<ComponentEventReceived<SecondEvent.Event>> IDiffEventStorage<SecondEvent.Event>.GetEvents(EntityId entityId)
            {
                if (!secondEventSorted)
                {
                    secondEventEventStorage.Sort(secondEventComparer);
                    secondEventSorted = true;
                }

                var range = secondEventEventStorage.GetEntityRange(entityId);
                return new ReceivedMessagesSpan<ComponentEventReceived<SecondEvent.Event>>(
                    secondEventEventStorage, range.FirstIndex, range.Count);
            }

            ReceivedMessagesSpan<ComponentEventReceived<SecondEvent.Event>> IDiffEventStorage<SecondEvent.Event>.GetEvents()
            {
                if (!secondEventSorted)
                {
                    secondEventEventStorage.Sort(secondEventComparer);
                    secondEventSorted = true;
                }

                return new ReceivedMessagesSpan<ComponentEventReceived<SecondEvent.Event>>(secondEventEventStorage);
            }

            void IDiffEventStorage<SecondEvent.Event>.AddEvent(ComponentEventReceived<SecondEvent.Event> ev)
            {
                secondEventEventStorage.Add(ev);
            }
        }
    }
}


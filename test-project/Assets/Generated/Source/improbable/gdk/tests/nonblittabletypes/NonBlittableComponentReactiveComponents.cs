// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.ReactiveComponents;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class UpdateReactiveComponentManager : IReactiveComponentManager
        {
            public void PopulateReactiveComponents(ComponentUpdateSystem updateSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var updatesReceived = updateSystem.GetComponentUpdatesReceived<Update>();

                for (int i = 0; i < updatesReceived.Count; ++i)
                {
                    ref readonly var update = ref updatesReceived[i];
                    if (!workerSystem.TryGetEntity(update.EntityId, out var entity))
                    {
                        continue;
                    }

                    List<Update> updates;
                    if (entityManager.HasComponent<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReceivedUpdates>(entity))
                    {
                        updates = entityManager.GetComponentData<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReceivedUpdates>(entity).Updates;
                    }
                    else
                    {
                        updates = Update.Pool.Count > 0 ? Update.Pool.Pop() : new List<Update>();
                        var updatesComponent = new ReceivedUpdates
                        {
                            handle = ReferenceTypeProviders.UpdatesProvider.Allocate(world)
                        };
                        ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                        entityManager.AddComponentData(entity, updatesComponent);
                    }

                    updates.Add(update.Update);
                }
            }

            public void Clean(World world)
            {
                global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(world);
            }
        }

        public class AddComponentReactiveComponentManager : IReactiveComponentManager
        {
            public void PopulateReactiveComponents(ComponentUpdateSystem updateSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var added = updateSystem.GetComponentsAdded(ComponentId);

                foreach (var entityId in added)
                {
                    if (!workerSystem.TryGetEntity(entityId, out var entity))
                    {
                        continue;
                    }

                    if (entityManager.HasComponent<ComponentRemoved<Component>>(entity))
                    {
                        entityManager.RemoveComponent<ComponentRemoved<Component>>(entity);
                    }

                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Component>>());
                }
            }

            public void Clean(World world)
            {
            }
        }

        public class RemoveComponentReactiveComponentManager : IReactiveComponentManager
        {
            public void PopulateReactiveComponents(ComponentUpdateSystem updateSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var removed = updateSystem.GetComponentsRemoved(ComponentId);

                foreach (var entityId in removed)
                {
                    if (!workerSystem.TryGetEntity(entityId, out var entity))
                    {
                        continue;
                    }

                    if (entityManager.HasComponent<ComponentAdded<Component>>(entity))
                    {
                        entityManager.RemoveComponent<ComponentAdded<Component>>(entity);
                    }

                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Component>>());
                }
            }

            public void Clean(World world)
            {
            }
        }

        public class AuthorityReactiveComponentManager : IReactiveComponentManager
        {
            public void PopulateReactiveComponents(ComponentUpdateSystem updateSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var authorityChanges = updateSystem.GetAuthorityChangesReceived(ComponentId);

                for (int i = 0; i < authorityChanges.Count; ++i)
                {
                    ref readonly var auth = ref authorityChanges[i];
                    if (!workerSystem.TryGetEntity(auth.EntityId, out var entity))
                    {
                        continue;
                    }

                    List<Authority> changes;
                    if (entityManager.HasComponent<AuthorityChanges<Component>>(entity))
                    {
                        changes = entityManager.GetComponentData<AuthorityChanges<Component>>(entity).Changes;
                    }
                    else
                    {
                        changes = new List<Authority>();
                        var authComponent = new AuthorityChanges<Component>
                        {
                            Handle = AuthorityChangesProvider.Allocate(world)
                        };
                        AuthorityChangesProvider.Set(authComponent.Handle, changes);
                        entityManager.AddComponentData(entity, authComponent);
                    }

                    changes.Add(auth.Authority);
                }
            }

            public void Clean(World world)
            {
                AuthorityChangesProvider.CleanDataInWorld(world);
            }
        }


        public class FirstEventEventReactiveComponentManager : IReactiveComponentManager
        {
            public void PopulateReactiveComponents(ComponentUpdateSystem updateSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var eventsReceived = updateSystem.GetEventsReceived<FirstEvent.Event>();

                for (int i = 0; i < eventsReceived.Count; ++i)
                {
                    ref readonly var ev = ref eventsReceived[i];
                    if (!workerSystem.TryGetEntity(ev.EntityId, out var entity))
                    {
                        continue;
                    }

                    List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload> events;
                    if (entityManager.HasComponent<ReceivedEvents.FirstEvent>(entity))
                    {
                        events = entityManager.GetComponentData<ReceivedEvents.FirstEvent>(entity).Events;
                    }
                    else
                    {
                        events = new List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload>();
                        var eventsComponent = new ReceivedEvents.FirstEvent
                        {
                            handle = ReferenceTypeProviders.FirstEventProvider.Allocate(world)
                        };
                        ReferenceTypeProviders.FirstEventProvider.Set(eventsComponent.handle, events);
                        entityManager.AddComponentData(entity, eventsComponent);
                    }

                    events.Add(ev.Event.Payload);
                }

                var authorityChanges = updateSystem.GetAuthorityChangesReceived(ComponentId);

                for (int i = 0; i < authorityChanges.Count; ++i)
                {
                    ref readonly var auth = ref authorityChanges[i];
                    if (!workerSystem.TryGetEntity(auth.EntityId, out var entity))
                    {
                        continue;
                    }

                    // Add event senders
                    if (auth.Authority == Authority.Authoritative)
                    {
                        var eventSender = new EventSender.FirstEvent()
                        {
                            handle = ReferenceTypeProviders.FirstEventProvider.Allocate(world)
                        };
                        ReferenceTypeProviders.FirstEventProvider.Set(eventSender.handle, new List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload>());
                        entityManager.AddComponentData(entity, eventSender);
                    }

                    // Remove event senders
                    if (auth.Authority == Authority.NotAuthoritative)
                    {
                        var eventSender = entityManager.GetComponentData<EventSender.FirstEvent>(entity);
                        ReferenceTypeProviders.FirstEventProvider.Free(eventSender.handle);
                        entityManager.RemoveComponent<EventSender.FirstEvent>(entity);
                    }
                }
            }

            public void Clean(World world)
            {
                global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstEventProvider.CleanDataInWorld(world);
            }
        }

        public class SecondEventEventReactiveComponentManager : IReactiveComponentManager
        {
            public void PopulateReactiveComponents(ComponentUpdateSystem updateSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var eventsReceived = updateSystem.GetEventsReceived<SecondEvent.Event>();

                for (int i = 0; i < eventsReceived.Count; ++i)
                {
                    ref readonly var ev = ref eventsReceived[i];
                    if (!workerSystem.TryGetEntity(ev.EntityId, out var entity))
                    {
                        continue;
                    }

                    List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload> events;
                    if (entityManager.HasComponent<ReceivedEvents.SecondEvent>(entity))
                    {
                        events = entityManager.GetComponentData<ReceivedEvents.SecondEvent>(entity).Events;
                    }
                    else
                    {
                        events = new List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload>();
                        var eventsComponent = new ReceivedEvents.SecondEvent
                        {
                            handle = ReferenceTypeProviders.SecondEventProvider.Allocate(world)
                        };
                        ReferenceTypeProviders.SecondEventProvider.Set(eventsComponent.handle, events);
                        entityManager.AddComponentData(entity, eventsComponent);
                    }

                    events.Add(ev.Event.Payload);
                }

                var authorityChanges = updateSystem.GetAuthorityChangesReceived(ComponentId);

                for (int i = 0; i < authorityChanges.Count; ++i)
                {
                    ref readonly var auth = ref authorityChanges[i];
                    if (!workerSystem.TryGetEntity(auth.EntityId, out var entity))
                    {
                        continue;
                    }

                    // Add event senders
                    if (auth.Authority == Authority.Authoritative)
                    {
                        var eventSender = new EventSender.SecondEvent()
                        {
                            handle = ReferenceTypeProviders.SecondEventProvider.Allocate(world)
                        };
                        ReferenceTypeProviders.SecondEventProvider.Set(eventSender.handle, new List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload>());
                        entityManager.AddComponentData(entity, eventSender);
                    }

                    // Remove event senders
                    if (auth.Authority == Authority.NotAuthoritative)
                    {
                        var eventSender = entityManager.GetComponentData<EventSender.SecondEvent>(entity);
                        ReferenceTypeProviders.SecondEventProvider.Free(eventSender.handle);
                        entityManager.RemoveComponent<EventSender.SecondEvent>(entity);
                    }
                }
            }

            public void Clean(World world)
            {
                global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondEventProvider.CleanDataInWorld(world);
            }
        }

        public class LegacyAuthorityComponentManager : IReactiveComponentManager
        {
            public void PopulateReactiveComponents(ComponentUpdateSystem updateSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var authorityChanges = updateSystem.GetAuthorityChangesReceived(ComponentId);

                foreach (var entityId in world.GetExistingManager<EntitySystem>().GetEntitiesAdded())
                {
                    workerSystem.TryGetEntity(entityId, out var entity);
                    entityManager.AddComponent(entity,
                        ComponentType.Create<NotAuthoritative<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>());
                }

                for (int i = 0; i < authorityChanges.Count; ++i)
                {
                    ref readonly var auth = ref authorityChanges[i];
                    if (!workerSystem.TryGetEntity(auth.EntityId, out var entity))
                    {
                        continue;
                    }

                    ApplyAuthorityChange(entity, auth.Authority, auth.EntityId, world, entityManager);
                }
            }

            public void Clean(World world)
            {
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Gdk.Core.EntityId entityId, World world, EntityManager entityManager)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>());

                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>());
                        break;
                }
            }

            // todo delete this or fix it
            private void LogInvalidAuthorityTransition(Authority newAuthority, Authority expectedOldAuthority, global::Improbable.Gdk.Core.EntityId entityId)
            {
                // LogDispatcher.HandleLog(LogType.Error, new LogEvent(InvalidAuthorityChange)
                //     .WithField(LoggingUtils.LoggerName, LoggerName)
                //     .WithField(LoggingUtils.EntityId, entityId.Id)
                //     .WithField("New Authority", newAuthority)
                //     .WithField("Expected Old Authority", expectedOldAuthority)
                //     .WithField("Component", "global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent")
                // );
            }
        }
    }
}
#endif

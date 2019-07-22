// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.ReactiveComponents;
using Improbable.Worker.CInterop;

[assembly: RegisterGenericComponentType(typeof(ComponentAdded<global::Improbable.DependentSchema.DependentDataComponent.Component>))]
[assembly: RegisterGenericComponentType(typeof(ComponentRemoved<global::Improbable.DependentSchema.DependentDataComponent.Component>))]
[assembly: RegisterGenericComponentType(typeof(AuthorityChanges<global::Improbable.DependentSchema.DependentDataComponent.Component>))]
[assembly: RegisterGenericComponentType(typeof(Authoritative<global::Improbable.DependentSchema.DependentDataComponent.Component>))]
[assembly: RegisterGenericComponentType(typeof(NotAuthoritative<global::Improbable.DependentSchema.DependentDataComponent.Component>))]
[assembly: RegisterGenericComponentType(typeof(AuthorityLossImminent<global::Improbable.DependentSchema.DependentDataComponent.Component>))]

namespace Improbable.DependentSchema
{
    public partial class DependentDataComponent
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
                    if (entityManager.HasComponent<global::Improbable.DependentSchema.DependentDataComponent.ReceivedUpdates>(entity))
                    {
                        updates = entityManager.GetComponentData<global::Improbable.DependentSchema.DependentDataComponent.ReceivedUpdates>(entity).Updates;
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
                global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(world);
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

                    entityManager.AddComponent(entity, ComponentType.ReadWrite<ComponentAdded<Component>>());
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

                    entityManager.AddComponent(entity, ComponentType.ReadWrite<ComponentRemoved<Component>>());
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


        public class FooEventEventReactiveComponentManager : IReactiveComponentManager
        {
            public void PopulateReactiveComponents(ComponentUpdateSystem updateSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var eventsReceived = updateSystem.GetEventsReceived<FooEvent.Event>();

                for (int i = 0; i < eventsReceived.Count; ++i)
                {
                    ref readonly var ev = ref eventsReceived[i];
                    if (!workerSystem.TryGetEntity(ev.EntityId, out var entity))
                    {
                        continue;
                    }

                    List<global::Improbable.TestSchema.SomeType> events;
                    if (entityManager.HasComponent<ReceivedEvents.FooEvent>(entity))
                    {
                        events = entityManager.GetComponentData<ReceivedEvents.FooEvent>(entity).Events;
                    }
                    else
                    {
                        events = new List<global::Improbable.TestSchema.SomeType>();
                        var eventsComponent = new ReceivedEvents.FooEvent
                        {
                            handle = ReferenceTypeProviders.FooEventProvider.Allocate(world)
                        };
                        ReferenceTypeProviders.FooEventProvider.Set(eventsComponent.handle, events);
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
                        var eventSender = new EventSender.FooEvent()
                        {
                            handle = ReferenceTypeProviders.FooEventProvider.Allocate(world)
                        };
                        ReferenceTypeProviders.FooEventProvider.Set(eventSender.handle, new List<global::Improbable.TestSchema.SomeType>());
                        entityManager.AddComponentData(entity, eventSender);
                    }

                    // Remove event senders
                    if (auth.Authority == Authority.NotAuthoritative)
                    {
                        var eventSender = entityManager.GetComponentData<EventSender.FooEvent>(entity);
                        ReferenceTypeProviders.FooEventProvider.Free(eventSender.handle);
                        entityManager.RemoveComponent<EventSender.FooEvent>(entity);
                    }
                }
            }

            public void Clean(World world)
            {
                global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.FooEventProvider.CleanDataInWorld(world);
            }
        }

        public class LegacyAuthorityComponentManager : IReactiveComponentManager
        {
            public void PopulateReactiveComponents(ComponentUpdateSystem updateSystem, EntityManager entityManager, WorkerSystem workerSystem, World world)
            {
                var authorityChanges = updateSystem.GetAuthorityChangesReceived(ComponentId);

                foreach (var entityId in world.GetExistingSystem<EntitySystem>().GetEntitiesAdded())
                {
                    workerSystem.TryGetEntity(entityId, out var entity);
                    entityManager.AddComponent(entity,
                        ComponentType.ReadWrite<NotAuthoritative<global::Improbable.DependentSchema.DependentDataComponent.Component>>());
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
                        if (!entityManager.HasComponent<NotAuthoritative<global::Improbable.DependentSchema.DependentDataComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<global::Improbable.DependentSchema.DependentDataComponent.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.ReadWrite<Authoritative<global::Improbable.DependentSchema.DependentDataComponent.Component>>());

                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<global::Improbable.DependentSchema.DependentDataComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.ReadWrite<AuthorityLossImminent<global::Improbable.DependentSchema.DependentDataComponent.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<global::Improbable.DependentSchema.DependentDataComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<global::Improbable.DependentSchema.DependentDataComponent.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<global::Improbable.DependentSchema.DependentDataComponent.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<global::Improbable.DependentSchema.DependentDataComponent.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.ReadWrite<NotAuthoritative<global::Improbable.DependentSchema.DependentDataComponent.Component>>());
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
                //     .WithField("Component", "global::Improbable.DependentSchema.DependentDataComponent")
                // );
            }
        }
    }
}
#endif

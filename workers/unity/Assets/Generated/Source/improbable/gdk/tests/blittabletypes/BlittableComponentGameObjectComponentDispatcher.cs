// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        internal class GameObjectComponentDispatcher : GameObjectComponentDispatcherBase
        {
            public override ComponentType[] ComponentAddedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentAdded<SpatialOSBlittableComponent>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentRemovedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentRemoved<SpatialOSBlittableComponent>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] AuthoritiesChangedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthoritiesChanged<SpatialOSBlittableComponent>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentsUpdated<SpatialOSBlittableComponent.Update>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[][] EventsReceivedComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { ComponentType.ReadOnly<EventsReceived<FirstEventEvent>>(), ComponentType.ReadOnly<GameObjectReference>() },
                new ComponentType[] { ComponentType.ReadOnly<EventsReceived<SecondEventEvent>>(), ComponentType.ReadOnly<GameObjectReference>() },
            };

            public override ComponentType[][] CommandRequestsComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { ComponentType.ReadOnly<CommandRequests<FirstCommand.Request>>(), ComponentType.ReadOnly<GameObjectReference>() },
                new ComponentType[] { ComponentType.ReadOnly<CommandRequests<SecondCommand.Request>>(), ComponentType.ReadOnly<GameObjectReference>() },
            };

            protected override uint GetComponentId()
            {
                return 1001;
            }

            public override void InvokeOnComponentUpdateUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var entities = ComponentsUpdatedComponentGroup.GetEntityArray();
                var updateLists = ComponentsUpdatedComponentGroup.GetComponentArray<ComponentsUpdated<SpatialOSBlittableComponent.Update>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var manager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                    if (!manager.TryGetReadersWriters(1001, out var readers))
                    {
                        continue;
                    }

                    var updateList = updateLists[i];
                    foreach (BlittableComponent.ReaderWriterImpl reader in readers)
                    {
                        foreach (var update in updateList.Buffer)
                        {
                            reader.OnComponentUpdate(update);
                        }
                    }
                }
            }

            public override void InvokeOnEventUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                {
                    var entities = EventsReceivedComponentGroups[0].GetEntityArray();
                    var eventLists = EventsReceivedComponentGroups[0].GetComponentArray<EventsReceived<FirstEventEvent>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var manager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                        if (!manager.TryGetReadersWriters(1001, out var readers))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (BlittableComponent.ReaderWriterImpl reader in readers)
                        {
                            foreach (var e in eventList.Buffer)
                            {
                                reader.OnFirstEventEvent(e);
                            }
                        }
                    }
                }
                {
                    var entities = EventsReceivedComponentGroups[1].GetEntityArray();
                    var eventLists = EventsReceivedComponentGroups[1].GetComponentArray<EventsReceived<SecondEventEvent>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var manager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                        if (!manager.TryGetReadersWriters(1001, out var readers))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (BlittableComponent.ReaderWriterImpl reader in readers)
                        {
                            foreach (var e in eventList.Buffer)
                            {
                                reader.OnSecondEventEvent(e);
                            }
                        }
                    }
                }
            }

            public override void InvokeOnCommandRequestUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                {
                    var entities = CommandRequestsComponentGroups[0].GetEntityArray();
                    var commandLists = CommandRequestsComponentGroups[0].GetComponentArray<CommandRequests<FirstCommand.Request>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var manager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                        if (!manager.TryGetReadersWriters(1001, out var readers))
                        {
                            continue;
                        }

                        var commandList = commandLists[i];
                        foreach (BlittableComponent.ReaderWriterImpl reader in readers)
                        {
                            foreach (var req in commandList.Buffer)
                            {
                                reader.OnFirstCommandCommandRequest(req);
                            }
                        }
                    }
                }
                {
                    var entities = CommandRequestsComponentGroups[1].GetEntityArray();
                    var commandLists = CommandRequestsComponentGroups[1].GetComponentArray<CommandRequests<SecondCommand.Request>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var manager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                        if (!manager.TryGetReadersWriters(1001, out var readers))
                        {
                            continue;
                        }

                        var commandList = commandLists[i];
                        foreach (BlittableComponent.ReaderWriterImpl reader in readers)
                        {
                            foreach (var req in commandList.Buffer)
                            {
                                reader.OnSecondCommandCommandRequest(req);
                            }
                        }
                    }
                }
            }

            public override void InvokeOnAuthorityChangeLifecycleCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var authoritiesChangedTags = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSBlittableComponent>>();
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var spatialOSBehaviourManager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                    for (var j = 0; j < authoritiesChangedTags[i].Buffer.Count; j++)
                    {
                        spatialOSBehaviourManager.ChangeAuthority(1001, authoritiesChangedTags[i].Buffer[j]);
                    }
                }
            }

            public override void InvokeOnAuthorityChangeUserCallbacks(GameObjectDispatcherSystem gameObjectDispatcherSystem)
            {
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                var authChangeLists = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSBlittableComponent>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var manager = gameObjectDispatcherSystem.GetSpatialOSBehaviourManager(entities[i].Index);
                    if (!manager.TryGetReadersWriters(1001, out var readers))
                    {
                        continue;
                    }

                    var authChanges = authChangeLists[i];
                    foreach (BlittableComponent.ReaderWriterImpl reader in readers)
                    {
                        foreach (var auth in authChanges.Buffer)
                        {
                            reader.OnAuthorityChange(auth);
                        }
                    }
                }
            }
        }
    }
}

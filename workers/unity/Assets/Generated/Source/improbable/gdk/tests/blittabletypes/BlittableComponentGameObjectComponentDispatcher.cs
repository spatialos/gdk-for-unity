// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using System.Linq;
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

            public override void InvokeOnAuthorityChangeLifecycleCallbacks(Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers)
            {
                var authoritiesChangedTags = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSBlittableComponent>>();
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityIndexToManagers[entities[i].Index];
                    for (var j = 0; j < authoritiesChangedTags[i].Buffer.Count; j++)
                    {
                        activationManager.ChangeAuthority(1001, authoritiesChangedTags[i].Buffer[j]);
                    }
                }
            }

            public override void InvokeOnComponentUpdateUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores)
            {
                var entities = ComponentsUpdatedComponentGroup.GetEntityArray();
                var updateLists = ComponentsUpdatedComponentGroup.GetComponentArray<ComponentsUpdated<SpatialOSBlittableComponent.Update>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var readerWriterStore = readerWriterStores[entities[i].Index];
                    if (!readerWriterStore.TryGetReaderWritersForComponent(1001, out var readers))
                    {
                        continue;
                    }

                    var updateList = updateLists[i];
                    foreach (var reader in readers.OfType<BlittableComponent.ReaderWriterImpl>())
                    {
                        foreach (var update in updateList.Buffer)
                        {
                            reader.OnComponentUpdate(update);
                        }
                    }
                }
            }

            public override void InvokeOnEventUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores)
            {
                {
                    var entities = EventsReceivedComponentGroups[0].GetEntityArray();
                    var eventLists = EventsReceivedComponentGroups[0].GetComponentArray<EventsReceived<FirstEventEvent>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var readerWriterStore = readerWriterStores[entities[i].Index];
                        if (!readerWriterStore.TryGetReaderWritersForComponent(1001, out var readers))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (var reader in readers.OfType<BlittableComponent.ReaderWriterImpl>())
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
                        var readerWriterStore = readerWriterStores[entities[i].Index];
                        if (!readerWriterStore.TryGetReaderWritersForComponent(1001, out var readers))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (var reader in readers.OfType<BlittableComponent.ReaderWriterImpl>())
                        {
                            foreach (var e in eventList.Buffer)
                            {
                                reader.OnSecondEventEvent(e);
                            }
                        }
                    }
                }
            }

            public override void InvokeOnCommandRequestUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores)
            {
                {
                    var entities = CommandRequestsComponentGroups[0].GetEntityArray();
                    var commandLists = CommandRequestsComponentGroups[0].GetComponentArray<CommandRequests<FirstCommand.Request>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var readerWriterStore = readerWriterStores[entities[i].Index];
                        if (!readerWriterStore.TryGetReaderWritersForComponent(1001, out var readers))
                        {
                            continue;
                        }

                        var commandList = commandLists[i];
                        foreach (var reader in readers.OfType<BlittableComponent.ReaderWriterImpl>())
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
                        var readerWriterStore = readerWriterStores[entities[i].Index];
                        if (!readerWriterStore.TryGetReaderWritersForComponent(1001, out var readers))
                        {
                            continue;
                        }

                        var commandList = commandLists[i];
                        foreach (var reader in readers.OfType<BlittableComponent.ReaderWriterImpl>())
                        {
                            foreach (var req in commandList.Buffer)
                            {
                                reader.OnSecondCommandCommandRequest(req);
                            }
                        }
                    }
                }
            }

            public override void InvokeOnAuthorityChangeUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores)
            {
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                var authChangeLists = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSBlittableComponent>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var readerWriterStore = readerWriterStores[entities[i].Index];
                    if (!readerWriterStore.TryGetReaderWritersForComponent(1001, out var readers))
                    {
                        continue;
                    }

                    var authChanges = authChangeLists[i];
                    foreach (var reader in readers.OfType<BlittableComponent.ReaderWriterImpl>())
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

// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        internal class GameObjectComponentDispatcher : GameObjectComponentDispatcherBase
        {
            public override ComponentType[] ComponentAddedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentAdded<SpatialOSNonBlittableComponent>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentRemovedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentRemoved<SpatialOSNonBlittableComponent>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] AuthoritiesChangedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthoritiesChanged<SpatialOSNonBlittableComponent>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentsUpdated<SpatialOSNonBlittableComponent.Update>>(), ComponentType.ReadOnly<GameObjectReference>()
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
                return 1002;
            }

            public override void InvokeOnAuthorityChangeLifecycleCallbacks(Dictionary<int, SpatialOSBehaviourManager> entityIndexToManagers)
            {
                var authoritiesChangedTags = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSNonBlittableComponent>>();
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var spatialOSBehaviourManager = entityIndexToManagers[entities[i].Index];
                    for (var j = 0; j < authoritiesChangedTags[i].Buffer.Count; j++)
                    {
                        spatialOSBehaviourManager.ChangeAuthority(1002, authoritiesChangedTags[i].Buffer[j]);
                    }
                }
            }

            public override void InvokeOnComponentUpdateUserCallbacks(ReaderWriterStore readerWriterStore)
            {
                var entities = ComponentsUpdatedComponentGroup.GetEntityArray();
                var updateLists = ComponentsUpdatedComponentGroup.GetComponentArray<ComponentsUpdated<SpatialOSNonBlittableComponent.Update>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    if (!readerWriterStore.TryGetReaderWritersForComponent(1002, out var readers))
                    {
                        continue;
                    }

                    var updateList = updateLists[i];
                    foreach (var reader in readers.OfType<NonBlittableComponent.ReaderWriterImpl>())
                    {
                        foreach (var update in updateList.Buffer)
                        {
                            reader.OnComponentUpdate(update);
                        }
                    }
                }
            }

            public override void InvokeOnEventUserCallbacks(ReaderWriterStore readerWriterStore)
            {
                {
                    var entities = EventsReceivedComponentGroups[0].GetEntityArray();
                    var eventLists = EventsReceivedComponentGroups[0].GetComponentArray<EventsReceived<FirstEventEvent>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        if (!readerWriterStore.TryGetReaderWritersForComponent(1002, out var readers))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (var reader in readers.OfType<NonBlittableComponent.ReaderWriterImpl>())
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
                        if (!readerWriterStore.TryGetReaderWritersForComponent(1002, out var readers))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (var reader in readers.OfType<NonBlittableComponent.ReaderWriterImpl>())
                        {
                            foreach (var e in eventList.Buffer)
                            {
                                reader.OnSecondEventEvent(e);
                            }
                        }
                    }
                }
            }

            public override void InvokeOnCommandRequestUserCallbacks(ReaderWriterStore readerWriterStore)
            {
                {
                    var entities = CommandRequestsComponentGroups[0].GetEntityArray();
                    var commandLists = CommandRequestsComponentGroups[0].GetComponentArray<CommandRequests<FirstCommand.Request>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        if (!readerWriterStore.TryGetReaderWritersForComponent(1002, out var readers))
                        {
                            continue;
                        }

                        var commandList = commandLists[i];
                        foreach (var reader in readers.OfType<NonBlittableComponent.ReaderWriterImpl>())
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
                        if (!readerWriterStore.TryGetReaderWritersForComponent(1002, out var readers))
                        {
                            continue;
                        }

                        var commandList = commandLists[i];
                        foreach (var reader in readers.OfType<NonBlittableComponent.ReaderWriterImpl>())
                        {
                            foreach (var req in commandList.Buffer)
                            {
                                reader.OnSecondCommandCommandRequest(req);
                            }
                        }
                    }
                }
            }

            public override void InvokeOnAuthorityChangeUserCallbacks(ReaderWriterStore readerWriterStore)
            {
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                var authChangeLists = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSNonBlittableComponent>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    if (!readerWriterStore.TryGetReaderWritersForComponent(1002, out var readers))
                    {
                        continue;
                    }

                    var authChanges = authChangeLists[i];
                    foreach (var reader in readers.OfType<NonBlittableComponent.ReaderWriterImpl>())
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

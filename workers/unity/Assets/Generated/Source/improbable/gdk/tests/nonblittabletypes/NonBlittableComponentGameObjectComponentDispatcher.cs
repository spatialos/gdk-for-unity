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

            private const uint componentId = 1002;

            public override void MarkComponentsAddedForActivation(Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers)
            {
                if (ComponentAddedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entities = ComponentAddedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityIndexToManagers[entities[i].Index];
                    activationManager.AddComponent(componentId);
                }
            }

            public override void MarkComponentsRemovedForDeactivation(Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers)
            {
                if (ComponentRemovedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entities = ComponentRemovedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityIndexToManagers[entities[i].Index];
                    activationManager.RemoveComponent(componentId);
                }
            }

            public override void MarkAuthorityChangesForActivation(Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers)
            {
                if (AuthoritiesChangedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var authoritiesChangedTags = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSNonBlittableComponent>>();
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityIndexToManagers[entities[i].Index];
                    for (var j = 0; j < authoritiesChangedTags[i].Buffer.Count; j++)
                    {
                        activationManager.ChangeAuthority(componentId, authoritiesChangedTags[i].Buffer[j]);
                    }
                }
            }

            public override void InvokeOnComponentUpdateCallbacks(Dictionary<int, ReaderWriterStore> entityIdToReaderWriterStore)
            {
                if (ComponentsUpdatedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entities = ComponentsUpdatedComponentGroup.GetEntityArray();
                var updateLists = ComponentsUpdatedComponentGroup.GetComponentArray<ComponentsUpdated<SpatialOSNonBlittableComponent.Update>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var readerWriterStore = entityIdToReaderWriterStore[entities[i].Index];
                    if (!readerWriterStore.TryGetReaderWritersForComponent(componentId, out var readers))
                    {
                        continue;
                    }

                    var updateList = updateLists[i];
                    foreach (ReaderWriterImpl reader in readers)
                    {
                        foreach (var update in updateList.Buffer)
                        {
                            reader.OnComponentUpdate(update);
                        }
                    }
                }
            }

            public override void InvokeOnEventCallbacks(Dictionary<int, ReaderWriterStore> entityIdToReaderWriterStore)
            {
                if (!EventsReceivedComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var entities = EventsReceivedComponentGroups[0].GetEntityArray();
                    var eventLists = EventsReceivedComponentGroups[0].GetComponentArray<EventsReceived<FirstEventEvent>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var readerWriterStore = entityIdToReaderWriterStore[entities[i].Index];
                        if (!readerWriterStore.TryGetReaderWritersForComponent(componentId, out var readers))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (ReaderWriterImpl reader in readers)
                        {
                            foreach (var e in eventList.Buffer)
                            {
                                reader.OnFirstEventEvent(e);
                            }
                        }
                    }
                }
                if (!EventsReceivedComponentGroups[1].IsEmptyIgnoreFilter)
                {
                    var entities = EventsReceivedComponentGroups[1].GetEntityArray();
                    var eventLists = EventsReceivedComponentGroups[1].GetComponentArray<EventsReceived<SecondEventEvent>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var readerWriterStore = entityIdToReaderWriterStore[entities[i].Index];
                        if (!readerWriterStore.TryGetReaderWritersForComponent(componentId, out var readers))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (ReaderWriterImpl reader in readers)
                        {
                            foreach (var e in eventList.Buffer)
                            {
                                reader.OnSecondEventEvent(e);
                            }
                        }
                    }
                }
            }

            public override void InvokeOnCommandRequestCallbacks(Dictionary<int, ReaderWriterStore> entityIdToReaderWriterStore)
            {
                if (!CommandRequestsComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var entities = CommandRequestsComponentGroups[0].GetEntityArray();
                    var commandLists = CommandRequestsComponentGroups[0].GetComponentArray<CommandRequests<FirstCommand.Request>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var readerWriterStore = entityIdToReaderWriterStore[entities[i].Index];
                        if (!readerWriterStore.TryGetReaderWritersForComponent(componentId, out var readers))
                        {
                            continue;
                        }

                        var commandList = commandLists[i];
                        foreach (ReaderWriterImpl reader in readers)
                        {
                            foreach (var req in commandList.Buffer)
                            {
                                reader.OnFirstCommandCommandRequest(req);
                            }
                        }
                    }
                }
                if (!CommandRequestsComponentGroups[1].IsEmptyIgnoreFilter)
                {
                    var entities = CommandRequestsComponentGroups[1].GetEntityArray();
                    var commandLists = CommandRequestsComponentGroups[1].GetComponentArray<CommandRequests<SecondCommand.Request>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var readerWriterStore = entityIdToReaderWriterStore[entities[i].Index];
                        if (!readerWriterStore.TryGetReaderWritersForComponent(componentId, out var readers))
                        {
                            continue;
                        }

                        var commandList = commandLists[i];
                        foreach (ReaderWriterImpl reader in readers)
                        {
                            foreach (var req in commandList.Buffer)
                            {
                                reader.OnSecondCommandCommandRequest(req);
                            }
                        }
                    }
                }
            }

            public override void InvokeOnAuthorityChangeCallbacks(Dictionary<int, ReaderWriterStore> entityIdToReaderWriterStore)
            {
                if (AuthoritiesChangedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                var authChangeLists = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSNonBlittableComponent>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var readerWriterStore = entityIdToReaderWriterStore[entities[i].Index];
                    if (!readerWriterStore.TryGetReaderWritersForComponent(componentId, out var readers))
                    {
                        continue;
                    }

                    var authChanges = authChangeLists[i];
                    foreach (ReaderWriterImpl reader in readers)
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

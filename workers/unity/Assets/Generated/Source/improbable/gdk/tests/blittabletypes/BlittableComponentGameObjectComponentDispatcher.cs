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

            public override ComponentType[] AuthorityChangesComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthorityChanges<SpatialOSBlittableComponent>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<SpatialOSBlittableComponent.ReceivedUpdates>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[][] ReceivedEventsComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { ComponentType.ReadOnly<ReceivedEvents.FirstEvent>(), ComponentType.ReadOnly<GameObjectReference>() },
                new ComponentType[] { ComponentType.ReadOnly<ReceivedEvents.SecondEvent>(), ComponentType.ReadOnly<GameObjectReference>() },
            };

            public override ComponentType[][] CommandRequestsComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { ComponentType.ReadOnly<CommandRequests.FirstCommand>(), ComponentType.ReadOnly<GameObjectReference>() },
                new ComponentType[] { ComponentType.ReadOnly<CommandRequests.SecondCommand>(), ComponentType.ReadOnly<GameObjectReference>() },
            };

            private const uint componentId = 1001;

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
                if (AuthorityChangesComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var authoritiesChangedTags = AuthorityChangesComponentGroup.GetComponentDataArray<AuthorityChanges<SpatialOSBlittableComponent>>();
                var entities = AuthorityChangesComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityIndexToManagers[entities[i].Index];
                    for (var j = 0; j < authoritiesChangedTags[i].Changes.Count; j++)
                    {
                        activationManager.ChangeAuthority(componentId, authoritiesChangedTags[i].Changes[j]);
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
                var updateLists = ComponentsUpdatedComponentGroup.GetComponentDataArray<SpatialOSBlittableComponent.ReceivedUpdates>();
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
                        foreach (var update in updateList.Updates)
                        {
                            reader.OnComponentUpdate(update);
                        }
                    }
                }
            }

            public override void InvokeOnEventCallbacks(Dictionary<int, ReaderWriterStore> entityIdToReaderWriterStore)
            {
                if (!ReceivedEventsComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var entities = ReceivedEventsComponentGroups[0].GetEntityArray();
                    var eventLists = ReceivedEventsComponentGroups[0].GetComponentDataArray<ReceivedEvents.FirstEvent>();
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
                            foreach (var e in eventList.Events)
                            {
                                reader.OnFirstEventEvent(e);
                            }
                        }
                    }
                }
                if (!ReceivedEventsComponentGroups[1].IsEmptyIgnoreFilter)
                {
                    var entities = ReceivedEventsComponentGroups[1].GetEntityArray();
                    var eventLists = ReceivedEventsComponentGroups[1].GetComponentDataArray<ReceivedEvents.SecondEvent>();
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
                            foreach (var e in eventList.Events)
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
                    var commandLists = CommandRequestsComponentGroups[0].GetComponentDataArray<CommandRequests.FirstCommand>();
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
                            foreach (var req in commandList.Requests)
                            {
                                reader.OnFirstCommandCommandRequest(req);
                            }
                        }
                    }
                }
                if (!CommandRequestsComponentGroups[1].IsEmptyIgnoreFilter)
                {
                    var entities = CommandRequestsComponentGroups[1].GetEntityArray();
                    var commandLists = CommandRequestsComponentGroups[1].GetComponentDataArray<CommandRequests.SecondCommand>();
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
                            foreach (var req in commandList.Requests)
                            {
                                reader.OnSecondCommandCommandRequest(req);
                            }
                        }
                    }
                }
            }

            public override void InvokeOnAuthorityChangeCallbacks(Dictionary<int, ReaderWriterStore> entityIdToReaderWriterStore)
            {
                if (AuthorityChangesComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entities = AuthorityChangesComponentGroup.GetEntityArray();
                var authChangeLists = AuthorityChangesComponentGroup.GetComponentDataArray<AuthorityChanges<SpatialOSBlittableComponent>>();
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
                        foreach (var auth in authChanges.Changes)
                        {
                            reader.OnAuthorityChange(auth);
                        }
                    }
                }
            }
        }
    }
}

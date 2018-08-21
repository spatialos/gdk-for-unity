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

            private const uint componentId = 1001;
            private static readonly InjectableId reaederWriterInjectableId = new InjectableId(InjectableType.ReaderWriter, componentId);
            private static readonly InjectableId commandRequestHandlerInjectableId = new InjectableId(InjectableType.CommandRequestHandler, componentId);

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

                var authoritiesChangedTags = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSBlittableComponent>>();
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

            public override void InvokeOnComponentUpdateCallbacks(Dictionary<int, InjectableStore> entityIdToInjectableStore)
            {
                if (ComponentsUpdatedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entities = ComponentsUpdatedComponentGroup.GetEntityArray();
                var updateLists = ComponentsUpdatedComponentGroup.GetComponentArray<ComponentsUpdated<SpatialOSBlittableComponent.Update>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityIdToInjectableStore[entities[i].Index];
                    if (!injectableStore.TryGetInjectablesForComponent(reaederWriterInjectableId, out var readers))
                    {
                        continue;
                    }

                    var updateList = updateLists[i];
                    foreach (Requirables.ReaderWriterImpl reader in readers)
                    {
                        foreach (var update in updateList.Buffer)
                        {
                            reader.OnComponentUpdate(update);
                        }
                    }
                }
            }

            public override void InvokeOnEventCallbacks(Dictionary<int, InjectableStore> entityIdToInjectableStore)
            {
                if (!EventsReceivedComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var entities = EventsReceivedComponentGroups[0].GetEntityArray();
                    var eventLists = EventsReceivedComponentGroups[0].GetComponentArray<EventsReceived<FirstEventEvent>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityIdToInjectableStore[entities[i].Index];
                        if (!injectableStore.TryGetInjectablesForComponent(reaederWriterInjectableId, out var readers))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (Requirables.ReaderWriterImpl reader in readers)
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
                        var injectableStore = entityIdToInjectableStore[entities[i].Index];
                        if (!injectableStore.TryGetInjectablesForComponent(reaederWriterInjectableId, out var readers))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (Requirables.ReaderWriterImpl reader in readers)
                        {
                            foreach (var e in eventList.Buffer)
                            {
                                reader.OnSecondEventEvent(e);
                            }
                        }
                    }
                }

            }

            public override void InvokeOnCommandRequestCallbacks(Dictionary<int, InjectableStore> entityIdToInjectableStore)
            {
                if (!CommandRequestsComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var entities = CommandRequestsComponentGroups[0].GetEntityArray();
                    var commandRequestLists = CommandRequestsComponentGroups[0].GetComponentArray<CommandRequests<FirstCommand.Request>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityIdToInjectableStore[entities[i].Index];
                        if (!injectableStore.TryGetInjectablesForComponent(commandRequestHandlerInjectableId, out var commandRequestHandlers))
                        {
                            continue;
                        }

                        var commandRequestList = commandRequestLists[i];

                        foreach (Requirables.CommandRequestHandler commandRequestHandler in commandRequestHandlers)
                        {
                            foreach (var commandRequest in commandRequestList.Buffer)
                            {
                                commandRequestHandler.OnFirstCommandRequestInternal(commandRequest);
                            }
                        }
                    }
                }

                if (!CommandRequestsComponentGroups[1].IsEmptyIgnoreFilter)
                {
                    var entities = CommandRequestsComponentGroups[1].GetEntityArray();
                    var commandRequestLists = CommandRequestsComponentGroups[1].GetComponentArray<CommandRequests<SecondCommand.Request>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityIdToInjectableStore[entities[i].Index];
                        if (!injectableStore.TryGetInjectablesForComponent(commandRequestHandlerInjectableId, out var commandRequestHandlers))
                        {
                            continue;
                        }

                        var commandRequestList = commandRequestLists[i];

                        foreach (Requirables.CommandRequestHandler commandRequestHandler in commandRequestHandlers)
                        {
                            foreach (var commandRequest in commandRequestList.Buffer)
                            {
                                commandRequestHandler.OnSecondCommandRequestInternal(commandRequest);
                            }
                        }
                    }
                }

            }

            public override void InvokeOnAuthorityChangeCallbacks(Dictionary<int, InjectableStore> entityIdToInjectableStore)
            {
                if (AuthoritiesChangedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                var authChangeLists = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSBlittableComponent>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityIdToInjectableStore[entities[i].Index];
                    if (!injectableStore.TryGetInjectablesForComponent(reaederWriterInjectableId, out var readers))
                    {
                        continue;
                    }

                    var authChanges = authChangeLists[i];
                    foreach (Requirables.ReaderWriterImpl reader in readers)
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

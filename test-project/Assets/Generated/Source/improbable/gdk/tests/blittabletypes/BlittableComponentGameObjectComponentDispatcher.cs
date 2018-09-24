// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine.Profiling;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Worker.Core;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        internal class GameObjectComponentDispatcher : GameObjectComponentDispatcherBase
        {
            public override ComponentType[] ComponentAddedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentAdded<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentRemovedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentRemoved<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] AuthorityGainedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(), ComponentType.ReadOnly<GameObjectReference>(),
                ComponentType.ReadOnly<Authoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>()
            };

            public override ComponentType[] AuthorityLostComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(), ComponentType.ReadOnly<GameObjectReference>(),
                ComponentType.ReadOnly<NotAuthoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>()
            };

            public override ComponentType[] AuthorityLossImminentComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(), ComponentType.ReadOnly<GameObjectReference>(),
                ComponentType.ReadOnly<AuthorityLossImminent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>()
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[][] EventsReceivedComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { ComponentType.ReadOnly<ReceivedEvents.FirstEvent>(), ComponentType.ReadOnly<GameObjectReference>() },
                new ComponentType[] { ComponentType.ReadOnly<ReceivedEvents.SecondEvent>(), ComponentType.ReadOnly<GameObjectReference>() },
            };

            public override ComponentType[][] CommandRequestsComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { ComponentType.ReadOnly<CommandRequests.FirstCommand>(), ComponentType.ReadOnly<GameObjectReference>() },
                new ComponentType[] { ComponentType.ReadOnly<CommandRequests.SecondCommand>(), ComponentType.ReadOnly<GameObjectReference>() },
            };

            public override ComponentType[][] CommandResponsesComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { ComponentType.ReadOnly<CommandResponses.FirstCommand>(), ComponentType.ReadOnly<GameObjectReference>() },
                new ComponentType[] { ComponentType.ReadOnly<CommandResponses.SecondCommand>(), ComponentType.ReadOnly<GameObjectReference>() },
            };

            private const uint componentId = 1001;
            private static readonly InjectableId readerWriterInjectableId = new InjectableId(InjectableType.ReaderWriter, componentId);
            private static readonly InjectableId commandRequestHandlerInjectableId = new InjectableId(InjectableType.CommandRequestHandler, componentId);
            private static readonly InjectableId commandResponseHandlerInjectableId = new InjectableId(InjectableType.CommandResponseHandler, componentId);

            public override void MarkComponentsAddedForActivation(Dictionary<Unity.Entities.Entity, MonoBehaviourActivationManager> entityToManagers)
            {
                if (ComponentAddedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("BlittableComponent");
                var entities = ComponentAddedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityToManagers[entities[i]];
                    activationManager.AddComponent(componentId);
                }

                Profiler.EndSample();
            }

            public override void MarkComponentsRemovedForDeactivation(Dictionary<Unity.Entities.Entity, MonoBehaviourActivationManager> entityToManagers)
            {
                if (ComponentRemovedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("BlittableComponent");
                var entities = ComponentRemovedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityToManagers[entities[i]];
                    activationManager.RemoveComponent(componentId);
                }

                Profiler.EndSample();
            }

            public override void MarkAuthorityGainedForActivation(Dictionary<Unity.Entities.Entity, MonoBehaviourActivationManager> entityToManagers)
            {
                if (AuthorityGainedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("BlittableComponent");
                var authoritiesChangedTags = AuthorityGainedComponentGroup.GetComponentDataArray<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>();
                var entities = AuthorityGainedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityToManagers[entities[i]];
                    // Call once except if flip-flopped back to starting state
                    if (IsFirstAuthChange(Authority.Authoritative, authoritiesChangedTags[i]))
                    {
                        activationManager.ChangeAuthority(componentId, Authority.Authoritative);
                    }
                }

                Profiler.EndSample();
            }

            public override void MarkAuthorityLostForDeactivation(Dictionary<Unity.Entities.Entity, MonoBehaviourActivationManager> entityToManagers)
            {
                if (AuthorityLostComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("BlittableComponent");
                var authoritiesChangedTags = AuthorityLostComponentGroup.GetComponentDataArray<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>();
                var entities = AuthorityLostComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityToManagers[entities[i]];
                    // Call once except if flip-flopped back to starting state
                    if (IsFirstAuthChange(Authority.NotAuthoritative, authoritiesChangedTags[i]))
                    {
                        activationManager.ChangeAuthority(componentId, Authority.NotAuthoritative);
                    }
                }

                Profiler.EndSample();
            }

            public override void InvokeOnComponentUpdateCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                if (ComponentsUpdatedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("BlittableComponent");
                var entities = ComponentsUpdatedComponentGroup.GetEntityArray();
                var updateLists = ComponentsUpdatedComponentGroup.GetComponentDataArray<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityToInjectableStore[entities[i]];
                    if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                    {
                        continue;
                    }

                    var updateList = updateLists[i];
                    foreach (Requirable.ReaderWriterImpl readerWriter in readersWriters)
                    {
                        foreach (var update in updateList.Updates)
                        {
                            readerWriter.OnComponentUpdate(update);
                        }
                    }
                }

                Profiler.EndSample();
            }

            public override void InvokeOnEventCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                Profiler.BeginSample("BlittableComponent");
                if (!EventsReceivedComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var entities = EventsReceivedComponentGroups[0].GetEntityArray();
                    var eventLists = EventsReceivedComponentGroups[0].GetComponentDataArray<ReceivedEvents.FirstEvent>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityToInjectableStore[entities[i]];
                        if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];
                        foreach (Requirable.ReaderWriterImpl readerWriter in readersWriters)
                        {
                            foreach (var e in eventList.Events)
                            {
                                readerWriter.OnFirstEventEvent(e);
                            }
                        }
                    }
                }

                if (!EventsReceivedComponentGroups[1].IsEmptyIgnoreFilter)
                {
                    var entities = EventsReceivedComponentGroups[1].GetEntityArray();
                    var eventLists = EventsReceivedComponentGroups[1].GetComponentDataArray<ReceivedEvents.SecondEvent>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityToInjectableStore[entities[i]];
                        if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];
                        foreach (Requirable.ReaderWriterImpl readerWriter in readersWriters)
                        {
                            foreach (var e in eventList.Events)
                            {
                                readerWriter.OnSecondEventEvent(e);
                            }
                        }
                    }
                }

                Profiler.EndSample();
            }

            public override void InvokeOnCommandRequestCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                Profiler.BeginSample("BlittableComponent");
                if (!CommandRequestsComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var entities = CommandRequestsComponentGroups[0].GetEntityArray();
                    var commandRequestLists = CommandRequestsComponentGroups[0].GetComponentDataArray<CommandRequests.FirstCommand>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityToInjectableStore[entities[i]];
                        if (!injectableStore.TryGetInjectablesForComponent(commandRequestHandlerInjectableId, out var commandRequestHandlers))
                        {
                            continue;
                        }

                        var commandRequestList = commandRequestLists[i];
                        foreach (Requirable.CommandRequestHandler commandRequestHandler in commandRequestHandlers)
                        {
                            foreach (var commandRequest in commandRequestList.Requests)
                            {
                                commandRequestHandler.OnFirstCommandRequestInternal(commandRequest);
                            }
                        }
                    }
                }

                if (!CommandRequestsComponentGroups[1].IsEmptyIgnoreFilter)
                {
                    var entities = CommandRequestsComponentGroups[1].GetEntityArray();
                    var commandRequestLists = CommandRequestsComponentGroups[1].GetComponentDataArray<CommandRequests.SecondCommand>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityToInjectableStore[entities[i]];
                        if (!injectableStore.TryGetInjectablesForComponent(commandRequestHandlerInjectableId, out var commandRequestHandlers))
                        {
                            continue;
                        }

                        var commandRequestList = commandRequestLists[i];
                        foreach (Requirable.CommandRequestHandler commandRequestHandler in commandRequestHandlers)
                        {
                            foreach (var commandRequest in commandRequestList.Requests)
                            {
                                commandRequestHandler.OnSecondCommandRequestInternal(commandRequest);
                            }
                        }
                    }
                }

                Profiler.EndSample();
            }

            public override void InvokeOnCommandResponseCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                Profiler.BeginSample("BlittableComponent");

                if (!CommandResponsesComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var entities = CommandResponsesComponentGroups[0].GetEntityArray();
                    var commandResponseLists = CommandResponsesComponentGroups[0].GetComponentDataArray<CommandResponses.FirstCommand>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityToInjectableStore[entities[i]];
                        if (!injectableStore.TryGetInjectablesForComponent(commandResponseHandlerInjectableId, out var commandResponseHandlers))
                        {
                            continue;
                        }

                        var commandResponseList = commandResponseLists[i];
                        foreach (Requirable.CommandResponseHandler commandResponseHandler in commandResponseHandlers)
                        {
                            foreach (var commandResponse in commandResponseList.Responses)
                            {
                                commandResponseHandler.OnFirstCommandResponseInternal(commandResponse);
                            }
                        }
                    }
                }

                if (!CommandResponsesComponentGroups[1].IsEmptyIgnoreFilter)
                {
                    var entities = CommandResponsesComponentGroups[1].GetEntityArray();
                    var commandResponseLists = CommandResponsesComponentGroups[1].GetComponentDataArray<CommandResponses.SecondCommand>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityToInjectableStore[entities[i]];
                        if (!injectableStore.TryGetInjectablesForComponent(commandResponseHandlerInjectableId, out var commandResponseHandlers))
                        {
                            continue;
                        }

                        var commandResponseList = commandResponseLists[i];
                        foreach (Requirable.CommandResponseHandler commandResponseHandler in commandResponseHandlers)
                        {
                            foreach (var commandResponse in commandResponseList.Responses)
                            {
                                commandResponseHandler.OnSecondCommandResponseInternal(commandResponse);
                            }
                        }
                    }
                }
                Profiler.EndSample();
            }

            public override void InvokeOnAuthorityGainedCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                if (AuthorityGainedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("BlittableComponent");
                var entities = AuthorityGainedComponentGroup.GetEntityArray();
                var changeOpsLists = AuthorityGainedComponentGroup.GetComponentDataArray<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>();

                // Call once on all entities unless they flip-flopped back into the state they started in
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityToInjectableStore[entities[i]];
                    if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                    {
                        continue;
                    }

                    if (IsFirstAuthChange(Authority.Authoritative, changeOpsLists[i]))
                    {
                        foreach (Requirable.ReaderWriterImpl readerWriter in readersWriters)
                        {
                            readerWriter.OnAuthorityChange(Authority.Authoritative);
                        }
                    }
                }

                Profiler.EndSample();
            }

            public override void InvokeOnAuthorityLostCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                if (AuthorityLostComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("BlittableComponent");
                var entities = AuthorityLostComponentGroup.GetEntityArray();
                var changeOpsLists = AuthorityLostComponentGroup.GetComponentDataArray<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>();

                // Call once on all entities unless they flip-flopped back into the state they started in
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityToInjectableStore[entities[i]];
                    if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                    {
                        continue;
                    }

                    if (IsFirstAuthChange(Authority.NotAuthoritative, changeOpsLists[i]))
                    {
                        foreach (Requirable.ReaderWriterImpl readerWriter in readersWriters)
                        {
                            readerWriter.OnAuthorityChange(Authority.NotAuthoritative);
                        }
                    }
                }

                Profiler.EndSample();
            }

            private bool IsFirstAuthChange(Authority authToMatch, AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component> changeOps)
            {
                foreach (var auth in changeOps.Changes)
                {
                    if (auth != Authority.AuthorityLossImminent) // not relevant
                    {
                        return auth == authToMatch;
                    }
                }

                return false;
            }

            public override void InvokeOnAuthorityLossImminentCallbacks(Dictionary<Unity.Entities.Entity, InjectableStore> entityToInjectableStore)
            {
                if (AuthorityLossImminentComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                Profiler.BeginSample("BlittableComponent");
                var entities = AuthorityLossImminentComponentGroup.GetEntityArray();

                // Call once on all entities
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityToInjectableStore[entities[i]];
                    if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                    {
                        continue;
                    }

                    foreach (Requirable.ReaderWriterImpl readerWriter in readersWriters)
                    {
                        readerWriter.OnAuthorityChange(Authority.AuthorityLossImminent);
                    }
                }

                Profiler.EndSample();
            }
        }
    }
}

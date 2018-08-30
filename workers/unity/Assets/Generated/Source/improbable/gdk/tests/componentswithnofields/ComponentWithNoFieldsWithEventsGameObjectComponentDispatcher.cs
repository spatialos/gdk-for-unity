// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Worker.Core;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithEvents
    {
        internal class GameObjectComponentDispatcher : GameObjectComponentDispatcherBase
        {
            public override ComponentType[] ComponentAddedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentRemovedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] AuthorityGainedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>(), ComponentType.ReadOnly<GameObjectReference>(),
                ComponentType.ReadOnly<Authoritative<SpatialOSComponentWithNoFieldsWithEvents>>()
            };

            public override ComponentType[] AuthorityLostComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>(), ComponentType.ReadOnly<GameObjectReference>(),
                ComponentType.ReadOnly<NotAuthoritative<SpatialOSComponentWithNoFieldsWithEvents>>()
            };

            public override ComponentType[] AuthorityLossImminentComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>(), ComponentType.ReadOnly<GameObjectReference>(),
                ComponentType.ReadOnly<AuthorityLossImminent<SpatialOSComponentWithNoFieldsWithEvents>>()
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
            };

            public override ComponentType[][] EventsReceivedComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { ComponentType.ReadOnly<ReceivedEvents.Evt>(), ComponentType.ReadOnly<GameObjectReference>() },
            };

            public override ComponentType[][] CommandRequestsComponentTypeArrays => new ComponentType[][]
            {
            };

            private const uint componentId = 1004;
            private static readonly InjectableId readerWriterInjectableId = new InjectableId(InjectableType.ReaderWriter, componentId);

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

            public override void MarkAuthorityGainedForActivation(Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers)
            {
                if (AuthorityGainedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var authoritiesChangedTags = AuthorityGainedComponentGroup.GetComponentDataArray<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>();
                var entities = AuthorityGainedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityIndexToManagers[entities[i].Index];
                    // Call once except if flip-flopped back to starting state
                    if (IsFirstAuthChange(Authority.Authoritative, authoritiesChangedTags[i]))
                    {
                        activationManager.ChangeAuthority(componentId, Authority.Authoritative);
                    }
                }
            }

            public override void MarkAuthorityLostForDeactivation(Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers)
            {
                if (AuthorityLostComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var authoritiesChangedTags = AuthorityLostComponentGroup.GetComponentDataArray<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>();
                var entities = AuthorityLostComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityIndexToManagers[entities[i].Index];
                    // Call once except if flip-flopped back to starting state
                    if (IsFirstAuthChange(Authority.NotAuthoritative, authoritiesChangedTags[i]))
                    {
                        activationManager.ChangeAuthority(componentId, Authority.NotAuthoritative);
                    }
                }
            }

            public override void InvokeOnComponentUpdateCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore)
            {
            }

            public override void InvokeOnEventCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore)
            {
                if (!EventsReceivedComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var entities = EventsReceivedComponentGroups[0].GetEntityArray();
                    var eventLists = EventsReceivedComponentGroups[0].GetComponentDataArray<ReceivedEvents.Evt>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var injectableStore = entityIndexToInjectableStore[entities[i].Index];
                        if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (Requirables.ReaderWriterImpl readerWriter in readersWriters)
                        {
                            foreach (var e in eventList.Events)
                            {
                                readerWriter.OnEvtEvent(e);
                            }
                        }
                    }
                }

            }

            public override void InvokeOnCommandRequestCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore)
            {
            }

            public override void InvokeOnCommandResponseCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore)
            {
                // TODO UTY-542 Command Response handlers
            }

            public override void InvokeOnAuthorityGainedCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore)
            {
                if (AuthorityGainedComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entities = AuthorityGainedComponentGroup.GetEntityArray();
                var changeOpsLists = AuthorityGainedComponentGroup.GetComponentDataArray<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>();

                // Call once on all entities unless they flip-flopped back into the state they started in
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityIndexToInjectableStore[entities[i].Index];
                    if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                    {
                        continue;
                    }

                    if (IsFirstAuthChange(Authority.Authoritative, changeOpsLists[i]))
                    {
                        foreach (Requirables.ReaderWriterImpl readerWriter in readersWriters)
                        {
                            readerWriter.OnAuthorityChange(Authority.Authoritative);
                        }
                    }
                }
            }

            public override void InvokeOnAuthorityLostCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore)
            {
                if (AuthorityLostComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entities = AuthorityLostComponentGroup.GetEntityArray();
                var changeOpsLists = AuthorityLostComponentGroup.GetComponentDataArray<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>();

                // Call once on all entities unless they flip-flopped back into the state they started in
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityIndexToInjectableStore[entities[i].Index];
                    if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                    {
                        continue;
                    }

                    if (IsFirstAuthChange(Authority.NotAuthoritative, changeOpsLists[i]))
                    {
                        foreach (Requirables.ReaderWriterImpl readerWriter in readersWriters)
                        {
                            readerWriter.OnAuthorityChange(Authority.NotAuthoritative);
                        }
                    }
                }
            }

            private bool IsFirstAuthChange(Authority authToMatch, AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents> changeOps)
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

            public override void InvokeOnAuthorityLossImminentCallbacks(Dictionary<int, InjectableStore> entityIndexToInjectableStore)
            {
                if (AuthorityLossImminentComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entities = AuthorityLossImminentComponentGroup.GetEntityArray();

                // Call once on all entities
                for (var i = 0; i < entities.Length; i++)
                {
                    var injectableStore = entityIndexToInjectableStore[entities[i].Index];
                    if (!injectableStore.TryGetInjectablesForComponent(readerWriterInjectableId, out var readersWriters))
                    {
                        continue;
                    }
                    foreach (Requirables.ReaderWriterImpl readerWriter in readersWriters)
                    {
                        readerWriter.OnAuthorityChange(Authority.AuthorityLossImminent);
                    }
                }
            }
        }
    }
}

// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;

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

            public override ComponentType[] AuthoritiesChangedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthoritiesChanged<SpatialOSComponentWithNoFieldsWithEvents>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
            };

            public override ComponentType[][] EventsReceivedComponentTypeArrays => new ComponentType[][]
            {
                new ComponentType[] { ComponentType.ReadOnly<EventsReceived<EvtEvent>>(), ComponentType.ReadOnly<GameObjectReference>() },
            };

            public override ComponentType[][] CommandRequestsComponentTypeArrays => new ComponentType[][]
            {
            };

            protected override uint GetComponentId()
            {
                return 1004;
            }

            public override void InvokeOnAuthorityChangeLifecycleCallbacks(Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers)
            {
                var authoritiesChangedTags = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSComponentWithNoFieldsWithEvents>>();
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityIndexToManagers[entities[i].Index];
                    for (var j = 0; j < authoritiesChangedTags[i].Buffer.Count; j++)
                    {
                        activationManager.ChangeAuthority(1004, authoritiesChangedTags[i].Buffer[j]);
                    }
                }
            }

            public override void InvokeOnComponentUpdateUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores)
            {
            }

            public override void InvokeOnEventUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores)
            {
                {
                    var entities = EventsReceivedComponentGroups[0].GetEntityArray();
                    var eventLists = EventsReceivedComponentGroups[0].GetComponentArray<EventsReceived<EvtEvent>>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        var readerWriterStore = readerWriterStores[entities[i].Index];
                        if (!readerWriterStore.TryGetReaderWritersForComponent(1004, out var readers))
                        {
                            continue;
                        }

                        var eventList = eventLists[i];

                        foreach (var reader in readers.OfType<ComponentWithNoFieldsWithEvents.ReaderWriterImpl>())
                        {
                            foreach (var e in eventList.Buffer)
                            {
                                reader.OnEvtEvent(e);
                            }
                        }
                    }
                }
            }

            public override void InvokeOnCommandRequestUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores)
            {
            }

            public override void InvokeOnAuthorityChangeUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores)
            {
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                var authChangeLists = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSComponentWithNoFieldsWithEvents>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var readerWriterStore = readerWriterStores[entities[i].Index];
                    if (!readerWriterStore.TryGetReaderWritersForComponent(1004, out var readers))
                    {
                        continue;
                    }

                    var authChanges = authChangeLists[i];
                    foreach (var reader in readers.OfType<ComponentWithNoFieldsWithEvents.ReaderWriterImpl>())
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

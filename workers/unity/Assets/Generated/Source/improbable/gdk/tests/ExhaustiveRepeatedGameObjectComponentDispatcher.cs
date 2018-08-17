// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;

namespace Generated.Improbable.Gdk.Tests
{
    public partial class ExhaustiveRepeated
    {
        internal class GameObjectComponentDispatcher : GameObjectComponentDispatcherBase
        {
            public override ComponentType[] ComponentAddedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentAdded<SpatialOSExhaustiveRepeated>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentRemovedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentRemoved<SpatialOSExhaustiveRepeated>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] AuthorityChangesComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthorityChanges<SpatialOSExhaustiveRepeated>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<SpatialOSExhaustiveRepeated.ReceivedUpdates>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[][] ReceivedEventsComponentTypeArrays => new ComponentType[][]
            {
            };

            public override ComponentType[][] CommandRequestsComponentTypeArrays => new ComponentType[][]
            {
            };

            private const uint componentId = 197717;

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

                var authoritiesChangedTags = AuthorityChangesComponentGroup.GetComponentDataArray<AuthorityChanges<SpatialOSExhaustiveRepeated>>();
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
                var updateLists = ComponentsUpdatedComponentGroup.GetComponentDataArray<SpatialOSExhaustiveRepeated.ReceivedUpdates>();
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
            }

            public override void InvokeOnCommandRequestCallbacks(Dictionary<int, ReaderWriterStore> entityIdToReaderWriterStore)
            {
            }

            public override void InvokeOnAuthorityChangeCallbacks(Dictionary<int, ReaderWriterStore> entityIdToReaderWriterStore)
            {
                if (AuthorityChangesComponentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entities = AuthorityChangesComponentGroup.GetEntityArray();
                var authChangeLists = AuthorityChangesComponentGroup.GetComponentDataArray<AuthorityChanges<SpatialOSExhaustiveRepeated>>();
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

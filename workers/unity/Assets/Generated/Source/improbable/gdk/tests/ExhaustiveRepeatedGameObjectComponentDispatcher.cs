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

            public override ComponentType[] AuthoritiesChangedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<AuthoritiesChanged<SpatialOSExhaustiveRepeated>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[] ComponentsUpdatedComponentTypes => new ComponentType[]
            {
                ComponentType.ReadOnly<ComponentsUpdated<SpatialOSExhaustiveRepeated.Update>>(), ComponentType.ReadOnly<GameObjectReference>()
            };

            public override ComponentType[][] EventsReceivedComponentTypeArrays => new ComponentType[][]
            {
            };

            public override ComponentType[][] CommandRequestsComponentTypeArrays => new ComponentType[][]
            {
            };

            protected override uint GetComponentId()
            {
                return 197717;
            }

            public override void InvokeOnAuthorityChangeLifecycleCallbacks(Dictionary<int, MonoBehaviourActivationManager> entityIndexToManagers)
            {
                var authoritiesChangedTags = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSExhaustiveRepeated>>();
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    var activationManager = entityIndexToManagers[entities[i].Index];
                    for (var j = 0; j < authoritiesChangedTags[i].Buffer.Count; j++)
                    {
                        activationManager.ChangeAuthority(197717, authoritiesChangedTags[i].Buffer[j]);
                    }
                }
            }

            public override void InvokeOnComponentUpdateUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores)
            {
                var entities = ComponentsUpdatedComponentGroup.GetEntityArray();
                var updateLists = ComponentsUpdatedComponentGroup.GetComponentArray<ComponentsUpdated<SpatialOSExhaustiveRepeated.Update>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var readerWriterStore = readerWriterStores[entities[i].Index];
                    if (!readerWriterStore.TryGetReaderWritersForComponent(197717, out var readers))
                    {
                        continue;
                    }

                    var updateList = updateLists[i];
                    foreach (var reader in readers.OfType<ExhaustiveRepeated.ReaderWriterImpl>())
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
            }

            public override void InvokeOnCommandRequestUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores)
            {
            }

            public override void InvokeOnAuthorityChangeUserCallbacks(Dictionary<int, ReaderWriterStore> readerWriterStores)
            {
                var entities = AuthoritiesChangedComponentGroup.GetEntityArray();
                var authChangeLists = AuthoritiesChangedComponentGroup.GetComponentArray<AuthoritiesChanged<SpatialOSExhaustiveRepeated>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    var readerWriterStore = readerWriterStores[entities[i].Index];
                    if (!readerWriterStore.TryGetReaderWritersForComponent(197717, out var readers))
                    {
                        continue;
                    }

                    var authChanges = authChangeLists[i];
                    foreach (var reader in readers.OfType<ExhaustiveRepeated.ReaderWriterImpl>())
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

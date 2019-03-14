// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.CodegenAdapters;

namespace Improbable.Gdk.Tests
{
    public partial class ExhaustiveSingular
    {
        internal class ComponentReplicator : IComponentReplicationHandler
        {
            public uint ComponentId => 197715;

            public EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<Improbable.Gdk.Tests.ExhaustiveSingular.Component>(),
                    ComponentType.Create<Improbable.Gdk.Tests.ExhaustiveSingular.ComponentAuthority>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
            };

            public void SendUpdates(
                NativeArray<ArchetypeChunk> chunkArray,
                ComponentSystemBase system,
                EntityManager entityManager,
                ComponentUpdateSystem componentUpdateSystem)
            {
                Profiler.BeginSample("ExhaustiveSingular");

                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.ExhaustiveSingular.Component>();

                var authorityType = system.GetArchetypeChunkSharedComponentType<ComponentAuthority>();

                foreach (var chunk in chunkArray)
                {
                    var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                    var componentArray = chunk.GetNativeArray(componentType);

                    var authorityIndex = chunk.GetSharedComponentIndex(authorityType);

                    if (!entityManager.GetSharedComponentData<ComponentAuthority>(authorityIndex).HasAuthority)
                    {
                        continue;
                    }

                    for (var i = 0; i < componentArray.Length; i++)
                    {
                        var data = componentArray[i];
                        if (data.IsDataDirty())
                        {
                            Update update = new Update();

                            if (data.IsDataDirty(0))
                            {
                                update.Field1 = data.Field1;
                            }

                            if (data.IsDataDirty(1))
                            {
                                update.Field2 = data.Field2;
                            }

                            if (data.IsDataDirty(2))
                            {
                                update.Field3 = data.Field3;
                            }

                            if (data.IsDataDirty(3))
                            {
                                update.Field4 = data.Field4;
                            }

                            if (data.IsDataDirty(4))
                            {
                                update.Field5 = data.Field5;
                            }

                            if (data.IsDataDirty(5))
                            {
                                update.Field6 = data.Field6;
                            }

                            if (data.IsDataDirty(6))
                            {
                                update.Field7 = data.Field7;
                            }

                            if (data.IsDataDirty(7))
                            {
                                update.Field8 = data.Field8;
                            }

                            if (data.IsDataDirty(8))
                            {
                                update.Field9 = data.Field9;
                            }

                            if (data.IsDataDirty(9))
                            {
                                update.Field10 = data.Field10;
                            }

                            if (data.IsDataDirty(10))
                            {
                                update.Field11 = data.Field11;
                            }

                            if (data.IsDataDirty(11))
                            {
                                update.Field12 = data.Field12;
                            }

                            if (data.IsDataDirty(12))
                            {
                                update.Field13 = data.Field13;
                            }

                            if (data.IsDataDirty(13))
                            {
                                update.Field14 = data.Field14;
                            }

                            if (data.IsDataDirty(14))
                            {
                                update.Field15 = data.Field15;
                            }

                            if (data.IsDataDirty(15))
                            {
                                update.Field16 = data.Field16;
                            }

                            if (data.IsDataDirty(16))
                            {
                                update.Field17 = data.Field17;
                            }

                            if (data.IsDataDirty(17))
                            {
                                update.Field18 = data.Field18;
                            }

                            componentUpdateSystem.SendUpdate(in update, entityIdArray[i].EntityId);
                            data.MarkDataClean();
                            componentArray[i] = data;
                        }
                    }
                }

                Profiler.EndSample();
            }
        }
    }
}

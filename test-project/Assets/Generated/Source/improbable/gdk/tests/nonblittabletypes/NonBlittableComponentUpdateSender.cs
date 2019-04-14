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

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        internal class ComponentReplicator : IComponentReplicationHandler
        {
            public uint ComponentId => 1002;

            public EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>(),
                    ComponentType.Create<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ComponentAuthority>(),
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
                Profiler.BeginSample("NonBlittableComponent");

                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>();

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
                                update.BoolField = data.BoolField;
                            }

                            if (data.IsDataDirty(1))
                            {
                                update.IntField = data.IntField;
                            }

                            if (data.IsDataDirty(2))
                            {
                                update.LongField = data.LongField;
                            }

                            if (data.IsDataDirty(3))
                            {
                                update.FloatField = data.FloatField;
                            }

                            if (data.IsDataDirty(4))
                            {
                                update.DoubleField = data.DoubleField;
                            }

                            if (data.IsDataDirty(5))
                            {
                                update.StringField = data.StringField;
                            }

                            if (data.IsDataDirty(6))
                            {
                                update.OptionalField = data.OptionalField;
                            }

                            if (data.IsDataDirty(7))
                            {
                                update.ListField = data.ListField;
                            }

                            if (data.IsDataDirty(8))
                            {
                                update.MapField = data.MapField;
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

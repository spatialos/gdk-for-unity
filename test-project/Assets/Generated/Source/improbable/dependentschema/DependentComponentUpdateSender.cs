// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.CodegenAdapters;

namespace Improbable.DependentSchema
{
    public partial class DependentComponent
    {
        internal class ComponentReplicator : IComponentReplicationHandler
        {
            public uint ComponentId => 198800;

            public EntityQueryDesc ComponentUpdateQuery => new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<global::Improbable.DependentSchema.DependentComponent.Component>(),
                    ComponentType.ReadWrite<global::Improbable.DependentSchema.DependentComponent.ComponentAuthority>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public void SendUpdates(
                NativeArray<ArchetypeChunk> chunkArray,
                ComponentSystemBase system,
                EntityManager entityManager,
                ComponentUpdateSystem componentUpdateSystem)
            {
                Profiler.BeginSample("DependentComponent");

                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<global::Improbable.DependentSchema.DependentComponent.Component>();
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
                                update.A = data.A;
                            }

                            if (data.IsDataDirty(1))
                            {
                                update.B = data.B;
                            }

                            if (data.IsDataDirty(2))
                            {
                                update.C = data.C;
                            }

                            if (data.IsDataDirty(3))
                            {
                                update.D = data.D;
                            }

                            if (data.IsDataDirty(4))
                            {
                                update.E = data.E;
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

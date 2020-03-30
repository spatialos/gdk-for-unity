// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using Unity.Entities;
using Unity.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.CodegenAdapters;
using Unity.Profiling;

namespace Improbable.TestSchema
{
    public partial class ExhaustiveMapKey
    {
        internal class ComponentReplicator : IComponentReplicationHandler
        {
            private ProfilerMarker componentMarker = new ProfilerMarker("ExhaustiveMapKey");

            public uint ComponentId => 197719;

            public EntityQueryDesc ComponentUpdateQuery => new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<global::Improbable.TestSchema.ExhaustiveMapKey.Component>(),
                    ComponentType.ReadOnly<global::Improbable.TestSchema.ExhaustiveMapKey.HasAuthority>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
            };

            public void SendUpdates(
                NativeArray<ArchetypeChunk> chunkArray,
                ComponentSystemBase system,
                EntityManager entityManager,
                ComponentUpdateSystem componentUpdateSystem)
            {
                using (componentMarker.Auto())
                {
                    var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                    var componentType = system.GetArchetypeChunkComponentType<global::Improbable.TestSchema.ExhaustiveMapKey.Component>();

                    foreach (var chunk in chunkArray)
                    {
                        var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                        var componentArray = chunk.GetNativeArray(componentType);

                        for (var i = 0; i < componentArray.Length; i++)
                        {
                            var data = componentArray[i];

                            if (data.IsDataDirty())
                            {
                                var update = new Update();

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
                }
            }
        }
    }
}

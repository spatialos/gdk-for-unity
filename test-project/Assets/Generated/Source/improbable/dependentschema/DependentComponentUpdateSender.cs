// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using Unity.Entities;
using Unity.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.CodegenAdapters;
using Unity.Profiling;

namespace Improbable.DependentSchema
{
    public partial class DependentComponent
    {
        internal class ComponentReplicator : IComponentReplicationHandler
        {
            private ProfilerMarker componentMarker = new ProfilerMarker("DependentComponent");

            public uint ComponentId => 198800;

            public EntityQueryDesc ComponentUpdateQuery => new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<global::Improbable.DependentSchema.DependentComponent.Component>(),
                    ComponentType.ReadOnly<global::Improbable.DependentSchema.DependentComponent.HasAuthority>(),
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
                    var componentType = system.GetArchetypeChunkComponentType<global::Improbable.DependentSchema.DependentComponent.Component>();

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
                }
            }
        }
    }
}

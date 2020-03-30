// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using Unity.Entities;
using Unity.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.CodegenAdapters;
using Unity.Profiling;

namespace Improbable.Tests
{
    public partial class DependencyTestGrandchild
    {
        internal class ComponentReplicator : IComponentReplicationHandler
        {
            private ProfilerMarker componentMarker = new ProfilerMarker("DependencyTestGrandchild");

            public uint ComponentId => 11113;

            public EntityQueryDesc ComponentUpdateQuery => new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<global::Improbable.Tests.DependencyTestGrandchild.Component>(),
                    ComponentType.ReadOnly<global::Improbable.Tests.DependencyTestGrandchild.HasAuthority>(),
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
                    var componentType = system.GetArchetypeChunkComponentType<global::Improbable.Tests.DependencyTestGrandchild.Component>();

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
                                    update.Grandchild = data.Grandchild;
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

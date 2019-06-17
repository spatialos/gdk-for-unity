using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    /// <summary>
    ///     Removes reactive World Command components
    /// </summary>
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(InternalSpatialOSCleanGroup))]
    public class WorldCommandsCleanSystem : ComponentSystem
    {
        private readonly EntityQueryDesc worldCommandResponseQuery = new EntityQueryDesc()
        {
            All = Array.Empty<ComponentType>(),
            Any = new[]
            {
                ComponentType.ReadWrite<WorldCommands.CreateEntity.CommandResponses>(),
                ComponentType.ReadWrite<WorldCommands.DeleteEntity.CommandResponses>(),
                ComponentType.ReadWrite<WorldCommands.ReserveEntityIds.CommandResponses>(),
                ComponentType.ReadWrite<WorldCommands.EntityQuery.CommandResponses>(),
            },
            None = Array.Empty<ComponentType>(),
        };

        private EntityQuery worldCommandResponseGroup;

        protected override void OnCreate()
        {
            base.OnCreate();
            worldCommandResponseGroup = GetEntityQuery(worldCommandResponseQuery);
        }

        protected override void OnUpdate()
        {
            var entityType = GetArchetypeChunkEntityType();

            var createEntityType = GetArchetypeChunkComponentType<WorldCommands.CreateEntity.CommandResponses>();
            var deleteEntityType = GetArchetypeChunkComponentType<WorldCommands.DeleteEntity.CommandResponses>();
            var reserveEntityIdsType =
                GetArchetypeChunkComponentType<WorldCommands.ReserveEntityIds.CommandResponses>();
            var entityQueryType = GetArchetypeChunkComponentType<WorldCommands.EntityQuery.CommandResponses>();

            var chunkArray = worldCommandResponseGroup.CreateArchetypeChunkArray(Allocator.TempJob);

            foreach (var chunk in chunkArray)
            {
                var entityArray = chunk.GetNativeArray(entityType);

                if (chunk.Has(createEntityType))
                {
                    var responseArray = chunk.GetNativeArray(createEntityType);
                    for (var i = 0; i < entityArray.Length; i++)
                    {
                        WorldCommands.CreateEntity.ResponsesProvider.Free(responseArray[i].Handle);
                        PostUpdateCommands.RemoveComponent<WorldCommands.CreateEntity.CommandResponses>(entityArray[i]);
                    }
                }

                if (chunk.Has(deleteEntityType))
                {
                    var responseArray = chunk.GetNativeArray(deleteEntityType);
                    for (var i = 0; i < entityArray.Length; i++)
                    {
                        WorldCommands.DeleteEntity.ResponsesProvider.Free(responseArray[i].Handle);
                        PostUpdateCommands.RemoveComponent<WorldCommands.DeleteEntity.CommandResponses>(entityArray[i]);
                    }
                }

                if (chunk.Has(reserveEntityIdsType))
                {
                    var responseArray = chunk.GetNativeArray(reserveEntityIdsType);
                    for (var i = 0; i < entityArray.Length; i++)
                    {
                        WorldCommands.ReserveEntityIds.ResponsesProvider.Free(responseArray[i].Handle);
                        PostUpdateCommands.RemoveComponent<WorldCommands.ReserveEntityIds.CommandResponses>(
                            entityArray[i]);
                    }
                }

                if (chunk.Has(entityQueryType))
                {
                    var responseArray = chunk.GetNativeArray(entityQueryType);
                    for (var i = 0; i < entityArray.Length; i++)
                    {
                        WorldCommands.EntityQuery.ResponsesProvider.Free(responseArray[i].Handle);
                        PostUpdateCommands.RemoveComponent<WorldCommands.EntityQuery.CommandResponses>(entityArray[i]);
                    }
                }
            }

            chunkArray.Dispose();
        }
    }
}

using Improbable.Gdk.Core.Commands;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSCleanGroup))]
    public class WorldCommandsCleanSystem : ComponentSystem
    {
        private struct CreateEntityResponsesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.CreateEntity.CommandResponses> Responses;
        }

        [Inject] private CreateEntityResponsesData createEntityResponsesData;

        private struct DeleteEntityResponsesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.DeleteEntity.CommandResponses> Responses;
        }

        [Inject] private DeleteEntityResponsesData deleteEntityResponsesData;

        private struct ReserveEntityIdsResponsesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.ReserveEntityIds.CommandResponses> Responses;
        }

        [Inject] private ReserveEntityIdsResponsesData reserveEntityIdsResponsesData;

        private struct EntityQueryResponsesData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.EntityQuery.CommandResponses> Responses;
        }

        [Inject] private EntityQueryResponsesData entityQueryResponsesData;


        protected override void OnUpdate()
        {
            for (var i = 0; i < createEntityResponsesData.Length; i++)
            {
                var responses = createEntityResponsesData.Responses[i];
                var entity = createEntityResponsesData.Entities[i];

                WorldCommands.CreateEntity.ResponsesProvider.Free(responses.Handle);
                PostUpdateCommands.RemoveComponent<WorldCommands.CreateEntity.CommandResponses>(entity);
            }

            for (var i = 0; i < deleteEntityResponsesData.Length; i++)
            {
                var responses = deleteEntityResponsesData.Responses[i];
                var entity = deleteEntityResponsesData.Entities[i];

                WorldCommands.DeleteEntity.ResponsesProvider.Free(responses.Handle);
                PostUpdateCommands.RemoveComponent<WorldCommands.DeleteEntity.CommandResponses>(entity);
            }

            for (var i = 0; i < reserveEntityIdsResponsesData.Length; i++)
            {
                var responses = reserveEntityIdsResponsesData.Responses[i];
                var entity = reserveEntityIdsResponsesData.Entities[i];

                WorldCommands.ReserveEntityIds.ResponsesProvider.Free(responses.Handle);
                PostUpdateCommands.RemoveComponent<WorldCommands.ReserveEntityIds.CommandResponses>(entity);
            }

            for (var i = 0; i < entityQueryResponsesData.Length; i++)
            {
                var responses = entityQueryResponsesData.Responses[i];
                var entity = entityQueryResponsesData.Entities[i];

                WorldCommands.EntityQuery.ResponsesProvider.Free(responses.Handle);
                PostUpdateCommands.RemoveComponent<WorldCommands.EntityQuery.CommandResponses>(entity);
            }
        }
    }
}

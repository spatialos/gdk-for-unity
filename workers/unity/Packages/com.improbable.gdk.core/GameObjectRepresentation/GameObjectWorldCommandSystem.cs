using System.Linq;
using Unity.Collections;
using Unity.Entities;

#region Diagnostic control

// Disable the "variable is never assigned" for injected fields.
#pragma warning disable 649

// ReSharper disable ClassNeverInstantiated.Global

#endregion


namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public class GameObjectWorldCommandSystem : ComponentSystem
    {
        private struct ReserveEntityResponseData
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Commands.WorldCommands.ReserveEntityIds.CommandResponses> Responses;
        }

        [Inject] private ReserveEntityResponseData reserveEntityResponseData;

        private struct CreateEntityResponseData
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Commands.WorldCommands.CreateEntity.CommandResponses> Responses;
        }

        [Inject] private CreateEntityResponseData createEntityResponseData;

        private struct DeleteEntityResponseData
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<Commands.WorldCommands.DeleteEntity.CommandResponses> Responses;
        }

        [Inject] private DeleteEntityResponseData deleteEntityResponseData;

        [Inject] private GameObjectDispatcherSystem GameObjectDispatcherSystem;

        private static readonly InjectableId WorldCommandResponseHandlerInjectableId =
            new InjectableId(InjectableType.WorldCommandResponseHandler, InjectableId.NullComponentId);

        protected override void OnUpdate()
        {
            for (var i = 0; i < reserveEntityResponseData.Length; ++i)
            {
                if (reserveEntityResponseData.Responses[i].Responses.Count == 0)
                {
                    continue;
                }

                var entity = reserveEntityResponseData.Entities[i];
                var worldCommandResponseHandlers = GetWorldCommandResponseHandlersForEntity(entity);

                if (worldCommandResponseHandlers == null)
                {
                    continue;
                }

                foreach (var receivedResponse in reserveEntityResponseData.Responses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseHandlers)
                    {
                        worldCommandHandler.OnReserveEntityIdsResponseInternal(receivedResponse);
                    }
                }
            }

            for (var i = 0; i < createEntityResponseData.Length; ++i)
            {
                if (createEntityResponseData.Responses[i].Responses.Count == 0)
                {
                    continue;
                }

                var entity = createEntityResponseData.Entities[i];
                var worldCommandResponseHandlers = GetWorldCommandResponseHandlersForEntity(entity);

                if (worldCommandResponseHandlers == null)
                {
                    continue;
                }

                foreach (var receivedResponse in createEntityResponseData.Responses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseHandlers)
                    {
                        worldCommandHandler.OnCreateEntityResponseInternal(receivedResponse);
                    }
                }
            }

            for (var i = 0; i < deleteEntityResponseData.Length; ++i)
            {
                if (deleteEntityResponseData.Responses[i].Responses.Count == 0)
                {
                    continue;
                }

                var entity = deleteEntityResponseData.Entities[i];
                var worldCommandResponseHandlers = GetWorldCommandResponseHandlersForEntity(entity);

                if (worldCommandResponseHandlers == null)
                {
                    continue;
                }

                foreach (var receivedResponse in deleteEntityResponseData.Responses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseHandlers)
                    {
                        worldCommandHandler.OnDeleteEntityResponseInternal(receivedResponse);
                    }
                }
            }
        }

        private WorldCommandsRequirables.WorldCommandResponseHandler[] GetWorldCommandResponseHandlersForEntity(
            Entity entity)
        {
            var entityToReaderWriterStore = GameObjectDispatcherSystem.entityToReaderWriterStore;

            if (!entityToReaderWriterStore.TryGetValue(entity, out var injectableStore))
            {
                return null;
            }

            if (!injectableStore.TryGetInjectablesForComponent(WorldCommandResponseHandlerInjectableId,
                out var injectables))
            {
                return null;
            }

            if (injectables.Count == 0)
            {
                return null;
            }

            return injectables.Cast<WorldCommandsRequirables.WorldCommandResponseHandler>().ToArray();
        }
    }
}

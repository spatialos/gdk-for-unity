using Improbable.Gdk.Core.Commands;
using Unity.Collections;
using Unity.Entities;

#region Diagnostic control

// Disable the "variable is never assigned" for injected fields.
#pragma warning disable 649

// ReSharper disable ClassNeverInstantiated.Global

#endregion


namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Listens for World Command responses and invokes callbacks on GameObjects.
    /// </summary>
    [DisableAutoCreation]
    public class GameObjectWorldCommandSystem : ComponentSystem
    {
        private struct ReserveEntityIdsResponseData
        {
            public readonly int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.ReserveEntityIds.CommandResponses> CommandResponses;
            [ReadOnly] public ComponentArray<GameObjectReference> HasGameObjectReference;
        }

        private struct CreateEntityResponseData
        {
            public readonly int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.CreateEntity.CommandResponses> CommandResponses;
            [ReadOnly] public ComponentArray<GameObjectReference> HasGameObjectReference;
        }

        private struct DeleteEntityResponseData
        {
            public readonly int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.DeleteEntity.CommandResponses> CommandResponses;
            [ReadOnly] public ComponentArray<GameObjectReference> HasGameObjectReference;
        }

        private struct EntityQueryResponseData
        {
            public readonly int Length;
            [ReadOnly] public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.EntityQuery.CommandResponses> CommandResponses;
            [ReadOnly] public ComponentArray<GameObjectReference> HasGameObjectReference;
        }

        [Inject] private GameObjectDispatcherSystem gameObjectDispatcherSystem;
        [Inject] private ReserveEntityIdsResponseData reserveEntityIdsResponseData;
        [Inject] private CreateEntityResponseData createEntityResponseData;
        [Inject] private DeleteEntityResponseData deleteEntityResponseData;
        [Inject] private EntityQueryResponseData entityQueryResponseData;

        private static readonly InjectableId WorldCommandResponseHandlerInjectableId =
            new InjectableId(InjectableType.WorldCommandResponseHandler, InjectableId.NullComponentId);

        protected override void OnUpdate()
        {
            for (var i = 0; i < reserveEntityIdsResponseData.Length; ++i)
            {
                var entity = reserveEntityIdsResponseData.Entities[i];
                var worldCommandResponseHandlers = GetWorldCommandResponseHandlersForEntity(entity);

                if (worldCommandResponseHandlers == null)
                {
                    continue;
                }

                foreach (var receivedResponse in reserveEntityIdsResponseData.CommandResponses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseHandlers)
                    {
                        worldCommandHandler.OnReserveEntityIdsResponseInternal(receivedResponse);
                    }
                }
            }

            for (var i = 0; i < createEntityResponseData.Length; ++i)
            {
                var entity = createEntityResponseData.Entities[i];
                var worldCommandResponseHandlers = GetWorldCommandResponseHandlersForEntity(entity);

                if (worldCommandResponseHandlers == null)
                {
                    continue;
                }

                foreach (var receivedResponse in createEntityResponseData.CommandResponses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseHandlers)
                    {
                        worldCommandHandler.OnCreateEntityResponseInternal(receivedResponse);
                    }
                }
            }

            for (var i = 0; i < deleteEntityResponseData.Length; ++i)
            {
                var entity = deleteEntityResponseData.Entities[i];
                var worldCommandResponseHandlers = GetWorldCommandResponseHandlersForEntity(entity);

                if (worldCommandResponseHandlers == null)
                {
                    continue;
                }

                foreach (var receivedResponse in deleteEntityResponseData.CommandResponses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseHandlers)
                    {
                        worldCommandHandler.OnDeleteEntityResponseInternal(receivedResponse);
                    }
                }
            }

            for (var i = 0; i < entityQueryResponseData.Length; ++i)
            {
                var entity = entityQueryResponseData.Entities[i];
                var worldCommandResponseHandlers = GetWorldCommandResponseHandlersForEntity(entity);

                if (worldCommandResponseHandlers == null)
                {
                    continue;
                }

                foreach (var receivedResponse in entityQueryResponseData.CommandResponses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseHandlers)
                    {
                        worldCommandHandler.OnEntityQueryResponseInternal(receivedResponse);
                    }
                }
            }
        }

        private WorldCommands.Requirable.WorldCommandResponseHandler[] GetWorldCommandResponseHandlersForEntity(
            Entity entity)
        {
            var entityToReaderWriterStore = gameObjectDispatcherSystem.EntityToReaderWriterStore;

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

            var result = new WorldCommands.Requirable.WorldCommandResponseHandler[injectables.Count];

            for (var i = 0; i < injectables.Count; ++i)
            {
                result[i] = (WorldCommands.Requirable.WorldCommandResponseHandler) injectables[i];
            }

            return result;
        }
    }
}

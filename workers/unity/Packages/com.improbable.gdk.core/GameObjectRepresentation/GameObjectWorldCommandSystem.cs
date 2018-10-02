using System;
using System.Linq;
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

        private static readonly InjectableId WorldCommandResponseReceiverInjectableId =
            new InjectableId(InjectableType.WorldCommandResponseReceiver, InjectableId.NullComponentId);

        protected override void OnUpdate()
        {
            for (var i = 0; i < reserveEntityIdsResponseData.Length; ++i)
            {
                var entity = reserveEntityIdsResponseData.Entities[i];
                var worldCommandResponseReceivers = GetWorldCommandResponseReceiversForEntity(entity);

                if (worldCommandResponseReceivers == null)
                {
                    continue;
                }

                foreach (var receivedResponse in reserveEntityIdsResponseData.CommandResponses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseReceivers)
                    {
                        worldCommandHandler.OnReserveEntityIdsResponseInternal(receivedResponse);
                    }
                }
            }

            for (var i = 0; i < createEntityResponseData.Length; ++i)
            {
                var entity = createEntityResponseData.Entities[i];
                var worldCommandResponseReceivers = GetWorldCommandResponseReceiversForEntity(entity);

                if (worldCommandResponseReceivers == null)
                {
                    continue;
                }

                foreach (var receivedResponse in createEntityResponseData.CommandResponses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseReceivers)
                    {
                        worldCommandHandler.OnCreateEntityResponseInternal(receivedResponse);
                    }
                }
            }

            for (var i = 0; i < deleteEntityResponseData.Length; ++i)
            {
                var entity = deleteEntityResponseData.Entities[i];
                var worldCommandResponseReceivers = GetWorldCommandResponseReceiversForEntity(entity);

                if (worldCommandResponseReceivers == null)
                {
                    continue;
                }

                foreach (var receivedResponse in deleteEntityResponseData.CommandResponses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseReceivers)
                    {
                        worldCommandHandler.OnDeleteEntityResponseInternal(receivedResponse);
                    }
                }
            }

            for (var i = 0; i < entityQueryResponseData.Length; ++i)
            {
                var entity = entityQueryResponseData.Entities[i];
                var worldCommandResponseReceivers = GetWorldCommandResponseReceiversForEntity(entity);

                if (worldCommandResponseReceivers == null)
                {
                    continue;
                }

                foreach (var receivedResponse in entityQueryResponseData.CommandResponses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseReceivers)
                    {
                        worldCommandHandler.OnEntityQueryResponseInternal(receivedResponse);
                    }
                }
            }
        }

        private WorldCommands.Requirable.WorldCommandResponseReceiver[] GetWorldCommandResponseReceiversForEntity(
            Entity entity)
        {
            var entityToReaderWriterStore = gameObjectDispatcherSystem.EntityToReaderWriterStore;

            if (!entityToReaderWriterStore.TryGetValue(entity, out var injectableStore))
            {
                return null;
            }

            if (!injectableStore.TryGetInjectablesForComponent(WorldCommandResponseReceiverInjectableId,
                out var injectables))
            {
                return null;
            }

            if (injectables.Count == 0)
            {
                return null;
            }

            return Array.ConvertAll(injectables.ToArray(),
                injectable => (WorldCommands.Requirable.WorldCommandResponseReceiver) injectable);
        }
    }
}

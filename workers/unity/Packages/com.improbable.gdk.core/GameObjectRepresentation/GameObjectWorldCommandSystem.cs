using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

#pragma warning disable 649

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GameObjectWorldCommandSystem : ComponentSystem
    {
        private struct WorldCommandResponderData : ISystemStateComponentData
        {
            public uint ResponderHandle;

            public HashSet<WorldCommands.WorldCommandResponseHandler> ResponseHandlers
            {
                get => WorldCommandResponseHandlerProvider.Get(ResponderHandle);
                set => WorldCommandResponseHandlerProvider.Set(ResponderHandle,
                    value);
            }
        }

        private struct WorldCommandResponderAddedTag : IComponentData
        {
        }

        private struct ReserveEntityResponseData
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public ComponentDataArray<WorldCommandResponderAddedTag> WithWorldCommandResponder;
            [ReadOnly] public ComponentDataArray<WorldCommandResponderData> WorldCommandHandlers;
            [ReadOnly] public ComponentDataArray<Commands.WorldCommands.ReserveEntityIds.CommandResponses> Responses;
        }

        [Inject] private ReserveEntityResponseData reserveEntityResponseData;

        private struct CreateEntityResponseData
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public ComponentDataArray<WorldCommandResponderAddedTag> WithWorldCommandResponder;
            [ReadOnly] public ComponentDataArray<WorldCommandResponderData> WorldCommandHandlers;
            [ReadOnly] public ComponentDataArray<Commands.WorldCommands.CreateEntity.CommandResponses> Responses;
        }

        [Inject] private CreateEntityResponseData createEntityResponseData;

        private struct DeleteEntityResponseData
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public ComponentDataArray<WorldCommandResponderAddedTag> WithWorldCommandResponder;
            [ReadOnly] public ComponentDataArray<WorldCommandResponderData> WorldCommandResponders;
            [ReadOnly] public ComponentDataArray<Commands.WorldCommands.DeleteEntity.CommandResponses> Responses;
        }

        [Inject] private DeleteEntityResponseData deleteEntityResponseData;

        private struct EntitiesToCleanUpData
        {
            [ReadOnly] public readonly int Length;

            [ReadOnly] public ComponentDataArray<WorldCommandResponderData> WorldCommandResponders;
            [ReadOnly] public SubtractiveComponent<WorldCommandResponderAddedTag> WithoutTag;
            [ReadOnly] public EntityArray Entities;
        }

        [Inject] private EntitiesToCleanUpData entitiesToCleanUpData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < reserveEntityResponseData.Length; ++i)
            {
                if (reserveEntityResponseData.Responses[i].Responses.Count == 0)
                {
                    continue;
                }

                var worldCommandResponseHandlers = reserveEntityResponseData
                    .WorldCommandHandlers[i]
                    .ResponseHandlers;

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

                var worldCommandResponseHandlers = createEntityResponseData
                    .WorldCommandHandlers[i]
                    .ResponseHandlers;

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

                var worldCommandResponseHandlers = deleteEntityResponseData
                    .WorldCommandResponders[i]
                    .ResponseHandlers;

                foreach (var receivedResponse in deleteEntityResponseData.Responses[i].Responses)
                {
                    foreach (var worldCommandHandler in worldCommandResponseHandlers)
                    {
                        worldCommandHandler.OnDeleteEntityResponseInternal(receivedResponse);
                    }
                }
            }

            for (var i = 0; i < entitiesToCleanUpData.Length; ++i)
            {
                var worldCommandResponderData = entitiesToCleanUpData.WorldCommandResponders[i];

                WorldCommandResponseHandlerProvider.Free(worldCommandResponderData.ResponderHandle);

                PostUpdateCommands.RemoveComponent<WorldCommandResponderData>(entitiesToCleanUpData.Entities[i]);
            }
        }

        protected override void OnDestroyManager()
        {
            WorldCommandResponseHandlerProvider.CleanDataInWorld(World);
        }

        internal void RegisterResponseHandler(Entity entity,
            WorldCommands.WorldCommandResponseHandler worldCommandResponseHandler)
        {
            GetOrCreateWorldCommandResponderData(entity, EntityManager)
                .ResponseHandlers
                .Add(worldCommandResponseHandler);
        }

        private WorldCommandResponderData GetOrCreateWorldCommandResponderData(
            Entity entity,
            EntityManager entityManager)
        {
            WorldCommandResponderData worldCommandSenderData;

            if (!entityManager.HasComponent<WorldCommandResponderData>(entity))
            {
                worldCommandSenderData = new WorldCommandResponderData();

                worldCommandSenderData.ResponderHandle =
                    WorldCommandResponseHandlerProvider.Allocate(World);

                worldCommandSenderData.ResponseHandlers =
                    new HashSet<WorldCommands.WorldCommandResponseHandler>();

                entityManager.AddComponentData(entity, worldCommandSenderData);
                entityManager.AddComponentData(entity, new WorldCommandResponderAddedTag());
            }
            else
            {
                worldCommandSenderData = entityManager.GetComponentData<WorldCommandResponderData>(entity);
            }

            return worldCommandSenderData;
        }

        private static class WorldCommandResponseHandlerProvider
        {
            private static readonly Dictionary<uint, HashSet<WorldCommands.WorldCommandResponseHandler>> Storage =
                new Dictionary<uint, HashSet<WorldCommands.WorldCommandResponseHandler>>();

            private static readonly Dictionary<uint, World> WorldMapping = new Dictionary<uint, World>();

            private static uint nextHandle = 0;

            public static uint Allocate(World world)
            {
                var handle = GetNextHandle();
                Storage.Add(handle, default(HashSet<WorldCommands.WorldCommandResponseHandler>));
                WorldMapping.Add(handle, world);

                return handle;
            }

            public static HashSet<WorldCommands.WorldCommandResponseHandler> Get(uint handle)
            {
                if (!Storage.TryGetValue(handle, out var value))
                {
                    throw new ArgumentException(
                        $"WorldCommandResponseHandlerProvider does not contain handle {handle}");
                }

                return value;
            }

            public static void Set(uint handle, HashSet<WorldCommands.WorldCommandResponseHandler> value)
            {
                if (!Storage.ContainsKey(handle))
                {
                    throw new ArgumentException(
                        $"WorldCommandResponseHandlerProvider does not contain handle {handle}");
                }

                Storage[handle] = value;
            }

            public static void Free(uint handle)
            {
                Storage.Remove(handle);
                WorldMapping.Remove(handle);
            }

            public static void CleanDataInWorld(World world)
            {
                var handles = WorldMapping.Where(pair => pair.Value == world).Select(pair => pair.Key).ToList();

                foreach (var handle in handles)
                {
                    Free(handle);
                }
            }

            private static uint GetNextHandle()
            {
                nextHandle++;

                while (Storage.ContainsKey(nextHandle))
                {
                    nextHandle++;
                }

                return nextHandle;
            }
        }
    }
}

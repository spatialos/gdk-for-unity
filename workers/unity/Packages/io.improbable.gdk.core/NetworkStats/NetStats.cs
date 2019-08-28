using System.Collections.Generic;
using System.Diagnostics;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core.NetworkStats
{
    public class NetStats
    {
        public enum WorldCommand
        {
            CreateEntity = 1,
            DeleteEntity = 2,
            ReserveEntityIds = 3,
            EntityQuery = 4
        }

        public IReadOnlyDictionary<uint, DataPoint> Updates => updates;
        public IReadOnlyDictionary<(uint, uint), DataPoint> CommandRequests => commandRequests;
        public IReadOnlyDictionary<(uint, uint), DataPoint> CommandResponses => commandResponses;
        public IReadOnlyDictionary<WorldCommand, DataPoint> WorldCommandRequests => worldCommandRequests;
        public IReadOnlyDictionary<WorldCommand, DataPoint> WorldCommandResponses => worldCommandResponses;

        private readonly Dictionary<uint, DataPoint> updates = new Dictionary<uint, DataPoint>();
        private readonly Dictionary<(uint, uint), DataPoint> commandRequests = new Dictionary<(uint, uint), DataPoint>();
        private readonly Dictionary<(uint, uint), DataPoint> commandResponses = new Dictionary<(uint, uint), DataPoint>();
        private readonly Dictionary<WorldCommand, DataPoint> worldCommandRequests = new Dictionary<WorldCommand, DataPoint>();
        private readonly Dictionary<WorldCommand, DataPoint> worldCommandResponses = new Dictionary<WorldCommand, DataPoint>();

        private NetStats()
        {
        }

        [Conditional("UNITY_EDITOR")]
        public void AddUpdate(in ComponentUpdate update)
        {
            var componentId = update.ComponentId;
            var size = update.SchemaData.Value.GetFields().GetWriteBufferLength() +
                update.SchemaData.Value.GetEvents().GetWriteBufferLength();

            updates.TryGetValue(componentId, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            updates[componentId] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddCommandRequest(in CommandRequest request)
        {
            var componentId = request.ComponentId;
            var commandIndex = request.CommandIndex;
            var size = request.SchemaData.Value.GetObject().GetWriteBufferLength();

            var key = (componentId, commandIndex);
            commandRequests.TryGetValue(key, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            commandRequests[key] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddCommandResponse(in CommandResponse response, string message)
        {
            var componentId = response.ComponentId;
            var commandIndex = response.CommandIndex;
            uint size;

            if (response.SchemaData.HasValue)
            {
                size = response.SchemaData.Value.GetObject().GetWriteBufferLength();
            }
            else
            {
                // Approximation of on-wire size.
                size = (uint) System.Text.Encoding.UTF8.GetByteCount(message);
            }

            var key = (componentId, commandIndex);
            commandResponses.TryGetValue(key, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            commandResponses[key] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddWorldCommandRequest(WorldCommand command)
        {
            worldCommandRequests.TryGetValue(command, out var metrics);
            metrics.Count += 1;
            worldCommandRequests[command] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddWorldCommandResponse(WorldCommand command)
        {
            worldCommandResponses.TryGetValue(command, out var metrics);
            metrics.Count += 1;
            worldCommandResponses[command] = metrics;
        }

        internal void CopyFrom(NetStats other)
        {
            Clear();

            foreach (var pair in other.updates)
            {
                updates[pair.Key] = pair.Value;
            }

            foreach (var pair in other.commandRequests)
            {
                commandRequests[pair.Key] = pair.Value;
            }

            foreach (var pair in other.commandResponses)
            {
                commandResponses[pair.Key] = pair.Value;
            }

            foreach (var pair in other.worldCommandRequests)
            {
                worldCommandRequests[pair.Key] = pair.Value;
            }

            foreach (var pair in other.worldCommandResponses)
            {
                worldCommandResponses[pair.Key] = pair.Value;
            }
        }

        internal void Clear()
        {
            updates.Clear();
            commandRequests.Clear();
            commandResponses.Clear();
            worldCommandRequests.Clear();
            worldCommandResponses.Clear();
        }

        public static class Pool
        {
            private static readonly Queue<NetStats> data = new Queue<NetStats>();

            public static NetStats Rent()
            {
                if (data.Count != 0)
                {
                    return data.Dequeue();
                }

                return new NetStats();
            }

            public static void Return(NetStats stats)
            {
                stats.Clear();
                data.Enqueue(stats);
            }
        }
    }
}

using System.Collections.Generic;

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

        public void AddUpdate(uint componentId, uint size)
        {
            updates.TryGetValue(componentId, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            updates[componentId] = metrics;
        }

        public void AddCommandRequest(uint componentId, uint commandIndex, uint size)
        {
            commandRequests.TryGetValue((componentId, commandIndex), out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            commandRequests[(componentId, commandIndex)] = metrics;
        }

        public void AddCommandResponse(uint componentId, uint commandIndex, uint size)
        {
            commandResponses.TryGetValue((componentId, commandIndex), out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            commandResponses[(componentId, commandIndex)] = metrics;
        }

        public void AddWorldCommandRequest(WorldCommand command)
        {
            worldCommandRequests.TryGetValue(command, out var metrics);
            metrics.Count += 1;
            worldCommandRequests[command] = metrics;
        }

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

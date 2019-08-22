using System.Collections.Generic;

namespace Improbable.Gdk.Core.NetworkStats
{
    internal class NetStats
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

        public void Clear()
        {
            updates.Clear();
            commandRequests.Clear();
            commandResponses.Clear();
            worldCommandRequests.Clear();
            worldCommandResponses.Clear();
        }
    }
}

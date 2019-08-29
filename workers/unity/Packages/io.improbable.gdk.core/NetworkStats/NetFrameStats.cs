using System.Collections.Generic;
using System.Diagnostics;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core.NetworkStats
{
    public class NetFrameStats
    {
        public readonly Dictionary<uint, DataPoint> Updates = new Dictionary<uint, DataPoint>();
        public readonly Dictionary<(uint, uint), DataPoint> CommandRequests = new Dictionary<(uint, uint), DataPoint>();
        public readonly Dictionary<(uint, uint), DataPoint> CommandResponses = new Dictionary<(uint, uint), DataPoint>();
        public readonly Dictionary<WorldCommand, DataPoint> WorldCommandRequests = new Dictionary<WorldCommand, DataPoint>();
        public readonly Dictionary<WorldCommand, DataPoint> WorldCommandResponses = new Dictionary<WorldCommand, DataPoint>();

        private NetFrameStats()
        {
        }

        [Conditional("UNITY_EDITOR")]
        public void AddUpdate(in ComponentUpdate update)
        {
            var componentId = update.ComponentId;
            var size = update.SchemaData.Value.GetFields().GetWriteBufferLength() +
                update.SchemaData.Value.GetEvents().GetWriteBufferLength();

            Updates.TryGetValue(componentId, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            Updates[componentId] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddCommandRequest(in CommandRequest request)
        {
            var componentId = request.ComponentId;
            var commandIndex = request.CommandIndex;
            var size = request.SchemaData.Value.GetObject().GetWriteBufferLength();

            var key = (componentId, commandIndex);
            CommandRequests.TryGetValue(key, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            CommandRequests[key] = metrics;
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
            CommandResponses.TryGetValue(key, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            CommandResponses[key] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddCommandResponse(string message, uint componentId, uint commandIndex)
        {
            // Approximation of on-wire size.
            var size = (uint) System.Text.Encoding.UTF8.GetByteCount(message);

            var key = (componentId, commandIndex);
            CommandResponses.TryGetValue(key, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            CommandResponses[key] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddWorldCommandRequest(WorldCommand command)
        {
            WorldCommandRequests.TryGetValue(command, out var metrics);
            metrics.Count += 1;
            WorldCommandRequests[command] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddWorldCommandResponse(WorldCommand command)
        {
            WorldCommandResponses.TryGetValue(command, out var metrics);
            metrics.Count += 1;
            WorldCommandResponses[command] = metrics;
        }

        internal void CopyFrom(NetFrameStats other)
        {
            Clear();

            foreach (var pair in other.Updates)
            {
                Updates[pair.Key] = pair.Value;
            }

            foreach (var pair in other.CommandRequests)
            {
                CommandRequests[pair.Key] = pair.Value;
            }

            foreach (var pair in other.CommandResponses)
            {
                CommandResponses[pair.Key] = pair.Value;
            }

            foreach (var pair in other.WorldCommandRequests)
            {
                WorldCommandRequests[pair.Key] = pair.Value;
            }

            foreach (var pair in other.WorldCommandResponses)
            {
                WorldCommandResponses[pair.Key] = pair.Value;
            }
        }

        internal void Merge(NetFrameStats other)
        {
            foreach (var pair in other.Updates)
            {
                Updates.TryGetValue(pair.Key, out var data);
                data += pair.Value;
                Updates[pair.Key] = data;
            }

            foreach (var pair in other.CommandRequests)
            {
                CommandRequests.TryGetValue(pair.Key, out var data);
                data += pair.Value;
                CommandRequests[pair.Key] = data;
            }

            foreach (var pair in other.CommandResponses)
            {
                CommandResponses.TryGetValue(pair.Key, out var data);
                data += pair.Value;
                CommandResponses[pair.Key] = data;
            }

            foreach (var pair in other.WorldCommandRequests)
            {
                WorldCommandRequests.TryGetValue(pair.Key, out var data);
                data += pair.Value;
                WorldCommandRequests[pair.Key] = data;
            }

            foreach (var pair in other.WorldCommandResponses)
            {
                WorldCommandResponses.TryGetValue(pair.Key, out var data);
                data += pair.Value;
                WorldCommandResponses[pair.Key] = data;
            }
        }

        internal void Clear()
        {
            Updates.Clear();
            CommandRequests.Clear();
            CommandResponses.Clear();
            WorldCommandRequests.Clear();
            WorldCommandResponses.Clear();
        }

        public static class Pool
        {
            private static readonly Queue<NetFrameStats> data = new Queue<NetFrameStats>();

            public static NetFrameStats Rent()
            {
                if (data.Count != 0)
                {
                    return data.Dequeue();
                }

                return new NetFrameStats();
            }

            public static void Return(NetFrameStats frameStats)
            {
                frameStats.Clear();
                data.Enqueue(frameStats);
            }
        }
    }
}

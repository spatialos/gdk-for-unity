using System.Collections.Generic;
using System.Diagnostics;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core.NetworkStats
{
    public class NetFrameStats
    {
        public readonly Dictionary<MessageTypeUnion, DataPoint> Messages = new Dictionary<MessageTypeUnion, DataPoint>();

        private NetFrameStats()
        {
        }

        [Conditional("UNITY_EDITOR")]
        public void AddUpdate(in ComponentUpdate update)
        {
            var messageType = MessageTypeUnion.Update(update.ComponentId);
            var size = update.SchemaData.Value.GetFields().GetWriteBufferLength() +
                update.SchemaData.Value.GetEvents().GetWriteBufferLength();

            Messages.TryGetValue(messageType, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            Messages[messageType] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddCommandRequest(in CommandRequest request)
        {
            var componentId = request.ComponentId;
            var commandIndex = request.CommandIndex;
            var messageType = MessageTypeUnion.CommandRequest(componentId, commandIndex);
            var size = request.SchemaData.Value.GetObject().GetWriteBufferLength();

            Messages.TryGetValue(messageType, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            Messages[messageType] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddCommandResponse(in CommandResponse response, string message)
        {
            var componentId = response.ComponentId;
            var commandIndex = response.CommandIndex;
            var messageType = MessageTypeUnion.CommandResponse(componentId, commandIndex);

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

            Messages.TryGetValue(messageType, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            Messages[messageType] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddCommandResponse(string message, uint componentId, uint commandIndex)
        {
            var messageType = MessageTypeUnion.CommandResponse(componentId, commandIndex);
            // Approximation of on-wire size.
            var size = (uint) System.Text.Encoding.UTF8.GetByteCount(message);

            Messages.TryGetValue(messageType, out var metrics);
            metrics.Count += 1;
            metrics.Size += size;
            Messages[messageType] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddWorldCommandRequest(WorldCommand command)
        {
            var messageType = MessageTypeUnion.WorldCommandRequest(command);
            Messages.TryGetValue(messageType, out var metrics);
            metrics.Count += 1;
            Messages[messageType] = metrics;
        }

        [Conditional("UNITY_EDITOR")]
        public void AddWorldCommandResponse(WorldCommand command)
        {
            var messageType = MessageTypeUnion.WorldCommandResponse(command);
            Messages.TryGetValue(messageType, out var metrics);
            metrics.Count += 1;
            Messages[messageType] = metrics;
        }

        internal void CopyFrom(NetFrameStats other)
        {
            Clear();

            foreach (var pair in other.Messages)
            {
                Messages[pair.Key] = pair.Value;
            }
        }

        internal void Merge(NetFrameStats other)
        {
            foreach (var pair in other.Messages)
            {
                Messages.TryGetValue(pair.Key, out var data);
                data += pair.Value;
                Messages[pair.Key] = data;
            }
        }

        internal void Clear()
        {
            Messages.Clear();
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

using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.NetworkStats
{
    public class NetStats
    {
        private readonly int sequenceLength;
        private readonly Dictionary<MessageTypeUnion, DataSequence> Data = new Dictionary<MessageTypeUnion, DataSequence>();
        private int nextFrameIndex;

        public NetStats(int sequenceLength)
        {
            this.sequenceLength = sequenceLength;
            nextFrameIndex = sequenceLength - 1;
        }

        public void AddFrame(NetFrameStats frameData, Direction direction)
        {
            // First we need to zero out the data before processing the _sparse_ data in the NetFrameStats.
            foreach (var pair in Data)
            {
                pair.Value.Clear(nextFrameIndex, direction);
            }

            foreach (var pair in frameData.Updates)
            {
                var type = MessageTypeUnion.Update(pair.Key);
                if (!Data.TryGetValue(type, out var data))
                {
                    data = new DataSequence(sequenceLength);
                }

                data.AddFrame(nextFrameIndex, pair.Value, direction);
                Data[type] = data;
            }

            foreach (var pair in frameData.CommandRequests)
            {
                var type = MessageTypeUnion.CommandRequest(pair.Key.Item1, pair.Key.Item2);
                if (!Data.TryGetValue(type, out var data))
                {
                    data = new DataSequence(sequenceLength);
                }

                data.AddFrame(nextFrameIndex, pair.Value, direction);
                Data[type] = data;
            }

            foreach (var pair in frameData.CommandResponses)
            {
                var type = MessageTypeUnion.CommandResponse(pair.Key.Item1, pair.Key.Item2);
                if (!Data.TryGetValue(type, out var data))
                {
                    data = new DataSequence(sequenceLength);
                }

                data.AddFrame(nextFrameIndex, pair.Value, direction);
                Data[type] = data;
            }

            foreach (var pair in frameData.WorldCommandRequests)
            {
                var type = MessageTypeUnion.WorldCommandRequest(pair.Key);
                if (!Data.TryGetValue(type, out var data))
                {
                    data = new DataSequence(sequenceLength);
                }

                data.AddFrame(nextFrameIndex, pair.Value, direction);
                Data[type] = data;
            }

            foreach (var pair in frameData.WorldCommandResponses)
            {
                var type = MessageTypeUnion.WorldCommandResponse(pair.Key);
                if (!Data.TryGetValue(type, out var data))
                {
                    data = new DataSequence(sequenceLength);
                }

                data.AddFrame(nextFrameIndex, pair.Value, direction);
                Data[type] = data;
            }

            if (--nextFrameIndex == -1)
            {
                nextFrameIndex += sequenceLength;
            }
        }

        public DataPoint GetSummary(MessageTypeUnion messageType, int numFrames, Direction direction)
        {
            if (!Data.TryGetValue(messageType, out var data))
            {
                return new DataPoint();
            }

            DataPoint[] frameData;

            switch (direction)
            {
                case Direction.Incoming:
                    frameData = data.Incoming;
                    break;
                case Direction.Outgoing:
                    frameData = data.Outgoing;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            var summary = new DataPoint();

            for (var i = 1; i <= numFrames; i++)
            {
                var index = (nextFrameIndex + i) % sequenceLength;
                summary += frameData[index];
            }

            return summary;
        }

        private struct DataSequence : IEquatable<DataSequence>
        {
            public readonly DataPoint[] Incoming;
            public readonly DataPoint[] Outgoing;

            public DataSequence(int size)
            {
                Incoming = new DataPoint[size];
                Outgoing = new DataPoint[size];
            }

            public void AddFrame(int index, DataPoint data, Direction direction)
            {
                switch (direction)
                {
                    case Direction.Incoming:
                        Incoming[index] = data;
                        break;
                    case Direction.Outgoing:
                        Outgoing[index] = data;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            }

            public void Clear(int index, Direction direction)
            {
                switch (direction)
                {
                    case Direction.Incoming:
                        Incoming[index] = new DataPoint();
                        break;
                    case Direction.Outgoing:
                        Outgoing[index] = new DataPoint();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            }

            public bool Equals(DataSequence other)
            {
                return Equals(Incoming, other.Incoming) && Equals(Outgoing, other.Outgoing);
            }

            public override bool Equals(object obj)
            {
                return obj is DataSequence other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Incoming != null ? Incoming.GetHashCode() : 0) * 397) ^ (Outgoing != null ? Outgoing.GetHashCode() : 0);
                }
            }

            public static bool operator ==(DataSequence left, DataSequence right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(DataSequence left, DataSequence right)
            {
                return !left.Equals(right);
            }
        }
    }
}

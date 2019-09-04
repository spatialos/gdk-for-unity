using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core.NetworkStats
{
    /// <summary>
    ///     Storage object for network data for a fixed number of frames.
    /// </summary>
    /// <remarks>
    ///     The underlying data is stored in a ring buffer.
    /// </remarks>
    public class NetStats
    {
        private readonly int sequenceLength;
        private readonly Dictionary<MessageTypeUnion, DataSequence> Data = new Dictionary<MessageTypeUnion, DataSequence>();
        private readonly float[] frameTimes;

        private int nextFrameIndex;

        /// <summary>
        ///     Creates an instance of network stats storage with a specific size.
        /// </summary>
        /// <param name="sequenceLength">
        ///     The maximum number of frames to store data for at any given time.
        /// </param>
        public NetStats(int sequenceLength)
        {
            this.sequenceLength = sequenceLength;
            nextFrameIndex = sequenceLength - 1;

            frameTimes = new float[sequenceLength];
        }

        /// <summary>
        ///     Sets the network statistics for a given direction for the current frame.
        /// </summary>
        /// <param name="frameData">The network statistics.</param>
        /// <param name="direction">The direction of those statistics.</param>
        public void SetFrameStats(NetFrameStats frameData, Direction direction)
        {
            // First we need to zero out the data before processing the _sparse_ data in the NetFrameStats.
            foreach (var pair in Data)
            {
                pair.Value.Clear(nextFrameIndex, direction);
            }

            foreach (var pair in frameData.Messages)
            {
                if (!Data.TryGetValue(pair.Key, out var data))
                {
                    data = new DataSequence(sequenceLength);
                }

                data.AddFrame(nextFrameIndex, pair.Value, direction);
                Data[pair.Key] = data;
            }
        }

        /// <summary>
        ///     Sets the frame time for the current frame.
        /// </summary>
        /// <param name="dt">The frame time.</param>
        public void SetFrameTime(float dt)
        {
            frameTimes[nextFrameIndex] = dt;
        }

        /// <summary>
        ///    Finalize data capture for this frame.
        /// </summary>
        public void FinishFrame()
        {
            if (--nextFrameIndex < 0)
            {
                nextFrameIndex += sequenceLength;
            }
        }

        /// <summary>
        ///     Retrieves summary statistics for a given message type for the last <see cref="numFrames"/> frames.
        /// </summary>
        /// <param name="messageType">The type of message to get summary statistics for.</param>
        /// <param name="numFrames">The number of frames to query against.</param>
        /// <param name="direction">The direction of the message type to get summary statistics for.</param>
        /// <returns>
        ///     A tuple where:
        ///         - the first element is a summed <see cref="DataPoint"/> for the given message type over the last <see cref="numFrames"/> frames.
        ///         - the second element is the summed time that the last <see cref="numFrames"/> took.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="numFrames"/> is larger than the number of frames this stores.</exception>
        public (DataPoint, float) GetSummary(MessageTypeUnion messageType, int numFrames, Direction direction)
        {
            if (numFrames > sequenceLength)
            {
                throw new ArgumentOutOfRangeException($"Cannot fetch {numFrames} worth of data. This instance can only store to up {sequenceLength} frames.");
            }

            float totalTime = 0.0f;

            for (var i = 1; i <= numFrames; i++)
            {
                // Iterate through the ring buffer (can potentially wrap around).
                var index = (nextFrameIndex + i) % sequenceLength;
                totalTime += frameTimes[index];
            }

            if (!Data.TryGetValue(messageType, out var data))
            {
                return (new DataPoint(), totalTime);
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
                // Iterate through the ring buffer (can potentially wrap around).
                var index = (nextFrameIndex + i) % sequenceLength;
                summary += frameData[index];
            }

            return (summary, totalTime);
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

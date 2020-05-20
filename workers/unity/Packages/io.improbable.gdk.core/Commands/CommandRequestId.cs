using System;

namespace Improbable.Gdk.Core.Commands
{
    public readonly struct CommandRequestId : IEquatable<CommandRequestId>, IComparable<CommandRequestId>
    {
        public readonly long Raw;

        public CommandRequestId(long requestId)
        {
            Raw = requestId;
        }

        public static bool operator ==(CommandRequestId left, CommandRequestId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CommandRequestId left, CommandRequestId right)
        {
            return !left.Equals(right);
        }

        public bool Equals(CommandRequestId other)
        {
            return Raw == other.Raw;
        }

        public override bool Equals(object obj)
        {
            return obj is CommandRequestId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Raw.GetHashCode();
        }

        public int CompareTo(CommandRequestId other)
        {
            return Raw.CompareTo(other.Raw);
        }
    }

    public readonly struct InternalCommandRequestId : IEquatable<InternalCommandRequestId>, IComparable<InternalCommandRequestId>
    {
        public readonly long Raw;

        public InternalCommandRequestId(long requestId)
        {
            Raw = requestId;
        }

        public static bool operator ==(InternalCommandRequestId left, InternalCommandRequestId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(InternalCommandRequestId left, InternalCommandRequestId right)
        {
            return !left.Equals(right);
        }

        public bool Equals(InternalCommandRequestId other)
        {
            return Raw == other.Raw;
        }

        public override bool Equals(object obj)
        {
            return obj is InternalCommandRequestId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Raw.GetHashCode();
        }

        public int CompareTo(InternalCommandRequestId other)
        {
            return Raw.CompareTo(other.Raw);
        }
    }
}

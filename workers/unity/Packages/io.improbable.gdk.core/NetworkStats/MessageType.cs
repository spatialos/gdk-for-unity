using System;
using System.Runtime.InteropServices;

namespace Improbable.Gdk.Core.NetworkStats
{
    public enum WorldCommand
    {
        CreateEntity = 0,
        DeleteEntity = 1,
        ReserveEntityIds = 2,
        EntityQuery = 3
    }

    public enum Direction
    {
        Incoming = 0,
        Outgoing = 1
    }

    public enum MessageType
    {
        Update = 0,
        CommandRequest = 1,
        CommandResponse = 2,
        WorldCommandRequest = 3,
        WorldCommandResponse = 4
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct MessageTypeUnion : IEquatable<MessageTypeUnion>
    {
        [FieldOffset(offset: 0)] private MessageType Type;

        [FieldOffset(sizeof(MessageType))] private uint UpdateInfo;

        [FieldOffset(sizeof(MessageType))] private (uint, uint) CommandInfo;

        [FieldOffset(sizeof(MessageType))] private WorldCommand WorldCommand;

        public static MessageTypeUnion Update(uint componentId)
        {
            return new MessageTypeUnion
            {
                Type = MessageType.Update,
                UpdateInfo = componentId
            };
        }

        public static MessageTypeUnion CommandRequest(uint componentId, uint commandIndex)
        {
            return new MessageTypeUnion
            {
                Type = MessageType.CommandRequest,
                CommandInfo = (componentId, commandIndex)
            };
        }

        public static MessageTypeUnion CommandResponse(uint componentId, uint commandIndex)
        {
            return new MessageTypeUnion
            {
                Type = MessageType.CommandResponse,
                CommandInfo = (componentId, commandIndex)
            };
        }

        public static MessageTypeUnion WorldCommandRequest(WorldCommand worldCommand)
        {
            return new MessageTypeUnion
            {
                Type = MessageType.WorldCommandRequest,
                WorldCommand = worldCommand
            };
        }

        public static MessageTypeUnion WorldCommandResponse(WorldCommand worldCommand)
        {
            return new MessageTypeUnion
            {
                Type = MessageType.WorldCommandResponse,
                WorldCommand = worldCommand
            };
        }

        public bool Equals(MessageTypeUnion other)
        {
            if (Type != other.Type)
            {
                return false;
            }

            switch (Type)
            {
                case MessageType.Update:
                    return UpdateInfo == other.UpdateInfo;
                case MessageType.CommandRequest:
                    return CommandInfo == other.CommandInfo;
                case MessageType.CommandResponse:
                    return CommandInfo == other.CommandInfo;
                case MessageType.WorldCommandRequest:
                    return WorldCommand == other.WorldCommand;
                case MessageType.WorldCommandResponse:
                    return WorldCommand == other.WorldCommand;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override bool Equals(object obj)
        {
            return obj is MessageTypeUnion other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Type;

                switch (Type)
                {
                    case MessageType.Update:
                        hashCode = (hashCode * 397) ^ (int) UpdateInfo;
                        break;
                    case MessageType.CommandRequest:
                        hashCode = (hashCode * 397) ^ CommandInfo.GetHashCode();
                        break;
                    case MessageType.CommandResponse:
                        hashCode = (hashCode * 397) ^ CommandInfo.GetHashCode();
                        break;
                    case MessageType.WorldCommandRequest:
                        hashCode = (hashCode * 397) ^ (int) WorldCommand;
                        break;
                    case MessageType.WorldCommandResponse:
                        hashCode = (hashCode * 397) ^ (int) WorldCommand;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return hashCode;
            }
        }

        public static bool operator ==(MessageTypeUnion left, MessageTypeUnion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MessageTypeUnion left, MessageTypeUnion right)
        {
            return !left.Equals(right);
        }
    }
}

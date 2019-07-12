using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Core
{
    // todo consider exposing the private methods - the upper limit one can be used to find the last event/auth change
    internal static class ReceivedMessageListExtensions
    {
        // binary search for the command for given request ID
        // invariant: lower <= target <= upper
        public static int GetResponseIndex<T>(this MessageList<T> list, long requestId)
            where T : struct, IReceivedCommandResponse
        {
            long targetId = requestId;

            int lower = 0;
            int upper = list.Count - 1;

            while (upper >= lower)
            {
                var current = (lower + upper) / 2;

                long id = list[current].GetRequestId();

                if (id > targetId)
                {
                    upper = current - 1;
                }
                else if (id < targetId)
                {
                    lower = current + 1;
                }
                else
                {
                    return current;
                }
            }

            return -1;
        }

        public static (int FirstIndex, int Count) GetEntityRange<T>(this MessageList<T> list, EntityId entityId)
            where T : struct, IReceivedEntityMessage
        {
            var range = list.LimitEntityRangeUpper(entityId, 0, list.Count);
            if (range.Count > 1)
            {
                range = list.LimitEntityRangeLower(entityId, range.FirstIndex, range.Count);
            }

            return range;
        }

        // binary search for the first update for given entity ID
        // invariant: lower < target <= upper
        private static (int FirstIndex, int Count) LimitEntityRangeLower<T>(this MessageList<T> list,
            EntityId entityId, int index, int length)
            where T : struct, IReceivedEntityMessage
        {
            long targetId = entityId.Id;

            int lower = index - 1;
            int upper = index + length - 1;

            // > last index with entity ID = targetId
            int lastIndexUpperBound = upper + 1;

            while (upper > lower)
            {
                var current = (lower + upper + 1) / 2;

                long id = list[current].GetEntityId().Id;

                if (id == targetId)
                {
                    if (current == 0 || list[current - 1].GetEntityId().Id != targetId)
                    {
                        return (current, lastIndexUpperBound - current);
                    }

                    upper = current;
                }
                else if (id > targetId)
                {
                    lastIndexUpperBound = current;
                    upper = current - 1;
                }
                else if (id < targetId)
                {
                    lower = current;
                }
            }

            return (-1, 0);
        }

        // binary search for the last update for given entity ID with bounds
        // invariant: lower <= target < upper
        private static (int FirstIndex, int Count) LimitEntityRangeUpper<T>(this MessageList<T> list,
            EntityId entityId, int index, int length)
            where T : struct, IReceivedEntityMessage
        {
            long targetId = entityId.Id;
            int lower = index;
            int upper = lower + length;

            // < first index with entity ID = targetId
            int firstIndexLowerBound = lower - 1;

            while (upper > lower)
            {
                var current = (lower + upper) / 2;

                long id = list[current].GetEntityId().Id;

                if (id == targetId)
                {
                    if (current == list.Count - 1 || list[current + 1].GetEntityId().Id != targetId)
                    {
                        return (firstIndexLowerBound + 1, current - firstIndexLowerBound);
                    }

                    lower = current + 1;
                }
                else if (id > targetId)
                {
                    upper = current;
                }
                else if (id < targetId)
                {
                    firstIndexLowerBound = current;
                    lower = current + 1;
                }
            }

            return (-1, 0);
        }
    }
}

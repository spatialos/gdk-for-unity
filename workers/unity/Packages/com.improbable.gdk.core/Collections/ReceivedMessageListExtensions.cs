namespace Improbable.Gdk.Core
{
    internal static class ReceivedMessageListExtensions
    {
        public static (int firstIndex, int count) GetEntityRange<T>(this ReceivedMessageList<T> list, EntityId entityId)
            where T : struct, IReceivedEntityMessage
        {
            long targetId = entityId.Id;
            int firstIndex = 0;

            // binary search for the first update for given entity ID
            // invariant: lower < target <= upper
            int lower = -1;
            int upper = list.Count - 1;
            int lastIndexUpperBound = list.Count;

            while (upper > lower)
            {
                var index = (lower + upper + 1) / 2;

                long id = list[index].GetEntityId().Id;

                if (id == targetId)
                {
                    if (index == 0 || list[index - 1].GetEntityId().Id != targetId)
                    {
                        firstIndex = index;
                        break;
                    }

                    upper = index;
                }
                else if (id > targetId)
                {
                    lastIndexUpperBound = index;
                    upper = index - 1;
                }
                else if (id < targetId)
                {
                    lower = index;
                }
            }


            if (firstIndex == -1)
            {
                return (-1, 0);
            }

            // binary search for the last update for given entity ID with bounds
            // invariant: lower <= target < upper
            // starting bounds must obey the invariant
            lower = firstIndex;
            upper = lastIndexUpperBound;

            while (upper > lower)
            {
                var index = (lower + upper) / 2;

                long id = list[index].GetEntityId().Id;

                if (id == targetId)
                {
                    if (index == list.Count - 1 || list[index + 1].GetEntityId().Id != targetId)
                    {
                        return (firstIndex, index - firstIndex + 1);
                    }

                    lower = index + 1;
                }
                else if (id > targetId)
                {
                    upper = index;
                }
                else if (id < targetId)
                {
                    lower = index + 1;
                }
            }

            return (-1, 0);
        }
    }
}

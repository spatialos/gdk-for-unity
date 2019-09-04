namespace Improbable.Gdk.Core.NetworkStats
{
    public struct DataPoint
    {
        public uint Count;
        public uint Size;

        public static DataPoint operator +(DataPoint first, DataPoint second)
        {
            return new DataPoint
            {
                Count = first.Count + second.Count,
                Size = first.Size + second.Size
            };
        }
    }
}

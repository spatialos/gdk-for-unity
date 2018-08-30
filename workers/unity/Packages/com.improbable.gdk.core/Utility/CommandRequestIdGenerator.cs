namespace Improbable.Gdk.Core
{
    public static class CommandRequestIdGenerator
    {
        private static long nextRequestId;

        public static long GetNext()
        {
            nextRequestId++;

            return nextRequestId;
        }
    }
}

namespace Improbable.Gdk.Core
{
    public static class CommandRequestIdGenerator
    {
        private static uint nextRequestId;

        public static uint GetNext()
        {
            nextRequestId++;

            return nextRequestId;
        }
    }
}

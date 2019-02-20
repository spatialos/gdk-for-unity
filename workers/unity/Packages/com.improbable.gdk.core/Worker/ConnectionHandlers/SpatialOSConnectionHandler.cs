using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class SpatialOSConnectionHandler : IConnectionHandler
    {
        private readonly ViewDiff diff = new ViewDiff();
        private readonly OpListDeserializer deserializer = new OpListDeserializer();
        private readonly SerializedMessagesToSend messagesToSend = new SerializedMessagesToSend();

        private readonly CommandMetaData commandMetaData = new CommandMetaData();

        private readonly Connection connection;

        public SpatialOSConnectionHandler(Connection connection)
        {
            this.connection = connection;
        }

        public bool IsConnected()
        {
            return connection.GetConnectionStatusCode() == ConnectionStatusCode.Success;
        }

        public ViewDiff GetMessagesReceived()
        {
            commandMetaData.FlushRemovedIds();
            diff.Clear();

            bool inCriticalSection = false;
            do
            {
                using (var opList = connection.GetOpList(0))
                {
                    inCriticalSection = deserializer.ParseOpListIntoDiff(opList, diff, commandMetaData);
                }
            }
            while (inCriticalSection);

            deserializer.Reset();

            return diff;
        }

        public void PushMessagesToSend(MessagesToSend messages)
        {
            messagesToSend.SerializeFrom(messages, commandMetaData);
            messagesToSend.SendAll(connection, commandMetaData);
            messagesToSend.Clear();
        }
    }
}

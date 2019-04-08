using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class SpatialOSConnectionHandler : IConnectionHandler
    {
        private readonly OpListConverter converter = new OpListConverter();
        private readonly MessagesToSend messagesToSend = new MessagesToSend();
        private readonly SerializedMessagesToSend serializedMessagesToSend = new SerializedMessagesToSend();

        private readonly CommandMetaDataManager commandMetaDataManager = new CommandMetaDataManager();

        private readonly Connection connection;

        public SpatialOSConnectionHandler(Connection connection)
        {
            this.connection = connection;
        }

        public bool IsConnected()
        {
            return connection.GetConnectionStatusCode() == ConnectionStatusCode.Success;
        }

        public void GetMessagesReceived(ref ViewDiff viewDiff)
        {
            bool inCriticalSection = false;
            do
            {
                using (var opList = connection.GetOpList(0))
                {
                    inCriticalSection = converter.ParseOpListIntoDiff(opList, commandMetaDataManager.GetAllCommandMetaData());
                }
            }
            while (inCriticalSection);

            viewDiff = converter.GetViewDiff();
        }

        public MessagesToSend GetMessagesToSendContainer()
        {
            return messagesToSend;
        }

        public void PushMessagesToSend(MessagesToSend messages)
        {
            serializedMessagesToSend.SerializeFrom(messages, commandMetaDataManager.GetEmptyMetaDataStorage());
            var metaData = serializedMessagesToSend.SendAndClear(connection);
            commandMetaDataManager.AddMetaData(metaData);
            serializedMessagesToSend.Clear();
            messages.Clear();
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}

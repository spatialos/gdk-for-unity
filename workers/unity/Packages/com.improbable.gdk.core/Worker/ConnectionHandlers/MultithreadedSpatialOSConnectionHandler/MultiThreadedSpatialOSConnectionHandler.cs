using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public class MultiThreadedSpatialOSConnectionHandler : IConnectionHandler
    {
        private readonly Connection connection;

        private readonly CommandMetaDataManager commandMetaDataManager;

        private readonly ThreadedSerializationHandler serializationHandler;
        private readonly ThreadedDeserializationHandler deserializationHandler;

        public MultiThreadedSpatialOSConnectionHandler(Connection connection)
        {
            this.connection = connection;

            commandMetaDataManager = new CommandMetaDataManager();
            serializationHandler = new ThreadedSerializationHandler(commandMetaDataManager);
            deserializationHandler = new ThreadedDeserializationHandler(commandMetaDataManager);
        }

        public bool IsConnected()
        {
            return connection.GetConnectionStatusCode() == ConnectionStatusCode.Success;
        }

        public void GetMessagesReceived(ref ViewDiff viewDiff)
        {
            deserializationHandler.GetDiff(ref viewDiff);

            var opList = connection.GetOpList(0);
            if (opList.GetOpCount() == 0)
            {
                opList.Dispose();
            }
            else
            {
                deserializationHandler.AddOpListToDeserialize(opList);
            }
        }

        public MessagesToSend GetMessagesToSendContainer()
        {
            return serializationHandler.GetMessagesToSendContainer();
        }

        public void PushMessagesToSend(MessagesToSend messages)
        {
            SendSerializedMessages();
            serializationHandler.EnqueueMessagesToSend(messages);
        }

        private void SendSerializedMessages()
        {
            while (serializationHandler.TryDequeueSerializedMessages(out var messages))
            {
                var metaData = messages.SendAndClear(connection);
                serializationHandler.ReturnSerializedMessageContainer(messages);
                commandMetaDataManager.AddMetaData(metaData);
            }
        }

        public void Dispose()
        {
            serializationHandler.Dispose();
            deserializationHandler.Dispose();
            connection?.Dispose();
        }
    }
}

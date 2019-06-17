using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Improbable.Gdk.Core
{
    internal class ThreadedSerializationHandler : IDisposable
    {
        private readonly CommandMetaDataManager commandMetaDataManager;

        private readonly BlockingCollection<MessagesToSend> messagesToSerialize;
        private readonly ConcurrentPool<MessagesToSend> messagesToSendPool;

        private readonly ConcurrentQueue<SerializedMessagesToSend> serializedMessages;
        private readonly ConcurrentPool<SerializedMessagesToSend> serializedMessagesToSendPool;

        private readonly Thread serialisationThread;

        public ThreadedSerializationHandler(CommandMetaDataManager commandMetaDataManager)
        {
            this.commandMetaDataManager = commandMetaDataManager;

            messagesToSerialize = new BlockingCollection<MessagesToSend>(new ConcurrentQueue<MessagesToSend>());
            serializedMessagesToSendPool = new ConcurrentPool<SerializedMessagesToSend>();
            messagesToSendPool = new ConcurrentPool<MessagesToSend>();
            serializedMessages = new ConcurrentQueue<SerializedMessagesToSend>();

            serialisationThread = new Thread(SerializeMessages);
            serialisationThread.Start();
        }

        public void EnqueueMessagesToSend(MessagesToSend messages)
        {
            messagesToSerialize.Add(messages);
        }

        public bool TryDequeueSerializedMessages(out SerializedMessagesToSend serializedMessagesToSend)
        {
            return serializedMessages.TryDequeue(out serializedMessagesToSend);
        }

        public MessagesToSend GetMessagesToSendContainer()
        {
            return messagesToSendPool.Rent();
        }

        public void ReturnSerializedMessageContainer(SerializedMessagesToSend serializedMessagesToSend)
        {
            serializedMessagesToSendPool.Return(serializedMessagesToSend);
        }

        private void SerializeMessages()
        {
            // Block until a new message is available to serialize.
            while (messagesToSerialize.TryTake(out var messagesToSend, -1))
            {
                // If adding is complete then there is no reason to deserialize the message.
                if (messagesToSerialize.IsAddingCompleted)
                {
                    return;
                }

                SerializedMessagesToSend serialized = serializedMessagesToSendPool.Rent();
                CommandMetaData metaData = commandMetaDataManager.GetEmptyMetaDataStorage();

                // Serialize the messages and add them to the queue to be sent
                serialized.SerializeFrom(messagesToSend, metaData);
                messagesToSend.Clear();
                messagesToSendPool.Return(messagesToSend);
                serializedMessages.Enqueue(serialized);
            }
        }

        public void Dispose()
        {
            // Signal that no more messages will be added and wait for the serialization thread to finish.
            messagesToSerialize.CompleteAdding();
            serialisationThread.Join();
            messagesToSerialize.Dispose();

            // Dispose of serialized but unsent messages.
            while (!serializedMessages.IsEmpty)
            {
                if (serializedMessages.TryDequeue(out var messages))
                {
                    messages.DestroyUnsentMessages();
                }
            }
        }
    }
}

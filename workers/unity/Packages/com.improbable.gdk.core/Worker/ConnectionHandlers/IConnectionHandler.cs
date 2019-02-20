namespace Improbable.Gdk.Core
{
    public interface IConnectionHandler
    {
        ViewDiff GetMessagesReceived();
        void PushMessagesToSend(MessagesToSend messages);
        bool IsConnected();
    }
}

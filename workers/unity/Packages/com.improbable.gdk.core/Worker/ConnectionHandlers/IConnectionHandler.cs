using System;

namespace Improbable.Gdk.Core
{
    public interface IConnectionHandler : IDisposable
    {
        void GetMessagesReceived(ref ViewDiff viewDiff);
        MessagesToSend GetMessagesToSendContainer();
        void PushMessagesToSend(MessagesToSend messages);
        bool IsConnected();
    }
}

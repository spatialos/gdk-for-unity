using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public interface IConnectionHandler : IDisposable
    {
        string GetWorkerId();
        List<string> GetWorkerAttributes();
        void GetMessagesReceived(ref ViewDiff viewDiff);
        MessagesToSend GetMessagesToSendContainer();
        void PushMessagesToSend(MessagesToSend messages);
        bool IsConnected();
    }
}

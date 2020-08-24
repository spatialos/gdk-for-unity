using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core.NetworkStats;
using Improbable.Worker.CInterop;
using Unity.Profiling;

namespace Improbable.Gdk.Core
{
    public class SpatialOSConnectionHandler : IConnectionHandler
    {
        private readonly OpListConverter converter = new OpListConverter();
        private readonly MessagesToSend messagesToSend = new MessagesToSend();
        private readonly SerializedMessagesToSend serializedMessagesToSend = new SerializedMessagesToSend();

        private readonly CommandMetaData commandMetaData = new CommandMetaData();

        private readonly Connection connection;

        private ProfilerMarker serializeFromMarker = new ProfilerMarker("SpatialOSConnectionHandler.SerializeFrom");
        private ProfilerMarker sendClearMarker = new ProfilerMarker("SpatialOSConnectionHandler.SendAndClear");
        private ProfilerMarker clearMarker = new ProfilerMarker("SpatialOSConnectionHandler.Clear");

        public SpatialOSConnectionHandler(Connection connection)
        {
            this.connection = connection;
        }

        public bool IsConnected()
        {
            return connection.GetConnectionStatusCode() == ConnectionStatusCode.Success;
        }

        public string GetWorkerId()
        {
            return connection.GetWorkerId();
        }

        public List<string> GetWorkerAttributes()
        {
            return connection.GetWorkerAttributes();
        }

        public void GetMessagesReceived(ref ViewDiff viewDiff)
        {
            bool inCriticalSection = false;
            do
            {
                using (var opList = connection.GetOpList(0))
                {
                    inCriticalSection = converter.ParseOpListIntoDiff(opList, commandMetaData);
                }
            }
            while (inCriticalSection);

            viewDiff = converter.GetViewDiff();
        }

        public MessagesToSend GetMessagesToSendContainer()
        {
            return messagesToSend;
        }

        public void PushMessagesToSend(MessagesToSend messages, NetFrameStats frameStats)
        {
            using (serializeFromMarker.Auto())
            {
                serializedMessagesToSend.SerializeFrom(messages, commandMetaData);
            }

            using (sendClearMarker.Auto())
            {
                serializedMessagesToSend.SendAndClear(connection, commandMetaData, frameStats);
            }

            using (clearMarker.Auto())
            {
                messages.Clear();
            }
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}

// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public class CmdCommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>
        {
            private readonly Dictionary<long, CommandContext<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>> requestIdToRequest =
                new Dictionary<long, CommandContext<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>>();

            private readonly Dictionary<uint, long> internalRequestIdToRequestId = new Dictionary<uint, long>();

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 1;
            }

            public void RemoveMetaData(uint internalRequestId)
            {
                var requestId = internalRequestIdToRequestId[internalRequestId];
                internalRequestIdToRequestId.Remove(internalRequestId);
                requestIdToRequest.Remove(requestId);
            }

            public void AddRequestId(uint internalRequestId, long requestId)
            {
                internalRequestIdToRequestId.Add(internalRequestId, requestId);
            }

            public void AddRequest(CommandContext<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> context, long requestId)
            {
                requestIdToRequest[requestId] = context;
            }

            public long GetRequestId(uint internalRequestId)
            {
                return internalRequestIdToRequestId[internalRequestId];
            }

            public CommandContext<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> GetPayload(long requestId)
            {
                return requestIdToRequest[requestId];
            }
        }

    }
}

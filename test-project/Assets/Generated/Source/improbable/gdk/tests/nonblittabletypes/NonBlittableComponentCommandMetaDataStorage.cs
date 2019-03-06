// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class FirstCommandCommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest>
        {
            private readonly Dictionary<long, CommandContext<global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest>> requestIdToRequest =
                new Dictionary<long, CommandContext<global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest>>();

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

            public void AddRequest(CommandContext<global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest> context, long requestId)
            {
                requestIdToRequest[requestId] = context;
            }

            public long GetRequestId(uint internalRequestId)
            {
                return internalRequestIdToRequestId[internalRequestId];
            }

            public CommandContext<global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest> GetPayload(long requestId)
            {
                return requestIdToRequest[requestId];
            }
        }

        public class SecondCommandCommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest>
        {
            private readonly Dictionary<long, CommandContext<global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest>> requestIdToRequest =
                new Dictionary<long, CommandContext<global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest>>();

            private readonly Dictionary<uint, long> internalRequestIdToRequestId = new Dictionary<uint, long>();

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 2;
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

            public void AddRequest(CommandContext<global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest> context, long requestId)
            {
                requestIdToRequest[requestId] = context;
            }

            public long GetRequestId(uint internalRequestId)
            {
                return internalRequestIdToRequestId[internalRequestId];
            }

            public CommandContext<global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest> GetPayload(long requestId)
            {
                return requestIdToRequest[requestId];
            }
        }

    }
}

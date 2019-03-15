// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public class FirstCommandCommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest>
        {
            private readonly Dictionary<long, CommandContext<global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest>> requestIdToRequest =
                new Dictionary<long, CommandContext<global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest>>();

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

            public void SetInternalRequestId(uint internalRequestId, long requestId)
            {
                internalRequestIdToRequestId.Add(internalRequestId, requestId);
            }

            public void AddRequest(in CommandContext<global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest> context)
            {
                requestIdToRequest[context.RequestId] = context;
            }

            public CommandContext<global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest> GetPayload(uint internalRequestId)
            {
                var id = internalRequestIdToRequestId[internalRequestId];
                return requestIdToRequest[id];
            }
        }

        public class SecondCommandCommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest>
        {
            private readonly Dictionary<long, CommandContext<global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest>> requestIdToRequest =
                new Dictionary<long, CommandContext<global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest>>();

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

            public void SetInternalRequestId(uint internalRequestId, long requestId)
            {
                internalRequestIdToRequestId.Add(internalRequestId, requestId);
            }

            public void AddRequest(in CommandContext<global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest> context)
            {
                requestIdToRequest[context.RequestId] = context;
            }

            public CommandContext<global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest> GetPayload(uint internalRequestId)
            {
                var id = internalRequestIdToRequestId[internalRequestId];
                return requestIdToRequest[id];
            }
        }

    }
}

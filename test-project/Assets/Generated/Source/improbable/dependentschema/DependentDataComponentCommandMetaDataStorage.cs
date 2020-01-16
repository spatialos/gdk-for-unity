// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.DependentSchema
{
    public partial class DependentDataComponent
    {
        public class BarCommandCommandMetaDataStorage : ICommandMetaDataStorage, ICommandPayloadStorage<global::Improbable.TestSchema.SomeType>
        {
            private readonly Dictionary<long, CommandContext<global::Improbable.TestSchema.SomeType>> requestIdToRequest =
                new Dictionary<long, CommandContext<global::Improbable.TestSchema.SomeType>>();

            private readonly Dictionary<long, long> internalRequestIdToRequestId = new Dictionary<long, long>();

            public uint GetComponentId()
            {
                return ComponentId;
            }

            public uint GetCommandId()
            {
                return 1;
            }

            public void RemoveMetaData(long internalRequestId)
            {
                var requestId = internalRequestIdToRequestId[internalRequestId];
                internalRequestIdToRequestId.Remove(internalRequestId);
                requestIdToRequest.Remove(requestId);
            }

            public void SetInternalRequestId(long internalRequestId, long requestId)
            {
                internalRequestIdToRequestId.Add(internalRequestId, requestId);
            }

            public void AddRequest(in CommandContext<global::Improbable.TestSchema.SomeType> context)
            {
                requestIdToRequest[context.RequestId] = context;
            }

            public CommandContext<global::Improbable.TestSchema.SomeType> GetPayload(long internalRequestId)
            {
                var id = internalRequestIdToRequestId[internalRequestId];
                return requestIdToRequest[id];
            }
        }

    }
}

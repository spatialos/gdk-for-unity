// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.DependentSchema
{
    public partial class DependentDataComponent
    {
        public class CommandSenders
        {
            public struct BarCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Request> RequestsToSend
                {
                    get => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.BarCommandSenderProvider.Get(CommandListHandle);
                    set => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.BarCommandSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct BarCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedRequest> Requests
                {
                    get => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.BarCommandRequestsProvider.Get(CommandListHandle);
                    set => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.BarCommandRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct BarCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Response> ResponsesToSend
                {
                    get => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.BarCommandResponderProvider.Get(CommandListHandle);
                    set => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.BarCommandResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct BarCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedResponse> Responses
                {
                    get => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.BarCommandResponsesProvider.Get(CommandListHandle);
                    set => global::Improbable.DependentSchema.DependentDataComponent.ReferenceTypeProviders.BarCommandResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
#endif

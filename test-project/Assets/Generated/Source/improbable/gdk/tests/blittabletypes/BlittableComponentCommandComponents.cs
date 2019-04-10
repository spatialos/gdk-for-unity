// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

#if !DISABLE_REACTIVE_COMPONENTS
using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public class CommandSenders
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Request> RequestsToSend
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Request> RequestsToSend
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedRequest> Requests
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedRequest> Requests
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Response> ResponsesToSend
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Response> ResponsesToSend
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedResponse> Responses
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedResponse> Responses
                {
                    get => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Get(CommandListHandle);
                    set => global::Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}
#endif

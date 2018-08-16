// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class CommandSenders
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Request> RequestsToSend
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Request> RequestsToSend
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest> Requests
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest> Requests
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response> ResponsesToSend
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response> ResponsesToSend
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse> Responses
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse> Responses
                {
                    get => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}

// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class CommandSenders
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Request> RequestsToSend
                {
                    get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Request> RequestsToSend
                {
                    get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest> Requests
                {
                    get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest> Requests
                {
                    get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response> ResponsesToSend
                {
                    get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response> ResponsesToSend
                {
                    get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse> Responses
                {
                    get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse> Responses
                {
                    get => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}

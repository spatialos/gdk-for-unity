// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public class CommandSenders
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Request> RequestsToSend
                {
                    get => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Request> RequestsToSend
                {
                    get => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedRequest> Requests
                {
                    get => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedRequest> Requests
                {
                    get => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Response> ResponsesToSend
                {
                    get => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Response> ResponsesToSend
                {
                    get => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedResponse> Responses
                {
                    get => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedResponse> Responses
                {
                    get => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}

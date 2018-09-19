// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

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
                public List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Request> RequestsToSend
                {
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Request> RequestsToSend
                {
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedRequest> Requests
                {
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedRequest> Requests
                {
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Response> ResponsesToSend
                {
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Response> ResponsesToSend
                {
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct FirstCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedResponse> Responses
                {
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Set(CommandListHandle, value);
                }
            }
            public struct SecondCommand : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedResponse> Responses
                {
                    get => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}

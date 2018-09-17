// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public class CommandSenders
        {
            public struct Cmd : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Request> RequestsToSend
                {
                    get => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdSenderProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct Cmd : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest> Requests
                {
                    get => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdRequestsProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct Cmd : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Response> ResponsesToSend
                {
                    get => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct Cmd : IComponentData
            {
                internal uint CommandListHandle;
                public List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse> Responses
                {
                    get => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponsesProvider.Get(CommandListHandle);
                    set => Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}

// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System.Collections.Generic;
using Unity.Entities;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public class CommandSenders
        {
            public struct Cmd : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Request> RequestsToSend
                {
                    get => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdSenderProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdSenderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandRequests
        {
            public struct Cmd : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest> Requests
                {
                    get => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdRequestsProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdRequestsProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponders
        {
            public struct Cmd : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Response> ResponsesToSend
                {
                    get => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.Set(CommandListHandle, value);
                }
            }
        }

        public class CommandResponses
        {
            public struct Cmd : IComponentData
            {
                internal uint CommandListHandle;
                public List<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse> Responses
                {
                    get => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponsesProvider.Get(CommandListHandle);
                    set => Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponsesProvider.Set(CommandListHandle, value);
                }
            }
        }
    }
}

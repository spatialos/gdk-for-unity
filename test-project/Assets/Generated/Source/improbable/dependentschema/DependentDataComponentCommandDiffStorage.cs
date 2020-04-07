// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;

namespace Improbable.DependentSchema
{
    public partial class DependentDataComponent
    {
        private class DiffBarCommandCommandStorage
            : CommandDiffStorageBase<BarCommand.ReceivedRequest, BarCommand.ReceivedResponse>
        {
            public override uint ComponentId => Improbable.DependentSchema.DependentDataComponent.ComponentId;
            public override uint CommandId => 1;
        }

        private class BarCommandCommandsToSendStorage :
            CommandSendStorage<BarCommand.Request, BarCommand.Response>,
            IComponentCommandSendStorage
        {
            uint IComponentCommandSendStorage.ComponentId => ComponentId;
            uint IComponentCommandSendStorage.CommandId => 1;
        }
    }
}

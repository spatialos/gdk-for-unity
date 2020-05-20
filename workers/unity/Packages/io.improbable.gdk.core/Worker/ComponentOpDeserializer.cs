using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    internal static class ComponentOpDeserializer
    {
        private static readonly Dictionary<uint, IComponentDiffDeserializer> ComponentIdToComponentDeserializer =
            new Dictionary<uint, IComponentDiffDeserializer>();

        private static readonly Dictionary<(uint, uint), ICommandDiffDeserializer> CommandIdsToCommandDeserializer =
            new Dictionary<(uint, uint), ICommandDiffDeserializer>();

        static ComponentOpDeserializer()
        {
            foreach (var pair in ComponentDatabase.Metaclasses)
            {
                var componentId = pair.Key;
                var metaclass = pair.Value;

                var componentInstance =
                    (IComponentDiffDeserializer) Activator.CreateInstance(metaclass.DiffDeserializer);
                ComponentIdToComponentDeserializer.Add(componentId, componentInstance);

                foreach (var command in metaclass.Commands)
                {
                    var commandInstance = (ICommandDiffDeserializer) Activator.CreateInstance(command.DiffDeserializer);
                    CommandIdsToCommandDeserializer.Add((componentId, command.CommandIndex), commandInstance);
                }
            }
        }

        public static void DeserializeAndAddComponent(AddComponentOp op, ViewDiff viewDiff)
        {
            if (!ComponentIdToComponentDeserializer.TryGetValue(op.Data.ComponentId, out var deserializer))
            {
                throw new ArgumentException($"Can not deserialize component with ID {op.Data.ComponentId}");
            }

            deserializer.AddComponentToDiff(op, viewDiff);
        }

        public static void DeserializeAndApplyComponentUpdate(ComponentUpdateOp op, ViewDiff viewDiff, uint updateId)
        {
            if (!ComponentIdToComponentDeserializer.TryGetValue(op.Update.ComponentId, out var deserializer))
            {
                throw new ArgumentException($"Can not deserialize update on component with ID {op.Update.ComponentId}");
            }

            deserializer.AddUpdateToDiff(op, viewDiff, updateId);
            ++updateId;
        }

        public static void DeserializeAndApplyCommandRequestReceived(CommandRequestOp op, ViewDiff viewDiff)
        {
            if (!CommandIdsToCommandDeserializer.TryGetValue(
                (op.Request.ComponentId, op.Request.CommandIndex),
                out var deserializer))
            {
                throw new ArgumentException($"Can not deserialize command request with command index " +
                    $"{op.Request.CommandIndex} on component with ID {op.Request.ComponentId}");
            }

            deserializer.AddRequestToDiff(op, viewDiff);
        }

        public static void DeserializeAndApplyCommandResponseReceived(CommandResponseOp op, ViewDiff viewDiff,
            CommandMetaData commandMetaData)
        {
            if (!CommandIdsToCommandDeserializer.TryGetValue((op.Response.ComponentId, op.Response.CommandIndex),
                out var deserializer))
            {
                throw new ArgumentException($"Can not deserialize command response command index " +
                    $"{op.Response.CommandIndex} on component with ID {op.Response.ComponentId}");
            }

            deserializer.AddResponseToDiff(op, viewDiff, commandMetaData);
        }

        public static void ApplyCreateEntityResponse(CreateEntityResponseOp op, ViewDiff viewDiff,
            CommandMetaData commandMetaData)
        {
            var context = commandMetaData.GetContext<WorldCommands.CreateEntity.Request>(0, 0, op.RequestId);
            var response =
                new WorldCommands.CreateEntity.ReceivedResponse(op, context.SendingEntity, context.Request,
                    context.RequestId);

            commandMetaData.RemoveRequest(0, 0, op.RequestId);

            viewDiff.AddCreateEntityResponse(response);
        }

        public static void ApplyDeleteEntityResponse(DeleteEntityResponseOp op, ViewDiff viewDiff,
            CommandMetaData commandMetaData)
        {
            var context = commandMetaData.GetContext<WorldCommands.DeleteEntity.Request>(0, 0, op.RequestId);
            var response =
                new WorldCommands.DeleteEntity.ReceivedResponse(op, context.SendingEntity, context.Request,
                    context.RequestId);

            commandMetaData.RemoveRequest(0, 0, op.RequestId);

            viewDiff.AddDeleteEntityResponse(response);
        }

        public static void ApplyReserveEntityIdsResponse(ReserveEntityIdsResponseOp op, ViewDiff viewDiff,
            CommandMetaData commandMetaData)
        {
            var context = commandMetaData.GetContext<WorldCommands.ReserveEntityIds.Request>(0, 0, op.RequestId);
            var response =
                new WorldCommands.ReserveEntityIds.ReceivedResponse(op, context.SendingEntity, context.Request,
                    context.RequestId);

            commandMetaData.RemoveRequest(0, 0, op.RequestId);

            viewDiff.AddReserveEntityIdsResponse(response);
        }

        public static void ApplyEntityQueryResponse(EntityQueryResponseOp op, ViewDiff viewDiff,
            CommandMetaData commandMetaData)
        {
            var context = commandMetaData.GetContext<WorldCommands.EntityQuery.Request>(0, 0, op.RequestId);
            var response =
                new WorldCommands.EntityQuery.ReceivedResponse(op, context.SendingEntity, context.Request,
                    context.RequestId);

            commandMetaData.RemoveRequest(0, 0, op.RequestId);

            viewDiff.AddEntityQueryResponse(response);
        }
    }
}

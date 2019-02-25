using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    internal class OpListDeserializer
    {
        private readonly Dictionary<uint, IComponentDiffDeserializer> componentIdToComponentDeserializer =
            new Dictionary<uint, IComponentDiffDeserializer>();

        private readonly Dictionary<(uint, uint), ICommandDiffDeserializer> commandIdsToCommandDeserializer =
            new Dictionary<(uint, uint), ICommandDiffDeserializer>();

        private uint componentUpdateId;

        public OpListDeserializer()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IComponentDiffDeserializer).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (IComponentDiffDeserializer) Activator.CreateInstance(type);

                        componentIdToComponentDeserializer.Add(instance.GetComponentId(), instance);
                    }

                    if (typeof(ICommandDiffDeserializer).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var instance = (ICommandDiffDeserializer) Activator.CreateInstance(type);

                        commandIdsToCommandDeserializer.Add((instance.GetComponentId(), instance.GetCommandId()), instance);
                    }
                }
            }
        }

        public void Reset()
        {
            componentUpdateId = 1;
        }

        public bool ParseOpListIntoDiff(OpList opList, ViewDiff viewDiff, CommandMetaData commandMetaData)
        {
            bool inCriticalSection = false;
            for (int i = 0; i < opList.GetOpCount(); ++i)
            {
                switch (opList.GetOpType(i))
                {
                    case OpType.Disconnect:
                        viewDiff.Disconnect(opList.GetDisconnectOp(i).Reason);
                        return false;
                    case OpType.FlagUpdate:
                        var flagOp = opList.GetFlagUpdateOp(i);
                        break;
                    case OpType.LogMessage:
                        var logOp = opList.GetLogMessageOp(i);
                        break;
                    case OpType.Metrics:
                        var metricsOp = opList.GetMetricsOp(i);
                        break;
                    case OpType.CriticalSection:
                        var criticalSectionOp = opList.GetCriticalSectionOp(i);
                        inCriticalSection = criticalSectionOp.InCriticalSection;
                        break;
                    case OpType.AddEntity:
                        viewDiff.AddEntity(opList.GetAddEntityOp(i).EntityId);
                        break;
                    case OpType.RemoveEntity:
                        viewDiff.RemoveEntity(opList.GetRemoveEntityOp(i).EntityId);
                        break;
                    case OpType.ReserveEntityIdResponse:
                        throw new InvalidOperationException("Reserve Entity ID is deprecated. Please use Reserve Entity IDs");
                    case OpType.ReserveEntityIdsResponse:
                        var reserveEntityIdsOp = opList.GetReserveEntityIdsResponseOp(i);
                        ApplyReserveEntityIdsResponse(reserveEntityIdsOp, viewDiff, commandMetaData);
                        break;
                    case OpType.CreateEntityResponse:
                        var createEntityOp = opList.GetCreateEntityResponseOp(i);
                        ApplyCreateEntityResponse(createEntityOp, viewDiff, commandMetaData);
                        break;
                    case OpType.DeleteEntityResponse:
                        var deleteEntityOp = opList.GetDeleteEntityResponseOp(i);
                        ApplyDeleteEntityResponse(deleteEntityOp, viewDiff, commandMetaData);
                        break;
                    case OpType.EntityQueryResponse:
                        var entityQueryOp = opList.GetEntityQueryResponseOp(i);
                        ApplyEntityQueryResponse(entityQueryOp, viewDiff, commandMetaData);
                        break;
                    case OpType.AddComponent:
                        DeserializeAndAddComponent(opList.GetAddComponentOp(i), viewDiff);
                        break;
                    case OpType.RemoveComponent:
                        var removeComponentOp = opList.GetRemoveComponentOp(i);
                        viewDiff.RemoveComponent(removeComponentOp.EntityId, removeComponentOp.ComponentId);
                        break;
                    case OpType.AuthorityChange:
                        var authorityOp = opList.GetAuthorityChangeOp(i);
                        viewDiff.SetAuthority(authorityOp.EntityId, authorityOp.ComponentId, authorityOp.Authority);
                        break;
                    case OpType.ComponentUpdate:
                        DeserializeAndApplyComponentUpdate(opList.GetComponentUpdateOp(i), viewDiff);
                        break;
                    case OpType.CommandRequest:
                        DeserializeApplyCommandRequestReceived(opList.GetCommandRequestOp(i), viewDiff);
                        break;
                    case OpType.CommandResponse:
                        DeserializeAndApplyCommandResponseReceived(opList.GetCommandResponseOp(i), viewDiff,
                            commandMetaData);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Can not deserialise unkown op type {opList.GetOpType(i)}");
                }
            }

            return inCriticalSection;
        }

        private void DeserializeAndAddComponent(AddComponentOp op, ViewDiff viewDiff)
        {
            if (!componentIdToComponentDeserializer.TryGetValue(op.Data.ComponentId, out var deserializer))
            {
                throw new ArgumentException($"Can not deserialize component with ID {op.Data.ComponentId}");
            }

            deserializer.AddComponentToDiff(op, viewDiff);
        }

        private void DeserializeAndApplyComponentUpdate(ComponentUpdateOp op, ViewDiff viewDiff)
        {
            if (!componentIdToComponentDeserializer.TryGetValue(op.Update.ComponentId, out var deserializer))
            {
                throw new ArgumentException($"Can not deserialize component with ID {op.Update.ComponentId}");
            }

            deserializer.AddUpdateToDiff(op, viewDiff, componentUpdateId);
            ++componentUpdateId;
        }

        private void DeserializeApplyCommandRequestReceived(CommandRequestOp op, ViewDiff viewDiff)
        {
            if (!commandIdsToCommandDeserializer.TryGetValue((op.Request.ComponentId, op.Request.SchemaData.Value.GetCommandIndex()),
                out var deserializer))
            {
                throw new ArgumentException($"Can not deserialize component with ID {op.Request.ComponentId}");
            }

            deserializer.AddRequestToDiff(op, viewDiff);
        }

        private void DeserializeAndApplyCommandResponseReceived(CommandResponseOp op, ViewDiff viewDiff, CommandMetaData commandMetaData)
        {
            if (!commandIdsToCommandDeserializer.TryGetValue((op.Response.ComponentId, op.CommandIndex),
                out var deserializer))
            {
                throw new ArgumentException($"Can not deserialize component with ID {op.Response.ComponentId}");
            }

            deserializer.AddResponseToDiff(op, viewDiff, commandMetaData);
        }

        private void ApplyCreateEntityResponse(CreateEntityResponseOp op, ViewDiff viewDiff,
            CommandMetaData commandMetaData)
        {
            var id = commandMetaData.GetRequestId(0, 0, op.RequestId);
            var context = commandMetaData.GetContext<WorldCommands.CreateEntity.Request>(0, 0, id);
            var response =
                new WorldCommands.CreateEntity.ReceivedResponse(op, context.SendingEntity, context.Request, id);

            commandMetaData.MarkIdForRemoval(0, 0, op.RequestId);

            viewDiff.AddCreateEntityResponse(response);
        }

        private void ApplyDeleteEntityResponse(DeleteEntityResponseOp op, ViewDiff viewDiff,
            CommandMetaData commandMetaData)
        {
            var id = commandMetaData.GetRequestId(0, 0, op.RequestId);
            var context = commandMetaData.GetContext<WorldCommands.DeleteEntity.Request>(0, 0, id);
            var response =
                new WorldCommands.DeleteEntity.ReceivedResponse(op, context.SendingEntity, context.Request, id);

            commandMetaData.MarkIdForRemoval(0, 0, op.RequestId);

            viewDiff.AddDeleteEntityResponse(response);
        }

        private void ApplyReserveEntityIdsResponse(ReserveEntityIdsResponseOp op, ViewDiff viewDiff,
            CommandMetaData commandMetaData)
        {
            var id = commandMetaData.GetRequestId(0, 0, op.RequestId);
            var context = commandMetaData.GetContext<WorldCommands.ReserveEntityIds.Request>(0, 0, id);
            var response =
                new WorldCommands.ReserveEntityIds.ReceivedResponse(op, context.SendingEntity, context.Request, id);

            commandMetaData.MarkIdForRemoval(0, 0, op.RequestId);

            viewDiff.AddReserveEntityIdsResponse(response);
        }

        private void ApplyEntityQueryResponse(EntityQueryResponseOp op, ViewDiff viewDiff,
            CommandMetaData commandMetaData)
        {
            var id = commandMetaData.GetRequestId(0, 0, op.RequestId);
            var context = commandMetaData.GetContext<WorldCommands.EntityQuery.Request>(0, 0, id);
            var response =
                new WorldCommands.EntityQuery.ReceivedResponse(op, context.SendingEntity, context.Request, id);

            commandMetaData.MarkIdForRemoval(0, 0, op.RequestId);

            viewDiff.AddEntityQueryResponse(response);
        }
    }
}

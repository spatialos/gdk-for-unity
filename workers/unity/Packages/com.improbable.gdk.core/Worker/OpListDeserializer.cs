using System;
using System.Collections.Generic;
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

        public bool ParseOpListIntoDiff(OpList opList, ViewDiff viewDiff)
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
                        viewDiff.AddReserveEntityIdsResponse(reserveEntityIdsOp);
                        break;
                    case OpType.CreateEntityResponse:
                        var createEntityOp = opList.GetCreateEntityResponseOp(i);
                        viewDiff.AddCreateEntityResponse(createEntityOp);
                        break;
                    case OpType.DeleteEntityResponse:
                        var deleteEntityOp = opList.GetDeleteEntityResponseOp(i);
                        viewDiff.AddDeleteEntityResponse(deleteEntityOp);
                        break;
                    case OpType.EntityQueryResponse:
                        var entityQueryOp = opList.GetEntityQueryResponseOp(i);
                        viewDiff.AddEntityQueryResponse(entityQueryOp);
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
                        DeserializeAndApplyCommandResponseReceived(opList.GetCommandResponseOp(i), viewDiff);
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

        private void DeserializeAndApplyCommandResponseReceived(CommandResponseOp op, ViewDiff viewDiff)
        {
            if (!commandIdsToCommandDeserializer.TryGetValue((op.Response.ComponentId, op.CommandIndex),
                out var deserializer))
            {
                throw new ArgumentException($"Can not deserialize component with ID {op.Response.ComponentId}");
            }

            deserializer.AddResponseToDiff(op, viewDiff);
        }
    }
}

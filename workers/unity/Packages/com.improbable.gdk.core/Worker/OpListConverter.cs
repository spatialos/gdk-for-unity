using System;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    internal class OpListConverter
    {
        private readonly ViewDiff viewDiff = new ViewDiff();

        private uint componentUpdateId;

        private bool shouldClear;

        /// <summary>
        ///     Iterate over the op list and populate a ViewDiff from the data contained.
        /// </summary>
        /// <returns>True if the current ViewDiff is inside a critical section.</returns>
        public bool ParseOpListIntoDiff(OpList opList, CommandMetaDataAggregate commandMetaData)
        {
            if (shouldClear)
            {
                viewDiff.Clear();
                shouldClear = false;
            }

            for (int i = 0; i < opList.GetOpCount(); ++i)
            {
                switch (opList.GetOpType(i))
                {
                    case OpType.Disconnect:
                        viewDiff.Disconnect(opList.GetDisconnectOp(i).Reason);
                        break;
                    case OpType.FlagUpdate:
                        var flagOp = opList.GetFlagUpdateOp(i);
                        viewDiff.SetWorkerFlag(flagOp.Name, flagOp.Value);
                        break;
                    case OpType.LogMessage:
                        var logOp = opList.GetLogMessageOp(i);
                        viewDiff.AddLogMessage(logOp.Message, logOp.Level);
                        break;
                    case OpType.Metrics:
                        var metricsOp = opList.GetMetricsOp(i);
                        viewDiff.AddMetrics(metricsOp.Metrics);
                        break;
                    case OpType.CriticalSection:
                        var criticalSectionOp = opList.GetCriticalSectionOp(i);
                        viewDiff.SetCriticalSection(criticalSectionOp.InCriticalSection);
                        break;
                    case OpType.AddEntity:
                        viewDiff.AddEntity(opList.GetAddEntityOp(i).EntityId);
                        break;
                    case OpType.RemoveEntity:
                        viewDiff.RemoveEntity(opList.GetRemoveEntityOp(i).EntityId);
                        break;
                    case OpType.ReserveEntityIdResponse:
                        throw new InvalidOperationException(
                            "Reserve Entity ID is deprecated. Please use Reserve Entity IDs");
                    case OpType.ReserveEntityIdsResponse:
                        var reserveEntityIdsOp = opList.GetReserveEntityIdsResponseOp(i);
                        ComponentOpDeserializer.ApplyReserveEntityIdsResponse(reserveEntityIdsOp, viewDiff,
                            commandMetaData);
                        break;
                    case OpType.CreateEntityResponse:
                        var createEntityOp = opList.GetCreateEntityResponseOp(i);
                        ComponentOpDeserializer.ApplyCreateEntityResponse(createEntityOp, viewDiff, commandMetaData);
                        break;
                    case OpType.DeleteEntityResponse:
                        var deleteEntityOp = opList.GetDeleteEntityResponseOp(i);
                        ComponentOpDeserializer.ApplyDeleteEntityResponse(deleteEntityOp, viewDiff, commandMetaData);
                        break;
                    case OpType.EntityQueryResponse:
                        var entityQueryOp = opList.GetEntityQueryResponseOp(i);
                        ComponentOpDeserializer.ApplyEntityQueryResponse(entityQueryOp, viewDiff, commandMetaData);
                        break;
                    case OpType.AddComponent:
                        ComponentOpDeserializer.DeserializeAndAddComponent(opList.GetAddComponentOp(i), viewDiff);
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
                        ComponentOpDeserializer.DeserializeAndApplyComponentUpdate(opList.GetComponentUpdateOp(i),
                            viewDiff, componentUpdateId);
                        ++componentUpdateId;
                        break;
                    case OpType.CommandRequest:
                        ComponentOpDeserializer.DeserializeAndApplyCommandRequestReceived(opList.GetCommandRequestOp(i),
                            viewDiff);
                        break;
                    case OpType.CommandResponse:
                        ComponentOpDeserializer.DeserializeAndApplyCommandResponseReceived(
                            opList.GetCommandResponseOp(i), viewDiff,
                            commandMetaData);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Can not deserialise unkown op type {opList.GetOpType(i)}");
                }
            }

            return viewDiff.InCriticalSection;
        }

        public ViewDiff GetViewDiff()
        {
            componentUpdateId = 1;
            shouldClear = true;
            return viewDiff;
        }
    }
}

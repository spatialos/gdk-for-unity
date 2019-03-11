using System;
using System.Threading;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Simplest possible current op-list-to-diff converter.
    ///     Acquires a lock when deserializing an op list and releases it when done
    /// </summary>
    /// <remarks>
    ///     This is not able to interrupt the deserialization thread to exchange diffs, which could increase latency when under heavy load.
    ///     If this presents a problem it could be improved with either a more complicated lock or a lock free approach.
    ///     Similarly if part of a critical section is received regularly the diffs may often not be possible to exchange, increasing latency.
    ///     To solve this we could buffer critical sections into an intermediate object and then move them to the main diff.
    ///     However this case is unlikely and the overhead associated with fixing it may be undesirable.
    /// </remarks>
    public class ConcurrentOpListConverter
    {
        // One ViewDiff contains the ops deserialized between the last two calls to TryGetViewDiff.
        // The other is the ViewDiff ops are currently being Deserialized into.
        // These diffs swap on a successful call to TryGetViewDiff.
        private readonly ViewDiff[] viewDiffs =
        {
            new ViewDiff(), new ViewDiff()
        };

        private uint diffIndex = 0;

        private ViewDiff DeserializationDiff => viewDiffs[diffIndex];
        private ViewDiff AvailableDiff => viewDiffs[diffIndex ^ 1];

        private uint componentUpdateId;

        private object viewLock = new object();

        /// <summary>
        ///     Iterate over the op list and populate a ViewDiff from the data contained.
        ///     Must not be called again before it returns.
        /// </summary>
        public void ParseOpListIntoDiff(OpList opList, CommandMetaDataAggregate commandMetaData)
        {
            lock (viewLock)
            {
                for (int i = 0; i < opList.GetOpCount(); ++i)
                {
                    switch (opList.GetOpType(i))
                    {
                        case OpType.Disconnect:
                            DeserializationDiff.Disconnect(opList.GetDisconnectOp(i).Reason);
                            break;
                        case OpType.FlagUpdate:
                            var flagOp = opList.GetFlagUpdateOp(i);
                            break;
                        case OpType.LogMessage:
                            var logOp = opList.GetLogMessageOp(i);
                            DeserializationDiff.AddLogMessage(logOp.Message, logOp.Level);
                            break;
                        case OpType.Metrics:
                            var metricsOp = opList.GetMetricsOp(i);
                            DeserializationDiff.AddMetrics(metricsOp.Metrics);
                            break;
                        case OpType.CriticalSection:
                            var criticalSectionOp = opList.GetCriticalSectionOp(i);
                            DeserializationDiff.SetCriticalSection(criticalSectionOp.InCriticalSection);
                            break;
                        case OpType.AddEntity:
                            DeserializationDiff.AddEntity(opList.GetAddEntityOp(i).EntityId);
                            break;
                        case OpType.RemoveEntity:
                            DeserializationDiff.RemoveEntity(opList.GetRemoveEntityOp(i).EntityId);
                            break;
                        case OpType.ReserveEntityIdResponse:
                            throw new InvalidOperationException(
                                "Reserve Entity ID is deprecated. Please use Reserve Entity IDs");
                        case OpType.ReserveEntityIdsResponse:
                            var reserveEntityIdsOp = opList.GetReserveEntityIdsResponseOp(i);
                            ComponentOpDeserializer.ApplyReserveEntityIdsResponse(reserveEntityIdsOp,
                                DeserializationDiff,
                                commandMetaData);
                            break;
                        case OpType.CreateEntityResponse:
                            var createEntityOp = opList.GetCreateEntityResponseOp(i);
                            ComponentOpDeserializer.ApplyCreateEntityResponse(createEntityOp, DeserializationDiff,
                                commandMetaData);
                            break;
                        case OpType.DeleteEntityResponse:
                            var deleteEntityOp = opList.GetDeleteEntityResponseOp(i);
                            ComponentOpDeserializer.ApplyDeleteEntityResponse(deleteEntityOp, DeserializationDiff,
                                commandMetaData);
                            break;
                        case OpType.EntityQueryResponse:
                            var entityQueryOp = opList.GetEntityQueryResponseOp(i);
                            ComponentOpDeserializer.ApplyEntityQueryResponse(entityQueryOp, DeserializationDiff,
                                commandMetaData);
                            break;
                        case OpType.AddComponent:
                            ComponentOpDeserializer.DeserializeAndAddComponent(opList.GetAddComponentOp(i),
                                DeserializationDiff);
                            break;
                        case OpType.RemoveComponent:
                            var removeComponentOp = opList.GetRemoveComponentOp(i);
                            DeserializationDiff.RemoveComponent(removeComponentOp.EntityId,
                                removeComponentOp.ComponentId);
                            break;
                        case OpType.AuthorityChange:
                            var authorityOp = opList.GetAuthorityChangeOp(i);
                            DeserializationDiff.SetAuthority(authorityOp.EntityId, authorityOp.ComponentId,
                                authorityOp.Authority);
                            break;
                        case OpType.ComponentUpdate:
                            ComponentOpDeserializer.DeserializeAndApplyComponentUpdate(opList.GetComponentUpdateOp(i),
                                DeserializationDiff, componentUpdateId);
                            ++componentUpdateId;
                            break;
                        case OpType.CommandRequest:
                            ComponentOpDeserializer.DeserializeAndApplyCommandRequestReceived(
                                opList.GetCommandRequestOp(i),
                                DeserializationDiff);
                            break;
                        case OpType.CommandResponse:
                            ComponentOpDeserializer.DeserializeAndApplyCommandResponseReceived(
                                opList.GetCommandResponseOp(i), DeserializationDiff,
                                commandMetaData);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(
                                $"Can not deserialise unkown op type {opList.GetOpType(i)}");
                    }
                }
            }
        }

        /// <summary>
        ///     Try to get a diff containing ops deserialized since the last call.
        ///     If successful the diff may only contain part of the last op list.
        /// </summary>
        /// <returns>True if the diffs could be exchanged and false otherwise.</returns>
        public bool TryGetViewDiff(out ViewDiff viewDiff)
        {
            // If the lock can not be entered then the previous op list is still being processed.
            if (!Monitor.TryEnter(viewLock))
            {
                viewDiff = default;
                return false;
            }

            try
            {
                // If we are in a critical section then do not swap the diffs.
                if (DeserializationDiff.InCriticalSection)
                {
                    viewDiff = default;
                    return false;
                }

                // Swap the diffs and return the potentially non-empty one.
                diffIndex = diffIndex ^ 1;
                DeserializationDiff.Clear();
                componentUpdateId = 1;

                viewDiff = AvailableDiff;
                return true;
            }
            finally
            {
                Monitor.Exit(viewLock);
            }
        }
    }
}

using System;
using System.Linq;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core.NetworkStats;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    internal class OpListConverter
    {
        private readonly ViewDiff viewDiff = new ViewDiff();
        private readonly IComponentSetManager componentSetManager;

        private uint componentUpdateId;
        private uint authorityChangeId;

        private bool shouldClear;

        public OpListConverter()
        {
            componentSetManager = (IComponentSetManager) Activator.CreateInstance(TypeCache.ComponentSetManager.Value);
        }

        /// <summary>
        ///     Iterate over the op list and populate a ViewDiff from the data contained.
        /// </summary>
        /// <returns>True if the current ViewDiff is inside a critical section.</returns>
        public bool ParseOpListIntoDiff(OpList opList, CommandMetaData commandMetaData)
        {
            if (shouldClear)
            {
                viewDiff.Clear();
                shouldClear = false;
            }

            var netStats = viewDiff.GetNetStats();

            for (var i = 0; i < opList.GetOpCount(); ++i)
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
                    case OpType.ReserveEntityIdsResponse:
                        var reserveEntityIdsOp = opList.GetReserveEntityIdsResponseOp(i);
                        ComponentOpDeserializer.ApplyReserveEntityIdsResponse(reserveEntityIdsOp, viewDiff,
                            commandMetaData);
                        netStats.AddWorldCommandResponse(WorldCommand.ReserveEntityIds);
                        break;
                    case OpType.CreateEntityResponse:
                        var createEntityOp = opList.GetCreateEntityResponseOp(i);
                        ComponentOpDeserializer.ApplyCreateEntityResponse(createEntityOp, viewDiff, commandMetaData);
                        netStats.AddWorldCommandResponse(WorldCommand.CreateEntity);
                        break;
                    case OpType.DeleteEntityResponse:
                        var deleteEntityOp = opList.GetDeleteEntityResponseOp(i);
                        ComponentOpDeserializer.ApplyDeleteEntityResponse(deleteEntityOp, viewDiff, commandMetaData);
                        netStats.AddWorldCommandResponse(WorldCommand.DeleteEntity);
                        break;
                    case OpType.EntityQueryResponse:
                        var entityQueryOp = opList.GetEntityQueryResponseOp(i);
                        ComponentOpDeserializer.ApplyEntityQueryResponse(entityQueryOp, viewDiff, commandMetaData);
                        netStats.AddWorldCommandResponse(WorldCommand.EntityQuery);
                        break;
                    case OpType.AddComponent:
                        ComponentOpDeserializer.DeserializeAndAddComponent(opList.GetAddComponentOp(i), viewDiff, componentUpdateId);
                        componentUpdateId++;
                        break;
                    case OpType.RemoveComponent:
                        var removeComponentOp = opList.GetRemoveComponentOp(i);
                        viewDiff.RemoveComponent(removeComponentOp.EntityId, removeComponentOp.ComponentId);
                        break;
                    case OpType.ComponentSetAuthorityChange:
                        ProcessComponentSetAuthorityChange(opList.GetComponentSetAuthorityChangeOp(i));
                        break;
                    case OpType.ComponentUpdate:
                        var updateOp = opList.GetComponentUpdateOp(i);
                        ComponentOpDeserializer.DeserializeAndApplyComponentUpdate(updateOp, viewDiff,
                            componentUpdateId);
                        ++componentUpdateId;
                        netStats.AddUpdate(updateOp.Update);
                        break;
                    case OpType.CommandRequest:
                        var commandRequestOp = opList.GetCommandRequestOp(i);
                        ComponentOpDeserializer.DeserializeAndApplyCommandRequestReceived(commandRequestOp, viewDiff);
                        netStats.AddCommandRequest(commandRequestOp.Request);
                        break;
                    case OpType.CommandResponse:
                        var commandResponseOp = opList.GetCommandResponseOp(i);
                        ComponentOpDeserializer.DeserializeAndApplyCommandResponseReceived(
                            commandResponseOp, viewDiff, commandMetaData);
                        netStats.AddCommandResponse(commandResponseOp.Response, commandResponseOp.Message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Can not deserialise unkown op type {opList.GetOpType(i)}");
                }
            }

            return viewDiff.InCriticalSection;
        }

        private void ProcessComponentSetAuthorityChange(ComponentSetAuthorityChangeOp authorityChangeOp)
        {
            if (!componentSetManager.TryGetComponentSet(authorityChangeOp.ComponentSetId, out var componentSet))
            {
                Debug.LogWarning($"Unknown component set ID: {authorityChangeOp.ComponentSetId}");
                return;
            }

            // Set the authority for each.
            foreach (var componentId in componentSet.ComponentIds)
            {
                viewDiff.SetComponentAuthority(authorityChangeOp.EntityId, componentId, authorityChangeOp.Authority,
                    authorityChangeId);
            }

            if (authorityChangeOp.CanonicalComponentSetData.Length == 0)
            {
                foreach (var componentId in componentSet.ComponentIds)
                {
                    viewDiff.RemoveComponent(authorityChangeOp.EntityId, componentId);
                }
            }
            else
            {
                // Sort the canonical component data set.
                // Iterate through it and remove any components that are in the set,
                // but not the canonical component set data.
                using (var sortedComponentData =
                    authorityChangeOp.CanonicalComponentSetData.OrderBy(cd => cd.ComponentId).GetEnumerator())
                {
                    sortedComponentData.MoveNext();
                    // Otherwise go through each list and remove the elements.
                    foreach (var componentId in componentSet.ComponentIds)
                    {
                        if (sortedComponentData.Current.ComponentId != componentId)
                        {
                            viewDiff.RemoveComponent(authorityChangeOp.EntityId, componentId);
                        }
                        else
                        {
                            ComponentOpDeserializer.DeserializeAndAddComponent(new AddComponentOp
                            {
                                EntityId = authorityChangeOp.EntityId,
                                Data = sortedComponentData.Current
                            }, viewDiff, componentUpdateId);
                            componentUpdateId++;
                            sortedComponentData.MoveNext();
                        }
                    }
                }
            }

            authorityChangeId++;
        }

        public ViewDiff GetViewDiff()
        {
            componentUpdateId = 1;
            authorityChangeId = 1;
            shouldClear = true;
            return viewDiff;
        }
    }
}

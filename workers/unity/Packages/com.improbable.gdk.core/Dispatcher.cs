using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    internal class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }

    // We need to avoid changing the underlying map while iterating over it. For example, if a
    // callback removes itself, a call to InvokeAll iterates over the map to invoke each callback.
    // This then indirectly invokes Remove, which makes the iteration crash.
    //
    // A simple solution to this problem would be to copy the map for each iteration, but this
    // allocates a significant amount of memory each time. Instead, we fix this issue by postponing
    // changes to the map until the end of the top-level call to this object. In the mentioned
    // example, when InvokeAll indirectly calls Remove, the removal is not done immediately, but
    // postponed to the end of the (top-level) InvokeAll call.
    //
    // The current callbacks are those in the map, plus toAdd, minus toRemove. The changes are
    // merged via the UpdateGuard.
    internal class Callbacks<T>
    {
        private SortedDictionary<ulong, Action<T>> map = new SortedDictionary<ulong, Action<T>>();

        private SortedDictionary<ulong, Action<T>> mapReversed =
            new SortedDictionary<ulong, Action<T>>(new DescendingComparer<ulong>());

        private SortedDictionary<ulong, Action<T>> toAdd = new SortedDictionary<ulong, Action<T>>();
        private SortedDictionary<ulong, uint> toRemove = new SortedDictionary<ulong, uint>();

        // Current call level with 0 for the top-level call.
        private int callDepth = 0;

        public void Add(ulong key, Action<T> callback)
        {
            try
            {
                EnterUpdateGuard();
                toAdd.Add(key, callback);
                toRemove.Remove(key);
            }
            finally
            {
                ExitUpdateGuard();
            }
        }

        public bool Remove(ulong key)
        {
            try
            {
                EnterUpdateGuard();

                var isInMap = map.ContainsKey(key);
                var hadPendingAdd = toAdd.Remove(key);

                if (isInMap)
                {
                    var createdPendingRemove = !toRemove.ContainsKey(key);
                    toRemove.Add(key, /* ignored */ 0);
                    return createdPendingRemove;
                }
                else
                {
                    return hadPendingAdd;
                }
            }
            finally
            {
                ExitUpdateGuard();
            }
        }

        public void InvokeAll(T op)
        {
            try
            {
                EnterUpdateGuard();
                foreach (var pair in map)
                {
                    pair.Value(op);
                }
            }
            finally
            {
                ExitUpdateGuard();
            }
        }

        public void InvokeAllReverse(T op)
        {
            try
            {
                EnterUpdateGuard();
                foreach (var pair in mapReversed)
                {
                    pair.Value(op);
                }
            }
            finally
            {
                ExitUpdateGuard();
            }
        }

        /// <summary>
        ///     Merge toAdd and toRemove with map.
        /// </summary>
        private void UpdateCallbacks()
        {
            foreach (var node in toAdd)
            {
                map.Add(node.Key, node.Value);
                mapReversed.Add(node.Key, node.Value);
            }

            toAdd.Clear();
            foreach (var node in toRemove)
            {
                map.Remove(node.Key);
                mapReversed.Remove(node.Key);
            }

            toRemove.Clear();
        }

        /// <summary>
        ///     Registers that a call is entered to update the state of the map accordingly.
        /// </summary>
        private void EnterUpdateGuard()
        {
            ++callDepth;
        }

        /// <summary>
        ///     Registers that a call is exited to update the state of the map accordingly.
        /// </summary>
        private void ExitUpdateGuard()
        {
            if (--callDepth == 0)
            {
                UpdateCallbacks();
            }
        }
    }

    public class Dispatcher
    {
        private ulong currentCallbackKey;
        private readonly Callbacks<DisconnectOp> disconnectCallbacks = new Callbacks<DisconnectOp>();
        private readonly Callbacks<FlagUpdateOp> flagUpdateCallbacks = new Callbacks<FlagUpdateOp>();
        private readonly Callbacks<LogMessageOp> logMessageCallbacks = new Callbacks<LogMessageOp>();
        private readonly Callbacks<MetricsOp> metricsCallbacks = new Callbacks<MetricsOp>();
        private readonly Callbacks<CriticalSectionOp> criticalSectionCallbacks = new Callbacks<CriticalSectionOp>();
        private readonly Callbacks<AddEntityOp> addEntityCallbacks = new Callbacks<AddEntityOp>();
        private readonly Callbacks<RemoveEntityOp> removeEntityCallbacks = new Callbacks<RemoveEntityOp>();

        private readonly Callbacks<ReserveEntityIdResponseOp> reserveEntityIdResponseCallbacks =
            new Callbacks<ReserveEntityIdResponseOp>();

        private readonly Callbacks<ReserveEntityIdsResponseOp> reserveEntityIdsResponseCallbacks =
            new Callbacks<ReserveEntityIdsResponseOp>();

        private readonly Callbacks<CreateEntityResponseOp> createEntityResponseCallbacks =
            new Callbacks<CreateEntityResponseOp>();

        private readonly Callbacks<DeleteEntityResponseOp> deleteEntityResponseCallbacks =
            new Callbacks<DeleteEntityResponseOp>();

        private readonly Callbacks<EntityQueryResponseOp> entityQueryResponseCallbacks =
            new Callbacks<EntityQueryResponseOp>();

        private readonly Callbacks<AddComponentOp> addComponentCallbacks = new Callbacks<AddComponentOp>();
        private readonly Callbacks<RemoveComponentOp> removeComponentCallbacks = new Callbacks<RemoveComponentOp>();
        private readonly Callbacks<AuthorityChangeOp> authorityChangeCallbacks = new Callbacks<AuthorityChangeOp>();
        private readonly Callbacks<ComponentUpdateOp> componentUpdateCallbacks = new Callbacks<ComponentUpdateOp>();
        private readonly Callbacks<CommandRequestOp> commandRequestCallbacks = new Callbacks<CommandRequestOp>();
        private readonly Callbacks<CommandResponseOp> commandResponseCallbacks = new Callbacks<CommandResponseOp>();

        public Dispatcher()
        {
        }

        public ulong OnDisconnect(Action<DisconnectOp> callback)
        {
            disconnectCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnFlagUpdate(Action<FlagUpdateOp> callback)
        {
            flagUpdateCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnLogMessage(Action<LogMessageOp> callback)
        {
            logMessageCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnMetrics(Action<MetricsOp> callback)
        {
            metricsCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnCriticalSection(Action<CriticalSectionOp> callback)
        {
            criticalSectionCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnAddEntity(Action<AddEntityOp> callback)
        {
            addEntityCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnRemoveEntity(Action<RemoveEntityOp> callback)
        {
            removeEntityCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnReserveEntityIdsResponse(Action<ReserveEntityIdsResponseOp> callback)
        {
            reserveEntityIdsResponseCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnCreateEntityResponse(Action<CreateEntityResponseOp> callback)
        {
            createEntityResponseCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnDeleteEntityResponse(Action<DeleteEntityResponseOp> callback)
        {
            deleteEntityResponseCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }


        public ulong OnEntityQueryResponse(Action<EntityQueryResponseOp> callback)
        {
            entityQueryResponseCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnAddComponent(Action<AddComponentOp> callback)
        {
            addComponentCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnRemoveComponent(Action<RemoveComponentOp> callback)
        {
            removeComponentCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnAuthorityChange(Action<AuthorityChangeOp> callback)
        {
            authorityChangeCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnComponentUpdate(Action<ComponentUpdateOp> callback)
        {
            componentUpdateCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnCommandRequest(Action<CommandRequestOp> callback)
        {
            commandRequestCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public ulong OnCommandResponse(Action<CommandResponseOp> callback)
        {
            commandResponseCallbacks.Add(currentCallbackKey, callback);
            return currentCallbackKey++;
        }

        public void Remove(ulong callbackKey)
        {
            if (!disconnectCallbacks.Remove(callbackKey) &&
                !flagUpdateCallbacks.Remove(callbackKey) &&
                !logMessageCallbacks.Remove(callbackKey) &&
                !metricsCallbacks.Remove(callbackKey) &&
                !criticalSectionCallbacks.Remove(callbackKey) &&
                !addEntityCallbacks.Remove(callbackKey) &&
                !removeEntityCallbacks.Remove(callbackKey) &&
                !reserveEntityIdsResponseCallbacks.Remove(callbackKey) &&
                !createEntityResponseCallbacks.Remove(callbackKey) &&
                !deleteEntityResponseCallbacks.Remove(callbackKey) &&
                !entityQueryResponseCallbacks.Remove(callbackKey) &&
                !addComponentCallbacks.Remove(callbackKey) &&
                !removeComponentCallbacks.Remove(callbackKey) &&
                !authorityChangeCallbacks.Remove(callbackKey) &&
                !componentUpdateCallbacks.Remove(callbackKey) &&
                !commandRequestCallbacks.Remove(callbackKey) &&
                !commandResponseCallbacks.Remove(callbackKey))
            {
                throw new ArgumentException("Unknown callback key");
            }
        }

        public void Process(OpList opList)
        {
            for (var opIndex = 0; opIndex < opList.GetOpCount(); ++opIndex)
            {
                switch (opList.GetOpType(opIndex))
                {
                    case OpType.Disconnect:
                        HandleDisconnect(opList.GetDisconnectOp(opIndex));
                        break;
                    case OpType.FlagUpdate:
                        HandleFlagUpdate(opList.GetFlagUpdateOp(opIndex));
                        break;
                    case OpType.LogMessage:
                        HandleLogMessage(opList.GetLogMessageOp(opIndex));
                        break;
                    case OpType.Metrics:
                        HandleMetrics(opList.GetMetricsOp(opIndex));
                        break;
                    case OpType.CriticalSection:
                        HandleCriticalSection(opList.GetCriticalSectionOp(opIndex));
                        break;
                    case OpType.AddEntity:
                        HandleAddEntity(opList.GetAddEntityOp(opIndex));
                        break;
                    case OpType.RemoveEntity:
                        HandleRemoveEntity(opList.GetRemoveEntityOp(opIndex));
                        break;
                    case OpType.ReserveEntityIdsResponse:
                        HandleReserveEntityIdsResponse(opList.GetReserveEntityIdsResponseOp(opIndex));
                        break;
                    case OpType.CreateEntityResponse:
                        HandleCreateEntityResponse(opList.GetCreateEntityResponseOp(opIndex));
                        break;
                    case OpType.DeleteEntityResponse:
                        HandleDeleteEntityResponse(opList.GetDeleteEntityResponseOp(opIndex));
                        break;
                    case OpType.EntityQueryResponse:
                        HandleEntityQueryResponse(opList.GetEntityQueryResponseOp(opIndex));
                        break;
                    case OpType.AddComponent:
                        HandleAddComponent(opList.GetAddComponentOp(opIndex));
                        break;
                    case OpType.RemoveComponent:
                        HandleRemoveComponent(opList.GetRemoveComponentOp(opIndex));
                        break;
                    case OpType.AuthorityChange:
                        HandleAuthorityChange(opList.GetAuthorityChangeOp(opIndex));
                        break;
                    case OpType.ComponentUpdate:
                        HandleComponentUpdate(opList.GetComponentUpdateOp(opIndex));
                        break;
                    case OpType.CommandRequest:
                        HandleCommandRequest(opList.GetCommandRequestOp(opIndex));
                        break;
                    case OpType.CommandResponse:
                        HandleCommandResponse(opList.GetCommandResponseOp(opIndex));
                        break;
                    default:
                        throw new ArgumentException($"Unknown op type: {opList.GetOpType(opIndex)}");
                }
            }
        }

        private void HandleDisconnect(DisconnectOp op)
        {
            disconnectCallbacks.InvokeAll(op);
        }

        private void HandleFlagUpdate(FlagUpdateOp op)
        {
            if (op.Value != null)
            {
                flagUpdateCallbacks.InvokeAll(op);
            }
            else
            {
                flagUpdateCallbacks.InvokeAllReverse(op);
            }
        }

        private void HandleLogMessage(LogMessageOp op)
        {
            logMessageCallbacks.InvokeAll(op);
        }

        private void HandleMetrics(MetricsOp op)
        {
            metricsCallbacks.InvokeAll(op);
        }

        private void HandleCriticalSection(CriticalSectionOp op)
        {
            if (op.InCriticalSection)
            {
                criticalSectionCallbacks.InvokeAll(op);
            }
            else
            {
                criticalSectionCallbacks.InvokeAllReverse(op);
            }
        }

        private void HandleAddEntity(AddEntityOp op)
        {
            addEntityCallbacks.InvokeAll(op);
        }

        private void HandleRemoveEntity(RemoveEntityOp op)
        {
            removeEntityCallbacks.InvokeAllReverse(op);
        }

        private void HandleReserveEntityIdResponse(ReserveEntityIdResponseOp op)
        {
            reserveEntityIdResponseCallbacks.InvokeAll(op);
        }

        private void HandleReserveEntityIdsResponse(ReserveEntityIdsResponseOp op)
        {
            reserveEntityIdsResponseCallbacks.InvokeAll(op);
        }

        private void HandleCreateEntityResponse(CreateEntityResponseOp op)
        {
            createEntityResponseCallbacks.InvokeAll(op);
        }

        private void HandleDeleteEntityResponse(DeleteEntityResponseOp op)
        {
            deleteEntityResponseCallbacks.InvokeAll(op);
        }

        private void HandleEntityQueryResponse(EntityQueryResponseOp op)
        {
            entityQueryResponseCallbacks.InvokeAll(op);
        }

        private void HandleAddComponent(AddComponentOp op)
        {
            addComponentCallbacks.InvokeAll(op);
        }

        private void HandleRemoveComponent(RemoveComponentOp op)
        {
            removeComponentCallbacks.InvokeAllReverse(op);
        }

        private void HandleAuthorityChange(AuthorityChangeOp op)
        {
            if (op.Authority == Authority.Authoritative)
            {
                authorityChangeCallbacks.InvokeAll(op);
            }
            else
            {
                authorityChangeCallbacks.InvokeAllReverse(op);
            }
        }

        private void HandleComponentUpdate(ComponentUpdateOp op)
        {
            componentUpdateCallbacks.InvokeAll(op);
        }

        private void HandleCommandRequest(CommandRequestOp op)
        {
            commandRequestCallbacks.InvokeAll(op);
        }

        private void HandleCommandResponse(CommandResponseOp op)
        {
            commandResponseCallbacks.InvokeAll(op);
        }
    }
}

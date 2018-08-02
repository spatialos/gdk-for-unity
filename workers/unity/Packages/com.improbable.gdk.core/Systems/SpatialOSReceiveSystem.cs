using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class SpatialOSReceiveSystem : ComponentSystem
    {
        private WorkerBase worker;
        private MutableView view;
        private Dispatcher dispatcher;

        private readonly Dictionary<uint, ComponentDispatcherHandler> componentSpecificDispatchers =
            new Dictionary<uint, ComponentDispatcherHandler>();

        private bool inCriticalSection = false;

        private const string UnknownComponentIdError = "Received an op with an unknown ComponentId";

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = WorkerRegistry.GetWorkerForWorld(World);
            view = worker.View;

            dispatcher = new Dispatcher();
            SetupDispatcherHandlers();
        }

        protected override void OnUpdate()
        {
            if (worker.Connection == null)
            {
                return;
            }

            do
            {
                using (var opList = worker.Connection.GetOpList(0))
                {
                    dispatcher.Process(opList);
                }
            }
            while (inCriticalSection);
        }

        private void OnAddEntity(AddEntityOp op)
        {
            view.CreateEntity(op.EntityId);
        }

        private void OnRemoveEntity(RemoveEntityOp op)
        {
            view.RemoveEntity(op.EntityId);
        }

        private void OnDisconnect(DisconnectOp op)
        {
            view.Disconnect(op.Reason);
        }

        private void OnAddComponent(AddComponentOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Data.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.Data.ComponentId));
                return;
            }

            specificDispatcher.OnAddComponent(op);
        }

        private void OnRemoveComponent(RemoveComponentOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.ComponentId));
                return;
            }

            specificDispatcher.OnRemoveComponent(op);
        }

        private void OnComponentUpdate(ComponentUpdateOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Update.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.Update.ComponentId));
                return;
            }

            specificDispatcher.OnComponentUpdate(op);
        }

        private void OnAuthorityChange(AuthorityChangeOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.ComponentId));
                return;
            }

            specificDispatcher.OnAuthorityChange(op);
        }

        private void OnCommandRequest(CommandRequestOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Request.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.Request.ComponentId));
                return;
            }

            specificDispatcher.OnCommandRequest(op);
        }

        private void OnCommandResponse(CommandResponseOp op)
        {
            if (!componentSpecificDispatchers.TryGetValue(op.Response.ComponentId, out var specificDispatcher))
            {
                view.LogDispatcher.HandleLog(LogType.Error,
                    new LogEvent(UnknownComponentIdError).WithField("Op Type", op.GetType())
                        .WithField("ComponentId", op.Response.ComponentId));
                return;
            }

            specificDispatcher.OnCommandResponse(op);
        }

        private void SetupDispatcherHandlers()
        {
            // Find all component specific dispatchers and create an instance.
            var componentDispatcherTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentDispatcherHandler).IsAssignableFrom(type) && !type.IsAbstract);

            foreach (var componentDispatcherType in componentDispatcherTypes)
            {
                var componentDispatcher =
                    (ComponentDispatcherHandler) Activator.CreateInstance(componentDispatcherType,
                        new { view });
                componentSpecificDispatchers.Add(componentDispatcher.ComponentId, componentDispatcher);
            }

            dispatcher.OnAddEntity(OnAddEntity);
            dispatcher.OnRemoveEntity(OnRemoveEntity);
            dispatcher.OnDisconnect(OnDisconnect);
            dispatcher.OnCriticalSection(op => { inCriticalSection = op.InCriticalSection; });

            dispatcher.OnAddComponent(OnAddComponent);
            dispatcher.OnRemoveComponent(OnRemoveComponent);
            dispatcher.OnComponentUpdate(OnComponentUpdate);
            dispatcher.OnAuthorityChange(OnAuthorityChange);

            dispatcher.OnCommandRequest(OnCommandRequest);
            dispatcher.OnCommandResponse(OnCommandResponse);
        }
    }
}

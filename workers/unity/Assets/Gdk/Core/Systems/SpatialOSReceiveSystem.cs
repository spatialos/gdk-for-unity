using System;
using Improbable.Worker.Core;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Components;
using Unity.Entities;

namespace Improbable.Gdk.Core
{

    public struct SpecificCommandReference
    {
        public uint ComponentId;
        public uint CommandId;
    }

    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class SpatialOSReceiveSystem : ComponentSystem
    {
        private WorkerBase worker;
        private MutableView view;
        private Dispatcher dispatcher;

        private Dictionary<uint, ComponentDispatcher> componentCallbacks = new Dictionary<uint, ComponentDispatcher>();
        private Dictionary<SpecificCommandReference, Action<MutableView, CommandRequestOp>> commandRequestCallbacks = new Dictionary<SpecificCommandReference, Action<MutableView, CommandRequestOp>>();
        private Dictionary<SpecificCommandReference, Action<MutableView, CommandResponseOp>> commandResponseCallbacks = new Dictionary<SpecificCommandReference, Action<MutableView, CommandResponseOp>>();

        private bool inCriticalSection = false;

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
            view.CreateEntity(op.EntityId.Id);
        }

        private void OnRemoveEntity(RemoveEntityOp op)
        {
            view.RemoveEntity(op.EntityId.Id);
        }

        private void OnDisconnect(DisconnectOp op)
        {
            view.Disconnect(op.Reason);
        }

        private void OnAddComponent(AddComponentOp op)
        {
            componentCallbacks[op.Data.ComponentId].OnAddComponent(view, op);
        }

        private void OnRemoveComponent(RemoveComponentOp op)
        {
            componentCallbacks[op.ComponentId].OnRemoveComponent(view, op);
        }

        private void OnComponentUpdate(ComponentUpdateOp op)
        {
            componentCallbacks[op.Update.ComponentId].OnComponentUpdate(view, op);
        }

        private void OnAuthorityChange(AuthorityChangeOp op)
        {
            componentCallbacks[op.ComponentId].OnAuthorityChange(view, op);
        }

        private void OnCommandRequest(CommandRequestOp op)
        {
            var commandSpecifier = new SpecificCommandReference
            {
                ComponentId = op.Request.ComponentId,
                CommandId = op.Request.SchemaData.Value.GetCommandIndex()
            };
            commandRequestCallbacks[commandSpecifier](view, op);
        }

        private void OnCommandResponse(CommandResponseOp op)
        {
            var commandSpecifier = new SpecificCommandReference
            {
                ComponentId = op.Response.Value.ComponentId,
                CommandId = op.Response.Value.SchemaData.Value.GetCommandIndex()
            };

            commandResponseCallbacks[commandSpecifier](view, op);
        }

        private void SetupDispatcherHandlers()
        {
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

            var componentDispatchers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentDispatcher).IsAssignableFrom(type) && !type.IsAbstract).ToList();

            foreach (var componentDispatcher in componentDispatchers)
            {
                var cd = (ComponentDispatcher) Activator.CreateInstance(componentDispatcher, this);
                componentCallbacks[cd.ComponentId] = cd;
                cd.AddCommandCallbacks(commandRequestCallbacks, commandResponseCallbacks);
            }
        }
    }
}

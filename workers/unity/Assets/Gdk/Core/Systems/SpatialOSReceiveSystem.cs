using Improbable.Worker.Core;
using System.Collections.Generic;
using Improbable.Gdk.Core.Components;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class SpatialOSReceiveSystem : ComponentSystem
    {
        private WorkerBase worker;
        private MutableView view;
        private Dispatcher dispatcher;

        private Dictionary<long, IDispatcherCallbacks> componentCallbacks = new Dictionary<long, IDispatcherCallbacks>();

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
            componentCallbacks[op.Data.ComponentId].OnAddComponent(op);
        }

        private void OnRemoveComponent(RemoveComponentOp op)
        {
            componentCallbacks[op.ComponentId].OnRemoveComponent(op);
        }

        private void OnComponentUpdate(ComponentUpdateOp op)
        {
            componentCallbacks[op.Update.ComponentId].OnComponentUpdate(op);
        }

        private void OnAuthorityChange(AuthorityChangeOp op)
        {
            componentCallbacks[op.ComponentId].OnAuthorityChange(op);
        }

        private void SetupDispatcherHandlers()
        {
            dispatcher.OnDisconnect(OnDisconnect);

            dispatcher.OnAddEntity(OnAddEntity);
            dispatcher.OnRemoveEntity(OnRemoveEntity);

            dispatcher.OnCriticalSection(op => { inCriticalSection = op.InCriticalSection; });

            dispatcher.OnAddComponent(OnAddComponent);
            dispatcher.OnRemoveComponent(OnRemoveComponent);
            dispatcher.OnComponentUpdate(OnComponentUpdate);
            dispatcher.OnAuthorityChange(OnAuthorityChange);

            foreach (var translationUnit in view.TranslationUnits.Values)
            {
                if (translationUnit is IDispatcherCallbacks)
                {
                    componentCallbacks.Add(translationUnit.ComponentId, translationUnit as IDispatcherCallbacks);
                }
                translationUnit.RegisterWithDispatcher(dispatcher);
            }
        }
    }
}

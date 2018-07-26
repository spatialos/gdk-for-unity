using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class SpatialOSReceiveSystem : ComponentSystem
    {
        private WorkerBase worker;
        private MutableView view;
        private Dispatcher dispatcher;

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

        private void SetupDispatcherHandlers()
        {
            dispatcher.OnAddEntity(OnAddEntity);
            dispatcher.OnRemoveEntity(OnRemoveEntity);
            dispatcher.OnDisconnect(OnDisconnect);
            dispatcher.OnCriticalSection(op => { inCriticalSection = op.InCriticalSection; });

            foreach (var translationUnit in view.TranslationUnits.Values)
            {
                translationUnit.RegisterWithDispatcher(dispatcher);
            }
        }
    }
}

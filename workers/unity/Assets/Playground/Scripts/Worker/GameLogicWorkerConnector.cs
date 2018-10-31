using Improbable.Gdk.Core;
using UnityEngine;

namespace Playground
{
    public class GameLogicWorkerConnector : DefaultWorkerConnector
    {
        [SerializeField] private GameObject level;

        private GameObject levelInstance;

        private async void Start()
        {
            await Connect(WorkerUtils.UnityGameLogic, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            WorkerUtils.AddGameLogicSystems(Worker.World);
            if (level == null)
            {
                return;
            }

            levelInstance = Instantiate(level, transform.position, transform.rotation);
        }

        public override void Dispose()
        {
            if (levelInstance != null)
            {
                Destroy(levelInstance);
            }

            base.Dispose();
        }
    }
}

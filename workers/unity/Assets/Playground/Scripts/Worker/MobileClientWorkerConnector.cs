using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
using UnityEngine;

namespace Playground
{
    public class MobileClientWorkerConnector : DefaultMobileWorkerConnector
    {
#pragma warning disable 649
        [SerializeField] private GameObject level;
#pragma warning restore 649

        private GameObject levelInstance;

        public async void Start()
        {
            await Connect(WorkerUtils.MobileClient, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            WorkerUtils.AddClientSystems(Worker.World);

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

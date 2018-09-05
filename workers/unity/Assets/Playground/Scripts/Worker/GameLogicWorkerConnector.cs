using Improbable.Gdk.Core;
using Playground;

public class GameLogicWorkerConnector : WorkerConnectorBase
{
    private async void Start()
    {
        await Connect(WorkerUtils.UnityGameLogic, new ForwardingDispatcher()).ConfigureAwait(false);
    }

    protected override void AddWorkerSystems()
    {
        WorkerUtils.AddGameLogicSystems(Worker.World);
    }
}

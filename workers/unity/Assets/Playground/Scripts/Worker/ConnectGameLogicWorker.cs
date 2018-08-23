using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Improbable.Gdk.Core;
using Playground;
using Unity.Entities;
using UnityEngine;

public class ConnectGameLogicWorker : MonoBehaviour, IDisposable
{
    public Vector3 Origin = Vector3.zero;
    public GameObject Level;
    public int ConnectionAttempts = 3;

    public Worker GameLogicWorker;

    protected async void Start()
    {
        await CreateWorkerAsync(ConnectionAttempts);
    }

    protected void OnApplicationQuit()
    {
        Dispose();
    }

    private async Task CreateWorkerAsync(int attempts)
    {
        const string workerType = SystemConfig.UnityGameLogic;
        ReceptionistConfig config;
        if (Application.isEditor)
        {
            config = new ReceptionistConfig
            {
                WorkerType = workerType,
                WorkerId = $"{workerType}-{Guid.NewGuid()}"
            };
        }
        else
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            config = ReceptionistConfig.CreateConnectionConfigFromCommandLine(commandLineArgs);
            if (!commandLineArgs.ContainsKey(RuntimeConfigNames.WorkerId))
            {
                config.WorkerId = $"{workerType}-{Guid.NewGuid()}";
            }
        }

        try
        {
            var worker = await ConnectWithRetriesAsync(config, new ForwardingDispatcher(), Origin, attempts);
            InitializeWorker(worker);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to create worker {config.WorkerId} with error:{Environment.NewLine}{e.Message}");
            Dispose();
        }
    }

    private async Task<Worker> ConnectWithRetriesAsync(ReceptionistConfig config, ILogDispatcher dispatcher, Vector3 origin,
        int attempts)
    {
        while (attempts > 0)
        {
            try
            {
                var worker = await Worker.CreateWorkerAsync(config, dispatcher, origin);
                return worker;
            }
            catch (ConnectionFailedException e)
            {
                Debug.LogError(
                    $"Failed attempt to create game logic worker with error:{Environment.NewLine}{e.Message}");
                attempts--;
            }
        }

        throw new ConnectionFailedException(
            $"Exceeded maximum connection attempts ",
            ConnectionErrorReason.ExceededMaximumRetries);
    }

    private void InitializeWorker(Worker worker)
    {
        GameLogicWorker = worker;
        SystemConfig.AddGameLogicSystems(GameLogicWorker.World);
        InstantiateLevel();
        GameLogicWorker.OnDisconnect += OnDisconnected;
        if (World.Active == null)
        {
            World.Active = GameLogicWorker.World;
        }

        ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World.AllWorlds.ToArray());
    }

    private void InstantiateLevel()
    {
        Level.transform.position = Origin;
        Level.SetActive(true);
    }

    private void OnDisconnected(string reason)
    {
        GameLogicWorker.LogDispatcher.HandleLog(LogType.Log, new LogEvent($"Worker disconnected")
            .WithField("WorkerId", GameLogicWorker.WorkerId)
            .WithField("Reason", reason));
        StartCoroutine(DeferredDisposeWorker());
    }

    private IEnumerator DeferredDisposeWorker()
    {
        yield return null;
        Dispose();
    }

    public void Dispose()
    {
        GameLogicWorker?.Dispose();
        GameLogicWorker = null;
        Destroy(this);
    }
}

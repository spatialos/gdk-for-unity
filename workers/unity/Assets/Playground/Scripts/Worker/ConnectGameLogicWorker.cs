using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Improbable.Gdk.Core;
using Playground;
using Unity.Entities;
using UnityEngine;

public class ConnectGameLogicWorker : MonoBehaviour
{
    public Vector3 Origin = Vector3.zero;
    public GameObject Level;
    public int ConnectionAttempts = 3;

    public Worker GameLogicWorker;

    protected void Start()
    {
        CreateWorker(ConnectionAttempts);
    }

    protected void OnApplicationQuit()
    {
        GameLogicWorker?.Dispose();
    }

    private void CreateWorker(int attempts)
    {
        Task<Worker> workerTask;
        const string workerType = SystemConfig.UnityGameLogic;
        if (Application.isEditor)
        {
            var config = new ReceptionistConfig
            {
                WorkerType = workerType,
                WorkerId = $"{workerType}-{Guid.NewGuid()}"
            };
            workerTask = Worker.CreateWorkerAsync(config, new ForwardingDispatcher(), Origin);
        }
        else
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            Debug.LogFormat("Command line {0}", string.Join(" ", commandLineArguments.ToArray()));
            var commandLineArgs = CommandLineUtility.ParseCommandLineArgs(commandLineArguments);
            var config = ReceptionistConfig.CreateConnectionConfigFromCommandLine(commandLineArgs);
            if (!commandLineArgs.ContainsKey(RuntimeConfigNames.WorkerId))
            {
                config.WorkerId = $"{workerType}-{Guid.NewGuid()}";
            }

            workerTask = Worker.CreateWorkerAsync(config, new ForwardingDispatcher(), Origin);
        }

        StartCoroutine(CheckForWorkerReady(workerTask, attempts - 1));
    }

    private IEnumerator CheckForWorkerReady(Task<Worker> workerTask, int attempts)
    {
        while (!workerTask.IsCompleted)
        {
            yield return null;
        }

        if (workerTask.IsFaulted)
        {
            Debug.LogError(
                $"Failed to create game logic worker with error: {workerTask.Exception.InnerException.Message}");
            if (attempts > 0)
            {
                CreateWorker(attempts);
            }

            yield break;
        }

        GameLogicWorker = workerTask.Result;
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
        GameLogicWorker.LogDispatcher.HandleLog(LogType.Warning, new LogEvent($"Worker disconnected")
            .WithField("WorkerId", GameLogicWorker.WorkerId)
            .WithField("Reason", reason));
        StartCoroutine(DefferedDisposeWorker());
    }

    private IEnumerator DefferedDisposeWorker()
    {
        yield return null;
        GameLogicWorker.Dispose();
        Destroy(this);
    }
}

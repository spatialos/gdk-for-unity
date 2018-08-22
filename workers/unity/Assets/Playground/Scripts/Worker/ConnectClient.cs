using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using Playground;
using Unity.Entities;
using UnityEngine;

public class ConnectClient : MonoBehaviour
{
    public Vector3 Origin = Vector3.zero;
    public GameObject Level;
    public int ConnectionAttempts = 3;

    public Worker ClientWorker;

    protected void Start()
    {
        CreateWorker(ConnectionAttempts);
    }

    protected void OnApplicationQuit()
    {
        ClientWorker?.Dispose();
    }

    private static string SelectDeploymentName(DeploymentList deploymentList)
    {
        return deploymentList.Deployments[0].DeploymentName;
    }

    private static bool HandleQueueStatus(QueueStatus queueStatus)
    {
        return true;
    }

    private void CreateWorker(int attempts)
    {
        Task<Worker> workerTask;
        const string workerType = SystemConfig.UnityClient;
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
            if (commandLineArgs.ContainsKey(RuntimeConfigNames.LoginToken))
            {
                var config = LocatorConfig.CreateConnectionConfigFromCommandLine(commandLineArgs);
                config.WorkerType = workerType;
                config.WorkerId = $"{workerType}-{Guid.NewGuid()}";
                workerTask = Worker.CreateWorkerAsync(config, SelectDeploymentName, HandleQueueStatus, new ForwardingDispatcher(),
                    Origin);
            }
            else
            {
                var config = ReceptionistConfig.CreateConnectionConfigFromCommandLine(commandLineArgs);
                config.WorkerId = $"{workerType}-{Guid.NewGuid()}";
                workerTask = Worker.CreateWorkerAsync(config, new ForwardingDispatcher(), Origin);
            }
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
                $"Failed to create client worker with error: {workerTask.Exception.InnerException.Message}");
            if (attempts > 0)
            {
                CreateWorker(attempts);
            }

            yield break;
        }

        ClientWorker = workerTask.Result;
        SystemConfig.AddClientSystems(ClientWorker.World);
        InstantiateLevel();
        ClientWorker.OnDisconnect += OnDisconnected;
        if (World.Active == null)
        {
            World.Active = ClientWorker.World;
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
        ClientWorker.LogDispatcher.HandleLog(LogType.Warning, new LogEvent($"Worker disconnected")
            .WithField("WorkerId", ClientWorker.WorkerId)
            .WithField("Reason", reason));
        StartCoroutine(DefferedDisposeWorker());
    }

    private IEnumerator DefferedDisposeWorker()
    {
        yield return null;
        ClientWorker.Dispose();
        Destroy(this);
    }
}
